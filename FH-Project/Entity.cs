using FH_Projekt;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public abstract class Entity : Subject
{
    #region Variablen


    protected static float maxWeidth = 0;

    protected static float maxHeight = 0;

    public float BaseSpeed { get; private set; }

    public Texture2D EntityTexture { get; set; }

    public int Health { get; set; }

    public float Speed { get; set; }
    public bool IsDestroyed { get; set; }

    public Vector2 Position { get; set; }

    private int maxHealth;

    public Sprite Sprite { get; set; }

    protected Vector2 velocity;

    //IdleAnimation 
    protected int currentIdleFrame = 0;
    protected int totalIdleFrames = 4;

    //Für Bewegung nach rechts
    protected int currentRightFrame = 0;
    protected int totalRightFrames = 6;

    //Für Bewegung nach links
    protected int currentLeftFrame = 0;
    protected int totalLeftFrames = 6;

    //Für Bewegung nach Oben
    protected int currentUpFrame = 0;
    protected int totalUpFrames = 6;

    //Für Bewegung nach Unten
    protected int currentDownFrame = 0;
    protected int totalDownFrames = 6;


    #endregion Variablen

    public Entity(int health, float speed, Vector2 pos, Vector2 velocity)
    {
        maxHealth = health;
        this.Health = health;
        this.Speed = speed;
        this.Position = pos;
        this.velocity = velocity;
        IsDestroyed = false;
        BaseSpeed = 1f;

    }

    public static void View(float maxW, float maxH)
    {
        maxHeight = maxH;
        maxWeidth = maxW;
    }


    public bool CheckIfDead()
    {
        if (Health == 0)
            return true;

        return false;
    }

    public void AddHealth(int value)
    {
        Health += value;
        if (Health > maxHealth)
            Health = maxHealth;
    }

    public int getHealth()
    {
        return Health;
    }

    public void ReduceHealth(int value)
    {
        Health -= value;
        if (Health < 0)
            Health = 0;
    }



    public abstract void Attack();

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(EntityTexture, Position, null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
    }

    public void Add(Entity entity, SpriteBatch spriteBatch)
    {

    }

    public void Remove(Entity entity, SpriteBatch spriteBatch)
    {

    }

    public bool CheckCollision(Rectangle playerBounds)
    {
        // *3 weil Skalierung 3
        Rectangle enemyBounds = new Rectangle((int)Position.X, (int)Position.Y, EntityTexture.Width * 3, EntityTexture.Height * 3);

        return playerBounds.Intersects(enemyBounds);
    }

    public abstract void LoadContent();





}
