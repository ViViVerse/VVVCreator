# Overview

This is a .NET GUI utility which lets you create ViViVerse artefacts from templates.

These are the available templates:

+ Package<br />An empty package.
+ OrganelleModule<br />An empty organelle module.

Building the tool:
VVVCreator can be built with Visual Studio 2019 or Visual Studio Code with the C# extension installed.

# Creating templates

When creating a template, you have to provide **meta information** and **template files**.

The meta information consists of two files:

+ MacroDefinitions.xml<br />Contains a list of the macros (s.b.) used in the template files, with descripotions and default values.
+ WhatToDoNext.html<br />Contains the text which is displayed to the user after the target has been successfully created.

The template files reside in a folder called _Content_. The files and subfolders in this folder are copied to the target folder.

## Macros

Macros are used to manipulate the names of the folders and files and the contents of the files. A macro entry consists of delimiters and a macro name:<br />

\#\#PACKAGE_NAME\#\#

The entire entry will be replaced by the macro value which either needs to be entered by the user in the tool or which is created by the tool when the macro descriptions are loaded.

### Macro types

The macro type needs to be defined in the macro definitions. 

+ SysDate<br />The current date in the format yyyy-MM-dd.
+ SysGuid<br />A new guid in the format {00000000-0000-0000-0000-000000000000}.
+ SysCodeGuid<br />A new guid in the format 00000000, 0000, 0000, 0000, 000000000000.
+ UserString<br />A string which has to be provided by the user.

Only macro values of type _UserString_ can be edited in the tool, all others are set automatically by the system.

### Predefined macros

Macros whose values are assigned by the system and which do not need to be referenced (i.e. which are used only once) do not need to be defined. These macros can be used in the templates without prior definition:

+ \#\#GUID\#\#<br />A new guid in the format {00000000-0000-0000-0000-000000000000}.
+ \#\#CODE_GUID\#\#<br />A new guid in the format 00000000, 0000, 0000, 0000, 000000000000.
+ \#\#DATE\#\#<br />The current date in the format yyyy-MM-dd.
+ \#\#YEAR\#\#<br />The current year in the format yyyy.

### Macro modifiers

Modifiers can be applied to macro values. A modifier appears in the macro entry in front of the macro name:

\#\#uc(PACKAGE_NAME\#\#

That's right: no closing parenthesis!<br/>
These are the existing modifiers:

+ uc<br />Convert the entire macro value to uppercase.
+ lc<br />Convert the entire macro value to lowercase.
+ us<br />Convert the entire macro value to lowercase and put an underscore in front of uppercase letters and digits. No underscore is put at the beginning of the macro value. If there are groups of uppercase letters an underscore is only put in front of the first and last letter. If there are groups of digits an underscore is only put in front of the first digit. E.g. VVVFlowerPot3d becomes vvv_flower_pot_3d.

### Conditional macros

The use of macros can be controlled by the values of other macros:

\#\#USE_NAME?NAME\#\#

In this example, the macro entry is only substituted with the value of the macro NAME if the macro USE_NAME evaluates to 'true', otherwise the entry is just removed without substitution.
Conditions can be negated using '!':

\#\#!USE_NAME?NAME\#\#

Conditions and modifiers can be used together in one macro entry, with the condition having to appear first:

\#\#USE_NAME?uc(NAME\#\#

For file and folder names, the '?' cannot be used. For those use '¿', which can be entered by using ALT + 168 (on the number block).
The use of entire folders and files can be controlled with a conditional macro entry without macro name:

\#\#USE_FILE¿\#\#

In this example, the entire file or folder is only created in the target if USE_FILE evaluates to 'true'. This macro entry must be the first entry in the file or folder name.

## Examples

For further examples, please see the existing templates.
