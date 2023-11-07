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
    public Weapon Weapon { get; set; }
    #endregion Variablen

    public Player(int health, float speed, Vector2 postion, Vector2 velocity, Texture2D texture, float maxH, float maxW) : base(health, speed, postion, velocity, texture, maxH, maxW)
    {
        isAttacking = false;
        shield = 0;
        Weapon = new Sword();
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
        Postion = new Vector2(Postion.X - speed, Postion.Y);

        if (Postion.X < 0)
            Postion = new Vector2(0, Postion.Y);
    }

    public void MovePlayerRight()
    {
        Postion = new Vector2(Postion.X + speed, Postion.Y);

        // Durch 2 weil man unten beim Draw 0.33f scale hat. 
        float z = (float) EntityTexture.Width / 3 ;

        if (Postion.X + z  > maxW)
            Postion = new Vector2(maxW - z, Postion.Y);
    }

    public void MovePlayerUp()
    {
        Postion = new Vector2(Postion.X, Postion.Y - speed);



        if (Postion.Y < 0)
            Postion = new Vector2(Postion.X, 0);

    }

    public void MovePlayerDown()
    {
        Postion = new Vector2(Postion.X, Postion.Y + speed);

        // Durch 2 weil man unten beim Draw 0.33f scale hat. 
        float z = (float)EntityTexture.Height / 3;

        if (Postion.Y  + z > maxH)
            Postion = new Vector2(Postion.X , maxH -z);
    }

    public override void Attack()
    {
        //isAttacking = true;
        GetNotified(PlayerActions.Attack);
        Animation();
    }

    public void Animation()
    {
        
        
    }

    

}
