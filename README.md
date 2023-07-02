# Garrys Mod Pre-Publishing Helper (WPF Version)

A little tool I wrote to automate the creation of the gma file when uploading a garry's mod map. You still need to use a program like crowbar to upload the map to the workshop. I could implement that myself but the way crowbar does it would blow this program out of the water.

While the download is a single executable, I recommend you put it in a subfolder as it creates log files in its own folder and you may not be able to find them if the directory you put the program in is cluttered with files and folders.

## Installation

1. Download from github (https://github.com/The1Wolfcast/GModPrePubHelper/releases)

2. Extract using Winrar (or other similar program)

3. Run GModPrePubHelper.exe

## How to use:

1. Select a foler to store map - This is where the files/folders used in the gma creation will be stored

2. Select map file - This is the compiled bsp file that hammer/hammer++ created when you run the map

3. Custom thumbnail - The image that shows up in Garry's Mod map selection screen

4. Custom Content - Does not do anything as of now but I plan for it to pack custom content in the future

5. gmad.exe location - This is the executable file that actually turns your map folder into a .gma file

6. Map name - used to ensure the map and thumbnail name are the same so they show up properly in garrys mod and is used in the name section of addon.json

7. JSON file tags - Tags for the workshop page, still needed for .gma creation 

8. Create Directories - Creates the folder structure and puts the appropriate files in their location as well as run gmad.exe when complete

## For Additional Help

Refer to: https://github.com/The1Wolfcast/GModPrePubHelper/wiki

Facepunch: https://wiki.facepunch.com/gmod/Workshop_Addon_Creation

Video Demonstration: TO BE ADDED
