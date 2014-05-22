using System.Collections.Generic;
using System.IO;

namespace SvnClientLib
{
    public class FileCombiner
    {
        public void CombineFiles(IList<IFolderDetails> folderDetailes)
        {
            foreach (IFolderDetails folderDetail in folderDetailes)
            {
                if (!Directory.Exists(folderDetail.SvnTempFolder))
                {
                    continue;
                }

                using (StreamWriter writer = File.CreateText(folderDetail.TargetFileName))
                {
                    foreach (string file in Directory.GetFiles(folderDetail.SvnTempFolder))
                    {
                        using (StreamReader reader = File.OpenText(file))
                        {
                            writer.WriteLine(string.Format("/* * * * * {0} * * * * */",Path.GetFileName(file)));
                            string text = reader.ReadToEnd();
                            writer.Write(text);
                            writer.WriteLine();
                        }
                    }
                }
            }
        }
    }
}