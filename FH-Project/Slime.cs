
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace FH_Project;
class Slime : Enemy
{
    Random rnd = new Random();


    public Slime(int health, float speed, Vector2 pos, Vector2 velocity, Texture2D texture,Player player) : base(health, speed, pos, velocity, texture, player)
    {
    }
    public override void Attack()
    {
        throw new NotImplementedException();
    }

   

}