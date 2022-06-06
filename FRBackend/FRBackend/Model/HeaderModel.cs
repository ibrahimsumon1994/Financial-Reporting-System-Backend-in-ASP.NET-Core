using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Model
{
    public class HeaderModel
    {
        public long HeaderId { get; set; }
        public string HeaderName { get; set; }
        public long? HeaderTypeId { get; set; }
        public string HeaderType { get; set; }
        public int? HeaderLayer { get; set; }
        public string HeaderCode { get; set; }
        public long? ParentHeaderId { get; set; }
        public string ParentHeader { get; set; }
        public string Recstatus { get; set; }
        public string Remarks { get; set; }
    }
}
