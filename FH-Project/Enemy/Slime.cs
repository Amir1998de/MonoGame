using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using FH_Project;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;

namespace FH_Project;
class Slime : Enemy
{
    Random rnd = new Random();
    private TimeSpan randomMovementTimer;
    private TimeSpan randomMovementInterval = TimeSpan.FromSeconds(3);
    private Vector2 previousDirection;


    public Slime(int health, float speed, Vector2 pos, Vector2 velocity,float chaseRadius, int scale) : base(health, speed, pos, velocity, chaseRadius, scale)
    {
        randomMovementTimer = TimeSpan.Zero;
    }


    public override void Attack(GameTime gameTime, PlayerActions data)
    {
        if (Globals.Player != null)
        {
            float distanceToPlayer = Vector2.Distance(Position, Globals.Player.Position);

            Room roomPlayerIsin = Map.GetRoomPlayerIsIn();
            if (roomPlayerIsin != null && roomPlayerIsin.WhichKorridor != null)
            {

                


                if (distanceToPlayer <= chaseRadius && roomPlayerIsin.Bereich.Intersects(EnemyBounds))
                {
                    ChasePlayer();
                    Speed = 3f;
                }
                    
                else
                {
                    Speed = 1.5f;
                    if (Position.Y > roomPlayerIsin.Bereich.Bottom - EntityTexture.Height * 3)
                        ChangeRandomDirection();
                    else if (Position.Y < roomPlayerIsin.Bereich.Y - EntityTexture.Height)
                        ChangeRandomDirection();
                    else if (Position.X > roomPlayerIsin.Bereich.Right - EntityTexture.Width * 2)
                        ChangeRandomDirection();
                    else if (Position.X < roomPlayerIsin.Bereich.Left - EntityTexture.Width)
                        ChangeRandomDirection();

                    if (randomMovementTimer <= TimeSpan.Zero)
                        ChangeRandomDirection();
                    
                    else
                        randomMovementTimer -= gameTime.ElapsedGameTime;

                    

                    Position += velocity;
                }
            }


        }



        EnemyBounds = new Rectangle((int)Position.X + 20, (int)Position.Y + 25, EntityTexture.Width * 2, EntityTexture.Height * 2);

        if (CollisionHandler.CollisionEntitys(Globals.Player.PlayerBounds, EnemyBounds))
        {
            if (Globals.Player.CanGetHit)
            {
                Globals.Player.ReduceHealth(damage);
                Globals.Player.CanGetHit = false;
            }

            
            Debug.WriteLine("HIT! " + GetHashCode().ToString() + "\n " + Globals.Player.Health);
        }
    }



    private void ChangeRandomDirection()
    {
        Vector2 newDirection;
        do
        {
            float angle = (float)(rnd.NextDouble() * Math.PI * 2);
            newDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Speed;
        } while (newDirection == previousDirection);

        // Setze die neue Richtung und speichere sie als vorherige Richtung
        velocity = newDirection;
        previousDirection = newDirection;
        randomMovementTimer = randomMovementInterval;
    }

    public override void Draw()
    {
        Globals.SpriteBatch.Draw(EntityTexture, Position, null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0);
        if (enemydrops.Any())
        {
            enemydrops.ForEach(enemyDrop =>
            {
                enemyDrop.Draw();
            });
        }
        Globals.SpriteBatch.DrawRectangle(EnemyBounds, Color.White);
    }


    public override void LoadContent()
    {
        EntityTexture = Globals.Content.Load<Texture2D>("Enemy/Slime/slime-idle-0");

        enemyIdleTexture = new Texture2D[totalIdleFrames];
        for (int i = 0; i < totalIdleFrames; i++)
        {
            enemyIdleTexture[i] = Globals.Content.Load<Texture2D>($"Enemy/Slime/slime-idle-{i}");
        }


    }


}
