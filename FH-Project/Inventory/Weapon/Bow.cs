
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public class Bow : Weapon
{
    private TimeSpan attackCooldown = TimeSpan.FromSeconds(3);
    private TimeSpan lastAttackTime = TimeSpan.Zero;
    private List<Arrow> arrows = new List<Arrow>();
    public Bow(int damage, int speed, Texture2D texture) : base(damage, speed, texture)
    {
        ID = 2;
    }


    public override void UseEffect()
    {
        throw new NotImplementedException();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        attack(gameTime);
    }

    public void attack(GameTime gameTime)
    {
        int index = -1;
        if (arrows.Count > 0)
        {
            arrows.ForEach(arrow =>
            {
                arrow.Update(gameTime, 1);

                if (!arrow.IsActive) index = arrows.IndexOf(arrow);
                Room room = Map.GetRoomPlayerIsIn();

                if(room != null)
                {
                    if(!room.Bereich.Intersects(arrow.ArrowBounds))
                    {
                        arrow.SetToFalse();
                    }
                }


            });
            if (index != -1)
                arrows.RemoveAt(index);
        }



        if (gameTime.TotalGameTime - lastAttackTime > attackCooldown)
        {
            Vector2 mousePosition = Globals.MouseState.Position.ToVector2() + Camera.Position;
            Vector2 mouseToPlayer = mousePosition - Globals.Player.Position;
            float distanceToMouse = Vector2.Distance(Globals.Player.Position, mousePosition);


            // Pfeil in Richtung des Gegners schießen
            Vector2 directionToMouse = Vector2.Normalize(mouseToPlayer);
            Vector2 arrowVelocity = directionToMouse * 10; // Setze ArrowSpeed als gewünschte Geschwindigkeit des Pfeils
            Arrow arrow = new Arrow(Position, arrowVelocity, 10); // Annahme: Arrow ist eine Klasse für Pfeile
            arrows.Add(arrow); // Annahme: Arrows ist eine Liste für alle Pfeile im Spiel

            // Aktualisiere die Zeit des letzten Angriffs
            lastAttackTime = gameTime.TotalGameTime;


        }
    }

    public void DrawArrow()
    {
        arrows.ForEach(arrow => arrow.Draw());
    }

    public override void Draw()
    {
        Globals.SpriteBatch.Draw(Texture, Position, null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
        DrawArrow();
    }

}
