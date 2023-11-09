using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Numerics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace FH_Project;

 public class EnemyFactory
{

    public EnemyFactory()
    {
    }
    public  Enemy createEnemy(string existingEnemy, int health, float speed, Microsoft.Xna.Framework.Vector2 pos, Microsoft.Xna.Framework.Vector2 velocity, Texture2D textur, Player player
)
    {
        if (existingEnemy.Equals("Slime"))
        {
            return new Slime(health, speed, pos, velocity, textur, player);
        }
        return null;
    }
}

