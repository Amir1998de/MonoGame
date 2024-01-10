using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace FH_Project;

public class Room : MapEntity
{
    public Rectangle Bereich { get; set; }

    protected readonly Sprite[,] tiles;
    protected readonly Point tileSize = new(4, 3);
    private List<Texture2D> textures = new List<Texture2D>();
    private Random random = new Random();
    public int MaxWidth { get; private set; }
    public int MaxHeight { get; private set; }

    public int TileWidth { get; private set; }
    public int TileHeight { get; private set; }

    public int EnemyCap { get; set; }

    public RoomDirections Directions { get; set; }
    public RoomDirections ReverseDircetion { get; set; }

    public List<Room> Korridore { get; set; }
    Texture2D roomTexture = Globals.Content.Load<Texture2D>("tile1");

    public Room WhichKorridor { get; set; }

    public bool IsKorridor { get; private set; }
    private List<Enemy> enemiesInRoom = new List<Enemy>();



    public Room(int x, int y, int breite, int höhe, bool isKorridor)
    {
        Korridore = new List<Room>();

        IsKorridor = isKorridor;

        
        EnemyCap = random.Next(3, 6);

        //EnemyCap = 0;

        if (Game1.tileNr > 4)
            Game1.tileNr = 1;

        for (int i = 1; i < 4; i++)
            textures.Add(Globals.Content.Load<Texture2D>($"{Game1.tileNr}tile{i}"));

        Game1.tileNr++;


        TileWidth = textures[0].Width;
        TileHeight = textures[0].Height;

        tileSize = new Point(Math.Abs(breite), Math.Abs(höhe));

        tiles = new Sprite[tileSize.X, tileSize.Y]; // Anzahl der Tiles in x- und y-Richtung

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

            // Array mit den Werten des MonsterType-Enums
            MonsterType[] monsterTypes = (MonsterType[])Enum.GetValues(typeof(MonsterType));

            // Zufälligen Index auswählen
            int randomIndex = random.Next(monsterTypes.Length);

            // Gewählten MonsterType-Wert
            MonsterType selectedMonsterType = monsterTypes[randomIndex];

            // Je nach ausgewähltem MonsterType Monster erstellen

            switch (selectedMonsterType)
            {
                case MonsterType.SLIME:
                    enemiesInRoom.Add(Enemy.AddEnemy("Slime", 3, 2, pos, new Vector2(0, 0), 300f, 3));
                    break;
                case MonsterType.SKELETON:
                    float chaseRadius = Math.Max(Bereich.Width, Bereich.Height);
                    enemiesInRoom.Add(Enemy.AddEnemy("Skeleton", 2, 0.3f, pos, new Vector2(0, 0), 1000, 3));
                    break;
                case MonsterType.GOLEM:
                    // Hier entsprechenden Code für GOLEM hinzufügen
                    break;
                    // Weitere MonsterType-Werte können nach Bedarf hinzugefügt werden
            }

            count++;
        }
    }

    public int GetEnemiesInRoomCount()
    {

        int index = -1;
        foreach (Enemy enemy in enemiesInRoom)
        {
            if (!Enemy.enemies.Contains(enemy)) index = enemiesInRoom.IndexOf(enemy);
        }
        if (index != -1)
            enemiesInRoom.RemoveAt(index);

        return enemiesInRoom.Count;
    }



    public bool PlayerIsInRoom()
    {
        return Bereich.Contains(Globals.Player.Position.X, Globals.Player.Position.Y);
    }

    public bool EnemiesAreInRoom(Enemy enemy)
    {
        return Bereich.Contains(enemy.Position.X, enemy.Position.Y);

    }

    public virtual void Draw()
    {
        for (int y = 0; y < tileSize.Y; y++)
        {
            for (int x = 0; x < tileSize.X; x++)
                tiles[x, y].Draw();
        }
        //Globals.SpriteBatch.DrawRectangle(Bereich, Color.White);
        Korridore.ForEach(k =>
        {
            Globals.SpriteBatch.DrawRectangle(k.Bereich, Color.White);
        });

        //DrawRoomTexture();


    }








}
