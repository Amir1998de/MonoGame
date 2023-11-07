using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;



//Leere Klasse mit Base Konstruktor
public class Player : Entity
{

    #region Variablen
    private bool isAttacking;
    protected int shield;
    private int maxShield = 100;
    #endregion Variablen

    public Player(int health, float speed, Vector2 pos, Vector2 velocity, Texture2D texture, float maxH, float maxW) : base(health, speed, pos, velocity, texture, maxH, maxW)
    {
        isAttacking = false;
        shield = 0;
    }

    public void AddShield(int value)
    {
        shield+= value;
        if (health > maxShield)
            health = maxShield;
    }

    public void ReduceShield(int value)
    {
        shield-= value;
        if (shield< 0)
            shield= 0;
    }

    public bool CanAttack()
    {
        return !(isAttacking);
    }

    public void MovePlayerLeft()
    {
        pos.X -= speed;

        if (pos.X < 0)
            pos.X = 0;      
    }

    public void MovePlayerRight()
    {
        pos.X = pos.X + speed;

        // Durch 2 weil man unten beim Draw 0.33f scale hat. 
        float z = (float) entityTexture.Width / 3 ;

        if (pos.X + z  > maxW)
            pos.X = maxW - z ;
    }

    public void MovePlayerUp()
    {
        pos.Y = pos.Y - speed;

       

        if (pos.Y < 0)
            pos.Y = 0;

    }

    public void MovePlayerDown()
    {
        pos.Y = pos.Y + speed;

        // Durch 2 weil man unten beim Draw 0.33f scale hat. 
        float z = (float)entityTexture.Height / 3;

        if (pos.Y  + z > maxH)
            pos.Y = maxH -z;
    }

    public void DrawPlayer(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(entityTexture, pos, null, Color.White, 0, new Vector2(0,0), 0.33f ,SpriteEffects.None,0);
    }

}
