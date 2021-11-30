using IPA.Utilities;
using IPA.Utilities.Async;
using UnityEngine;
using SiraUtil.Tools;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsistentSaberColors.Services
{
    /// <summary>
    /// Class that creates up to 5 local backups of your <see cref="PlayerData"/>. These backups can be found in the "_PlayerDataBackups" folder in your <see cref="UnityGame.UserDataPath"/> path.
    /// </summary>
    public class PlayerDataServicesProvider
    {
        public PlayerDataModel currentPlayerDataModel = null!;
        private SiraLog _log = null!;

        public PlayerDataServicesProvider(PlayerDataModel dataModel, SiraLog log)
        {
            currentPlayerDataModel = dataModel!;
            _log = log!;

            UnityMainThreadTaskScheduler.Factory.StartNew(() => CreateCopyOfDataAsync());
        }

        private Task CreateCopyOfDataAsync()
        {
            string fileName;
            string destFile;

            string copyFrom = Application.persistentDataPath;
            string copyTo = UnityGame.UserDataPath + @"\_PlayerDataBackups\" + DateTime.Now.ToFileTimeUtc();

            if (Directory.Exists(copyFrom))
            {
                Directory.CreateDirectory(copyTo);
                string[] files = Directory.GetFiles(copyFrom);

                foreach (string f in files)
                {
                    fileName = Path.GetFileName(f);
                    destFile = Path.Combine(copyTo, fileName);
                    File.Copy(f, destFile, true);
                }

                _log!.Logger.Info($"Data backup made at {DateTime.Now} and stored at {copyTo.Replace(UnityGame.InstallPath, "")}");
            }

            CheckFolderCapacity(UnityGame.UserDataPath + @"\_PlayerDataBackups");
            return Task.CompletedTask;
        }

        private void CheckFolderCapacity(string path)
        {
            if (!Directory.Exists(path)) throw new ArgumentException("No backup folder found!");
            string[] dirs = Directory.GetDirectories(path);

            if (dirs?.Length > 5)
                Directory.Delete(dirs?[0], true);
        }
    }
}
