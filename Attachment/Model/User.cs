using LiteDB;

namespace Attachment.Model;

public class User
{
    public ObjectId Id { get; set; }
    private readonly string _phasePhrase = "груша"; 
    private string _privilege = "user";
    public string Login { get; set; }
    public string Password { get; set; }
    
    public User(string login, string password, string phasePhrase)
    {
        Login = login;
        Password = password;
        if (_phasePhrase == phasePhrase.ToLower())
        {
            _privilege = "admin";
        }
    }
}