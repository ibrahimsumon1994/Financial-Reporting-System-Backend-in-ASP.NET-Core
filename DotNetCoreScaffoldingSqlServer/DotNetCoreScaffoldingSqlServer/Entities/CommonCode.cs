using System;
using System.Collections.Generic;

#nullable disable

namespace DotNetCoreScaffoldingSqlServer.Entities
{
    public partial class CommonCode
    {
        public CommonCode()
        {
            Headers = new HashSet<Header>();
            Menus = new HashSet<Menu>();
            TransactionHeaderTypes = new HashSet<Transaction>();
            TransactionTransactionTypes = new HashSet<Transaction>();
            TransactionVersions = new HashSet<Transaction>();
        }

        public long CommonCodeId { get; set; }
        public string NameBangla { get; set; }
        public string NameEnglish { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Recstatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Header> Headers { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
        public virtual ICollection<Transaction> TransactionHeaderTypes { get; set; }
        public virtual ICollection<Transaction> TransactionTransactionTypes { get; set; }
        public virtual ICollection<Transaction> TransactionVersions { get; set; }
    }
}
