namespace AuthenticationServer.Domain.Entities.UserEntities
{
    public class Role
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual IList<User> UsersWithThisRole { get; set; }
    }
}
