namespace AuthenticationServer.Domain.Entities.UserEntities
{
    //To avoid repeating properties rquired for each Entity
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
