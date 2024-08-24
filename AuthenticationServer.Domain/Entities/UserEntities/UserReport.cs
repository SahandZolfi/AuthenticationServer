using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Domain.Entities.UserEntities
{
    public class UserReport : BaseEntity
    {
        public string Text { get; set; }

        public User ReportedByUser { get; set; }
        public long ReporterByUserId { get; set; }

        public User Reporting { get; set; }
        public long ReportingId { get; set; }
    }
}
