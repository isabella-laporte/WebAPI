using System.Text.Json.Serialization;

namespace WebAPI.DTO
{
    public class BooksDTO
    {
        [JsonPropertyName("bookName")]
        public string BookName { get; set; }
        [JsonPropertyName("author")]
        public string Author { get; set; }
        [JsonPropertyName("userRating")]
        public decimal UserRating { get; set; }
        [JsonPropertyName("year")]
        public int Year { get; set; }
        [JsonPropertyName("genres")]
        public string Genres { get; set; }

        public BooksDTO(string bookName, string author, decimal userRating, int year, string genres)
        {
            BookName = bookName;
            Author = author;
            UserRating = userRating;
            Year = year;
            Genres = genres;
        }
    }
}
