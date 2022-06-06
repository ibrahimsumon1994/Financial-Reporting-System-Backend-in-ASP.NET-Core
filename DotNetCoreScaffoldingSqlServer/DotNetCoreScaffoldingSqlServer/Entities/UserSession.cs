using System;
using System.Collections.Generic;

#nullable disable

namespace DotNetCoreScaffoldingSqlServer.Entities
{
    public partial class UserSession
    {
        public long SessionId { get; set; }
        public int? UserId { get; set; }
        public DateTime? LoginDateTime { get; set; }
        public DateTime? LogoutDateTime { get; set; }
        public string Recstatus { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }

        public virtual User User { get; set; }
    }
}
