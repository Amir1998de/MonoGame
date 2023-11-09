using FH_Projekt;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public abstract class Enemy : Entity, IObserver
{
    #region Variablen
    private float attackPower;
    private static List<Enemy> enemies = new List<Enemy>();
    private Player player;

    #endregion Variablen

    public Enemy(int health, float speed, Vector2 pos, Vector2 velocity, Texture2D textur, Player player) : base(health, speed,pos,velocity, textur)
	{
        enemies.Add(this);
        this.player = player;
        this.player.AddObserver(this);
	}

    

    public void OnNotify(PlayerActions data)
    {
        if (!(data == PlayerActions.Attack)) return;


        enemies.ForEach(enemy =>
        {
            if (enemy.CheckIfDead())
            {
                enemy.IsDestroyed = true;
                return;
            }

            if (CheckCollision(new Rectangle((int)player.Weapon.Position.X, (int)player.Weapon.Position.Y, player.Weapon.Texture.Width / 4, player.Weapon.Texture.Height / 4 )))
            {
                ReduceHealth(player.Weapon.Damage);
            }
        });

       
    }
}
