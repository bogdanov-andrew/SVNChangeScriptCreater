namespace SvnClientLib
{
    public class FolderDetails : IFolderDetails
    {
        private readonly string _observedFolder;
        private readonly string _targetFileName;
        private readonly string _svnTempFolder;

        public FolderDetails(string observedFolder, string targetFileName, string svnTempFolder)
        {
            _observedFolder = observedFolder;
            _targetFileName = targetFileName;
            _svnTempFolder = svnTempFolder;
        }

        public string ObservedFolder
        {
            get { return _observedFolder; }
        }

        public string TargetFileName
        {
            get { return _targetFileName; }
        }

        public string SvnTempFolder
        {
            get { return _svnTempFolder; }
        }
    }
}