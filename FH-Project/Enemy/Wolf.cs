using FH_Project;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FH_Projekt;

public class Wolf : Enemy
{
    

    public Wolf(int health, float speed, Vector2 pos, Vector2 velocity, float chaseRadius, int scale) : base(health, speed, pos, velocity, chaseRadius, scale)
    {
        
    }

    public override void Attack(GameTime gameTime, PlayerActions data)
    {
        if (Globals.Player != null)
        {
            float distanceToPlayer = Vector2.Distance(Position, Globals.Player.Position);

            Room roomPlayerIsin = Map.GetRoomPlayerIsIn();
            if (roomPlayerIsin != null && roomPlayerIsin.WhichKorridor != null)
            {
                if (distanceToPlayer + (chaseRadius - 200) <= chaseRadius && roomPlayerIsin.Bereich.Intersects(EnemyBounds))
                {
                    Speed = 1f + Map.EnemySpeedAdder;
                    ChasePlayer();
                }
                else
                {
                    Speed = 8f + Map.EnemySpeedAdder;
                    ChasePlayer();
                }
                    
            }


        }


        EnemyBounds = new Rectangle((int)Position.X + 15, (int)Position.Y + 23, EntityTexture.Width + 23, EntityTexture.Height);

        if (CollisionHandler.CollisionEntitys(Globals.Player.PlayerBounds, EnemyBounds))
        {
            if (Globals.Player.CanGetHit)
            {
                Globals.Player.ReduceHealth(Damage);
                Globals.Player.CanGetHit = false;
            }


            Debug.WriteLine("HIT! " + GetHashCode().ToString() + "\n " + Globals.Player.Health);
        }

    }

    public override void Draw()
    {
        Globals.SpriteBatch.Draw(EntityTexture, Position, null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0);

        /*for (int i = 0; i <= 5; i++)
        {
            Globals.SpriteBatch.Draw(enemyIdleTexture[i], Position, null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0);
        }*/


        if (enemydrops.Any())
        {
            enemydrops.ForEach(enemyDrop =>
            {
                enemyDrop.Draw();
            });
        }
        //Globals.SpriteBatch.DrawRectangle(EnemyBounds, Color.White);
    }

    public override void LoadContent()
    {
        EntityTexture = Globals.Content.Load<Texture2D>("Enemy/Wolf/tile000");

        enemyIdleTexture = new Texture2D[6];
        for (int i = 0; i <= 5; i++)
        {
            enemyIdleTexture[i] = Globals.Content.Load<Texture2D>($"Enemy/Wolf/tile00{i}");
        }
        
    }
}
