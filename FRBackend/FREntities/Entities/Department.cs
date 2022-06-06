using System;
using System.Collections.Generic;

#nullable disable

namespace DotNetCoreScaffoldingSqlServer.Entities
{
    public partial class Department
    {
        public Department()
        {
            CommonCodes = new HashSet<CommonCode>();
            Designations = new HashSet<Designation>();
            Users = new HashSet<User>();
        }

        public long DepartmentId { get; set; }
        public string NameBangla { get; set; }
        public string NameEnglish { get; set; }
        public string Recstatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UnitId { get; set; }
        public long? GroupId { get; set; }

        public virtual Group Group { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual ICollection<CommonCode> CommonCodes { get; set; }
        public virtual ICollection<Designation> Designations { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
