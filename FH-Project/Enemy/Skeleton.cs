using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace FH_Project;

public class Skeleton : Enemy
{

    private TimeSpan attackCooldown = TimeSpan.FromSeconds(3);
    private TimeSpan lastAttackTime = TimeSpan.Zero;
    private List<Arrow> arrows = new List<Arrow>();

    public Skeleton(int health, float speed, Vector2 pos, Vector2 velocity, float chaseRadius, int scale) : base(health, speed, pos, velocity, chaseRadius, scale)
    {
    }
    public override void Attack(GameTime gameTime,PlayerActions data)
    {
        EnemyBounds = new Rectangle((int)Position.X + 10, (int)Position.Y + 15, EntityTexture.Width * 2, EntityTexture.Height * 2);

        int index = -1;
        if (arrows.Count > 0)
        {
            arrows.ForEach(arrow =>
            {
                arrow.Update(gameTime);

                if (!arrow.IsActive) index = arrows.IndexOf(arrow);
                Room room = Map.GetRoomEnemyIsIn(this);

                if (room != null)
                {
                    if (!room.Bereich.Intersects(arrow.ArrowBounds))
                        arrow.SetToFalse();
                }


            });
            if (index != -1)
                arrows.RemoveAt(index);
        }



        if (gameTime.TotalGameTime - lastAttackTime > attackCooldown)
        {
            float distanceToPlayer = Vector2.Distance(Position, Globals.Player.Position);
            if (distanceToPlayer <= chaseRadius)
            {
                // Pfeil in Richtung des Spielers schießen
                Vector2 directionToPlayer = Vector2.Normalize(Globals.Player.Position - Position);
                Vector2 arrowVelocity = directionToPlayer * 10; // Setze ArrowSpeed als gewünschte Geschwindigkeit des Pfeils
                Arrow arrow = new Arrow(Position, arrowVelocity, 10); // Annahme: Arrow ist eine Klasse für Pfeile
                arrows.Add(arrow); // Annahme: Arrows ist eine Liste für alle Pfeile im Spiel

                // Aktualisiere die Zeit des letzten Angriffs
                lastAttackTime = gameTime.TotalGameTime;

            }

        }
        
    }

    public void DrawArrow()
    {
        arrows.ForEach(arrow => arrow.Draw());
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

        DrawArrow();
        Globals.SpriteBatch.DrawRectangle(EnemyBounds, Color.White);
    }


    public override void LoadContent()
    {
        EntityTexture = Globals.Content.Load<Texture2D>("skull_v2_1");

        /*enemyIdleTexture = new Texture2D[totalIdleFrames];
        for (int i = 1; i <= totalIdleFrames; i++)
        {
            enemyIdleTexture[i] = Globals.Content.Load<Texture2D>($"skull_v1_{i}");
        }*/


    }
}
