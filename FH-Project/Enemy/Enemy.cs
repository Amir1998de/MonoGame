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
    private float chaseRadius;

    protected Texture2D enemyTexture;
    protected Texture2D[] enemyIdleTexture;
    private float randomDirectionTimer;
    private int enemiesToDefeat;
    private int damage;
    private Random random;

    #endregion Variablen

    public Enemy(int health, float speed, Vector2 pos, Vector2 velocity) : base(health, speed, pos, velocity)
    {
        LoadContent();
        random = new();
        // AddEnemy(this);
        enemiesToDefeat = enemies.Count;
        Globals.Player.AddObserver(this);
        exisits = false;
        chaseRadius = 300f;
        damage = 1;
    }
    public void OnNotify(PlayerActions data)
    {
        if (!(data == PlayerActions.ATTACK)) return;

        int index = -1;


        // /8 weil Scalierung der Waffe auf 0.125
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

    public static void AddEnemy(string existingEnemy, int health, float speed, Vector2 pos, Vector2 velocity)
    {
        EnemyFactory factory = new EnemyFactory();
        enemies.Add(factory.createEnemy(existingEnemy, health, speed, pos, velocity));
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
            if (distanceToPlayer <= chaseRadius)
            {
                ChasePlayer(chaseRadius);
            }
            else
            {
                if (randomDirectionTimer > 1f)
                {
                    randomDirectionTimer = 0f;
                    Vector2 randomDirection = new Vector2((float)random.NextDouble() * 2 - 1, (float)random.NextDouble() * 2 - 1);
                    WanderRandomly(randomDirection);
                }

                if (randomDirectionTimer <= 1f)
                {
                    randomDirectionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }


        if (CollisionHandler.CollisionEntitys(Globals.Player, this))
        {
            if (Globals.Player.CanGetHit)
            {
                Globals.Player.ReduceHealth(damage);
                Globals.Player.CanGetHit = false;
            }

            Debug.WriteLine("HIT! " + GetHashCode().ToString() + "\n " + Globals.Player.Health);
        }
    }

    public void ChasePlayer(float chaseRadius)
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

        enemies.ForEach(enemy =>
        {
            try
            {
                if (Map.GetRoomEnemyIsIn(enemy).Bereich.Contains(newPosition))
                {
                    Position = newPosition;
                }
            }
            catch (NullReferenceException e) { }

        });


    }

}
