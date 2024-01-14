using FH_Project;
using FH_Projekt;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;

namespace FH_Project;

public class EnemyFactory
{

    public EnemyFactory()
    {
    }
    public Enemy createEnemy(string existingEnemy, int health, float speed, Vector2 pos, Vector2 velocity, float chaseRadius, int scale)
    {
        if (existingEnemy.Equals("Slime"))
        {
            return new Slime(health, speed, pos, velocity, chaseRadius, scale);
        }

        if (existingEnemy.Equals("Skeleton"))
        {
            return new Skeleton(health, speed, pos, velocity, chaseRadius, scale);
        }

        if (existingEnemy.Equals("Wolf"))
        {
            return new Wolf(health,speed,pos,velocity, chaseRadius, scale);
        }
        return null;
    }
}
