using System.Collections.ObjectModel;
using System.Linq;
using PAM.Core.Abstract;
using PAM.Core.Implementation;
using PAM.Core.Implementation.Application;

namespace PAM.Core
{
    public class Applications : ObservableCollection<Application>
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

        public void Refresh()
        {
            foreach (var application in this) {
                application.Refresh();
            }
        }
    }
}