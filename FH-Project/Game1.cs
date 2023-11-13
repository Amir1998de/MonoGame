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
    int currentIdleFrame = 0;
    int totalIdleFrames = 4;
    float frameIdleTime = 0.1f;
    float IdleTimer = 0f;

    float frameRunTime = 0.1f;
    float Runtimer = 0f;

    //Für Bewegung nach rechts
    int currentRightFrame = 0;
    int totalRightFrames = 6;

    //Für Bewegung nach links
    int currentLeftFrame = 0;
    int totalLeftFrames = 6;

    //Für Bewegung nach Oben
    int currentUpFrame = 0;
    int totalUpFrames = 6;

    //Für Bewegung nach Unten
    int currentDownFrame = 0;
    int totalDownFrames = 6;


    private ContentManager content;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch spriteBatch;
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
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        karte = new Map();
        karte.ErstelleZufälligeKarte(10, 15, 10, 20, 10, 20);



        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

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
        player = new Player(100, 2, new Vector2(0, 0), new Vector2(0, 0), playerTexture, sword);

        enemyFactory = new EnemyFactory();
        potion = new HealingPotion(50, player);

        enemy = enemyFactory.createEnemy("Slime", 700, 2, new Vector2(viewport.Height / 2, viewport.Width / 3), new Vector2(0, 0), enemyTexture, player);

        //  enemy = new Slime(10000, 2, new Vector2(viewport.Height / 2, viewport.Width / 3), new Vector2(0, 0), enemyTexture, player);
        //  enemyTwo = new Slime(10000, 2, new Vector2(viewport.Height / 4, viewport.Width / 3), new Vector2(0, 0), enemyTexture, player);


        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        keyboardState = Keyboard.GetState();

        
        Runtimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (Runtimer > frameRunTime)
        {
            currentRightFrame = (currentRightFrame + 1) % totalRightFrames;
            currentLeftFrame = (currentLeftFrame + 1) % totalLeftFrames;
            currentUpFrame = (currentUpFrame + 1) % totalUpFrames;
            currentDownFrame = (currentDownFrame + 1) % totalDownFrames;
            Runtimer = 0f;
        }


        prevKeyboardState = keyboardState;

        //Idle Animation
        if (!keyboardState.IsKeyDown(Keys.Right) &&
            !keyboardState.IsKeyDown(Keys.Left) &&
            !keyboardState.IsKeyDown(Keys.Up) &&
            !keyboardState.IsKeyDown(Keys.Down))
        {

            // Starte die Animation

            PlayerIdleAnimation(playerIdleTexture, gameTime);
        }
        
        if (keyboardState.IsKeyDown(Keys.Left))
        {
            // Nur die Animation aktualisieren, wenn die Taste zuvor nicht gedrückt war
            if (!prevKeyboardState.IsKeyDown(Keys.Left))
            {
                // Starte die Animation
                ResetAnimation(Runtimer, currentRightFrame);
            }

            PlayerRunAnimation(playerRightTexture, gameTime, Runtimer, frameRunTime, currentRightFrame, totalRightFrames);
            player.MovePlayerLeft();
        }
           
        if (keyboardState.IsKeyDown(Keys.Right))
        {
            // Nur die Animation aktualisieren, wenn die Taste zuvor nicht gedrückt war
            if (!prevKeyboardState.IsKeyDown(Keys.Right))
            {
                // Starte die Animation
                ResetAnimation(Runtimer, currentRightFrame);

            }
            PlayerRunAnimation(playerRightTexture, gameTime, Runtimer, frameRunTime, currentRightFrame, totalRightFrames);
            player.MovePlayerRight();
        }
               
         if (keyboardState.IsKeyDown(Keys.Up))
            {
            // Nur die Animation aktualisieren, wenn die Taste zuvor nicht gedrückt war
            if (!prevKeyboardState.IsKeyDown(Keys.Up))
            {
                // Starte die Animation
                ResetAnimation(Runtimer, currentRightFrame);
            }
            PlayerRunAnimation(playerRightTexture, gameTime, Runtimer, frameRunTime, currentRightFrame, totalRightFrames);
            player.MovePlayerUp();
            }

        if(keyboardState.IsKeyDown(Keys.Down))
        {
            // Nur die Animation aktualisieren, wenn die Taste zuvor nicht gedrückt war
            if (!prevKeyboardState.IsKeyDown(Keys.Down))
            {
                // Starte die Animation
                ResetAnimation(Runtimer, currentRightFrame);
            }
            PlayerRunAnimation(playerRightTexture, gameTime, Runtimer, frameRunTime, currentRightFrame, totalRightFrames);
            player.MovePlayerDown();
        }
                  
     
        if (keyboardState.IsKeyDown(Keys.Space) && player.CanAttack())
        {
            player.Attack();
        }

        if (keyboardState.IsKeyDown(Keys.H))
            Inventory.UseItem(potion);



        base.Update(gameTime);
    }



    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);



        spriteBatch.Begin();

        if(!Keyboard.GetState().IsKeyDown(Keys.Right) &&
            !Keyboard.GetState().IsKeyDown(Keys.Left) &&
            !Keyboard.GetState().IsKeyDown(Keys.Up) &&
            !Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            player.Draw(spriteBatch);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            player.Draw(spriteBatch);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            //Links Animation hinzufügen
            player.Draw(spriteBatch);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            //Oben Animation hinzufügen
            player.Draw(spriteBatch);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            //Unten Animation hinzufügen
            player.Draw(spriteBatch);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            player.Weapon.Draw(spriteBatch);
        }


       

        enemy.CheckEnemy(spriteBatch, player);

        karte.Draw(spriteBatch);

        spriteBatch.End();

        base.Draw(gameTime);
    }


    public void PlayerRunAnimation(Texture2D[] animation, GameTime gameTime, float timer, float frameTime, int currentFrame, int totalFrames)
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

    public void PlayerIdleAnimation(Texture2D[] animation, GameTime gameTime)
    {
        IdleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (IdleTimer > frameIdleTime)
        {
            // Zum nächsten Frame wechseln
            currentIdleFrame = (currentIdleFrame + 1) % totalIdleFrames;
            IdleTimer = 0f;
        }
        player.EntityTexture = animation[currentIdleFrame];
    }

    public void ResetAnimation(float Runtimer, int currentFrame)
    {
        Runtimer = 0f;
        currentFrame = 0;
    }




}