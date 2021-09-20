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
- Internal code cleanup

## Notes
This mod has a built-in PlayerData Local Backup service. The reason behind this is that during development, there were multiple instances that my PlayerData got wiped. 

The issue that caused this is fixed, but since I don't want to be held responsible for any data loss, this mod will also create up to *5 local backups* of your data, which can be accessed at `Beat Saber\UserData\.PlayerDataBackups\`. The folders are sorted from Oldest to Newest, Top to Bottom.

## Restoring Wiped Data
To restore wiped data, copy the contents of any Backup folder to `%AppData%\..\Hyperbolic Magnetism\Beat Saber`, and overwrite any existing files. **The most important files to restore would be `PlayerData.dat`, `AvatarData.dat`, `settings.cfg`, as well as all of their respective backup files (`.bak`)**. 

You do not need to restore every single file, as some are refreshed on startup, provided by other mods, or just aren't used anymore, but it's good practice to do so anyways.

## Bugs? Feature Request? Suggestion?
You can either open an issue here on GitHub, or open your own pull request if you want to develop it yourself! You can also reach me on Discord, **Exomanz#8083**
