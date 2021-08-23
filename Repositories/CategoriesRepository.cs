
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
    public class CategoriesRepository
    {
        private DatabaseContext _context;

        public CategoriesRepository(DatabaseContext context){
            _context = context;
        }


        public IEnumerable<Category> Filter(ResolveFieldContext<object> graphqlContext){
            var results = from category in _context.Categorys select category;
            if(graphqlContext.HasArgument("id")){
                var id = graphqlContext.GetArgument<string>("id");
                results = results.Where(n => n.id.Equals(id));
            }
            return results;
        }

        public Category Find(long id){
            return _context.Categorys.Find(id);
        }
       
    }
}