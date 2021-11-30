# ConsistentSaberColors
A Quest mod that allowed for your menu pointers to be the same colors as your sabers has been ported to PC!

## Overview - What exactly does this mod do?
Put simply, this mod just allows for your menu pointers to be the same colors as your in-level sabers!

There is no config for this mod--it comes enabled by default, and if you want to disable it, you will need to either uninstall the mod, or edit `Disabled Mods.json` in your UserData folder.

## Preview (Credit to [im-henri](https://github.com/im-henri/))
![Preview](https://github.com/im-henri/QonsistentSaberColors/blob/master/Animation.gif)

## Dependencies
- BSIPA: v4.2.0+
- SiraUtil v2.5.7+

## Roadpath
- Realtime color updates when editing color schemes

## PlayerData Backup Service
This mod has a built-in PlayerData backup service. The reason behind this is that during initial development, there were multiple instances where not only mine, but other player's PlayerData would get wiped. The issues that caused this are now fixed, but since I don't want to be held responsible for any data loss, this mod will also create up to **25** local backups of your data, which can be accessed at `Beat Saber\UserData\_PlayerDataBackups\`. The folders are sorted from oldest to newest from top to bottom. 

You can adjust the amount of backups that will be made through the `ConsistentSaberColors.json` file and by altering the `BackupLimit` field. This number can be between 1 and 25, and is defaulted to 5 upon config generation. If this number is negative or greater than 25, it will be reset to 25.

These files do not store any of your online scores since ScoreSaber leaderboards is completely separate from Beat Saber. These files store information such as local scores, campaign progress, and favorited songs. If you aren't concerned about losing these, then this probably won't concern you.

## Warning
If `EnableBackups` is set to `false`, or if `BackupLimit` is set to `0`, the PlayerData backup service will not run. However, your currently made backups will not be effected if these conditions are met. 

Doing either of these things **is** supported, but not recommended, and you accept full responsibility in the event that data loss occurs. 

## Restoring Wiped Data
To restore wiped data, copy the contents of any Backup folder to `%AppData%\..\Hyperbolic Magnetism\Beat Saber` and overwrite any existing files. **The most important files to restore would be `PlayerData.dat`, `AvatarData.dat`, `settings.cfg`, as well as all of their respective backup files (`.bak`)**. 

## Bugs? Feature Request? Suggestion?
You can either open an issue here on GitHub, or open your own pull request if you want to develop it yourself! You can also reach me on Discord, **Exomanz#8083**
