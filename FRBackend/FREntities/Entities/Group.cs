using System;
using System.Collections.Generic;

#nullable disable

namespace DotNetCoreScaffoldingSqlServer.Entities
{
    public partial class Group
    {
        public Group()
        {
            Departments = new HashSet<Department>();
            Transactions = new HashSet<Transaction>();
            Units = new HashSet<Unit>();
            Users = new HashSet<User>();
        }

        public long GroupId { get; set; }
        public string NameBangla { get; set; }
        public string NameEnglish { get; set; }
        public string Recstatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Unit> Units { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
