using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FH_Project;

class Slime : Enemy
{
    public Slime(int health, float speed, Vector2 pos, Vector2 velocity, Texture2D texture,Player player) : base(health, speed, pos, velocity, texture, player)
    {
    }

    public override void Attack()
    {
        throw new NotImplementedException();
    }
}