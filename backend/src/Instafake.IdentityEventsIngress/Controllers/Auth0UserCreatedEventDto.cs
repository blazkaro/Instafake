using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Instafake.IdentityEventsIngress.Controllers;

public class Auth0UserCreatedEventDto
{
    public class ObjectProperty
    {
        [Required, JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [Required]
        public string Nickname { get; set; }

        [Required]
        public string Picture { get; set; }

        [Required, JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
    }

    public class DataProperty
    {
        [Required]
        public ObjectProperty Object { get; set; }
    }

    [Required]
    public string Id { get; set; }

    [Required]
    public DataProperty Data { get; set; }
}
