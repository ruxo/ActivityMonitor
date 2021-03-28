using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace PAM.Utils.VersionChecking
{
    public static class VersionChecker
    {
        const string WebRequestAddress = "http://activitymonitor.codeplex.com/wikipage?title=Latest%20application%20version";
        const string HtmlTagB    = "<b>";
        const string HtmlEndTagB = "</b>";

        public static VersionInfo? GetLatestVersionInfo()
        {
            try
            {
                var request        = (HttpWebRequest)WebRequest.Create(WebRequestAddress);
                var response       = request.GetResponse();
                var responseStream = response.GetResponseStream();
                var objReader      = new StreamReader(responseStream);

                var sLine = string.Empty;
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null && sLine.Contains(@"<div class=""wikidoc"">"))
                        return Extract(sLine);
                }
            }
            catch (WebException e)
            {
                Trace.WriteLine($"Version checking failed.. {e.Message}");
            }
            return null;
        }

        static VersionInfo Extract(string text)
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