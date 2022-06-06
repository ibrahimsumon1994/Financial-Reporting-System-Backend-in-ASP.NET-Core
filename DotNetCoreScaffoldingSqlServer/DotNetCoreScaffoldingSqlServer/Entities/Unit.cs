using System;
using System.Collections.Generic;

#nullable disable

namespace DotNetCoreScaffoldingSqlServer.Entities
{
    public partial class Unit
    {
        public Unit()
        {
            Departments = new HashSet<Department>();
            Transactions = new HashSet<Transaction>();
            UserWiseUnitPermissions = new HashSet<UserWiseUnitPermission>();
            Users = new HashSet<User>();
        }

        public long UnitId { get; set; }
        public string NameBangla { get; set; }
        public string NameEnglish { get; set; }
        public string Recstatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? GroupId { get; set; }

        public virtual Group Group { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<UserWiseUnitPermission> UserWiseUnitPermissions { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
