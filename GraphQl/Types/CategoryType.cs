using GraphQL.Types;
using ProyectoWeb2GraphQLApi.Models;

namespace ProyectoWeb2GraphQLApi.GraphQl.Types
{
    public class CategoryType:ObjectGraphType<Category>
    {
        public CategoryType(){
            Name = "Category";
            Field(x => x.id);
            Field(x => x.name);
        }
    }
}