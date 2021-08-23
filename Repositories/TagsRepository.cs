using ProyectoWeb2GraphQLApi.Models;

namespace ProyectoWeb2GraphQLApi.Repositories
{
    public class TagsRepository
    {
         private DatabaseContext _context;

        public TagsRepository(DatabaseContext context){
            _context = context;
        }

        public User GetUser(long id){
            var tag = _context.Tags.Find(id);
            var user = _context.Users.Find(tag.userId);
            return user;
        }
    }
}