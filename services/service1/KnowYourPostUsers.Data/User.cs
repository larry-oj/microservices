using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KnowYourPostUsers.Data;

public class User
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
    public string Email { get; set; }       // is unique
    public string Password { get; set; }    // forbidden to store unhashed, never do this in real-life scenarios

    public User(string email, string password)
    {
        Email = email;
        Password = password;
    }
}