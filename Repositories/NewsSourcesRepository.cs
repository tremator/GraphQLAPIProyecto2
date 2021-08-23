
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Xml;
using System;
using ProyectoWeb2GraphQLApi.Models;
using GraphQL.Types;

namespace ProyectoWeb2GraphQLApi.Repositories
{
    public class NewsSourcesRepository
    {
        private DatabaseContext _context;

        public NewsSourcesRepository(DatabaseContext context){
            _context = context;
        }


        public IEnumerable<NewsSource> Filter(ResolveFieldContext<object> graphqlContext){
            var results = from sources in _context.NewsSources select sources;
            if(graphqlContext.HasArgument("userId")){
                var userId = graphqlContext.GetArgument<string>("userId");
                results = results.Where(s => s.userId.Equals(userId));
            }
            return results;
        }

        public NewsSource Find(long id){
            return _context.NewsSources.Find(id);
        }
        public Category GetCategory(long id){
            var source = _context.NewsSources.Find(id);
            var category = _context.Categorys.Find(source.categoryId);
            return category;
        }
        public User GetUser(long id){
            var source = _context.NewsSources.Find(id);
            var user = _context.Users.Find(source.categoryId);
            return user;
        }
        public  IEnumerable<News> Charge(long userId, HttpContext accesor){

            var validate = validateToken(accesor);

            var results = from categorys in _context.Categorys select categorys;
            List<News> oldNews =  _context.News.Where((news) => news.userId == userId).ToList();

            foreach (News item in oldNews)
            {
                _context.News.Remove(item);
            }
            
            List<NewsSource> sources =  _context.NewsSources.Where((source) => source.userId == userId).ToList();

            var httpClient = HttpClientFactory.Create();
            List<News> news = new List<News>();
            List<Tags> generalTags = new List<Tags>();
            
            foreach (NewsSource source in sources)
            {
                List<XmlNode> itemsList = getNodes(source,httpClient,results).Result;
                foreach (XmlNode item in itemsList)
                {
                    var title = item.SelectSingleNode("title").InnerText;
                    var description = item.SelectSingleNode("description").InnerText.Split("<");
                    var link = item.SelectSingleNode("link").InnerText;
                    var date = DateTime.Parse(item.SelectSingleNode("pubDate").InnerText);
                    var categoryText = item.SelectSingleNode("category").InnerText;
                    var category = results.Where((x)=> x.name == categoryText).Single();
                    var tags = new List<string>();
                    foreach (XmlNode node in item.SelectNodes("category"))
                    {
                        tags.Add(node.InnerText);
                        if(generalTags.Count == 0){
                            Tags tag = new Tags();
                            tag.tag = node.InnerText;
                            tag.userId = userId;
                            generalTags.Add(tag);
                        }else{
                            Tags tag = new Tags();
                            tag.tag = node.InnerText;
                            tag.userId = userId;
                            var check = generalTags.Find(x => x.tag == tag.tag);
                            if(check == null){
                                generalTags.Add(tag);
                            }
                        }
                    }
                    News newNotice = new News();
                    newNotice.title = title;
                    newNotice.description = description[0].Length > 200 ? description[0].Substring(0,200) : description[0];
                    newNotice.link = link;
                    newNotice.date = date;
                    newNotice.categoryId = category.id;
                    newNotice.tags = tags;
                    newNotice.userId = userId;
                    newNotice.newsSourceId = source.id;
                    news.Add(newNotice);
                }
                
                    
            }

            saveTags(generalTags,userId);

            foreach (var item in news)
                {
                     _context.Add(item);
                }
                 _context.SaveChanges();

            return  _context.News.Where((x) => x.userId == userId).OrderBy((news) => news.date).ToList();
        }


        public void saveTags(List<Tags> tags, long id){
            var results = from userTags in _context.Tags select userTags;
            var oldTags = results.Where(x => x.userId == id).ToList();
            foreach (Tags item in oldTags)
            {
                _context.Tags.Remove(item);
            }

            _context.Tags.AddRange(tags);
            _context.SaveChanges();
        }

        async Task<List<XmlNode>> getNodes(NewsSource source, HttpClient httpClient, IQueryable<Category> results){
            var doc = new XmlDocument();

            string url = source.url;
            
            var data = await httpClient.GetStringAsync(url);
            doc.LoadXml(data);
            
            XmlNodeList items = doc.GetElementsByTagName("item");
            List<XmlNode> temporalList = new List<XmlNode>();
            
            foreach (XmlNode item in items)
            {
                temporalList.Add(item);
            }

            List<XmlNode> itemsList = temporalList.ToList();
            foreach (XmlNode item in temporalList)
            {
                var categoryText = item.SelectSingleNode("category").InnerText;
                var category = results.Where((x) => x.name == categoryText);
                if(category.Count() == 0){
                    itemsList.Remove(item);
                }
            }
            return itemsList;
        }
        private bool validateToken(HttpContext accesor){

            var headers = accesor.Request.Headers.ToList();
            var accesToken = headers.Where(h => h.Key == "Authorization").Single();
            var users = _context.Users.ToList();
            User user = users.Find(u => u.token == accesToken.Value.ToString());
            if(user == null){
                return false;
            }else{
                return true;
            }
        }



        
    }
}