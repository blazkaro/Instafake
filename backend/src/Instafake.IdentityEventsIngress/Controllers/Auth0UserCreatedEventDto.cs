using System.Text.Json.Serialization;

namespace Instafake.IdentityEventsIngress.Controllers;

public class Auth0UserCreatedEventDto
{
    public class ObjectProperty
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        public string Nickname { get; set; }
        public string Picture { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
    }

    public class DataProperty
    {
        public ObjectProperty Object { get; set; }
    }

    public string Id { get; set; }
    public DataProperty Data { get; set; }
}
