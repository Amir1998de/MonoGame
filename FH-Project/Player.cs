using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;



public class Player : Entity
{

    #region Variablen
    private bool isAttacking;
    protected int shield;
    private int maxShield = 100;
    public Weapon Weapon { get; set; }
    #endregion Variablen

    public Player(int health, float speed, Vector2 postion, Vector2 velocity, Texture2D texture, Weapon weapon) : base(health, speed, postion, velocity, texture)
    {
        isAttacking = false;
        shield = 0;
        Weapon = weapon;
        Weapon.Position = new Vector2(Postion.X + EntityTexture.Width * 2, Postion.Y + EntityTexture.Height);
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
        Postion = new Vector2(Postion.X - Speed, Postion.Y);

        if (Postion.X < 0)
            Postion = new Vector2(0, Postion.Y);

        Weapon.Position = new Vector2(Postion.X + EntityTexture.Width * 2, Postion.Y + EntityTexture.Height);
    }

    public void MovePlayerRight()
    {
        Postion = new Vector2(Postion.X + Speed, Postion.Y);

        // Durch 2 weil man unten beim Draw 3 scale hat. 
        float z = (float) EntityTexture.Width * 3 ;

        if (Postion.X + z  > maxWeidth)
            Postion = new Vector2(maxWeidth - z, Postion.Y);
       
        Weapon.Position = new Vector2(Postion.X + EntityTexture.Width * 2, Postion.Y + EntityTexture.Height);
    }

    public void MovePlayerUp()
    {
        Postion = new Vector2(Postion.X, Postion.Y - Speed);



        if (Postion.Y < 0)
            Postion = new Vector2(Postion.X, 0);

        Weapon.Position = new Vector2(Postion.X + EntityTexture.Width * 2, Postion.Y + EntityTexture.Height);

    }

    public void MovePlayerDown()
    {
        Postion = new Vector2(Postion.X, Postion.Y + Speed);

        // Durch 2 weil man unten beim Draw 3 scale hat. 
        float z = (float)EntityTexture.Height * 3;

        if (Postion.Y  + z > maxHeight)
            Postion = new Vector2(Postion.X , maxHeight -z);

        Weapon.Position = new Vector2(Postion.X + EntityTexture.Width * 2, Postion.Y + EntityTexture.Height);
    }

    public override void Attack()
    {
        //isAttacking = true;
        GetNotified(PlayerActions.ATTACK);
        Animation();
    }

    public void Animation()
    {
        
    }

    

}
