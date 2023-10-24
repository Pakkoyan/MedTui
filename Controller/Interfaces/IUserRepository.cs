using System.Collections.Immutable;
using Attachment.Model;
using LiteDB;

namespace Controller.Interfaces;


public interface IUserRepository
{
    void AddUser(User user);
    void RemoveUser(ObjectId userId);
    User FindUserById(ObjectId userId);
    ImmutableList<User> GetAllUsers(); 
}