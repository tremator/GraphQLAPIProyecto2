using GraphQL;
using GraphQL.Types;

namespace ProyectoWeb2GraphQLApi.GraphQl
{

    public class ProjectSchema: Schema
    {
        public ProjectSchema(IDependencyResolver resolver): base(resolver){
            Query = resolver.Resolve<ProjectQuery>();
        }
    }

}