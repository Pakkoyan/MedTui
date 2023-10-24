using System.Collections.Immutable;
using Attachment.Model;
using Controller.Interfaces;
using LiteDB;

namespace Controller.Repository;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;
    private readonly BsonMapper _mapper;

    public UserRepository(string connectionString)
    {
        _mapper = new BsonMapper();
        _mapper.RegisterType<User>(
            bson => new BsonDocument
            {
                ["Login"] = new BsonValue(bson.Login),
                ["Password"] = new BsonValue(bson.Password),
                ["_privilege"] = new BsonValue(bson._privilege)
            },
            bsonDoc =>
            {
                var login = bsonDoc["Login"].AsString;
                var password = bsonDoc["Password"].AsString;
                var privilege = bsonDoc["_privilege"].AsString;
                
                return User.DeserializeConstructor(login, password, privilege);
            }
        );
        
        _connectionString = connectionString;
        using (var db = new LiteDatabase(_connectionString,_mapper))
        {
            var collection = db.GetCollection<User>("users");
            collection.EnsureIndex(x => x.Login, unique: true);
        }
    }
    
    public User AddUser(User user)
    {
        using (var db = new LiteDatabase(_connectionString,_mapper))
        {
            var collection = db.GetCollection<User>("users");
            collection.Insert(user);
            return user;
        }
    }

    public void RemoveUser(string login)
    {
        using (var db = new LiteDatabase(_connectionString,_mapper))
        {
            var collection = db.GetCollection<User>("users");
            collection.DeleteMany(x => x.Login == login);
        }
    }

    public User FindUserByLogin(string login)
    {
        using (var db = new LiteDatabase(_connectionString,_mapper))
        {
            var collection = db.GetCollection<User>("users");
            return collection.FindOne(x => x.Login == login);
        }
    }

    public ImmutableList<User> GetAllUsers()
    {
        using (var db = new LiteDatabase(_connectionString,_mapper))
        {
            var collection = db.GetCollection<User>("users");
            return ImmutableList.CreateRange(collection.FindAll());
        }
    }
}