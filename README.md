<img src="https://github.com/UiharuKazari2008/ARS-NOVA-Bootloader/blob/main/Assets/Texture2D/icon.png" height="100px"/>

# ARS NOVA Bootloader
ARS NOVA Bootloader preboot for ALLS NOVA OS for ALLS Hardware

## SOS Keychip Interaction
Designed to interface with Savior Of Song Keychip 2.18+
1. Import the keychip start scheduled task from `/Support`
2. Copy `Launch.vbs` to `C:\`
3. You should have a `init_start.ps1` that starts the keychip as Administrator
4. Update your init_start.ps1 to use displayState
  * `--displayState "C:\SEGA\system\preboot\preboot_Data\StreamingAssets\state.txt"`


## Skip BIOS Boot Screen
* Enable `Skip Boot` in `Scenes/boot/Canvas/Video Player`

![image](https://github.com/UiharuKazari2008/ARS-NOVA-Bootloader/assets/15165770/e58f5d85-805a-4b32-adf6-1452dd4a3364)


## Post-Build Unity Splash Removal
1. From the top search for `SEGA`, go to first `?`, and chnage hex to `00`
 ![image](https://github.com/UiharuKazari2008/ARS-NOVA-Bootloader/assets/15165770/342ce3d7-0d9f-41af-9b9b-c4e7ea874ccd)
2. Search for `2022.3.13f1` and select back 14h in length and set the first hex value to `01`
 ![image](https://github.com/UiharuKazari2008/ARS-NOVA-Bootloader/assets/15165770/e2508f91-fca3-47ff-81f0-01366efcb571)

