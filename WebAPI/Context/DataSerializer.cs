using System.Text.Json;
using WebAPI.Models;

namespace WebAPI.Context
{
    public class DataSerializer
    {
        private readonly MemoryContext _memoryContext;
        private readonly IConfiguration _configuration;


        public DataSerializer(MemoryContext memoryContext, IConfiguration configuration)
        {
            _memoryContext = memoryContext;
            _configuration = configuration;
        }

        public void Generate()
        {
            if (!_memoryContext.Books.Any())
            {
                List<Books> item;
                using (StreamReader r = new StreamReader("booksData.json"))
                {
                    string json = r.ReadToEnd();
                    item = JsonSerializer.Deserialize<List<Books>>(json);
                }
                _memoryContext.Books.AddRange(item);
                _memoryContext.SaveChanges();
            }

            if (!_memoryContext.Users.Any())
            {
                var user = new List<Users>()
                {
                    new() {Name = "Isabella Laporte Santos", Password = _configuration["UserAuthentication:password"],
                        Username = _configuration["UserAuthentication:login"], Role = "Developer" }
                };
                _memoryContext.Users.AddRange(user);
                _memoryContext.SaveChanges();
            }
        }
    }
 }
