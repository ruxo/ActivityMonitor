using System;
using System.Collections.Generic;
using System.Linq;

namespace PAM.Core
{
    public class ApplicationUsages : List<IApplicationUsage>
    {


        
        public TimeSpan TotalUsageTime()
        {

            return this.Aggregate(TimeSpan.Zero, (subtotal,
                                                  t) => subtotal.Add(t.Total));

        }

    }
}