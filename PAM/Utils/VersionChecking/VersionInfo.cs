namespace PAM.Utils.VersionChecking
{
    public class VersionInfo
    {
        public VersionInfo()
        {
        }

        public VersionInfo(string version,
                             string releaseDate)
        {
            Version = version;
            ReleaseDate = releaseDate;
        }

        public string Version { get; private set; }
        public string ReleaseDate { get; private set; }
    }
}