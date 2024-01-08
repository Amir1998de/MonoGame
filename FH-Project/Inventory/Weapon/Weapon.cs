using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public abstract class Weapon : Item
{
    public Weapon(int damage, int speed, Texture2D texture)
    {
        Damage = damage;
        Speed = speed;
        Texture = texture;
        Inventory.AddItem(this);
    }
    public int Damage { get; set; }

    //Mıt Speed ıst dıe Waffengeschwındıgkeıt gemeint. Zb beim Bogen wie oft er schießen kann pro Sekunde
    public int Speed { get; set; }

    //Wird in Player gemacht, weil es propotional zum Spieler Pos ist
    public Vector2 Position { get; set; }

    public Texture2D Texture { get; set; }

    //1=primary Weapon , 2=Bow
    public int ID { get; set; }

    public virtual void Draw()
    {
        Globals.SpriteBatch.Draw(Texture, Position, null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
    }
    public abstract void UseEffect();
}
