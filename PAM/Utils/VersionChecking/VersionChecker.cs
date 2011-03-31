using System.IO;
using System.Net;

namespace PAM.Utils.VersionChecking
{
    public class VersionChecker
    {
        private const string WebRequestAddress =
            @"http://activitymonitor.codeplex.com/wikipage?title=Latest%20application%20version";

        private const string HtmlTagB = "<b>";
        private const string HtmlEndTagB = "</b>";

        public VersionInfo GetLatestVersionInfo()
        {
            var request = (HttpWebRequest)WebRequest.Create(WebRequestAddress);
            var response = request.GetResponse();
            var responseStream = response.GetResponseStream();
            if (responseStream == null) return null;
            var objReader = new StreamReader(responseStream);

            var sLine = "";
            var i = 0;

            while (sLine != null)
            {
                i++;
                sLine = objReader.ReadLine();
                if (sLine != null &&
                    sLine.Contains(@"<div class=""wikidoc"">"))
                {
                    return Extract(sLine);
                }

            }

            return null;
        }

        private static VersionInfo Extract(string text)
        {
            var verStartIndex = text.IndexOf(HtmlTagB, 0);
            var verEndIndex = text.IndexOf(HtmlEndTagB, verStartIndex);
            var releaseDateStartIndex = text.IndexOf(HtmlTagB, verEndIndex);
            var releaseDateEndIndex = text.IndexOf(HtmlEndTagB, releaseDateStartIndex);

            var version = text.Substring(verStartIndex + HtmlTagB.Length, verEndIndex - verStartIndex - HtmlTagB.Length);
            var releaseDate = text.Substring(releaseDateStartIndex + HtmlTagB.Length, releaseDateEndIndex - releaseDateStartIndex - HtmlTagB.Length);

            return new VersionInfo(version, releaseDate);

        }

    }
}