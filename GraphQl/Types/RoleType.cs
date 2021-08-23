using GraphQL.Types;
using ProyectoWeb2GraphQLApi.Models;

namespace ProyectoWeb2GraphQLApi.GraphQl.Types
{
    public class RoleType:ObjectGraphType<Role>
    {
        public RoleType(){
            Name = "Role";
            Field(x => x.id);
            Field(x => x.name);
        }
    }
}