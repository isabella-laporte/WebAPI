using System.Text.Json.Serialization;

namespace WebAPI.DTO
{
    public class BooksPatchRatingDTO
    {
        [JsonPropertyName("userRating")]
        public decimal UserRating { get; set; }

        public BooksPatchRatingDTO(decimal userRating)
        {
            UserRating = userRating;
        }
    }
}
