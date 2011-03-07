using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using PAM.Core.Implementation.ApplicationImp;

namespace PAM.Core.Implementation.Monitor
{
    public class AppMonitorPersister
    {
        private readonly Applications _applications;
        private const string DataFile = "PersonalActivityMonitorData.xml";

        public AppMonitorPersister(Applications applications)
        {
            _applications = applications;
        }

        public void Save()
        {
            var file = IsolatedStorage.CreateFile(DataFile);

            var serializer = new XmlSerializer(_applications.GetType());
                        
            serializer.Serialize(file, _applications);
            
            file.Close();
        }

        public Applications Restore()
        {
            var deserializer = new XmlSerializer(_applications.GetType());
            var file = IsolatedStorage.OpenFile(DataFile,FileMode.Open,FileAccess.Read);
            var result = deserializer.Deserialize(file);
            file.Close();
            return (Applications) result;

        }


        private IsolatedStorageFile _isolatedStorage;
        private IsolatedStorageFile IsolatedStorage
        {
            get
            {
                return _isolatedStorage ??
                       (_isolatedStorage =
                        IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null,
                                                     null));
            }
        }
    }
}
