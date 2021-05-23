# H3VR.Sideloader

*Swap textures, materials, meshes and prefabs effortlessly*

## What is this?

H3VR.Sideloader is a [BepInEx](https://github.com/BepInEx/BepInEx) plugin for 
[Hot Dogs, Horseshoes & Hand Grenades](https://store.steampowered.com/app/450540/Hot_Dogs_Horseshoes__Hand_Grenades/) 
that allows to

* Swap weapon textures, meshes, materials and whole prefabs
* Do all the swapping **without editing any of game's files**
* Package swapped assets into single-file packages that a user can easily drop into a folder

## How do I get it?

### Requirements

Download these archives into the same folder:

* [BepInEx](https://h3vr.thunderstore.io/package/BepInEx/BepInExPack_H3VR)
* [XUnity.ResourceRedirector](https://h3vr.thunderstore.io/package/bbepis/XUnity_ResourceRedirector)
* [H3VR.Sideloader](https://github.com/ghorsington/H3VR.Sideloader/releases)

### Installation

[**Example video for the impatient**](https://webm.red/view/K0OR.webm)

Full guide:

1. Downloaded the requirements above and place them into the same folder
2. Extract the downloaded folders into the game folder (`<STEAM folder>/steamapps/common/H3VR`) 
   **while following the README of each requirement!**
   * It's recommended that you run the game now *at least once*. That way BepInEx initializes all the folders and configuration files.
   * *Optional* Enable the debug console by opening `<H3VR folder>/BepInEx/config/BepInEx.cfg`, finding and setting
      ```toml
      [Logging.Console]

      Enabled = true
      ```
3. Create a `Mods` folder in H3VR folder (optional, will get generated manually as well)
4. Run the game once for sideloader to initialize

## How does it work?

Sideloader makes mod installation easy: all mods are packaged into single `.h3mod` (or `.hotmod`) files that are simple to install.  
Each h3mod contains a special manifest file that provides necessary information for sideloader for asset swapping and 
additional metadata that can be useful in future mod managers.

## How do I install a mod?

[**Quick tutorial video for the impatient**](https://webm.red/view/8LIe.webm)

Simply take `.h3mod` or `.hotmod` that you obtained and place it into `<H3VR Game Folder>/Mods` folder. Refer to Steam guide on how 
to find your [H3VR game folder](https://steamcommunity.com/sharedfiles/filedetails/?id=760447682).

Launch the game and you should have your mods installed an available!

## How do I create a mod?

**Textures (weapon and item skins)**: 

Download [latest `SkinPacker`](https://github.com/denikson/H3VR.Sideloader/releases).  
Run the tool, fill the required info and press `Pack h3mod`.

[**Tutorial video**](https://webm.red/view/8b9h.webm)

**Meshes, materials, prefabs**: WIP, there is no tool yet to do this automatically.  
Refer to the [wiki](https://github.com/denikson/H3VR.Sideloader/wiki) for info on how to create these packages manually.

**Custom objects (weapons, spawner IDs)**: WIP, there is no tool yet to do this automatically.  
Refer to the [wiki](https://github.com/denikson/H3VR.Sideloader/wiki/Custom-items) for WIP info about creating new weapons.
