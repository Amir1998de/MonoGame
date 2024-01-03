using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Threading;

namespace FH_Project;

public class Room : MapEntity
{
    public Rectangle Bereich { get; set; }

    private readonly Sprite[,] tiles;
    private readonly Point tileSize = new(4,3);
    private List<Texture2D> textures = new List<Texture2D>();
    private Random random = new Random();
    public int MaxWidth { get; private set; }
    public int MaxHeight { get; private set; }

    public int TileWidth {  get; private set; }
    public int TileHeight {  get; private set; }

    public int EnemyCap { get; set; }

    public RoomDirections Directions { get;  set; }
    public RoomDirections ReverseDircetion { get; set; }
    

    public Room(int x, int y, int breite, int höhe)
    {
        EnemyCap = random.Next(1,3);

        tileSize = new Point(breite, höhe);

        tiles = new Sprite[tileSize.X, tileSize.Y]; // Anzahl der Tiles in x- und y-Richtung

        for (int i = 1; i < 6; i++)
            textures.Add(Globals.Content.Load<Texture2D>($"tile{i}"));
    

        TileWidth = textures[0].Width;
        TileHeight = textures[0].Height;

        MaxWidth = tileSize.X * TileWidth;
        MaxHeight = tileSize.Y * TileHeight;

        Bereich = new Rectangle(x - TileWidth / 2, y - TileHeight / 2, MaxWidth, MaxHeight);

        // Anzahl der Tiles in x- und y-Richtung
        for (int i = 0; i < tileSize.Y; i++)
        {
            for (int j = 0; j < tileSize.X; j++)
            {
                int r = random.Next(0, textures.Count);
                tiles[j, i] = new Sprite(textures[r], new Vector2(x + j * TileWidth, y + i * TileHeight));
            }
        }
    }

    public void CreateEnemyInRoom()
    {
        int count = 0;
        while (EnemyCap >= count)
        {
            int x = random.Next(Bereich.Left, Bereich.Right);
            int y = random.Next(Bereich.Top, Bereich.Bottom);

            Vector2 pos = new Vector2(x, y);

            Enemy.AddEnemy("Slime", 500, 2, pos, new Vector2(0, 0));
            count++;
        }
    }


    public bool PlayerIsInRoom()
    {
        return Bereich.Contains(Globals.Player.Position.X, Globals.Player.Position.Y);
    }



    public void Draw()
    {
        for (int y = 0; y < tileSize.Y; y++)
        {
            for (int x = 0; x < tileSize.X; x++)
                tiles[x, y].Draw();
        }
        //Globals.SpriteBatch.DrawRectangle(Bereich, Color.White);
    }
}

