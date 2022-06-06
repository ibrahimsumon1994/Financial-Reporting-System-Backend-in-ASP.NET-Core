using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Model
{
    public class MenuModel
    {
        public long MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuLocation { get; set; }
        public string ReportPath { get; set; }
        public long ParentId { get; set; }
        public string ParentName { get; set; }
        public string MenuIcon { get; set; }
        public int? MenuSequence { get; set; }
        public long? MenuTypeId { get; set; }
        public string MenuType { get; set; }
        public string Recstatus { get; set; }
    }

    public class MenuProcessing
    {
        public MenuProcessing()
        {
            children = new List<MenuProcessing>();
        }
        public long MenuId { get; set; }
        public string MenuName { get; set; }
        public long ParentId { get; set; }
        public string MenuLocation { get; set; }
        public int? MenuSequence { get; set; }
        public string MenuIcon { get; set; }
        public long? RoleId { get; set; }
        public string UserId { get; set; }
        public string CreateYn { get; set; }
        public string EditYn { get; set; }
        public string DeleteYn { get; set; }
        public string ViewDetailYn { get; set; }
        public string AuthYn { get; set; }
        public List<MenuProcessing> children { get; set; }
    }
}
