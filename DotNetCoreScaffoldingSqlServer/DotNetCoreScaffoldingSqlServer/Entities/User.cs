using System;
using System.Collections.Generic;

#nullable disable

namespace DotNetCoreScaffoldingSqlServer.Entities
{
    public partial class User
    {
        public User()
        {
            RoleAssigns = new HashSet<RoleAssign>();
            UserSessions = new HashSet<UserSession>();
            UserWiseUnitPermissions = new HashSet<UserWiseUnitPermission>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string EmployeeId { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public byte[] Picture { get; set; }
        public byte[] Signature { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? GroupId { get; set; }
        public long? UnitId { get; set; }
        public long? DepartmentId { get; set; }
        public long? DesignationId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public virtual Department Department { get; set; }
        public virtual Designation Designation { get; set; }
        public virtual Group Group { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual ICollection<RoleAssign> RoleAssigns { get; set; }
        public virtual ICollection<UserSession> UserSessions { get; set; }
        public virtual ICollection<UserWiseUnitPermission> UserWiseUnitPermissions { get; set; }
    }
}
