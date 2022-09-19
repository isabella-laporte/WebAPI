using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class Books
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }
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

        public Books(int id, string bookName, string author, decimal userRating, int year, string genres)
        {
            Id = id;
            BookName = bookName;
            Author = author;
            UserRating = userRating;
            Year = year;
            Genres = genres; 
        }


        public Books clone()
        {
            return (Books)this.MemberwiseClone(); 
        }
    }
}
