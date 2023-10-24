using System.Collections.Immutable;
using Attachment.Model;
using LiteDB;

namespace Controller.Interfaces;


public interface IUserRepository
{
    User AddUser(User user);
    void RemoveUser(string login);
    User FindUserByLogin(string login);
    ImmutableList<User> GetAllUsers(); 
}