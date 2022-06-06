using System;
using System.Collections.Generic;

#nullable disable

namespace DotNetCoreScaffoldingSqlServer.Entities
{
    public partial class Menu
    {
        public Menu()
        {
            RoleWiseMenuAssigns = new HashSet<RoleWiseMenuAssign>();
        }

        public long MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuLocation { get; set; }
        public string ReportPath { get; set; }
        public long ParentId { get; set; }
        public string MenuIcon { get; set; }
        public int? MenuSequence { get; set; }
        public string Recstatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? MenuTypeId { get; set; }

        public virtual CommonCode MenuType { get; set; }
        public virtual ICollection<RoleWiseMenuAssign> RoleWiseMenuAssigns { get; set; }
    }
}
