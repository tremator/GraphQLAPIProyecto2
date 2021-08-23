using GraphQL.Types;
using ProyectoWeb2GraphQLApi.Models;
using ProyectoWeb2GraphQLApi.Repositories;

namespace ProyectoWeb2GraphQLApi.GraphQl.Types
{
    public class NewsSourceType:ObjectGraphType<NewsSource>
    {
        public NewsSourceType(NewsSourcesRepository repository){
            Field(x => x.id);
            Field(x => x.name);
            Field(x => x.url);
            Field<CategoryType>(
                "Category",
                resolve: context => {
                    return repository.GetCategory(context.Source.id);
                }
            );
            Field<UserType>(
                "User",
                resolve: context => {
                    return repository.GetUser(context.Source.id);
                }
            );
        }
        
    }
}