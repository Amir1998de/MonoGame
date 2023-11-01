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

    public Player(int health, float speed, Vector2 pos, Vector2 velocity) : base(int health, float speed, Vector2 pos, Vector2 velocity)
    {
        isAttacking = false;
    }

    public bool CanAttack()
    {
        return !(isAttacking);
    }

    public void MovePlayerLeft()
    {
        pos.X = pos.X - speed;
        return pos;
    }

    public void MovePlayerRight()
    {
        pos.X = pos.X + speed;
        return pos;
    }

    public void MovePlayerUp()
    {
        pos.Y = pos.Y - speed;
        return pos;
    }

    public void MovePlayerDown()
    {
        pos.Y = pos.Y + speed;
        return pos;
    }

}
