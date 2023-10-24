using System.Collections.Immutable;
using Attachment.Model;
using Controller.Interfaces;
using LiteDB;

namespace Controller.Repository;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
        using (var db = new LiteDatabase(_connectionString))
        {
            var collection = db.GetCollection<User>("users");
            collection.EnsureIndex(x => x.Login, unique: true);
        }
    }
    
    public User AddUser(User user)
    {
        using (var db = new LiteDatabase(_connectionString))
        {
            var collection = db.GetCollection<User>("users");
            collection.Insert(user);
            return collection.FindOne(x => x.Login == user.Login);
        }
    }

    public void RemoveUser(string login)
    {
        using (var db = new LiteDatabase(_connectionString))
        {
            var collection = db.GetCollection<User>("users");
            collection.DeleteMany(x => x.Login == login);
        }
    }

    public User FindUserByLogin(string login)
    {
        using (var db = new LiteDatabase(_connectionString))
        {
            var collection = db.GetCollection<User>("users");
            return collection.FindOne(x => x.Login == login);
        }
    }

    public ImmutableList<User> GetAllUsers()
    {
        using (var db = new LiteDatabase(_connectionString))
        {
            var collection = db.GetCollection<User>("users");
            return ImmutableList.CreateRange(collection.FindAll());
        }
    }
}