
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

    public override void CheckEnemy(SpriteBatch spriteBatch, Player player)
    {

        if (!IsDestroyed)
        {
            Draw(spriteBatch);
            player.Weapon.Draw(spriteBatch);
         }
        Debug.WriteLine($" this.health: {health}");
        if (Enemy.GetEnemyCount() == 0 )
         {
            float randomX = rnd.Next(0, Game1.viewport.Width -20);
            float randomY = rnd.Next(0, Game1.viewport.Height - 20);

            Postion = new Vector2(randomX, randomY);

            IsDestroyed = false;
            health = 700;
            AddEnemy();


            //  Debug.WriteLine($"counter: {enemyCount}");
            //  Debug.WriteLine("mybase: Type is {0}", this.GetType());
        }

      
    }


}