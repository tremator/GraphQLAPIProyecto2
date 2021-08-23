
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
    public class UserRepository
    {
          private DatabaseContext _context;

        public UserRepository(DatabaseContext context){
            _context = context;
        }


        public IEnumerable<User> Filter(ResolveFieldContext<object> graphqlContext){
            var results = from users in _context.Users select users;
            if(graphqlContext.HasArgument("id")){
                var id = graphqlContext.GetArgument<string>("id");
                results = results.Where(n => n.id.Equals(id));
            }
            return results;
        }

        public User Find(long id){
            return _context.Users.Find(id);
        }
        public Role GetRole(long id){
            var user = _context.Users.Find(id);
            var role = _context.Roles.Find(user.roleId);
            return role;
        }

        public List<Tags> GetTags(long id, HttpContext accesor){

            var validate = validateToken(accesor);
            if(validate){

                var results = _context.Tags.Where(x => x.userId == id).ToList();
                return results;
            }else{
                return null;
            }
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