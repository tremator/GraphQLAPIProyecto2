using System;
using System.Collections.Generic;

namespace ProyectoWeb2GraphQLApi.Models
{
    public class News
    {
        public long id { get; set; }
        public string title { get; set; }
        public string description { get; set;}
        public string link { get; set; }
        public DateTime date { get; set; }
        public long newsSourceId { get; set; }
        public NewsSource newsSource { get; set; }
        public long userId { get; set; }
        public User user { get; set; }
        public long categoryId { get; set; }
        public Category category { get; set; }
        public List<string> tags { get; set; }
    }
}