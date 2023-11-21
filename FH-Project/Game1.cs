using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FH_Project;

public class Game1 : Game
{
    private Texture2D[] playerIdleTexture;
    private int currentIdleFrame = 0;
    private int totalIdleFrames = 4;
    private float frameIdleTime = 0.1f;
    private float idleTimer = 0f;

    private float frameRunTime = 0.1f;
    private float runtimer = 0f;

    //Für Bewegung nach rechts
    private int currentRightFrame = 0;
    private int totalRightFrames = 6;

    //Für Bewegung nach links
    private int currentLeftFrame = 0;
    private int totalLeftFrames = 6;

    //Für Bewegung nach Oben
    private int currentUpFrame = 0;
    private int totalUpFrames = 6;

    //Für Bewegung nach Unten
    private int currentDownFrame = 0;
    private int totalDownFrames = 6;

    //Fürs Sprinten
    private float sprintTimer;
    private bool coolDownForSprint;

    //Fürs Angreiffen
    private float attackTimer;


    public static ContentManager content;
    private GraphicsDeviceManager _graphics;
    public static SpriteBatch spriteBatch;
    private KeyboardState keyboardState;
    private KeyboardState prevKeyboardState;
    public static Viewport viewport;

    private Texture2D playerTexture;

    //Arrays für die jeweiligen Arrays
    private Texture2D[] playerRightTexture;
    private Texture2D[] playerLeftTexture;
    private Texture2D[] playerUpTexture;
    private Texture2D[] playerDownTexture;
    private Player player;

    private Texture2D enemyTexture;
    private Enemy enemy;
    private Enemy enemyTwo;
    private EnemyFactory enemyFactory;

    private Sword sword;
    private Texture2D swordTexture;

    private Hammer hammer;
    private Texture2D hammerTexture;

    private Bow bow;
    private Texture2D bowTexture;

    private Potion potion;

    private Map karte;

    private Matrix _translation;

