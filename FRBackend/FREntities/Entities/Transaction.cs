using System;
using System.Collections.Generic;

#nullable disable

namespace DotNetCoreScaffoldingSqlServer.Entities
{
    public partial class Transaction
    {
        public long TransactionId { get; set; }
        public long? TransactionTypeId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public long? VersionId { get; set; }
        public long? GroupId { get; set; }
        public long? UnitId { get; set; }
        public long? FirstHeaderId { get; set; }
        public long? SecondHeaderId { get; set; }
        public long? ThirdHeaderId { get; set; }
        public decimal? Value { get; set; }
        public string Remarks { get; set; }
        public string Recstatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? HeaderTypeId { get; set; }
        public int? NumberOfUpdate { get; set; }

        public virtual Header FirstHeader { get; set; }
        public virtual Group Group { get; set; }
        public virtual CommonCode HeaderType { get; set; }
        public virtual Header SecondHeader { get; set; }
        public virtual Header ThirdHeader { get; set; }
        public virtual CommonCode TransactionType { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual CommonCode Version { get; set; }
    }
}
