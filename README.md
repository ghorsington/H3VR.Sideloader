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

Please wait, this tool is still work in progress. Public builds are not yet available.

## How does it work?

Sideloader makes mod installation easy: all mods are packaged into single `.h3mod` files that are simple to install.  
Each h3mod contains a special manifest file that provides necessary information for sideloader for asset swapping and 
additional metadata that can be useful in future mod managers.

## How do I install a mod?

Simply take `.h3mod` that you obtained and place it into `<H3VR Game Folder>/Mods` folder. Refer to Steam guide on how 
to find your [H3VR game folder](https://steamcommunity.com/sharedfiles/filedetails/?id=760447682).

Launch the game and you should have your mods installed an available!

## How do I create a mod?

**Textures (weapon and item skins)**: Use the released `Skin Packer` tool to package textures into h3mod files.

**Meshes, materials, prefabs**: WIP, there is no tool yet to do this automatically. Refer to the wiki for info on how to create these packages manually.