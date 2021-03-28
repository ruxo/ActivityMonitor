using System;
using System.Collections.Generic;
using System.Linq;

namespace PAM.Core.Implementation.ApplicationImp
{
    public class ApplicationUsages : List<ApplicationUsage>
    {
        public TimeSpan TotalUsageTime()
        {

            return this.Aggregate(TimeSpan.Zero, (subtotal,
                                                  t) => subtotal.Add(t.Total));

        }

    }
}