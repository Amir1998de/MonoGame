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


    public Slime(int health, float speed, Vector2 pos, Vector2 velocity,float chaseRadius, int scale) : base(health, speed, pos, velocity, chaseRadius, scale)
    {
    }


    public override void Attack(GameTime gameTime, PlayerActions data)
    {
        EnemyBounds = new Rectangle((int)Position.X + 20, (int)Position.Y + 25, EntityTexture.Width * 2, EntityTexture.Height * 2);

        if (CollisionHandler.CollisionEntitys(Globals.Player.PlayerBounds, EnemyBounds))
        {
            if (Globals.Player.CanGetHit)
            {
                Globals.Player.ReduceHealth(damage);
                Globals.Player.CanGetHit = false;
            }

            
            Debug.WriteLine("HIT! " + this.GetHashCode().ToString() + "\n " + Globals.Player.Health);
        }
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
