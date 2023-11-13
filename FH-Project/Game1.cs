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

    int currentRunFrame = 0;
    int totalRunFrames = 6;
    float frameRunTime = 0.1f;
    float timer = 0f;


    private ContentManager content;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch spriteBatch;
    private KeyboardState keyboardState;
    private KeyboardState prevKeyboardState;
    public static Viewport viewport;

    private Texture2D playerTexture;
    private Texture2D[] playerRunTexture;
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

        playerRunTexture = new Texture2D[totalRunFrames];
        for (int i = 0; i < totalRunFrames; i++)
        {
            playerRunTexture[i] = Content.Load<Texture2D>($"Individual Sprites/adventurer-run-0{i}");
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

        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (timer > frameRunTime)
        {
            currentRunFrame = (currentRunFrame + 1) % totalRunFrames;
            timer = 0f;
        }

        IdleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (IdleTimer > frameIdleTime)
        {
            currentIdleFrame = (currentIdleFrame + 1) % totalIdleFrames;
            IdleTimer = 0f;
        }


        prevKeyboardState = keyboardState;

        //Idle Animation
        if (!keyboardState.IsKeyDown(Keys.Right) &&
            !keyboardState.IsKeyDown(Keys.Left) &&
            !keyboardState.IsKeyDown(Keys.Up) &&
            !keyboardState.IsKeyDown(Keys.Down))
        {
            
            if (!prevKeyboardState.IsKeyDown(Keys.Right) &&
                !prevKeyboardState.IsKeyDown(Keys.Left) &&
                !prevKeyboardState.IsKeyDown(Keys.Up) &&
                !prevKeyboardState.IsKeyDown(Keys.Down))
            {
                // Starte die Animation
                IdleTimer = 0f;
                currentIdleFrame = 0;
            }
            

            //IdleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (IdleTimer > frameIdleTime)
            {
                // Zum nächsten Frame wechseln
                currentIdleFrame = (currentIdleFrame + 1) % totalIdleFrames;
                IdleTimer = 0f;
            }
        }
        
        if (keyboardState.IsKeyDown(Keys.Left))
        {
            player.MovePlayerLeft();
        }
           
        if (keyboardState.IsKeyDown(Keys.Right))
        {
            // Nur die Animation aktualisieren, wenn die Taste zuvor nicht gedrückt war
            if (!prevKeyboardState.IsKeyDown(Keys.Right))
            {
                // Starte die Animation
                timer = 0f;
                currentRunFrame = 0;
            }
            //timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > frameRunTime)
            {
                // Zum nächsten Frame wechseln
                currentRunFrame = (currentRunFrame + 1) % totalRunFrames;
                timer = 0f;
            }
            
            player.MovePlayerRight();
        }
               
         if (keyboardState.IsKeyDown(Keys.Up))
            {
                player.MovePlayerUp();
            }

        if(keyboardState.IsKeyDown(Keys.Down))
        {
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
            PlayerAnimtation(playerIdleTexture, currentIdleFrame);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            PlayerAnimtation(playerRunTexture, currentRunFrame);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            //Links Animation hinzufügen
            PlayerAnimtation(playerRunTexture, currentRunFrame);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            //Oben Animation hinzufügen
            PlayerAnimtation(playerRunTexture, currentRunFrame);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            //Unten Animation hinzufügen
            PlayerAnimtation(playerRunTexture, currentRunFrame);
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


    public void PlayerAnimtation(Texture2D[] animation, int currentFrame)
    {
        player.EntityTexture = animation[currentFrame];
        player.Draw(spriteBatch);
    }


 

}