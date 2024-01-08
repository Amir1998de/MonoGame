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
using System.Reflection.Metadata.Ecma335;

namespace FH_Project;

public abstract class Enemy : Entity, IObserver
{
    #region Variablen


    public static List<Enemy> enemies { get; private set; } = new List<Enemy>();
    protected bool exisits;
    protected float chaseRadius;

    protected Texture2D enemyTexture;
    protected Texture2D[] enemyIdleTexture;
    private float randomDirectionTimer;
    private int enemiesToDefeat;
    protected int damage;
    private Random random;
    protected Rectangle enemyBounds;
   

    #endregion Variablen

    public Enemy(int health, float speed, Vector2 pos, Vector2 velocity, float chaseRadius ,int scale) : base(health, speed, pos, velocity, scale)
    {
        LoadContent();
        random = new();
        enemyBounds = new((int)Position.X,(int)Position.Y,EntityTexture.Width,EntityTexture.Height);
        // AddEnemy(this);
        enemiesToDefeat = enemies.Count;
        Globals.Player.AddObserver(this);
        exisits = false;
        this.chaseRadius = chaseRadius;
        damage = 1;
    }
    public void OnNotify(PlayerActions data)
    {
        if (!(data == PlayerActions.ATTACK)) return;

        int index = -1;


        // /8 weil Scalierung der Waffe auf 0.125
        //Debug.WriteLine("test");
        if (CheckCollision(new Rectangle((int)Globals.Player.Weapon.Position.X, (int)Globals.Player.Weapon.Position.Y, Globals.Player.Weapon.Texture.Width, Globals.Player.Weapon.Texture.Height)))
        { 
              ReduceHealth(Globals.Player.Weapon.Damage);
             Debug.WriteLine(GetHashCode() + " " + Health);
        }

        if (CheckIfDead())
        {
            IsDestroyed = true;
            index = enemies.IndexOf(this);
            if (index != -1)
                RemoveEnemyAtIndex(index);
            return;
        }


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

    public static Enemy AddEnemy(string existingEnemy, int health, float speed, Vector2 pos, Vector2 velocity, float chaseRadius, int scale)
    {
        EnemyFactory factory = new EnemyFactory();
        Enemy enemy = factory.createEnemy(existingEnemy, health, speed, pos, velocity, chaseRadius, scale);
        enemies.Add(enemy);
        return enemy;
    }

    public static void RemoveEnemyAtIndex(int index)
    {
        enemies.RemoveAt(index);
    }

    public void Update(GameTime gameTime)
    {


        if (Globals.Player != null)
        {
            float distanceToPlayer = Vector2.Distance(Position, Globals.Player.Position);

            // Überprüfen Sie, ob der Spieler in der Nähe ist
            if (distanceToPlayer <= chaseRadius && Map.GetRoomPlayerIsIn().Bereich.Intersects(enemyBounds))
            {
                ChasePlayer();
            }
            else
            {
                /*if (randomDirectionTimer > 1f)
                {
                    randomDirectionTimer = 0f;
                    Vector2 randomDirection = new Vector2((float)random.NextDouble() * 2 - 1, (float)random.NextDouble() * 2 - 1);
                    WanderRandomly(randomDirection);
                }

                if (randomDirectionTimer <= 1f)
                {
                    randomDirectionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }*/
            }
        }

        Attack(gameTime);
        enemyBounds.X = (int)Position.X;
        enemyBounds.Y = (int)Position.Y;
    }

    public void ChasePlayer()
    {
        Vector2 direction = Vector2.Normalize(Globals.Player.Position - Position);
        Position += direction * Speed;
    }

    



    public void WanderRandomly(Vector2 randomDirection)
    {


        randomDirection.Normalize();

        // Neue Position basierend auf zufälliger Richtung und Wander-Geschwindigkeit berechnen
        Vector2 newPosition = Position + randomDirection * 2;

        // Überprüfen Sie, ob die neue Position innerhalb der Raumgrenzen liegt

        /*enemies.ForEach(enemy =>
        {
            try
            {
                if (Map.GetRoomEnemyIsIn(enemy).Bereich.Contains(newPosition))
                {
                    Position = newPosition;
                }
            }
            catch(NullReferenceException e){}
            
        });*/


    }

}
