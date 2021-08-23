namespace ProyectoWeb2GraphQLApi.Models
{
    public class NewsSource
    {
        public long id { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public long userId { get; set; }
        public long categoryId { get; set; }
        public Category category { get; set; }
        public User user { get; set; }
    }
}