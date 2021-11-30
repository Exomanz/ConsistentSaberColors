namespace ConsistentSaberColors
{
    public class SaberColorConfig
    {
        /// <summary>
        /// Although the Backup Service is legacy, it will still be enabled by default until I am completely confident that data loss is impossible.
        /// </summary>
        public virtual bool EnableBackups { get; set; } = true;

        /// <summary>
        /// Specifies the amount of backups you wish to have. The maximum for this value is 25.
        /// </summary>
        public virtual uint BackupLimit { get; set; } = 5;
    }
}
