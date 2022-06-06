using System;
using System.Collections.Generic;

#nullable disable

namespace DotNetCoreScaffoldingSqlServer.Entities
{
    public partial class RoleWiseMenuAssign
    {
        public long RoleWiseMenuAssignId { get; set; }
        public long? RoleId { get; set; }
        public long? MenuId { get; set; }
        public string CreateYn { get; set; }
        public string EditYn { get; set; }
        public string ViewDetailYn { get; set; }
        public string DeleteYn { get; set; }
        public string AuthYn { get; set; }
        public string Recstatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual Role Role { get; set; }
    }
}
