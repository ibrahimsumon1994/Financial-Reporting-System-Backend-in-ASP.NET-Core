using System;
using System.Collections.Generic;

#nullable disable

namespace DotNetCoreScaffoldingSqlServer.Entities
{
    public partial class Header
    {
        public Header()
        {
            TransactionFirstHeaders = new HashSet<Transaction>();
            TransactionSecondHeaders = new HashSet<Transaction>();
            TransactionThirdHeaders = new HashSet<Transaction>();
        }

        public long HeaderId { get; set; }
        public string HeaderName { get; set; }
        public long? HeaderTypeId { get; set; }
        public int? HeaderLayer { get; set; }
        public string HeaderCode { get; set; }
        public long? ParentHeaderId { get; set; }
        public string Recstatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Remarks { get; set; }

        public virtual CommonCode HeaderType { get; set; }
        public virtual ICollection<Transaction> TransactionFirstHeaders { get; set; }
        public virtual ICollection<Transaction> TransactionSecondHeaders { get; set; }
        public virtual ICollection<Transaction> TransactionThirdHeaders { get; set; }
    }
}
