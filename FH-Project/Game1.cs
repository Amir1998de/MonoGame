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
    
   



    public static ContentManager content;
    private GraphicsDeviceManager _graphics;
    public static SpriteBatch spriteBatch;
    private KeyboardState keyboardState;
    
    public static Viewport viewport;


    
    private Player player;
    

    //Fürs Sprinten
    

    //Fürs Angreiffen
    

    //Enemy
    private Texture2D enemyTexture;
    private Texture2D[] enemyIdleTexture;
    private Enemy enemy;
    private Enemy enemyTwo;
    private EnemyFactory enemyFactory;

    //IdleAnimation Enemy
    private int enemycurrentIdleFrame = 0;
    private int enemytotalIdleFrames = 4;
    
    //Weapon
    private Sword sword;
    private Texture2D swordTexture;
    private Hammer hammer;
    private Texture2D hammerTexture;
    private Bow bow;
    private Texture2D bowTexture;

    //Potion
    private Texture2D healthPotionTexture;
    private Texture2D shieldPotionTexture;
    private Texture2D randomPotionTexture;
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
        Globals.WindowSize = new(1280, 720);
        _graphics.PreferredBackBufferWidth = Globals.WindowSize.X;
        _graphics.PreferredBackBufferHeight = Globals.WindowSize.Y;
        _graphics.ApplyChanges();

        Globals.Content = Content;

        karte = new Map();
        karte.ErstelleZufälligeKarte(4, 8, 80, 160, 80, 160);
        

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        // Test
        Globals.SpriteBatch = spriteBatch;

        //
        GraphicsDevice.LoadPixel();

        
        enemyTexture = Content.Load<Texture2D>("Enemy/Slime/slime-idle-0");
        swordTexture = Content.Load<Texture2D>("Items/Weapon/SwordT2");
        hammerTexture = Content.Load<Texture2D>("Items/Weapon/HammerT2");
        bowTexture = Content.Load<Texture2D>("Items/Weapon/BowT1");
        healthPotionTexture = Content.Load<Texture2D>("Items/Potions/PotionRed");
        shieldPotionTexture = Content.Load<Texture2D>("Items/Potions/PotionBlue");
        randomPotionTexture = Content.Load<Texture2D>("Items/Potions/PotionGreen");

      

        enemyIdleTexture = new Texture2D[enemytotalIdleFrames];
        for (int i = 0; i < enemytotalIdleFrames; i++)
        {
            enemyIdleTexture[i] = Content.Load<Texture2D>($"Enemy/Slime/slime-idle-{i}");
        }

        viewport = GraphicsDevice.Viewport;
        Entity.View(viewport.Width, viewport.Height);

        sword = new Sword(100, 5, swordTexture);
        hammer = new Hammer(100, 5, hammerTexture);
        bow = new Bow(100, 5, bowTexture);

        // player
        player = new Player(100, 200, new(0,0), new Vector2(0, 0), sword);
        player.SetBounds(karte.MapSize, karte.TileSize);
       

        enemyFactory = new EnemyFactory();
        //potion = new HealingPotion(player, healthPotionTexture);

        enemy = enemyFactory.createEnemy("Slime", 700, 2, new Vector2(viewport.Height / 2, viewport.Width / 3), new Vector2(0, 0), enemyTexture, player);
    }

    protected override void Update(GameTime gameTime)
    {

        Globals.KeyboardState = Keyboard.GetState();

        

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

        player.Update(gameTime);

        CalculateTranslation();

       
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

        karte.Draw(spriteBatch,player,50);

        //player.sprite.Draw();

        //potion.Draw();

        player.Draw(spriteBatch);

        //enemy.CheckEnemy(spriteBatch, player);

        spriteBatch.End();

        base.Draw(gameTime);
    }


    public void PlayerAnimation(Entity entity,Texture2D[] animation, GameTime gameTime, float timer, float frameTime, int currentFrame, int totalFrames)
    {
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (timer > frameTime)
        {
            // Zum nächsten Frame wechseln
            currentFrame = (currentFrame + 1) % totalFrames;
            timer = 0f;
        }
        entity.EntityTexture = animation[currentFrame];
    }

   

    private void CalculateTranslation()
    {
       
        var dx = (Globals.WindowSize.X / 2) - player.Sprite.Position.X;

        dx = MathHelper.Clamp(dx, - karte.MapSize.X + Globals.WindowSize.X + (karte.TileSize.X / 2) , karte.TileSize.X / 2);

        var dy = (Globals.WindowSize.Y / 2) - player.Sprite.Position.Y;

        dy = MathHelper.Clamp(dy, -karte.MapSize.Y + Globals.WindowSize.Y + (karte.TileSize.Y / 2), karte.TileSize.Y / 2);

        _translation = Matrix.CreateTranslation(dx, dy, 0f);
    }

    




}