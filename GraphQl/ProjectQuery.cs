using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using ProyectoWeb2GraphQLApi.GraphQl.Types;
using ProyectoWeb2GraphQLApi.Repositories;


namespace ProyectoWeb2GraphQLApi.GraphQl
{
    public class ProjectQuery: ObjectGraphType
    {
        public ProjectQuery(NewsSourcesRepository sourcesRepository, NewsRepository newsRepository, UserRepository userRepository,IHttpContextAccessor accessor){
            Field<ListGraphType<NewsType>>(
                "charge",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType>{ Name = "input"}
                ),
                resolve: context => sourcesRepository.Charge(context.GetArgument<int>("input"), accessor.HttpContext)
            );
            Field<ListGraphType<NewsType>>(
                "search",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType>{Name = "id"},
                    new QueryArgument<StringGraphType>{Name = "word" }
                ),
                resolve: context => newsRepository.Search(context.GetArgument<int>("id"),context.GetArgument<string>("word"),accessor.HttpContext)
            );
            Field<ListGraphType<TagType>>(
                "userTags",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType>{Name = "id"}
                ),
                resolve:  context => userRepository.GetTags(context.GetArgument<int>("id"),accessor.HttpContext)
            );
            Field<ListGraphType<NewsType>>(
                "newsFilter",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType>{Name = "userId"},
                    new QueryArgument<StringGraphType>{Name = "tag"},
                    new QueryArgument<IntGraphType>{Name = "category"}
                ),
                resolve: context => newsRepository.Filter(context, accessor.HttpContext)
            );
        }
    }
}