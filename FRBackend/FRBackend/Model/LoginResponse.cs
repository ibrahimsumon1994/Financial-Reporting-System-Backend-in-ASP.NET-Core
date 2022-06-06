using DotNetCoreScaffoldingSqlServer.Entities;
using System;
using System.Collections.Generic;

namespace FRBackend.Model
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string EmployeeId { get; set; }
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public byte[] Picture { get; set; }
        public byte[] Signature { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public bool IsActive { get; set; }
        public long? GroupId { get; set; }
        public long? UnitId { get; set; }
        public long? DepartmentId { get; set; }
        public long? DesignationId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public List<MenuProcessing> NavigationList { get; set; }
        public List<MenuPermission> MenuList { get; set; }
    }
}
