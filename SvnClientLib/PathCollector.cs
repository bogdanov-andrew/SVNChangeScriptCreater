using System.Collections.Generic;
using System.IO;
using SharpSvn;

namespace SvnClientLib
{
    public class PathCollector
    {
        private Dictionary<string, List<string>> _paths = new Dictionary<string, List<string>>();
        public PathCollector(IList<IFolderDetails> _observedFolders)
        {
            InitPaths(_observedFolders);
        }

        private void InitPaths(IList<IFolderDetails> observedFolders)
        {
            foreach (IFolderDetails folder in observedFolders)
            {
                string dirName = Path.GetDirectoryName(folder.ObservedFolder);
                if (!_paths.ContainsKey(dirName))
                {
                    _paths.Add(dirName,new List<string>());
                }
            }
        }

        public void Add(SvnChangeItem item)
        {
            string dirName = Path.GetDirectoryName(item.Path);
            if (string.IsNullOrEmpty(dirName))
            {
                return;
            }
            foreach (KeyValuePair<string, List<string>> keyValuePair in _paths)
            {
                if (dirName.Contains(keyValuePair.Key))
                {
                    keyValuePair.Value.Add(item.Path);
                }
            }
        }

        public IList<string> GetFilesByPath(string path)
        {
            string key = Path.GetDirectoryName(path);
            if (_paths.ContainsKey(key))
            {
                return _paths[key];
            }

            return new List<string>();
        } 
    }
}