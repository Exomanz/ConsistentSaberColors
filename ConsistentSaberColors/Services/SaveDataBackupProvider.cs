using IPA.Utilities;
using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsistentSaberColors.Services
{
    /// <summary>
    /// Provides backups of the player's local data at the <see cref="Application.persistentDataPath"/>. 
    /// In the event that save data is wiped at any point while using ConsistentSaberColors, 
    /// you can find the last 5 backups at your <see cref="UnityGame.UserDataPath"/>
    /// </summary>
    public class SaveDataBackupProvider
    {
#pragma warning disable CS4014 // Because this call is not awaited blah blah blah
        public SaveDataBackupProvider(PlayerDataModel dataModel) =>
            BackupSaveDataAsync(dataModel);
#pragma warning restore CS4014

        private async Task<PlayerData> BackupSaveDataAsync(PlayerDataModel dataModel)
        {
            await Task.Run(() => dataModel.playerData);

            // Handle a NRE just in-case, even though we're awaiting the PlayerData.
            if (dataModel.playerData == null || dataModel == null)
            {
                throw new NullReferenceException("Cannot find a suitable PlayerDataModel (PlayerData) to back up!\n" +
                    "Exiting...");
            }

            else
            {
                string fileName;
                string destFile;
                string copyFrom = Application.persistentDataPath;
                string copyTo = UnityGame.UserDataPath + @"\.PlayerDataBackups" + DateTime.Now.ToFileTime();

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
                }

                CheckFolderCapacity(copyTo);
                return dataModel.playerData;
            }
        }

        private void CheckFolderCapacity(string path)
        {
            if (!Directory.Exists(path)) throw new ArgumentException("Cannot find the backup folder!");
            string[] dirs = Directory.GetDirectories(path);

            if (dirs.Length > 5) Directory.Delete(dirs[0], true);
        }
    }
}