    private static Vector2 _direction;
    public static Vector2 Direction => _direction;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }


    protected override void Initialize()
    {
  
        // Test
        Globals.WindowSize = new(1024, 768);
        _graphics.PreferredBackBufferWidth = Globals.WindowSize.X;
        _graphics.PreferredBackBufferHeight = Globals.WindowSize.Y;
        _graphics.ApplyChanges();

        Globals.Content = Content;

        karte = new Map();
        karte.ErstelleZufälligeKarte(10, 15, 10, 20, 10, 20);
        coolDownForSprint = false;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        // Test
        Globals.SpriteBatch = spriteBatch;
        //
        GraphicsDevice.LoadPixel();

        playerTexture = Content.Load<Texture2D>("Individual Sprites/adventurer-idle-00");
        enemyTexture = Content.Load<Texture2D>("Individual Sprites/adventurer-idle-00");
        swordTexture = Content.Load<Texture2D>("SwordT2");
        hammerTexture = Content.Load<Texture2D>("HammerT2");
        bowTexture = Content.Load<Texture2D>("BowT1");

        playerRightTexture = new Texture2D[totalRightFrames];
        for (int i = 0; i < totalRightFrames; i++)
        {
            playerRightTexture[i] = Content.Load<Texture2D>($"Individual Sprites/adventurer-run-0{i}");
        }

        playerIdleTexture = new Texture2D[totalIdleFrames];
        for (int i = 0; i < totalIdleFrames; i++)
        {
            playerIdleTexture[i] = Content.Load<Texture2D>($"Individual Sprites/adventurer-idle-0{i}");
        }

        viewport = GraphicsDevice.Viewport;
        Entity.View(viewport.Width, viewport.Height);

        sword = new Sword(100, 5, swordTexture);
        hammer = new Hammer(100, 5, hammerTexture);
        bow = new Bow(100, 5, bowTexture);

        // player
        player = new Player(100, 500, new(0,0), new Vector2(0, 0), playerTexture, sword);
        player.SetBounds(karte.MapSize, karte.TileSize);

        enemyFactory = new EnemyFactory();
        potion = new HealingPotion(50, player);

        enemy = enemyFactory.createEnemy("Slime", 700, 2, new Vector2(viewport.Height / 2, viewport.Width / 3), new Vector2(0, 0), enemyTexture, player);
    }

    protected override void Update(GameTime gameTime)
    {

        keyboardState = Keyboard.GetState();


        runtimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (runtimer > frameRunTime)
        {
            currentRightFrame = (currentRightFrame + 1) % totalRightFrames;
            currentLeftFrame = (currentLeftFrame + 1) % totalLeftFrames;
            currentUpFrame = (currentUpFrame + 1) % totalUpFrames;
            currentDownFrame = (currentDownFrame + 1) % totalDownFrames;
            runtimer = 0f;
        }


        prevKeyboardState = keyboardState;

        if (!keyboardState.IsKeyDown(Keys.Right) &&
            !keyboardState.IsKeyDown(Keys.Left) &&
            !keyboardState.IsKeyDown(Keys.Up) &&
            !keyboardState.IsKeyDown(Keys.Down)) PlayerAnimation(playerIdleTexture, gameTime, runtimer, frameRunTime, currentIdleFrame, totalIdleFrames);
      

        if (keyboardState.IsKeyDown(Keys.Left))
        {
            if (!prevKeyboardState.IsKeyDown(Keys.Left)) ResetAnimation(runtimer, currentRightFrame);
            
            PlayerAnimation(playerRightTexture, gameTime, runtimer, frameRunTime, currentRightFrame, totalRightFrames);
            player.MovePlayerLeft();
        }

        if (keyboardState.IsKeyDown(Keys.Right))
        {
            if (!prevKeyboardState.IsKeyDown(Keys.Right)) ResetAnimation(runtimer, currentRightFrame);

            PlayerAnimation(playerRightTexture, gameTime, runtimer, frameRunTime, currentRightFrame, totalRightFrames);
            player.MovePlayerRight();
        }

        if (keyboardState.IsKeyDown(Keys.Up))
        {
            if (!prevKeyboardState.IsKeyDown(Keys.Up)) ResetAnimation(runtimer, currentRightFrame);
            
            PlayerAnimation(playerRightTexture, gameTime, idleTimer, frameIdleTime, currentRightFrame, totalRightFrames);
            player.MovePlayerUp();
        }

        if (keyboardState.IsKeyDown(Keys.Down))
        {
            if (!prevKeyboardState.IsKeyDown(Keys.Down)) ResetAnimation(runtimer, currentRightFrame);
            
            PlayerAnimation(playerRightTexture, gameTime, runtimer, frameRunTime, currentRightFrame, totalRightFrames);
            player.MovePlayerDown();
        }


        

        if (keyboardState.IsKeyDown(Keys.H)) Inventory.UseItem(potion);

        if (keyboardState.IsKeyDown(Keys.LeftShift) && !coolDownForSprint)
        {
            if (sprintTimer <= 0f || sprintTimer <= 3f)
            {
                player.Speed = 4;
                sprintTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if(sprintTimer >= 3f) coolDownForSprint = true;
            
        }
        else player.Speed = player.BaseSpeed;

        if (coolDownForSprint)
        {
            sprintTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (sprintTimer < 0f)
            {
                sprintTimer = 0f;
                coolDownForSprint = false;
            }
        }



        if (keyboardState.IsKeyDown(Keys.Space) && player.CanAttack())
        {
            player.Attack();
        }

        if (player.IsAttacking && attackTimer >= 0.5f)
        {
            player.IsAttacking = false;
            attackTimer = 0f;
        }

        if(player.IsAttacking) attackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Test 

        Globals.Update(gameTime);

        _direction = Vector2.Zero;

        if (keyboardState.IsKeyDown(Keys.Up)) _direction.Y--;
        if (keyboardState.IsKeyDown(Keys.Down)) _direction.Y++;
        if (keyboardState.IsKeyDown(Keys.Left)) _direction.X--;
        if (keyboardState.IsKeyDown(Keys.Right)) _direction.X++;

        if (_direction != Vector2.Zero)
        {
            _direction.Normalize();
        }

        player.Update();

        CalculateTranslation();

       Debug.WriteLine(enemy.getHealth());
       Debug.WriteLine(attackTimer);
      //  Debug.WriteLine(_translation);

        base.Update(gameTime);
    }


    // Draw
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);



        spriteBatch.Begin(transformMatrix: _translation);

        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            player.Weapon.Draw(spriteBatch);
            
        }

        // Test

        karte.Draw(spriteBatch);

        //player.sprite.Draw();

        player.Draw(spriteBatch);

        enemy.CheckEnemy(spriteBatch, player);

        spriteBatch.End();

        base.Draw(gameTime);
    }


    public void PlayerAnimation(Texture2D[] animation, GameTime gameTime, float timer, float frameTime, int currentFrame, int totalFrames)
    {
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (timer > frameTime)
        {
            // Zum nächsten Frame wechseln
            currentFrame = (currentFrame + 1) % totalFrames;
            timer = 0f;
        }
        player.EntityTexture = animation[currentFrame];
    }

    public void ResetAnimation(float Runtimer, int currentFrame)
    {
        Runtimer = 0f;
        currentFrame = 0;
    }

    private void CalculateTranslation()
    {
       
        var dx = (Globals.WindowSize.X / 2) - player.sprite.Position.X;

        dx = MathHelper.Clamp(dx, - karte.MapSize.X + Globals.WindowSize.X + (karte.TileSize.X / 2) , karte.TileSize.X / 2);

        var dy = (Globals.WindowSize.Y / 2) - player.sprite.Position.Y;

        dy = MathHelper.Clamp(dy, -karte.MapSize.Y + Globals.WindowSize.Y + (karte.TileSize.Y / 2), karte.TileSize.Y / 2);

        _translation = Matrix.CreateTranslation(dx, dy, 0f);
    }




}