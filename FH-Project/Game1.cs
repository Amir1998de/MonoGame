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

    private Enemy enemy;
    private Enemy enemyTwo;
    private EnemyFactory enemyFactory;

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
    private Potion HealthPotion;
    private Potion ShieldPotion;
    private Potion RandomPotion;

    private Map karte;

    private Camera camera;

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
        Globals.WindowSize = new(1920, 1080);
        _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
        _graphics.ApplyChanges();

        Globals.Content = Content;

        karte = new Map();
        karte.ErstelleZufälligeKarte(4, 400, 800, 400, 800);




        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        Globals.SpriteBatch = spriteBatch;

        GraphicsDevice.LoadPixel();



        swordTexture = Content.Load<Texture2D>("Items/Weapon/SwordT2");
        hammerTexture = Content.Load<Texture2D>("Items/Weapon/HammerT2");
        bowTexture = Content.Load<Texture2D>("Items/Weapon/BowT1");
        healthPotionTexture = Content.Load<Texture2D>("Items/Potions/PotionRed");
        shieldPotionTexture = Content.Load<Texture2D>("Items/Potions/PotionBlue");
        randomPotionTexture = Content.Load<Texture2D>("Items/Potions/PotionGreen");





        viewport = GraphicsDevice.Viewport;
        Entity.View(viewport.Width, viewport.Height);

        sword = new Sword(100, 5, swordTexture);
        hammer = new Hammer(100, 5, hammerTexture);
        bow = new Bow(100, 5, bowTexture);

        player = new Player(100, 100000, new(Globals.WindowSize.X / 2, Globals.WindowSize.Y / 2), new Vector2(0, 0), sword);
        player.SetBounds(karte.MapSize, karte.TileSize);

        camera = new Camera(karte, player);
        camera.Position = player.Position;
        camera.Zoom = 1f;



        enemyFactory = new EnemyFactory();
        //potion = new HealingPotion(player, healthPotionTexture);

        enemy = enemyFactory.createEnemy("Slime", 700, 2, new Vector2(viewport.Height / 2, viewport.Width / 3), new Vector2(0, 0), player);
        HealthPotion = new HealingPotion(player, healthPotionTexture);
        ShieldPotion = new ShieldPotion(player, shieldPotionTexture);
        RandomPotion = new RandomPotion(player, randomPotionTexture);
    }

    protected override void Update(GameTime gameTime)
    {

        Globals.KeyboardState = Keyboard.GetState();

        _direction = Vector2.Zero;

        if (keyboardState.IsKeyDown(Keys.Up)) _direction.Y--;
        if (keyboardState.IsKeyDown(Keys.Down)) _direction.Y++;
        if (keyboardState.IsKeyDown(Keys.Left)) _direction.X--;
        if (keyboardState.IsKeyDown(Keys.Right)) _direction.X++;

        if (_direction != Vector2.Zero) _direction.Normalize();


        Globals.Update(gameTime);
        player.Update(gameTime);
        base.Update(gameTime);
        //CalculateTranslation();
    }


    // Draw
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);



        spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());

        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            player.Weapon.Draw(spriteBatch);

        }

        // Test

        karte.Draw(spriteBatch, player, 50);

        //player.sprite.Draw();

        //potion.Draw();

        player.Draw(spriteBatch);

        //enemy.CheckEnemy(spriteBatch, player);

        spriteBatch.End();

        base.Draw(gameTime);
    }




    private void CalculateTranslation()
    {

        var dx = (Globals.WindowSize.X / 2) - player.Position.X;
        var dy = (Globals.WindowSize.Y / 2) - player.Position.Y;

        dx = MathHelper.Clamp(dx, 0, karte.MapSize.X - Globals.WindowSize.X);
        dy = MathHelper.Clamp(dy, 0, karte.MapSize.Y - Globals.WindowSize.Y);

        _translation = Matrix.CreateTranslation(dx, dy, 0f) * Matrix.CreateScale(1f);
    }






}
