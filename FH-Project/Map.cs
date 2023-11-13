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

    public Map()
    {
        räume = new List<Room>();
    }

    public void ErstelleZufälligeKarte(int minRaumAnzahl, int maxRaumAnzahl, int minBreite, int maxBreite, int minHöhe, int maxHöhe)
    {
        Random zufallsgenerator = new Random();

        int raumAnzahl = zufallsgenerator.Next(minRaumAnzahl, maxRaumAnzahl + 1);

        for (int i = 0; i < raumAnzahl; i++)
        {
            int breite = zufallsgenerator.Next(minBreite, maxBreite + 1);
            int höhe = zufallsgenerator.Next(minHöhe, maxHöhe + 1);
            int x = zufallsgenerator.Next(0, 100 - breite); 
            int y = zufallsgenerator.Next(0, 100 - höhe);

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

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var raum in räume)
        {
            spriteBatch.DrawRectangle(raum.Bereich, Color.White);
        }
    }
}
