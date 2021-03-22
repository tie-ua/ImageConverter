#**Image Converter**

The program automates the process of preparing the images for placement in the photo gallery of the news feed of the corporate website.
From a technical point of view, the program processes a group of images according to the same template - resizes the image in accordance with the site's requirements and saves the file in the JPG file format.

#**Installation**

For the program to work, it is not required to install it on a computer using any installation programs.
To run the program for the first time, you must follow these steps:
1) Download the archive file ImageConverter.zip to your hard drive (the archive is located in the exe folder in the root of the Image Converter project;
2) Extract the ImageConverter folder, located inside the archive file.
3) Run the ImageConverter.exe file located in this folder.


#**Program interface**

##The main window of the program consists of two parts:

![The main program window](/images/main_screen.png)

1) The left part is a list of files to be processed.
2) The right one is a demonstration of the contents of the selected file from the list of files to be processed. Click the left mouse button on the required file to select it from the list of files.

##The main program window also contains the following program control buttons:

1) ADD - adding one or more files to the list.
2) REMOVE - removing a file from the list. The deletion is performed one file at a time from the list.
3) CLEAR LIST - deletes all files from the list.
4) CONFIG - displays a window with program settings. The description of the settings will be given below.
5) CONVERT - starts the process of converting images.
6) CLOSE - closes the main window and exits the program.

When you press the CONVERT button, the program displays a horizontal progress bar below the control buttons to notify the user about the progress of the conversion process.

#**Program settings window**

![Program settings window](/images/programsettings_screen.png)

Program settings window shows the images sizes after processing and the directory in which they will be placed after the completion of processing. Please note that the program creates an additional subfolder RESULT in the folder ("Export directory" field). The images after processing will be placed in the subfolder RESULT.
The user can change the current values of image sizes to their own.
Press the DEFAULT button if you want to return to the default values.
Press the BROWSE button to select a different directory for saving images.
To save the program settings, press the SAVE button.

#**Working with the program**

1) Click on the ADD button and select in the Add new file window the files to be added to the list. Files can be added to the list one at a time or several at once.
2) If a file needs to be excluded from the list, select it with the mouse and press the REMOVE button.
3) If you want to delete all files from the list at one time, press the CLEAR LIST button
4) Check the program settings before starting the image processing procedure. To do this, click on the CONFIG button. The program will display the program settings window.
5) Press the CONVERT button to start the image conversion process. After the process is completed, the program will display on the screen the informational message about it.
6) Exit work with the program by pressing the CLOSE button.

#**The list of software that was used to build this program**

1) Microsoft C# 2019 Community Edition for Windows;
2) ImageProcessor (a collection of lightweight libraries written in C# that allows you to manipulate images on-the-fly using .NET 4.5+).
Source: https://imageprocessor.org/;
GitHub: https://github.com/JimBobSquarePants/ImageProcessor
License: a free and open source.
3) Ookii.Dialogs.Wpf (a class library for .NET and .NET Core applications providing several common dialogs)
Source: https://www.nuget.org/packages/Ookii.Dialogs.Wpf/
GitHub: https://github.com/ookii-dialogs/ookii-dialogs-wpf
License: the license redistribution and use in source and binary forms, with or without modification, are permitted. Detail - https://licenses.nuget.org/BSD-3-Clause

#**The authors**

The code, with the exception of modules #2 and #3, was written by me. I drew up the terms of reference and completed it myself.
Currently, I use the program almost daily in my organization's workflow.
If you have any comments about the work of the program or suggestions for expanding its functionality, please let me know. Your comments or suggestions will be considered.
You can freely use, modify and compile the source code of module 1 without any restrictions.