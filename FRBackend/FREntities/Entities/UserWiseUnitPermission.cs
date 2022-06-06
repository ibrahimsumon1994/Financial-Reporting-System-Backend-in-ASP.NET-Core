using System;
using System.Collections.Generic;

#nullable disable

namespace DotNetCoreScaffoldingSqlServer.Entities
{
    public partial class UserWiseUnitPermission
    {
        public long UnitPermissionId { get; set; }
        public int? UserId { get; set; }
        public long? UnitId { get; set; }
        public string Recstatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Unit Unit { get; set; }
        public virtual User User { get; set; }
    }
}
