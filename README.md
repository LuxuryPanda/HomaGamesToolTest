# HomaToolsTest
Repository of the Senior Unity Game Tool Developer test


## Overview
To start, I would like to thank you for the opportunity to take part in this test, I tried to approach it in the best way I could.
The process has been automated as much as possible, so the user can create characters with just a few clicks.

I slightly modified the original code to use Scriptable Objects instead of serialized data in the scene, so the user doesn't have to save the scene every time a new character is added or edited.
The tools have been designed to work with the system already in place, even though I would have used a fully data-driven approach, where each character is a Scriptable Object.

To perform the test I used Unity v2021.3.11f1.


## Characters Database
The Characters Database is a Scriptable Object that holds all the characters in the game. 
It's inspector has been modified to show everything in a more organized and simple way.

![Characters Database](Docs/Docs.CharacterDatabase.png?raw=true "Characters Database")

It also has different configurations to easily tweak the the characters creation process.

![Characters Database Configurations](Docs/Docs.CharacterDatabase2.png?raw=true "Characters Database Configurations")

## Character Builder
The Character Builder is a tool that allows everyone to create characters, by just filling in a few fields. 
It also allows to create materials, prefabs and add them to the store, all in one window.

![Character Builder Window](Docs/Docs.CharacterBuilder.png?raw=true "Character Builder Window")

The tool has a simple workflow:
1. Select the model for the character
2. Select or create the materials for the character
3. Select the character animator controller
4. Fill the character info
5. Create the character, by clicking the "Create Character" button

I added the possibility to create materials from the Character Builder, so the user doesn't have to create them by hand.
It can be accessed by clicking the "Create New Material" button in the Character Builder window.

![Material Creation](Docs/Docs.MaterialCreation.png?raw=true "Material Creation")

## Asset Scanner
The Asset Scanner is a tool that allows to scan the project for potentially forgotten assets, by looking for assets that have not been used for Store Characters.
It is based on the assumption that all the assets use a specific naming conventions, which can be changed in the configuration.

![Asset Scanner Window](Docs/Docs.AssetScanner.png?raw=true "Asset Scanner Window")

Its usage is simple, just click the "Run Asset Scan" button, and it will show all the assets that have not been used for Store Characters.
As of now, it only looks for prefabs and textures, but it can be easily extended to look for other types of assets.
It checks for asset usage only for the prefabs, textures don't have any references to them, so it just checks for the name.


It is customizable, so the user can change the naming conventions for the assets and also choose whether to look for naming conventions or not.

![Asset Scanner Configuration](Docs/Docs.AssetScanner2.png?raw=true "Asset Scanner Configuration")


## Asset Optimization
The AssetsPostProcessor is a tool that automatically optimizes audio, models, and textures when they are imported into the project.
It is quite simple, but it can be easily extended to further optimize the current types or add other types of assets.