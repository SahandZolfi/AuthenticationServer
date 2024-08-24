using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Core.Services
{
    public static class DateTimeService
    {
        public static bool IsExpired(this DateTime specificDate)
        {
            return specificDate < DateTime.Now;
        }
    }
}
