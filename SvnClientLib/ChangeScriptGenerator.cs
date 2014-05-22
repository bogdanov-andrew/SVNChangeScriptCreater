using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using SharpSvn;
using SharpSvn.Security;

namespace SvnClientLib
{
    public class ChangeScriptGenerator
    {
        private const string WorkingCopyUri = @"D:\Projects\RepositoryTest\";
        private const string RepositoryUri = @"https://andrew-pc:8443/svn/RepositoryTest";
        private const int RevisionNumberStart = 1;
        private const int RevisionNumberStop = 8;

        public void Execute()
        {
            using (SvnClient client = new SvnClient())
            {
                //client.Authentication.Clear(); // Clear predefined handlers
                //client.Authentication.UserNamePasswordHandlers
                //    += delegate(object obj, SharpSvn.Security.SvnUserNamePasswordEventArgs args)
                //    {
                //        args.UserName = "Test";
                //        args.Password = "Test";
                //    };

                //client.Authentication.SslServerTrustHandlers +=
                //delegate(object sender, SvnSslServerTrustEventArgs e)
                //{
                //    e.AcceptedFailures = e.Failures;
                //    e.Save = true; // Save acceptance to authentication store
                //};

                client.CheckOut(new SvnUriTarget(new Uri(RepositoryUri)), WorkingCopyUri);
                IItemValidator validator = new ItemValidator(InitObservedFolders(), InitFileExtensions());
                PathCollector collector = new PathCollector(InitObservedFolders());
                Collection<SvnLogEventArgs> list = new Collection<SvnLogEventArgs>();
                SvnLogArgs logArgs = new SvnLogArgs(new SvnRevisionRange(RevisionNumberStart, RevisionNumberStop));
                client.GetLog(new Uri(RepositoryUri), logArgs, out list);
                foreach (SvnLogEventArgs svnLogEventArg in list)
                {
                    foreach (SvnChangeItem item in svnLogEventArg.ChangedPaths)
                    {
                        if (validator.Validate(item))
                        {
                            collector.Add(item);
                        }
                    }
                }

                foreach (IFolderDetails folder in InitObservedFolders())
                {
                    foreach (string filePath in collector.GetFilesByPath(folder.ObservedFolder))
                    {
                        if (!Directory.Exists(folder.SvnTempFolder))
                        {
                            Directory.CreateDirectory(folder.SvnTempFolder);
                        }

                        DownloadFile(client, WorkingCopyUri, NormalizePath(filePath), RevisionNumberStop, ConstructOutputFilePath(folder.SvnTempFolder, filePath));
                    }
                }

                FileCombiner combiner = new FileCombiner();
                combiner.CombineFiles(InitObservedFolders());
                
                //DownloadFile(client, WorkingCopyUri, @"SQL\StoredProc\st1.sql", 2, @"D:/Projects/1.txt");
            }
        }

        private string ConstructOutputFilePath(string folderPath, string filePath)
        {
            return Path.Combine(folderPath, Path.GetFileName(filePath));
        }

        private string NormalizePath(string path)
        {
            //makes "SQL\\StoredProc\\st1.sql"
            return Path.GetFullPath(path).Split(':')[1].Substring(1);
        }

        private static void DownloadFile(SvnClient client, string workingCopyPath, string filePath, int revision, string outputFile)
        {
            using (FileStream fileStream = File.Create(outputFile))
            {
                SvnTarget target = new SvnPathTarget(Path.Combine(workingCopyPath, filePath), revision);
                client.Write(target, fileStream);
            }   
        }

        private IList<IFolderDetails> InitObservedFolders()
        {
            List<IFolderDetails> paths = new List<IFolderDetails>();
            paths.Add(new FolderDetails(@"SQL\StoredProc\", "D:/Projects/sp1.txt", "D:/Projects/svnTemp"));

            return paths;
        }
        private IList<string> InitFileExtensions()
        {
            List<string> extensions = new List<string>();
            extensions.Add(".sql");

            return extensions;
        }
    }
}
