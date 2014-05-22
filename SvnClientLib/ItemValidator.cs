using System.Collections.Generic;
using System.IO;
using SharpSvn;

namespace SvnClientLib
{
    public class ItemValidator : IItemValidator
    {
        private readonly IList<IFolderDetails> _observedPaths;
        private readonly IList<string> _extentions;

        public ItemValidator(IList<IFolderDetails> observedPaths, IList<string> extentions)
        {
            _observedPaths = observedPaths;
            _extentions = extentions;
        }

        public bool Validate(SvnChangeItem item)
        {
            //if (item.NodeKind != SvnNodeKind.File)
            //{
            //    return false;
            //}

            if (!ValidateFolder(item.Path))
            {
                return false;
            }

            if (!ValidateExtension(item.Path))
            {
                return false;
            }

            return true;
        }

        private bool ValidateFolder(string itemPath)
        {
            foreach (IFolderDetails path in _observedPaths)
            {
                if (Path.GetDirectoryName(itemPath).Contains(Path.GetDirectoryName(path.ObservedFolder)))
                {
                    return true;
                }
            }
            return false;
        }

        private bool ValidateExtension(string itemPath)
        {
            foreach (string extension in _extentions)
            {
                if (extension.Equals(Path.GetExtension(itemPath)))
                {
                    return true;
                }
            }
            return false;
        }
    }
}