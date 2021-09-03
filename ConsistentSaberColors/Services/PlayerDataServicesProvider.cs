using IPA.Utilities;
using UnityEngine;
using SiraUtil.Tools;
using System;
using System.IO;

namespace ConsistentSaberColors.Services
{
    /// <summary>
    /// Hosts a simple set of functions that creates local backups of your <see cref="PlayerData"/>.
    /// In the event that your data gets wiped, you can find 5 local backups at the <see cref="UnityGame.UserDataPath"/>
    /// </summary>
    public class PlayerDataServicesProvider
    {
        public PlayerDataModel currentPlayerDataModel;
        private SiraLog _log;

        public PlayerDataServicesProvider(PlayerDataModel dataModel, SiraLog log)
        {
            _log = log;

            currentPlayerDataModel = dataModel;
            CreateCopyOfData(dataModel);
        }

        private void CreateCopyOfData(PlayerDataModel dataModel)
        {
            if (dataModel == null)
            {
                throw new NullReferenceException("Cannot find a suitable PlayerDataModel (PlayerData) to read from!");
            }

            string fileName = string.Empty;
            string destFile = string.Empty;

            // Both of these strings should be compatbile across ALL drive configurations.
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
                _log.Logger.Info($"Data backup made at {DateTime.Now} and stored at {copyTo.Replace(UnityGame.InstallPath, "")}");
            }

            CheckFolderCapacity(UnityGame.UserDataPath + @"\_PlayerDataBackups");
        }

        private void CheckFolderCapacity(string path)
        {
            if (!Directory.Exists(path)) throw new ArgumentException("No backup folder found!");
            string[] dirs = Directory.GetDirectories(path);

            if (dirs.Length > 5)
                Directory.Delete(dirs[0], true);
        }
    }
}
