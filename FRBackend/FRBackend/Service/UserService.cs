using Common.Helpers;
using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Helpers;
using FRBackend.Interface;
using FRBackend.Model;
using FRBackend.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Service
{
    //public class AppSettingsFile
    //{
    //    public string DefaultPassword { get; set; }
    //}
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IUserSessionRepository _userSessionRepository;
        public UserService(IUserRepository userRepository, IUserSessionRepository userSessionRepository)
        {
            _userRepository = userRepository;
            _userSessionRepository = userSessionRepository;
        }
        public LoginResponse Login(LoginRequest loginRequest, out string message)
        {
            var saltTest = HashingHelper.GenerateSalt();
            var passwordHashTest = HashingHelper.HashUsingPbkdf2(loginRequest.Password, saltTest);
            var user = _userRepository.AsQueryable().FirstOrDefault(x => x.IsActive && x.UserId == loginRequest.UserId);
            if (user == null)
            {
                message = MessageConst.InvalidUserId;
                return null;
            }
            var salt = HashingHelper.GenerateSalt();
            var passwordHash = HashingHelper.HashUsingPbkdf2(loginRequest.Password, user.PasswordSalt);
            if (user.Password != passwordHash)
            {
                message = MessageConst.InvalidPassword;
                return null;
            }
            var existUserSessions = _userSessionRepository.AsQueryable().Where(x => x.UserId == user.Id && x.Recstatus == ActionConst.Authorize).AsParallel().ToList();
            foreach (var existUserSession in existUserSessions)
            {
                var getSession = _userSessionRepository.AsQueryable().FirstOrDefault(x => x.SessionId == existUserSession.SessionId);
                if (getSession != null)
                {
                    getSession.Recstatus = ActionConst.Cancel;
                    _userSessionRepository.Update(getSession);
                }
            }
            var userSession = new UserSession();
            userSession.UserId = user.Id;
            userSession.IpAddress = loginRequest.IpAddress;
            userSession.MacAddress = loginRequest.MacAddress;
            userSession.Recstatus = ActionConst.Authorize;
            userSession.LoginDateTime = DateTime.Now;
            _userSessionRepository.Add(userSession);

            var token = TokenHelper.GenerateToken(user);
            var refreshToken = TokenHelper.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddHours(4);
            _userRepository.Update(user);
            var obj = new LoginResponse
            {
                Id = user.Id,
                UserId = user.UserId,
                EmployeeId = user.EmployeeId,
                FullName = user.FullName,
                MobileNo = user.MobileNo,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                Picture = user.Picture,
                Signature = user.Signature,
                PresentAddress = user.PresentAddress,
                PermanentAddress = user.PermanentAddress,
                IsActive = user.IsActive,
                GroupId = user.GroupId,
                UnitId = user.UnitId,
                DepartmentId = user.DepartmentId,
                DesignationId = user.DesignationId,
                Token = token,
                RefreshToken = refreshToken
            };
            message = MessageConst.SuccussLogin;
            return obj;
        }

        public LoginResponse RefreshToken(Token token, out string message)
        {
            message = "";
            var principal = TokenHelper.GetPrincipalFromExpiredToken(token.AccessToken);
            var userId = principal.Claims.FirstOrDefault().Value; //this is mapped to the Name claim by default
            var checkUserSession = _userSessionRepository.AsQueryable().FirstOrDefault(x => x.UserId == Convert.ToInt32(userId) && x.Recstatus == ActionConst.Authorize);
            if (checkUserSession == null)
            {
                message = MessageConst.StopSession;
                return null;
            }
            var user = _userRepository.AsQueryable().FirstOrDefault(x => x.IsActive && x.Id == Convert.ToInt32(userId));
            if (user == null)
            {
                message = MessageConst.InvalidUserId;
                return null;
            }
            if (user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                message = MessageConst.InvalidRefreshToken;
                return null;
            }
            var newAccessToken = TokenHelper.GenerateToken(user);
            var newRefreshToken = TokenHelper.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            _userRepository.Update(user);
            var obj = new LoginResponse
            {
                UserId = user.UserId,
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            };
            return obj;
        }

        public bool Revoke(string userId)
        {
            var user = _userRepository.AsQueryable().FirstOrDefault(u => u.Id == Convert.ToInt32(userId));
            if (user == null) 
                return false;
            user.RefreshToken = null;
            _userRepository.Update(user);
            var userSession = _userSessionRepository.AsQueryable().FirstOrDefault(x => x.UserId == Convert.ToInt32(userId) && x.Recstatus == ActionConst.Authorize);
            if (userSession != null)
            {
                userSession.Recstatus = ActionConst.Cancel;
                userSession.LogoutDateTime = DateTime.Now;
                _userSessionRepository.Update(userSession);
            }
            return true;
        }

        public bool SignUp(User user)
        {
            //using StreamReader r = new StreamReader("appsettings.json");
            //string json = r.ReadToEnd();
            //var item = JsonConvert.DeserializeObject<AppSettingsFile>(json);
            var salt = HashingHelper.GenerateSalt();
            var passwordHash = HashingHelper.HashUsingPbkdf2(user.Password, salt);
            user.Password = passwordHash;
            user.PasswordSalt = salt;
            user.CreatedDate = DateTime.Now;
            user.IsActive = true;
            _userRepository.Add(user);
            return true;
        }

        public List<User> GetAll(PagingParam pagingParam, out int total)
        {
            var data = new List<User>();
            if (string.IsNullOrEmpty(pagingParam.SearchString))
            {
                data = _userRepository.AsQueryable().Include(x => x.Designation).Include(x => x.Group).Include(x => x.Unit).Include(x => x.Department)
                        .OrderByDescending(o => o.UserId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _userRepository.AsQueryable().Count();
            }
            else
            {
                data = _userRepository.AsQueryable().Include(x => x.Designation).Include(x => x.Group).Include(x => x.Unit).Include(x => x.Department)
                        .Where(x => x.FullName.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) 
                        || x.UserId.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) 
                        || x.EmployeeId.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) 
                        || x.Designation.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())
                        || x.Group.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())
                        || x.Unit.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())
                        || x.Department.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())
                        ).OrderByDescending(o => o.UserId).Skip(pagingParam.Skip).Take(pagingParam.PageSize).ToList();
                total = _userRepository.AsQueryable().Include(x => x.Designation).Include(x => x.Group).Include(x => x.Unit).Include(x => x.Department)
                        .Where(x => x.FullName.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) 
                        || x.UserId.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower()) 
                        || x.EmployeeId.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())
                        || x.Designation.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())
                        || x.Group.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())
                        || x.Unit.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())
                        || x.Department.NameEnglish.Trim().ToLower().Contains(pagingParam.SearchString.Trim().ToLower())
                        ).Count();
            }
            return data;
        }

        public bool Update(User user)
        {
            var updateUser = _userRepository.AsQueryable().FirstOrDefault(x => x.UserId == user.UserId);
            if (updateUser != null)
            {
                updateUser.FullName = user.FullName;
                updateUser.MobileNo = user.MobileNo;
                updateUser.EmployeeId = user.EmployeeId;
                updateUser.Email = user.Email;
                updateUser.DateOfBirth = user.DateOfBirth;
                updateUser.Picture = user.Picture;
                updateUser.Signature = user.Signature;
                updateUser.PresentAddress = user.PresentAddress;
                updateUser.PermanentAddress = user.PermanentAddress;
                updateUser.DesignationId = user.DesignationId;
                updateUser.GroupId = user.GroupId;
                updateUser.UnitId = user.UnitId;
                updateUser.DepartmentId = user.DepartmentId;
                updateUser.UpdatedBy = user.UpdatedBy;
                updateUser.UpdatedDate = DateTime.Now;
                _userRepository.Update(updateUser);
                return true;
            }
            else
            {
                return false;
            }
        }

        public User GetById(string userId)
        {
            return _userRepository.AsQueryable().FirstOrDefault(x => x.UserId == userId);
        }

        public bool ActivateUser(User user)
        {
            var updateUser = _userRepository.AsQueryable().FirstOrDefault(x => x.UserId == user.UserId);
            if (updateUser != null)
            {
                updateUser.IsActive = true;
                updateUser.UpdatedBy = user.UpdatedBy;
                updateUser.UpdatedDate = DateTime.Now;
                _userRepository.Update(updateUser);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeactivateUser(User user)
        {
            var updateUser = _userRepository.AsQueryable().FirstOrDefault(x => x.UserId == user.UserId);
            if (updateUser != null)
            {
                updateUser.IsActive = false;
                updateUser.UpdatedBy = user.UpdatedBy;
                updateUser.UpdatedDate = DateTime.Now;
                _userRepository.Update(updateUser);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CurrentPasswordMatcher(string userId, string currentPassword)
        {
            var user = _userRepository.AsQueryable().FirstOrDefault(x => x.Id == Convert.ToInt32(userId));
            if (user == null)
            {
                return false;
            }
            var passwordHash = HashingHelper.HashUsingPbkdf2(currentPassword, user.PasswordSalt);

            if (user.Password != passwordHash)
            {
                return false;
            }
            return true;
        }

        public bool ChangePassword(string userId, string newPassword)
        {
            var user = _userRepository.AsQueryable().FirstOrDefault(x => x.Id == Convert.ToInt32(userId));
            if (user == null)
            {
                return false;
            }
            var salt = HashingHelper.GenerateSalt();
            var passwordHash = HashingHelper.HashUsingPbkdf2(newPassword, salt);
            user.Password = passwordHash;
            user.PasswordSalt = salt;
            _userRepository.Update(user);
            return true;
        }

        public List<User> GetAllActiveUserForDropdown()
        {
            var data = new List<User>();
            data = _userRepository.AsQueryable().Where(x => x.IsActive == true)
                .Select(x => new User() 
                {
                    Id = x.Id,
                    FullName = x.FullName + " (" + x.UserId + ")"
                })
                .OrderBy(o => o.FullName).ToList();
            return data;
        }
    }
}
