using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using PAM.Core.Implementation.ApplicationImp;

namespace PAM.Utils.Export
{
    internal class DataExporter
    {
        readonly Applications _data;

        public DataExporter(Applications data)
        {
            _data = data;
        }

        public void SaveToXml(Stream resultFile)
        {
            var outputXml = new XDocument(
                new XElement("Applications", new XAttribute("ApplicationVersion",  Assembly.GetExecutingAssembly().GetName().Version!),
                                             new XAttribute("ExportVersion", "1.0"),
                                             from app in _data
                                             select new XElement("Application",
                                                                 new XAttribute("Name", app.Name),
                                                                 new XAttribute("TotalUsage", app.TotalUsageTime),
                                                                 new XElement("Usages", from usage in app.Usage
                                                                                        where usage.IsClosed
                                                                                        select new XElement("Usage",
                                                                                            new XAttribute("Started", usage.BeginTime),
                                                                                            new XAttribute("Ended", usage.EndTime),
                                                                                            new XAttribute("Total", usage.Total)
                                                                                            )
                                                                             )
                                                 )
                    )
                );
            
            outputXml.Save(resultFile);
            resultFile.Flush();
            resultFile.Close();
        }
    }
}