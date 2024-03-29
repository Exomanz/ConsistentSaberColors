﻿using IPA.Utilities;
using UnityEngine;
using SiraUtil.Tools;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsistentSaberColors.Services
{
    /// <summary>
    /// This class creates, at most, 25 local backups of your data, but this can be limited in the <see cref="SaberColorConfig"/>.
    /// <br/>
    /// These backups can be accessed at the "_PlayerDataBackups" folder in the <see cref="UnityGame.UserDataPath"/> directory.
    /// </summary>
    public class PlayerDataServicesProvider
    {
        public PlayerDataModel currentPlayerDataModel = null!;
        private SiraLog _log = null!;
        private SaberColorConfig _config = null!;

        public PlayerDataServicesProvider(PlayerDataModel dataModel, SiraLog log, SaberColorConfig conf)
        {
            currentPlayerDataModel = dataModel!;
            _log = log!;
            _config = conf!;

            if (!conf.EnableBackups)
            {
                _log!.Logger.Error("NOTICE: The EnableBackups setting has been disabled.\n" +
                    "The copies that you alreday made will NOT be effected.\n" + 
                    "Although this mod is safe and data loss is extremely rare, disabling this is not recommended.\n" +
                    "By disabling this, you accept the risks and understand that I am no longer responsible for any data loss.");
                return;
            }

            if (conf.BackupLimit == 0)
            {
                _log!.Logger.Error("NOTICE: BackupLimit is set to 0. This will act as though EnableBackups is disabled.\n" +
                    "The copies that you already have will NOT be effected.\n" +
                    "Although this mod is safe and data loss is extremely rare, doing this is not recommended.\n" + 
                    "By doing so, you accept the risks and understand that I am no longer responsible for any data loss.");
                return;
            }

            Task.Factory.StartNew(() => CreateCopyOfDataAsync());
        }

        private async Task CreateCopyOfDataAsync()
        {
            await Task.Yield();

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
            return;
        }

        private void CheckFolderCapacity(string path)
        {
            if (!Directory.Exists(path)) throw new ArgumentException("No backup folder found!");
            string[] dirs = Directory.GetDirectories(path);

            if (_config.BackupLimit > 25)
            {
                _log!.Logger.Notice($"BackupLimit of {_config.BackupLimit} exceeds the maximum allowed value of 25, and will be reset to 25");
                _config.BackupLimit = 25;
            }

            if (dirs?.Length > _config.BackupLimit && _config.BackupLimit != 0)
            {
                for (int i = 0; i < dirs?.Length - _config.BackupLimit; i++)
                {
                    Directory.Delete(dirs?[i], true);
                }
            }
        }
    }
}
