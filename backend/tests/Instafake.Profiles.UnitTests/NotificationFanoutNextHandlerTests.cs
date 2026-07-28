using Instafake.Profiles.Application.Dtos;
using Instafake.Profiles.Application.Integration.Outgoing;
using Instafake.Profiles.Application.Options;
using Instafake.Profiles.Infrastructure.DbContexts;
using Instafake.Profiles.Infrastructure.Entities;
using Instafake.Profiles.Infrastructure.Events.Self;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Instafake.Profiles.UnitTests;

public class NotificationFanoutNextHandlerTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<ProfilesDbContext> _options;

    public NotificationFanoutNextHandlerTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        _options = new DbContextOptionsBuilder<ProfilesDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new ProfilesDbContext(_options);
        context.Database.EnsureCreated();
    }

    private ProfilesDbContext CreateDbContext()
    {
        return new ProfilesDbContext(_options);
    }

    [Theory]
    [InlineData(1394, 0, 500)]
    [InlineData(1394, 500, 500)]
    [InlineData(1500, 1498, 1)]
    public async Task When_NotificationFanoutNext_IsHandled_AndNotAllBucketsWereProcessed_Then_FanoutNextShouldBeReturnedWithNewStartingBucket(int totalBuckets, int nextBucket, int batchSize)
    {
        var fanoutNext = new NotificationFanoutNext(Guid.NewGuid(), new PostAuthorDto("id", "name", new Uri("https://example.com")), totalBuckets, nextBucket, batchSize);

        var result = await NotificationFanoutNextHandler.Handle(fanoutNext);

        var fanoutNextToEmit = fanoutNext with { TotalBuckets = totalBuckets, NextBucket = Math.Min(totalBuckets, nextBucket + batchSize), BatchSize = batchSize };
        Assert.Equal(fanoutNextToEmit, Assert.Single(result.OfType<NotificationFanoutNext>()));
    }

    [Theory]
    [InlineData(1394, 1000, 500)]
    [InlineData(1500, 1500, 500)]
    [InlineData(1500, 1498, 500)]
    public async Task When_NotificationFanoutNext_IsHandled_AndAllBucketsWereProcessed_Then_FanoutNextShouldNotBeReturned(int totalBuckets, int nextBucket, int batchSize)
    {
        var fanoutNext = new NotificationFanoutNext(Guid.NewGuid(), new PostAuthorDto("id", "name", new Uri("https://example.com")), totalBuckets, nextBucket, batchSize);

        var result = await NotificationFanoutNextHandler.Handle(fanoutNext);

        Assert.Empty(result);
    }

    private static Profile ProfileMock(string id)
    {
        return new Profile
        {
            Id = id,
            AvatarUrl = "https://example.com",
            Description = "dummy",
            FollowersCount = 0,
            Name = Guid.NewGuid().ToString(),
            PostsCount = 0
        };
    }

    [Fact]
    public async Task When_NotificationFanoutNextIsProcessed_Then_PostNotificationEventPerBucketInBatchShouldBeReturned()
    {
        await using var dbContext = CreateDbContext();

        dbContext.Profiles.AddRange(
            ProfileMock("author-1"),
            ProfileMock("other"),
            ProfileMock("follower-a"),
            ProfileMock("follower-b"),
            ProfileMock("follower-x")
        );

        dbContext.Follows.AddRange(
        // 2 different buckets (0, 1) for the same author
            new Follow { ProfileId = "author-1", FollowerId = "follower-a", BucketId = 0, Seq = 51 },
            new Follow { ProfileId = "author-1", FollowerId = "follower-b", BucketId = 1, Seq = 107 },
        // the same bucket but for different author (buckets are relative to author and we test it)
            new Follow { ProfileId = "other", FollowerId = "follower-x", BucketId = 0, Seq = 0 }
        );
        await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var authorDto = new PostAuthorDto("author-1", "Author Name", new Uri("https://example.com/avatar.png"));
        var fanoutNext = new NotificationFanoutNext(Guid.NewGuid(), authorDto, 10, 0, 2);

        var options = Options.Create(new FollowBucketOptions { BucketSize = 100 });
        var messages = await NotificationFanoutNextHandler.Handle(fanoutNext, dbContext, options, TestContext.Current.CancellationToken);

        var postNotifications = (messages as System.Collections.IEnumerable)?
            .Cast<object>()
            .OfType<PostNotification>()
            .ToList();

        Assert.NotNull(postNotifications);
        Assert.Equal(2, postNotifications.Count);

        // verify follower ids per bucket exist in excatly one of the produced PostNotification messages
        var allFollowerIds = postNotifications.SelectMany(p => p.FollowerIds).ToList();
        Assert.Equal(2, allFollowerIds.Count);
        Assert.Contains("follower-a", allFollowerIds);
        Assert.Contains("follower-b", allFollowerIds);
        // ensure follower from another profile is not present
        Assert.DoesNotContain("follower-x", allFollowerIds);
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();

        GC.SuppressFinalize(this);
    }
}
