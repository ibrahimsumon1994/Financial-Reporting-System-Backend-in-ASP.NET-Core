using System;
using System.Collections.Generic;

#nullable disable

namespace DotNetCoreScaffoldingSqlServer.Entities
{
    public partial class MenuPermission
    {
        public long MenuId { get; set; }
        public string MenuName { get; set; }
        public long ParentId { get; set; }
        public string MenuLocation { get; set; }
        public int? MenuSequence { get; set; }
        public string MenuIcon { get; set; }
        public long? RoleId { get; set; }
        public int? Id { get; set; }
        public string UserId { get; set; }
        public string CreateYn { get; set; }
        public string EditYn { get; set; }
        public string DeleteYn { get; set; }
        public string ViewDetailYn { get; set; }
        public string AuthYn { get; set; }
    }
}
