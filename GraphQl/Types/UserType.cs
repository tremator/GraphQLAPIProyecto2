using GraphQL.Types;
using ProyectoWeb2GraphQLApi.Models;
using ProyectoWeb2GraphQLApi.Repositories;

namespace ProyectoWeb2GraphQLApi.GraphQl.Types
{
    public class UserType:ObjectGraphType<User>
    {
        public UserType(UserRepository repositorie){
            Name = "User";
            Field(x => x.id);
            Field(x => x.firstName);
            Field(x => x.LastName);
            Field(x => x.email);
            Field(x => x.password);
            Field<RoleType>(
                "Role",
                resolve:context => {
                    return repositorie.GetRole(context.Source.id);
                }
            );
            Field(x => x.token);
            
        }
    }
}