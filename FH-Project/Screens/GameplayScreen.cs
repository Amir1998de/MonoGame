#region File Description


#endregion File Description

#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using GameStateManagement.Fachlogik;
using System;
using System.Threading;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Diagnostics;
#endregion Using Statements

namespace FH_Project;

internal class GameplayScreen : GameScreen
{
    #region Fields
    private SoundManagement soundManagement;
    private KeyboardState currentKeyboardState;
    public static bool isPasue = false;

    private ContentManager content;
    private SpriteFont gameFont;

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

    private SpriteFont font;


    private Camera camera;

    private Matrix _translation;

    private static Vector2 _direction;
    public static Vector2 Direction => _direction;

    #endregion Fields

    #region Initialization


    public GameplayScreen()
    {
        TransitionOnTime = TimeSpan.FromSeconds(1.5);
        TransitionOffTime = TimeSpan.FromSeconds(0.5);
        soundManagement = new SoundManagement();

    }



    public override void LoadContent()
    {
        if (content == null)
            content = new ContentManager(ScreenManager.Game.Services, "Content");

        Globals.Content = content;
        Globals.MouseState = Mouse.GetState();

        swordTexture = content.Load<Texture2D>("Items/Weapon/SwordT2");
        hammerTexture = content.Load<Texture2D>("Items/Weapon/HammerT2");
        bowTexture = content.Load<Texture2D>("Items/Weapon/BowT1");
        healthPotionTexture = content.Load<Texture2D>("Items/Potions/PotionRed");
        shieldPotionTexture = content.Load<Texture2D>("Items/Potions/PotionBlue");
        randomPotionTexture = content.Load<Texture2D>("Items/Potions/PotionGreen");
        Arrow.ArrowTexture = content.Load<Texture2D>("Enemy/Slime/slime-idle-0");
        Enemydrops.EnemyDropTexture = content.Load<Texture2D>("GemBlue");
        font = Globals.Content.Load<SpriteFont>("Verdana");

        sword = new Sword(1, 5, swordTexture);
        hammer = new Hammer(100, 5, hammerTexture);
        bow = new Bow(4, 5, bowTexture);

        Globals.Player = new Player(3, 100000, new(Globals.WindowSize.X / 2, Globals.WindowSize.Y / 2), new Vector2(0, 0), sword, 3);

        Globals.Map = new Map();
        Globals.Map.GenerateMap(5, 4, 6, 4, 6);


        spriteBatch = ScreenManager.SpriteBatch;
        viewport = ScreenManager.GraphicsDevice.Viewport;


        Entity.View(viewport.Width, viewport.Height);

        //player.SetBounds(karte.MapSize, karte.TileSize);

        camera = new Camera(Globals.Map);
        Camera.Position = Globals.Player.Position;
        camera.Zoom = 1f;



        enemyFactory = new EnemyFactory();
        //potion = new HealingPotion(player, healthPotionTexture);

        //enemy = enemyFactory.createEnemy("Slime", 700, 2, new Vector2(viewport.Height / 2, viewport.Width / 3), new Vector2(0, 0));
        HealthPotion = new HealingPotion(player, healthPotionTexture);
        ShieldPotion = new ShieldPotion(player, shieldPotionTexture);
        RandomPotion = new RandomPotion(player, randomPotionTexture);



        base.LoadContent();
        ScreenManager.Game.ResetElapsedTime();
    }

    /// <summary>
    /// Unload graphics content used by the game.
    /// </summary>
    public override void UnloadContent()
    {
        content.Unload();
    }

    #endregion Initialization

    #region Update and Draw

    // update 
    public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                   bool coveredByOtherScreen)
    {
        if (!isPasue)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // update 
            Globals.KeyboardState = Keyboard.GetState();


            _direction = Vector2.Zero;
            //Debug.WriteLine(Map.GetRoomPlayerIsIn().WallCollision());
            if (keyboardState.IsKeyDown(Keys.Up)) _direction.Y--;
            if (keyboardState.IsKeyDown(Keys.Down)) _direction.Y++;
            if (keyboardState.IsKeyDown(Keys.Left)) _direction.X--;
            if (keyboardState.IsKeyDown(Keys.Right)) _direction.X++;

            if (_direction != Microsoft.Xna.Framework.Vector2.Zero) _direction.Normalize();
            Globals.Map.Update(gameTime);


            Enemy.GetEnemies().ForEach(enemy => enemy.Update(gameTime));

            Globals.Player.Update(gameTime);
            Globals.Player.Weapon.Update(gameTime);
            Globals.Update(gameTime);
        }


    }

    // Draw
    public override void Draw(GameTime gameTime)
    {

        ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                           Color.CornflowerBlue, 0, 0);

        spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(Map.GetRoomPlayerIsIn()));

        Globals.Map.Draw();

        if (Globals.MouseState.LeftButton == ButtonState.Pressed && Globals.Player.Weapon.GetType().ToString().Equals("FH_Project.Sword"))
        {
            Globals.Player.Weapon.Draw();

        }


        if (Globals.Player.Weapon.GetType().ToString().Equals("FH_Project.Bow"))
        {
            Globals.Player.Weapon.Draw();
        }




        //player.sprite.Draw();

        //potion.Draw();
        spriteBatch.DrawString(font, Globals.Map.Count.ToString(), new Vector2(400, 10) + Camera.Position, Color.Red);

        Globals.Player.Draw();
        Enemy.DrawAll();

        Globals.Map.DrawEnemyCounter(10, 10, camera);
        Globals.Map.DrawEnemyRoomCounter(140, 10, camera);

        //enemy.CheckEnemy(spriteBatch, player);

        spriteBatch.End();

        base.Draw(gameTime);
    }
    public override void HandleInput(InputState input)
    {

        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        int playerIndex = (int)ControllingPlayer.Value;

        KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
        GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

        bool gamePadDisconnected = !gamePadState.IsConnected &&
                                   input.GamePadWasConnected[playerIndex];

        if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
        {
            isPasue = true;
            ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            Debug.WriteLine("Pause");
        }
        else
        {
            // Otherwise move the player position.
            Vector2 movement = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                movement.X--;
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                movement.X++;
            }

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                movement.Y--;
            }

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                movement.Y++;
            }

            Vector2 thumbstick = gamePadState.ThumbSticks.Left;

            movement.X += thumbstick.X;
            movement.Y -= thumbstick.Y;

            if (movement.Length() > 1)
            {
                movement.Normalize();
            }

            Globals.Player.Position += movement * 2;
        }

    }


    /// <summary>
    /// Draws the gameplay screen.
    /// </summary>


    #endregion Update and Draw


}
