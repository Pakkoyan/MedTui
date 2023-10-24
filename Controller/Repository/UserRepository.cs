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
    }
    
    public void AddUser(User user)
    {
        using (var db = new LiteDatabase(_connectionString))
        {
            var collection = db.GetCollection<User>("users");
            collection.Insert(user);
        }
    }

    public void RemoveUser(ObjectId userId)
    {
        using (var db = new LiteDatabase(_connectionString))
        {
            var collection = db.GetCollection<User>("users");
            collection.Delete(userId);
        }
    }

    public User FindUserById(ObjectId userId)
    {
        using (var db = new LiteDatabase(_connectionString))
        {
            var collection = db.GetCollection<User>("users");
            return collection.FindById(userId);
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