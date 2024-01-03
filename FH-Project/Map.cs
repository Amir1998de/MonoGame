using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace FH_Project;

public class Map : MapEntity
{
    private static List<Room> räume;

    private readonly Point _mapTileSize = new(16, 9);

    private readonly Sprite[,] _tiles;
    public Point TileSize { get; private set; }
    public Point MapSize { get; private set; }

    private List<Texture2D> textures = new(5);
    private Random random = new();

    

    public Map()
    {
        räume = new List<Room>();

        Debug.WriteLine(_mapTileSize.ToString());
        _tiles = new Sprite[_mapTileSize.X, _mapTileSize.Y];

        for (int i = 1; i < 6; i++)
            textures.Add(Globals.Content.Load<Texture2D>($"tile{i}"));


        TileSize = new(textures[0].Width, textures[0].Height); 

        for (int y = 0; y < _mapTileSize.Y; y++)
        {
            for (int x = 0; x < _mapTileSize.X; x++)
            {
                int r = random.Next(0, textures.Count);
                _tiles[x, y] = new(textures[r], new(x * TileSize.X, y * TileSize.Y));
            }
        }



    }


    public void ErstelleZufälligeKarte(int raumAnzahl, int minRaumBreite, int maxRaumBreite, int minRaumHöhe, int maxRaumHöhe)
    {
        Random zufallsgenerator = new Random();

        for (int i = 0; i < raumAnzahl; i++)
        {
            int x = 0;
            int y = 0;
            int raumBreite = zufallsgenerator.Next(minRaumBreite, maxRaumBreite + 1);
            int raumHöhe = zufallsgenerator.Next(minRaumHöhe, maxRaumHöhe + 1);
            RoomDirections directions = RoomDirections.START;

            if (räume.Count > 0)
            {
                bool intersectsExistingRoom;
                do
                {
                    intersectsExistingRoom = false;


                    int randDirection = zufallsgenerator.Next(0, 4); // 0: oben, 1: unten, 2: links, 3: rechts

                    switch (randDirection)
                    {
                        case 0: // oben
                            x = räume[i - 1].Bereich.X;
                            y = räume[i - 1].Bereich.Y - raumHöhe + (räume[i - 1].TileHeight / 2);
                            räume[i - 1].Directions = RoomDirections.UP;
                            räume[i - 1].ReverseDircetion = RoomDirections.DOWN;
                            break;
                        case 1: // unten
                            x = räume[i - 1].Bereich.X;
                            y = räume[i - 1].Bereich.Bottom + (räume[i - 1].TileHeight / 2);
                            räume[i - 1].Directions = RoomDirections.DOWN;
                            räume[i - 1].ReverseDircetion = RoomDirections.UP;
                            break;
                        case 2: // links
                            x = räume[i - 1].Bereich.X - raumBreite + (räume[i - 1].TileHeight / 2);
                            y = räume[i - 1].Bereich.Y;
                            räume[i - 1].Directions = RoomDirections.LEFT;
                            räume[i - 1].ReverseDircetion = RoomDirections.RIGHT;
                            break;
                        case 3: // rechts
                            x = räume[i - 1].Bereich.Right + (räume[i - 1].TileHeight / 2);
                            y = räume[i - 1].Bereich.Y;
                            räume[i - 1].Directions = RoomDirections.RIGHT;
                            räume[i - 1].ReverseDircetion = RoomDirections.LEFT;
                            break;
                    }

                    // Check for intersection with existing rooms based on dimensions
                    Rectangle newRoomRect = new Rectangle(x, y, raumBreite, raumHöhe);
                    foreach (var existingRoom in räume)
                    {
                        if (newRoomRect.Intersects(existingRoom.Bereich))
                        {
                            intersectsExistingRoom = true;
                            break;
                        }
                    }
                } while (intersectsExistingRoom);
            }
            else
            {
                // Erster Raum wird in der Mitte des Bildschirms platziert
                x = (Globals.WindowSize.X - minRaumBreite) / 2;
                y = (Globals.WindowSize.Y - minRaumHöhe) / 2;
            }

            Room neuerRaum = new Room(x, y, raumBreite, raumHöhe);
            räume.Add(neuerRaum);
        }
    }

    public static Room GetRoomPlayerIsIn()
    {
        foreach (Room room in räume)
        {
            if (room.PlayerIsInRoom())
                return room;
        }

        return null;
    }

    public void DrawEnemyInRoom()
    {
        räume.ForEach(room =>
        {
            room.CreateEnemyInRoom();
        });
        
    }

    public void Draw()
    {
        /*for (int y = 0; y < _mapTileSize.Y; y++)
        {
            for (int x = 0; x < _mapTileSize.X; x++)
                _tiles[x, y].Draw();
        }*/

        foreach (var raum in räume)
        {
            raum.Draw();
        }

        //räume[0].Draw();
    }


    public void DrawEnemyCounter(float x, float y, Camera camera)
    {
        SpriteFont font = Globals.Content.Load<SpriteFont>("Verdana");

        // Erhalte die kamerabezogene Position für den Score
        Vector2 cameraAdjustedPosition = new Vector2(x, y) + camera.Position;

        string enemyCountString = "Enemies: " + Enemy.enemies.Count.ToString();

        // Measure the size of the string to determine where to draw it
        Vector2 stringSize = font.MeasureString(enemyCountString);

        // Draw the string at the upper-right corner of the screen
        Globals.SpriteBatch.DrawString(font, enemyCountString, cameraAdjustedPosition, Color.Red);
    }

}
