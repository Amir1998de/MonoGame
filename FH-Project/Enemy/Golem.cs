using FH_Project;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FH_Projekt;

public class Golem : Enemy
{
    public Golem(int health, float speed, Vector2 pos, Vector2 velocity, float chaseRadius, int scale) : base(health, speed, pos, velocity, chaseRadius, scale)
    {
    }

    public override void Attack(GameTime gameTime, PlayerActions data)
    {
        throw new NotImplementedException();
    }

    public override void LoadContent()
    {
        throw new NotImplementedException();
    }
}
