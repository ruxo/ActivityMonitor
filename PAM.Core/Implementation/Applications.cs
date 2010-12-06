using System.Collections.Generic;
using System.Linq;

namespace PAM.Core
{
    public class Applications : List<Application>
    {

        public bool Contains(string application)
        {
            return (from app in this
                    where app.Name == application
                    select app).FirstOrDefault() != null;
        }

        public bool Contains(string application, string path)
        {
            return (from app in this
                    where app.Name == application && app.Path == path
                    select app).FirstOrDefault() != null;
        }




        public IApplication this[string applicationName]
        {
            get
            {
                return (from app in this
                        where app.Name == applicationName
                        select app).FirstOrDefault();
            }

        }
    }
}