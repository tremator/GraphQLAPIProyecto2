namespace ProyectoWeb2GraphQLApi.Models
{
    public class User
    {
        public long id {get; set;}
        public string email { get; set; }
        public string password { get; set; }    
        public string firstName { get; set; }
        public string LastName { get; set; }
        public long roleId { get; set; }
        public Role role { get; set; }
        public string token { get; set; }
        public string authCode { get; set; }
        public bool isLogged { get; set; }
        public string phone { get; set; }
        public bool registerConfirmation { get; set; }
    }
}