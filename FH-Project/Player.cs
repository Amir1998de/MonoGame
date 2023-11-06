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
    #endregion Variablen

    public Player(int health, float speed, Vector2 pos, Vector2 velocity) : base(health, speed, pos, velocity)
    {
        isAttacking = false;
    }

    public bool CanAttack()
    {
        return !(isAttacking);
    }

    public void MovePlayerLeft(Texture2D playerTexture)
    {
        pos.X -= speed;

        if (pos.X < playerTexture.Width - 40)
            pos.X = playerTexture.Width - 40;      
    }

    public void MovePlayerRight(Texture2D playerTexture)
    {
        pos.X = pos.X + speed;

        if (pos.X < playerTexture.Width + 40)
            pos.X = playerTexture.Width + 40;
    }

    public void MovePlayerUp(Texture2D playerTexture)
    {
        pos.Y = pos.Y - speed;

        if (pos.Y < playerTexture.Height - 40)
            pos.Y = playerTexture.Height - 40;

    }

    public void MovePlayerDown(Texture2D playerTexture)
    {
        pos.Y = pos.Y + speed;
        if (pos.Y < playerTexture.Height + 40)
            pos.Y = playerTexture.Height + 40;
    }

    public void DrawPlayer(SpriteBatch spriteBatch, Texture2D playerTexture)
    {
        spriteBatch.Draw(playerTexture, pos, null, Color.White, 0, new Vector2(playerTexture.Width / 2f, playerTexture.Height / 2f),0,SpriteEffects.None,0);
    }

}
