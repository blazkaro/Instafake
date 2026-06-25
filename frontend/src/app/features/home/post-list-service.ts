import { inject, Injectable, signal } from '@angular/core';
import { PaginatedListServiceBase } from '../../shared/pagination/paginated-list-service-base';
import { Post } from '../../shared/post/models/post';
import { PostsService } from '../../shared/post/services/posts-service';
import { PostEventsService } from '../../shared/post/services/post-events-service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

interface Params {
  tags: string[];
  authorName: string | null;
}

@Injectable()
export class PostListService extends PaginatedListServiceBase<Post, Params> {
  private readonly PAGE_SIZE: number = 20;

  private postsService = inject(PostsService);
  private readonly postEventsService = inject(PostEventsService);

  private tags = signal<string[]>([]);
  private userName = signal<string | null>(null);

  constructor() {
    super({
      params: () => ({
        tags: this.tags(),
        authorName: this.userName()
      }),
      stream: ({ params }) => this.postsService.getPosts(params.tags, params.authorName, { pageSize: this.PAGE_SIZE, cursor: this.cursor() })
    });
  }

  private postCreatedSub = this.postEventsService.postCreated$.pipe(
    takeUntilDestroyed()
  ).subscribe(ev => {
    this.prependItem(ev.post);
  });

  /**
   * Handles change of search query params.
   * @param newValue new search query params
   * @returns array of validated query params
   */
  onSearchQueryChange(newValue: string[]): string[] {
    // delete duplicates
    newValue = Array.from(new Set(newValue));

    const validUsernames = newValue.filter((item) => item.startsWith('@') && item.length > 1);
    const validTags = newValue.filter((item) => item.startsWith('#') && item.length > 1);

    const newestUsername = validUsernames.at(-1);
    const validSearchQuery = newestUsername ? [newestUsername, ...validTags] : validTags;

    // clear pagination (search filters changed)
    this.resetPagination();

    // update username if changed
    if (newestUsername !== this.userName()) {
      this.userName.set(newestUsername?.substring(1) ?? null);
    }

    // update tags if changed
    const validTagsSet = new Set(validTags);
    if (this.tags().length != validTagsSet.size || !this.tags().every((tag) => validTagsSet.has(tag))) {
      this.tags.set(validTags.map((val) => val.substring(1)));
    }

    return validSearchQuery;
  }
}
