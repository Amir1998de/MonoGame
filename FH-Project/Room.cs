using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public class Room : MapEntity
{
    public Rectangle Bereich { get; set; }
    public int EnemyCap { get; set; }

    public Room(int x, int y, int breite, int höhe)
    {
        Bereich = new Rectangle(x, y, breite, höhe);
        EnemyCap = 2;
    }

    public static void DrawEnemyInRoom(SpriteBatch spriteBatch)
    {
        Enemy.enemies.ForEach(enemy =>
        {
            enemy.Draw(spriteBatch);
            
        });
    }

    public bool PlayerIsInRoom(Player player)
    {
        return Bereich.Contains(player.Position.X, player.Position.Y);
    }



    public bool ÜberlapptMitAnderemRaum(Room andererRaum)
    {
        return Bereich.Intersects(andererRaum.Bereich);
    }
}
