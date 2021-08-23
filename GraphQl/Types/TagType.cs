using GraphQL.Types;
using ProyectoWeb2GraphQLApi.Models;
using ProyectoWeb2GraphQLApi.Repositories;

namespace ProyectoWeb2GraphQLApi.GraphQl.Types
{
    public class TagType: ObjectGraphType<Tags>
    {

        public TagType(TagsRepository repository){
            Name = "Tag";
            Field(x => x.id);
            Field(x => x.tag);
            Field<UserType>(
                Name =  "User",
                resolve: context =>{
                    return repository.GetUser(context.Source.id);
                }
            );
        }
        
    }
}