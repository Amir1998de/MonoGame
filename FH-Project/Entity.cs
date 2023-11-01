using Microsoft.Xna.Framework;
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
    private SpriteBatch _spriteBatch;

    private Texture2D entityTexture;

    private int health;
    public int Health { get {return health } set {health=value } }

    private float speed;
    public int Speed { get { return speed } set { speed = value } }

    private Vector2 pos;
    public int Pos { get { return pos } set { pos = value } }

    private Vector2 velocity;
    public int Velocity { get { return velocity } set { velocity = value } }

    #endregion Variablen

    public Entity(int health, float speed, Vector2 pos, Vector2 velocity)
	{
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        this.health = health;
        this.speed = speed;
        this.pos = pos;
        this.velocity = velocity;
        this.pos = pos;
	}
    public ~Entity()
    {
        this.health = 0;
        this.speed = 0;
        this.pos = null;
        this.velocity = null;
    }

    public bool CheckIfDead()
    {
        if(Health == 0)
            return true;
        else
            return false;
    }

    public void ChangeHealth(int value)
    {
        Health += value;
        if (Health < 0)
            Health = 0;
    }

    public void Destroy()
    {
        ~Entity();
    }

    public void Attack()
    {

    }

    public void Draw()
    {
        _spriteBatch.Draw(entityTexture, pos, Color.White);
    }

    public void Add(Entity entity)
    {

    }

    public void Remove(Entity entity)
    {

    }

    protected void GetNotified()
    {

    }

}
