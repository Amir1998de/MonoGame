﻿using Microsoft.Xna.Framework;
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

    private List<Texture2D> textures = new(6);

    private Random random = new();

    




    public Map()
    {
        räume = new List<Room>();


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
                            y = räume[i - 1].Korridore[1].Bereich.Bottom + (räume[i - 1].Korridore[1].TileWidth / 2);
                            räume[i - 1].Directions = RoomDirections.DOWN;
                            räume[i - 1].ReverseDircetion = RoomDirections.UP;
                            räume[i - 1].WhichKorridor = räume[i - 1].Korridore[1];
                            break;
                        case 2: // links
                            x = räume[i - 1].Bereich.X - raumBreite + (räume[i - 1].TileHeight / 2);
                            y = räume[i - 1].Bereich.Y;
                            räume[i - 1].Directions = RoomDirections.LEFT;
                            räume[i - 1].ReverseDircetion = RoomDirections.RIGHT;
                            break;
                        case 3: // rechts
                            x = räume[i - 1].Korridore[3].Bereich.Right + (räume[i - 1].Korridore[3].TileWidth / 2);
                            y = räume[i - 1].Bereich.Y;
                            räume[i - 1].Directions = RoomDirections.RIGHT;
                            räume[i - 1].ReverseDircetion = RoomDirections.LEFT;
                            räume[i - 1]. WhichKorridor = räume[i - 1].Korridore[3];
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
                räume[i - 1].Korridore.RemoveAll(item => item != räume[i - 1].WhichKorridor);
            }
            else
            {
                // Erster Raum wird in der Mitte des Bildschirms platziert
                x = (Globals.WindowSize.X - minRaumBreite) / 2;
                y = (Globals.WindowSize.Y - minRaumHöhe) / 2;
            }

            Room neuerRaum = new Room(x, y, raumBreite, raumHöhe, false);
            CreateCorrdior(neuerRaum);
            räume.Add(neuerRaum);

        }


    }

    public void CreateCorrdior(Room room)
    {
        int randDirection = random.Next(0, 4);
        Room corrdior = new(0, 0, 0, 0,true);
        int x = 0;
        int y = 0;


        x = room.Bereich.Center.X;
        y = room.Bereich.Y - room.TileHeight / 2;
        corrdior = new(x, y, 1, 1, true);
        corrdior.Directions = RoomDirections.UP;
        room.Korridore.Add(corrdior);

        x = room.Bereich.Center.X;
        y = room.Bereich.Bottom + room.TileHeight / 2;
        corrdior = new(x, y, 1, 1, true);
        corrdior.Directions = RoomDirections.DOWN;
        room.Korridore.Add(corrdior);

        x = room.Bereich.X - room.TileWidth / 2;
        y = room.Bereich.Center.Y;
        corrdior = new(x, y, 1, 1, true);
        corrdior.Directions = RoomDirections.LEFT;
        room.Korridore.Add(corrdior);

        x = room.Bereich.Right + room.TileWidth / 2;
        y = room.Bereich.Center.Y;
        corrdior = new(x, y, 1, 1, true);
        corrdior.Directions = RoomDirections.RIGHT;
        room.Korridore.Add(corrdior);

        /*x = room.Bereich.Left - room.TileHeight / 2;
        y = room.Bereich.Center.Y;*/








    }

    public static Room GetRoomPlayerIsIn()
    {
        foreach (Room room in räume)
        {
            if (room.PlayerIsInRoom())
                return room;
            if (!room.IsKorridor && room.WhichKorridor != null && room.WhichKorridor.Bereich.Contains(Globals.Player.PlayerBounds))
            {
                return room.WhichKorridor;
            }

        }

        return null;
    }

    public static bool CheckWallCollison()
    {
        foreach (Room room in räume)
        {
            if (room.WallCollision())
                return true;
        }
        return false;
    }

    public static Room GetRoomEnemyIsIn(Enemy enemy)
    {
        foreach (Room room in räume)
        {
            if (room.EnemiesAreInRoom(enemy))
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

    public bool isCorrdior(Vector2 playerPosition)
    {


        return false;
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
        //räume[1].Draw();
        //räume[2].Draw();
    }

    public void DrawEnemyRoomCounter(float x, float y, Camera camera)
    {
        SpriteFont font = Globals.Content.Load<SpriteFont>("Verdana");
        Room roomPlayerIsIn = GetRoomPlayerIsIn();
        string enemyCountString = "";

        // Erhalte die kamerabezogene Position für den Score
        Vector2 cameraAdjustedPosition = new Vector2(x, y) + camera.Position;

        if(roomPlayerIsIn != null)  enemyCountString = "Enemies To Defeat: " + roomPlayerIsIn.GetEnemiesInRoomCount().ToString();

        // Measure the size of the string to determine where to draw it
        Vector2 stringSize = font.MeasureString(enemyCountString);

        // Draw the string at the upper-right corner of the screen
        Globals.SpriteBatch.DrawString(font, enemyCountString, cameraAdjustedPosition, Color.Red);
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

    public void GenerateMap(int roomCount, int minRoomWidth, int maxRoomWidth, int minRoomHeight, int maxRoomHeight)
    {
        ErstelleZufälligeKarte(roomCount, minRoomWidth, maxRoomWidth, minRoomHeight, maxRoomHeight);
        DrawEnemyInRoom();
    }
}
