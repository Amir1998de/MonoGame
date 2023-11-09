using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FH_Project;

public class Game1 : Game
{
    int currentRunFrame = 0;
    int totalRunFrames = 15; 
    float frameRunTime = 0.1f; 
    float timer = 0f;


    private ContentManager content;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch spriteBatch;
    private KeyboardState keyboardState;
    private KeyboardState prevKeyboardState;
    private Viewport viewport;

    private Texture2D playerTexture;
    private Texture2D[] playerRunTexture;
    private Player player;

    private Texture2D enemyTexture;
    private Enemy enemy;
    private Enemy enemyTwo;

    private Sword sword;
    private Texture2D swordTexture;

    private Hammer hammer;
    private Texture2D hammerTexture;

    private Bow bow;
    private Texture2D bowTexture;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);





        playerTexture = Content.Load<Texture2D>("Idle (1)");
        enemyTexture = Content.Load<Texture2D>("Idle (1)");
        swordTexture = Content.Load<Texture2D>("SwordT2");
        hammerTexture = Content.Load<Texture2D>("HammerT2");
        bowTexture = Content.Load<Texture2D>("BowT1");

        playerRunTexture = new Texture2D[totalRunFrames];
        for (int i = 0; i < totalRunFrames; i++)
        {
            playerRunTexture[i] = Content.Load<Texture2D>($"Run ({i + 1})");
        }




        viewport = GraphicsDevice.Viewport;
        Entity.View(viewport.Width, viewport.Height);
        sword = new Sword(100, 5, swordTexture);
        hammer = new Hammer(100, 5, hammerTexture);
        bow = new Bow(100, 5, bowTexture);
        player = new Player(100,2,new Vector2(0, 0),new Vector2(0,0), playerTexture, sword);
        enemy = new Slime(10000, 2, new Vector2(viewport.Height/2, viewport.Width/3), new Vector2(0, 0), enemyTexture,player);
        enemyTwo = new Slime(10000, 2, new Vector2(viewport.Height/4, viewport.Width / 3), new Vector2(0, 0), enemyTexture, player);


        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        keyboardState = Keyboard.GetState();

        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (timer > frameRunTime)
        {
            // Zum nächsten Frame wechseln
            currentRunFrame = (currentRunFrame + 1) % totalRunFrames;
            timer = 0f;
        }
       

        prevKeyboardState = keyboardState;


        if (keyboardState.IsKeyDown(Keys.Left))
        {
            player.MovePlayerLeft();
        }


        if(keyboardState.IsKeyDown(Keys.Right))
        {
            {
                // Nur die Animation aktualisieren, wenn die Taste zuvor nicht gedrückt war
                if (prevKeyboardState == null || !prevKeyboardState.IsKeyDown(Keys.Right))
                {
                    // Starte die Animation
                    timer = 0f;
                    currentRunFrame = 0;
                }

                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (timer > frameRunTime)
                {
                    // Zum nächsten Frame wechseln
                    currentRunFrame = (currentRunFrame + 1) % totalRunFrames;
                    timer = 0f;
                }
            }
            player.MovePlayerRight();

        }


        if(keyboardState.IsKeyDown(Keys.Up))
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

        

        base.Update(gameTime);
    }

    

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);



        spriteBatch.Begin();

        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            PlayerAnimtation(playerRunTexture, currentRunFrame);
        }
        else
            PlayerIdle();


        if (!enemy.IsDestroyed)
        {
            enemy.Draw(spriteBatch);

        }
        if(!enemyTwo.IsDestroyed)
        {
            enemyTwo.Draw(spriteBatch);
        }
        player.Weapon.Draw(spriteBatch);
        spriteBatch.End();

        base.Draw(gameTime);
    }

    public void PlayerAnimtation(Texture2D[] animation, int currentFrame)
    {
            player.EntityTexture = animation[currentFrame];
            player.Draw(spriteBatch);
    }

    public void PlayerIdle()
    {
        player.EntityTexture = playerTexture;
        player.Draw(spriteBatch);
    }

}

