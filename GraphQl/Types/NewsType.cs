using GraphQL.Types;
using ProyectoWeb2GraphQLApi.Models;
using ProyectoWeb2GraphQLApi.Repositories;

namespace ProyectoWeb2GraphQLApi.GraphQl.Types
{
    public class NewsType:ObjectGraphType<News>
    {
        public NewsType(NewsRepository repository){
            Field(x => x.title);
            Field(x => x.id);
            Field(x => x.description);
            Field(x => x.link);
            Field(x => x.date);
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
            Field<NewsSourceType>(
                "Source",
                resolve: context => {
                    return repository.GetSource(context.Source.id);
                }
            );
            Field<ListGraphType<StringGraphType>>(
                "Tags",
                resolve: context => {
                    return repository.getTags(context.Source.id);
                }
            );
            
        }
        
    }
}