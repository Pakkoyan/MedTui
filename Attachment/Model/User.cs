using LiteDB;

namespace Attachment.Model;

public class User
{
    public ObjectId Id { get; set; }
    private readonly string _phasePhrase = "лампочка"; 
    public string _privilege { get; private set; }
    public string Login { get; set; }
    public string Password { get; set; }
    
    // Я оказывается узнал что есть такая штука как десириализация и LiteDB пытается вызвать конструктор с 3 аргументами 
    // При поиске обьекта, из-за чего и происходит ошибка, по этому надо создать конструктор для десирализации обьекта из дб
    public static User DeserializeConstructor(string login, string password, string privilege)
    {
        var DeserializeUser = new User();
        DeserializeUser.Login = login;
        DeserializeUser.Password = password;
        DeserializeUser._privilege = privilege;
        return DeserializeUser;
    }
    public User(string login, string password, string phasePhrase)
    {
        Login = login;
        Password = password;
        if (_phasePhrase == phasePhrase.ToLower())
        {
            _privilege = "admin";
            return;
        }

        _privilege = "user";
    }

    public User()
    {
        
    }
}