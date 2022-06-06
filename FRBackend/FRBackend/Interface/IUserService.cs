using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Interface
{
    public interface IUserService
    {
        LoginResponse Login(LoginRequest loginRequest, out string message);
        LoginResponse RefreshToken(Token token, out string message);
        bool Revoke(string userId);
        bool SignUp(User user);
        List<User> GetAll(PagingParam param, out int total);
        bool Update(User user);
        User GetById(string userId);
        bool ActivateUser(User user);
        bool DeactivateUser(User user);
        bool CurrentPasswordMatcher(string userId, string currentPassword);
        bool ChangePassword(string userId, string newPassword);
        List<User> GetAllActiveUserForDropdown();
    }
}
