
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
    public class NewsRepository
    {
        private DatabaseContext _context;

        public NewsRepository(DatabaseContext context){
            _context = context;
        }


        public IEnumerable<News> Filter(ResolveFieldContext<object> graphqlContext,HttpContext accesor){
            var validate = validateToken(accesor);
            if(validate){
                var results = from news in _context.News select news;
                if(graphqlContext.HasArgument("userId")){
                    var userId = graphqlContext.GetArgument<int>("userId");
                    results = results.Where(s => s.userId.Equals(userId));
                }
                if(graphqlContext.HasArgument("id")){
                    var id = graphqlContext.GetArgument<string>("id");
                    results = results.Where(n => n.id.Equals(id));
                }
                if(graphqlContext.HasArgument("tag")){
                    var tag = graphqlContext.GetArgument<string>("tag");
                    results = results.Where(x => x.tags.Contains(tag));
                }
                if(graphqlContext.HasArgument("category")){
                    var category = graphqlContext.GetArgument<int>("category");
                    results = results.Where(x => x.categoryId == category);
                }
                return results;
            }else{
                return null;
            }
            
        }

        public News Find(long id){
            return _context.News.Find(id);
        }
        public Category GetCategory(long id){
            var news = _context.News.Find(id);
            var category = _context.Categorys.Find(news.categoryId);
            return category;
        }
        public User GetUser(long id){
            var news = _context.News.Find(id);
            var user = _context.Users.Find(news.categoryId);
            return user;
        }
        public NewsSource GetSource(long id){
            var news = _context.News.Find(id);
            var source = _context.NewsSources.Find(news.categoryId);
            return source;
        }

        public List<News> Search(long id,string word, HttpContext accesor){

            var validate = validateToken(accesor);
            if(validate){

                var results = _context.News.Where(x => x.userId == id);
                var search = results.Where(x => x.description.Contains(word) || x.title.Contains(word)).ToList();
                return search;

            }else{
                return null;
            }
            
           
        }
        
        
        public List<string> getTags(long id){
            var tag = _context.News.Find(id);
            return tag.tags;
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