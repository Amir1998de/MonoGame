using FH_Projekt;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;

namespace FH_Project;

public abstract class Enemy : Entity, IObserver
{
    #region Variablen


    public static List<Enemy> enemies { get; private set; } = new List<Enemy>();
    protected bool exisits;
    protected Room room;

    protected Texture2D enemyTexture;
    protected Texture2D[] enemyIdleTexture;
    private int enemiesToDefeat;

    #endregion Variablen

    public Enemy(int health, float speed, Vector2 pos, Vector2 velocity) : base(health, speed, pos, velocity)
    {
        LoadContent();
        // AddEnemy(this);
        enemiesToDefeat = enemies.Count;
        Globals.Player.AddObserver(this);
        exisits = false;

    }
    public void OnNotify(PlayerActions data)
    {
        if (!(data == PlayerActions.ATTACK)) return;

        int index = -1;
        enemies.ForEach(enemy =>
        {
            if (enemy.CheckIfDead())
            {
                enemy.IsDestroyed = true;
                index = enemies.IndexOf(enemy);
                return;
            }

            // /8 weil Scalierung der Waffe auf 0.125
            if (CheckCollision(new Rectangle((int)Globals.Player.Weapon.Position.X, (int)Globals.Player.Weapon.Position.Y, Globals.Player.Weapon.Texture.Width, Globals.Player.Weapon.Texture.Height)))
            {
                ReduceHealth(Globals.Player.Weapon.Damage);
            }
        });
        if (index != -1)
            RemoveEnemyAtIndex(index);

    }

    public static int GetEnemyCount()
    {
        return enemies.Count;
    }

    public static List<Enemy> GetEnemies()
    {
        if (enemies == null)
            throw new NullReferenceException("The 'enemies' list is null.");
        else
            return enemies;
    }

    public static void DrawAll()
    {
        enemies.ForEach(enemy => enemy.Draw());
    }

    public static void AddEnemy(string existingEnemy, int health, float speed, Vector2 pos, Vector2 velocity)
    {
        EnemyFactory factory = new EnemyFactory();
        enemies.Add(factory.createEnemy(existingEnemy, health, speed, pos, velocity));    
    }

    public static void RemoveEnemyAtIndex(int index)
    {
        enemies.RemoveAt(index);
    }

    


}
