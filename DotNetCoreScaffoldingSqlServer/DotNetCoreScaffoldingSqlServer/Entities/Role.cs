using System;
using System.Collections.Generic;

#nullable disable

namespace DotNetCoreScaffoldingSqlServer.Entities
{
    public partial class Role
    {
        public Role()
        {
            RoleAssigns = new HashSet<RoleAssign>();
            RoleWiseMenuAssigns = new HashSet<RoleWiseMenuAssign>();
        }

        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public string Purpose { get; set; }
        public string Recstatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<RoleAssign> RoleAssigns { get; set; }
        public virtual ICollection<RoleWiseMenuAssign> RoleWiseMenuAssigns { get; set; }
    }
}
