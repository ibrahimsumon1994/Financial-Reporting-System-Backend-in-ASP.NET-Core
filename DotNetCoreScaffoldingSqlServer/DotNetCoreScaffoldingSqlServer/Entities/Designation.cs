using System;
using System.Collections.Generic;

#nullable disable

namespace DotNetCoreScaffoldingSqlServer.Entities
{
    public partial class Designation
    {
        public Designation()
        {
            Users = new HashSet<User>();
        }

        public long DesignationId { get; set; }
        public string NameBangla { get; set; }
        public string NameEnglish { get; set; }
        public string Recstatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
