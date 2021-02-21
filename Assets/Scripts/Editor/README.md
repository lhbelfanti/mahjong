# Editor

## Tools

The editor is composed by 2 tools:

- The [Levels Editor Window](#levels-editor-window-sections): This tool can be found in the menu `Editor -> Open Levels Editor`.
- The [Scene Autoloader script](http://wiki.unity3d.com/index.php/SceneAutoLoader?_ga=2.20682232.1917909840.1613845548-2073042353.1607907470): That is used to test the level created in the editor.

### Levels Editor Window Sections

#### Scripts

Contains all the necessary scripts to the tool to work:
- [GridEditor](../LevelEditor/GridEditor.cs).
- [BoardEditor](../LevelEditor/BoardEditor.cs).
- [FloorEditor](../LevelEditor/FloorEditor.cs).
- [TileEditor](../LevelEditor/TileEditor.cs).

#### Importer

It has only one button and imports a level into the editor. It allows you to save the current work in a temp file called `editorLevel.json`.

#### Board

Allows you to create a board specifying the width (min width = 1 / max width = 8) and the height (min height = 1 / max height = 7). The board is called Grid and it's managed by the [GridEditor](../LevelEditor/GridEditor.cs) script.

It also has the functionality to completely remove the board. It removes the tiles, the floors and the grid.

#### Floors

It manages the creation of the floors. The Mahjong may have different floors in one game, and this section allows you to create them, select the active one (there can only be one active at the same time), hide them to have a better visualization and remove them.

#### Tiles

There are 3 different types of tiles:
- **Single**: If it is not in the first floor, it needs a tile underneath it to be positioned, else it doesn't need it.
- **Double Horizontal**: This tile cannot be placed in the first floor. It needs 2 tiles underneath it to be positioned. As its name says, this tile ocuppies 2 places and it is positioned horizontally in the middle of the 2 tiles that it has underneath.
- **Double Vertical**: This tile cannot be placed in the first floor. It needs 2 tiles underneath it to be positioned. As its name says, this tile ocuppies 2 places and it is positioned vertically in the middle of the 2 tiles that it has underneath.

The tiles section allows you to create, on the active floor, any of the 3 mentioned tiles. When it is created, it will be shown in a list divided by floor, and there you can select it to be moved or to be removed. The default position of the tile is always the position (0,0) of the grid.

This section also shows the quantity of tiles that are in the grid. There sum should be even to be a valid level.

#### Validator

It allows you to validate the level. It does the necessary validations for each type of tile and also validates that the sum of the tiles is even.

When the validation is successful, the level is saved in a temporary file (called `editorLevel.json`) and that file is the one that is loaded to test the level.

##### Test the level

To test the level you previously have to set up the Scene Autoload script. To do so, select the [Game](../../Scenes/Game.unity) scene in the: `Editor -> Scene Autoload -> Select Master Scene ...`. After that you should select `Editor -> Scene Autoload -> Load Master On Play`. And that's it, now the `editorLevel.json` file will be loaded on press play.

##### Fill Method

There is a property called **Fill Method** in the Validator and Exporter sections. That property indicates the way the tiles will be created in the board. The posible **Fill Methods** are:
- **Random**: all the pairs are created randomly and there could be a tile in the first floor and its pair in the third floor.
- **By Floor**: it creates the pairs by floor, so each tile will mostly have its pair in the same floor.

#### Exporter

In this section all the properties related to the export of the created and validated level will be shown. 
> **_NOTE:_** The level should be validated to be able to be exported.

It cointains the following properties:

- **Save Path**: it can only be modified by using the Default and Open buttons. It is the location where the level will be saved.
- **Default Button**: set the Save Path to `Application.dataPath + /Resources/Text/`.
- **Open Button**: allows you to pick a location where the level will be saved.
- **Level Number**: the number of the level that will be used to save the file.
- **Fill Method**: see the explanation in the [Fill Method section](#fill-method).
- **Save Button**: It saves the current level. The level file name is created with the word `level` + the level number indicated in this property. The level number for the `editorLevel.json` is specified [here](https://gitlab.com/lhbelfanti/mahjong/-/blob/master/Assets/Scripts/LevelEditor/Exporter.cs#L19).