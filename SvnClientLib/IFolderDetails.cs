namespace SvnClientLib
{
    public interface IFolderDetails
    {
        string ObservedFolder { get; }

        string TargetFileName { get; }

        string SvnTempFolder { get; }
    }
}