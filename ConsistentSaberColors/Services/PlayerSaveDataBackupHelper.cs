using IPA.Utilities;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ConsistentSaberColors.Services
{
    public sealed class PlayerSaveDataBackupHelper
    {
        public static void BackupSaveData()
        {
            string fileName;
            string destFile;
            string copyFrom = Application.persistentDataPath;
            string copyTo = UnityGame.UserDataPath + @"\.PlayerDataBackups\" + DateTime.Now.ToFileTime();

            if (Directory.Exists(copyFrom))
            {
                Directory.CreateDirectory(copyTo);
                string[] files = Directory.GetFiles(copyFrom);

                foreach (string file in files)
                {
                    fileName = Path.GetFileName(file);
                    destFile = Path.Combine(copyTo, fileName);
                    File.Copy(file, destFile, true);
                }
            }

            CheckFolderCapacity(UnityGame.UserDataPath + @"\.PlayerDataBackups\");
        }

        public static void CheckFolderCapacity(string backupPath)
        {
            if (!Directory.Exists(backupPath)) throw new ArgumentException();
            string[] dirs = Directory.GetDirectories(backupPath);

            if (dirs.Count() > 5) Directory.Delete(dirs.First(), true);
        }
    }
}
