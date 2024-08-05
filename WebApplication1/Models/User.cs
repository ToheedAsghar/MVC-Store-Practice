namespace WebApplication1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        public User(int id, string? userName, string? password)
        {
            Id = id;
            Username = userName;
            Password = password;
        }
    }
}
