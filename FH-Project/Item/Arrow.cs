using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FH_Project;
public class Arrow
{

    public static Texture2D ArrowTexture;
    public Vector2 Position { get; private set; }
    public Vector2 Velocity { get; private set; }
    public bool IsActive { get; private set; }
    public Rectangle ArrowBounds { get; set; }
    private float lifetime;
    private float elapsedLifetime;


    public Arrow(Vector2 position, Vector2 velocity, float lifetime)
    {
        Position = position;
        ArrowBounds = new Rectangle((int)position.X, (int)position.Y, ArrowTexture.Width, ArrowTexture.Height);
        Velocity = velocity;
        IsActive = true;
        elapsedLifetime = 0f;
        this.lifetime = lifetime;
    }

    public void Update(GameTime gameTime, int z)
    {
        if (IsActive)
        {
            Position += Velocity;
            ArrowBounds = new Rectangle((int)Position.X, (int)Position.Y, ArrowTexture.Width, ArrowTexture.Height);

            foreach (var enemy in Enemy.enemies)
            {
                if (CollisionHandler.CollisionWithEnviorment(ArrowBounds, enemy))
                {
                    
                    IsActive = false; break;
                }
            }
            elapsedLifetime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedLifetime >= lifetime)
            {
                IsActive = false;
            }

        }

    }

    public void Update(GameTime gameTime)
    {
        if (IsActive)
        {
            Position += Velocity;
            ArrowBounds = new Rectangle((int)Position.X, (int)Position.Y, ArrowTexture.Width, ArrowTexture.Height);
            
            if (CollisionHandler.CollisionWithEnviorment(ArrowBounds, Globals.Player))
            {
                Globals.Player.ReduceHealth(1);
                IsActive = false;
            }
            /*
            foreach(var enemy in Enemy.enemies)
            {
                if(CollisionHandler.CollisionWithEnviorment(ArrowBounds,enemy))
                {
                    IsActive = false;
                    break;
                }
            }
            */
            elapsedLifetime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedLifetime >= lifetime)
            {
                IsActive = false;
            }
        }

        
    }

    public void SetToFalse()
    {
        IsActive = false;
    }

    public void Draw()
    {
        if (IsActive)
        {
            Globals.SpriteBatch.Draw(ArrowTexture, Position, Color.White);
        }
    }

}
