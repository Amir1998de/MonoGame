﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public abstract class Entity
{
    #region Variablen


    protected float maxW;

    protected float maxH;


    protected Texture2D entityTexture;

    protected int health;

    protected float speed;

    protected Vector2 pos;

    private int maxHealth;
    

    protected Vector2 velocity;
   

    #endregion Variablen

    public Entity(int health, float speed, Vector2 pos, Vector2 velocity, Texture2D entityTexture, float maxH, float maxW)
	{
        maxHealth = health;
        this.health = health;
        this.speed = speed;
        this.pos = pos;
        this.velocity = velocity;
        this.entityTexture = entityTexture;

        this.maxW = maxW;
        this.maxH = maxH;
	}
    ~Entity()
    {
        this.health = 0;
        this.speed = 0;
        //this.Pos = ;
        //this.velocity = null;
    }

    public bool CheckIfDead()
    {
        if(health == 0)
            return true;

        return false;
    }

    public void AddHealth(int value)
    {
        health += value;
        if (health > maxHealth)
            health = maxHealth;
    }

    public void ReduceHealth(int value)
    {
        health -= value;
        if (health < 0)
            health = 0;
    }

    public void Destroy()
    {
       
    }

    public void Attack()
    {

    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(entityTexture, pos, Color.White);
    }

    public void Add(Entity entity, SpriteBatch spriteBatch)
    {

    }

    public void Remove(Entity entity, SpriteBatch spriteBatch)
    {

    }

    protected void GetNotified()
    {

    }

}
