using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace FH_Project;
class Slime : Enemy
{
    Random rnd = new Random();


    public Slime(int health, float speed, Vector2 pos, Vector2 velocity, Player player) : base(health, speed, pos, velocity, player)
    {
    }
    public override void Attack()
    {
        throw new NotImplementedException();
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
