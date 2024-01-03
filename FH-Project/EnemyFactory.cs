using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;

namespace FH_Project;

public class EnemyFactory
{

    public EnemyFactory()
    {
    }
    public Enemy createEnemy(string existingEnemy, int health, float speed, Vector2 pos, Vector2 velocity)
    {
        if (existingEnemy.Equals("Slime"))
        {
            return new Slime(health, speed, pos, velocity);
        }
        return null;
    }    
}
