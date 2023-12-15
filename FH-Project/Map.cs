using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public class Map : MapEntity
{
    private List<Room> räume;

    private readonly Point _mapTileSize = new(5, 4);

    private readonly Sprite[,] _tiles;
    public Point TileSize { get; private set; }
    public Point MapSize { get; private set; }

    


    public Map()
    {
        räume = new List<Room>();

        _tiles = new Sprite[_mapTileSize.X, _mapTileSize.Y];

        List<Texture2D> textures = new(5);

        for (int i = 1; i < 6; i++)
            textures.Add(Globals.Content.Load<Texture2D>($"tile{i}"));
     

        TileSize = new(textures[0].Width, textures[0].Height);
        MapSize = new(TileSize.X * _mapTileSize.X, TileSize.Y * _mapTileSize.Y);

        Random random = new();

        for (int y = 0; y < _mapTileSize.Y; y++)
        {
            for (int x = 0; x < _mapTileSize.X; x++)
            {
                int r = random.Next(0, textures.Count);
                _tiles[x, y] = new(textures[r], new(x * TileSize.X, y * TileSize.Y));
            }
        }
    }

    public void ErstelleZufälligeKarte(int minRaumAnzahl, int maxRaumAnzahl, int minBreite, int maxBreite, int minHöhe, int maxHöhe)
    {
        Random zufallsgenerator = new Random();

        int raumAnzahl = zufallsgenerator.Next(minRaumAnzahl, maxRaumAnzahl + 1);

        for (int i = 0; i < raumAnzahl; i++)
        {
            int breite = zufallsgenerator.Next(minBreite, maxBreite + 1);
            int höhe = zufallsgenerator.Next(minHöhe, maxHöhe + 1);
            int x = zufallsgenerator.Next(0, 400 - breite); 
            int y = zufallsgenerator.Next(0, 400 - höhe);

            Room neuerRaum = new Room(x, y, breite, höhe);

            if (!KollisionMitAnderenRäumen(neuerRaum))
            {
                räume.Add(neuerRaum);
            }
        }
    }

    private bool KollisionMitAnderenRäumen(Room neuerRaum)
    {
        foreach (var raum in räume)
        {
            if (neuerRaum.ÜberlapptMitAnderemRaum(raum))
            {
                return true;
            }
        }
        return false;
    }

    public void Draw(SpriteBatch spriteBatch, Player player, int scale)
    {
        for (int y = 0; y < _mapTileSize.Y; y++)
        {
            for (int x = 0; x < _mapTileSize.X; x++)
                _tiles[x, y].Draw();
        }

        foreach (var raum in räume)
        {
            // Draw rooms based on the player's position and scale
            spriteBatch.DrawRectangle(
                new Rectangle(
                    (raum.Bereich.X - (int)player.Position.X) * scale,
                    (raum.Bereich.Y - (int)player.Position.Y) * scale,
                    raum.Bereich.Width * scale,
                    raum.Bereich.Height * scale),
                Color.White);
        }
    }
}
