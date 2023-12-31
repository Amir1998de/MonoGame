﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using FH_Project;
using static System.Net.Mime.MediaTypeNames;

namespace FH_Project;
class Slime : Enemy
{
    Random rnd = new Random();


    public Slime(int health, float speed, Vector2 pos, Vector2 velocity,float chaseRadius, int scale) : base(health, speed, pos, velocity, chaseRadius, scale)
    {
    }


    public override void Attack(GameTime gameTime)
    {
        if (CollisionHandler.CollisionEntitys(Globals.Player, this))
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
