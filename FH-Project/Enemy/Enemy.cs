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
using System.Linq;
using System.Security.Cryptography.X509Certificates;

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
    public static int Damage { get; set; } = 1;
    private Random random;
    public Rectangle EnemyBounds { get; set; }
    public static List<Enemydrops> enemydrops { get; private set; } = new List<Enemydrops>();
    private Rectangle arrowBounds;


    #endregion Variablen

    public Enemy(int health, float speed, Vector2 pos, Vector2 velocity, float chaseRadius, int scale) : base(health, speed, pos, velocity, scale)
    {
        LoadContent();
        random = new();
        EnemyBounds = new((int)Position.X, (int)Position.Y, EntityTexture.Width, EntityTexture.Height);
        // AddEnemy(this);
        enemiesToDefeat = enemies.Count;
        Globals.Player.AddObserver(this);
        exisits = false;
        this.chaseRadius = chaseRadius;
        
    }
    public void OnNotify(PlayerActions data)
    {
        if (data == PlayerActions.ATTACK && !IsDestroyed)
        {
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
                AddDrops();

                IsDestroyed = true;
                index = enemies.IndexOf(this);


                if (index != -1)
                    RemoveEnemyAtIndex(index);

                return;
            }

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

    public void AddDrops()
    {
        if (IsDestroyed) return;

        Random random = new Random();

        int rndZahl = random.Next(2);

        if (rndZahl == 0)
        {
            return;
        }

        enemydrops.Add(new Enemydrops(Position));
    }

    public static void RemoveEnemyAtIndex(int index)
    {
        enemies.RemoveAt(index);
    }



    public void Update(GameTime gameTime)
    {

        Room roomPlayerIsIn = Map.GetRoomPlayerIsIn();
        Room roomEnemyIsIn = Map.GetRoomEnemyIsIn(this);
        if (roomPlayerIsIn == null || roomEnemyIsIn == null) return;

        if (enemydrops.Any())
        {
            int index = -1;
            foreach (Enemydrops enemydrop in enemydrops)
            {
                if (Globals.Player.PlayerBounds.Intersects(enemydrop.DropBounds))
                {
                    enemydrop.UseEffect();
                    index = enemydrops.IndexOf(enemydrop);
                }

            }
            if (index != -1)
                enemydrops.RemoveAt(index);
        }

        if (roomPlayerIsIn.Equals(roomEnemyIsIn))
        {
            Attack(gameTime, PlayerActions.NONE);
        }
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
