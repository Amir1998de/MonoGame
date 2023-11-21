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


    protected static float maxWeidth=0;

    protected static float maxHeight=0;

    public float BaseSpeed { get; private set; }

    public Texture2D EntityTexture { get;  set; }

    protected int health;

    public float Speed { get; set; }
    public bool IsDestroyed { get; set; }

    public Vector2 Postion { get; set; }

    private int maxHealth;

    public Sprite sprite;
    private Vector2 _minPos, _maxPos;
    private float SPEED = 300;



    protected Vector2 velocity;
   

    #endregion Variablen

    public Entity(int health, float speed, Vector2 pos, Vector2 velocity, Texture2D entityTexture)
	{
        maxHealth = health;
        this.health = health;
        this.Speed = speed;
        this.Postion = pos;
        this.velocity = velocity;
        this.EntityTexture = entityTexture;
        IsDestroyed = false;
        BaseSpeed = 2;
        sprite = new Sprite(entityTexture, pos);
    }

    public static void View(float maxW, float maxH)
    {
        maxHeight = maxH;
        maxWeidth = maxW;
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

    public int getHealth()
    {
        return health;
    }

    public void ReduceHealth(int value)
    {
        health -= value;
        if (health < 0)
            health = 0;
    }

    

    public abstract void Attack();

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(EntityTexture, Postion, null, Color.White, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
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
        Rectangle enemyBounds = new Rectangle((int)Postion.X, (int)Postion.Y, EntityTexture.Width * 3, EntityTexture.Height * 3);
     
        return playerBounds.Intersects(enemyBounds);
    }
    public void SetBounds(Point mapSize, Point tileSize)
    {
        _minPos = new((-tileSize.X / 2) + sprite.Origin.X, (-tileSize.Y / 2) + sprite.Origin.Y);
        _maxPos = new(mapSize.X - (tileSize.X / 2) - sprite.Origin.X, mapSize.Y - (tileSize.X / 2) - sprite.Origin.Y);
    }

    public void Update()
    {
        sprite.Position += Game1.Direction * Globals.Time * SPEED;
        sprite.Position = Vector2.Clamp(sprite.Position, _minPos, _maxPos);
    }

}
