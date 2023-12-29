#region File Description

//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

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
#endregion Using Statements

namespace FH_Project;
/// <summary>
/// This screen implements the actual game logic. It is just a
/// placeholder to get the idea across: you'll probably want to
/// put some more interesting gameplay in here!
/// </summary>
internal class GameplayScreen : GameScreen
{
    #region Fields
    private SoundManagement soundManagement;
    private KeyboardState currentKeyboardState;


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

    private Map karte;

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
        karte = new Map();
        karte.ErstelleZufälligeKarte(4, 4, 8, 3, 6);

        spriteBatch = ScreenManager.SpriteBatch;
        viewport = ScreenManager.GraphicsDevice.Viewport;

        swordTexture = content.Load<Texture2D>("Items/Weapon/SwordT2");
        hammerTexture = content.Load<Texture2D>("Items/Weapon/HammerT2");
        bowTexture = content.Load<Texture2D>("Items/Weapon/BowT1");
        healthPotionTexture = content.Load<Texture2D>("Items/Potions/PotionRed");
        shieldPotionTexture = content.Load<Texture2D>("Items/Potions/PotionBlue");
        randomPotionTexture = content.Load<Texture2D>("Items/Potions/PotionGreen");

        Entity.View(viewport.Width, viewport.Height);

        sword = new Sword(100, 5, swordTexture);
        hammer = new Hammer(100, 5, hammerTexture);
        bow = new Bow(100, 5, bowTexture);

        player = new Player(100, 100000, new(Globals.WindowSize.X / 2, Globals.WindowSize.Y / 2), new Vector2(0, 0), sword);
        //player.SetBounds(karte.MapSize, karte.TileSize);

        camera = new Camera(karte, player);
        camera.Position = player.Position;
        camera.Zoom = 1f;



        enemyFactory = new EnemyFactory();
        //potion = new HealingPotion(player, healthPotionTexture);

        enemy = enemyFactory.createEnemy("Slime", 700, 2, new Vector2(viewport.Height / 2, viewport.Width / 3), new Vector2(0, 0), player);
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


    public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                   bool coveredByOtherScreen)
    {

        base.Update(gameTime, otherScreenHasFocus, false);

        // update 
        Globals.KeyboardState = Keyboard.GetState();


        _direction = Vector2.Zero;

        if (keyboardState.IsKeyDown(Keys.Up)) _direction.Y--;
        if (keyboardState.IsKeyDown(Keys.Down)) _direction.Y++;
        if (keyboardState.IsKeyDown(Keys.Left)) _direction.X--;
        if (keyboardState.IsKeyDown(Keys.Right)) _direction.X++;

        if (_direction != Microsoft.Xna.Framework.Vector2.Zero) _direction.Normalize();


        Globals.Update(gameTime);
        player.Update(gameTime);
        //CalculateTranslation();


    }

    public override void HandleInput(InputState input)
    {

        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        

    }


    /// <summary>
    /// Draws the gameplay screen.
    /// </summary>
    public override void Draw(GameTime gameTime)
    {

        ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                           Color.CornflowerBlue, 0, 0);

        spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(Map.GetRoomPlayerIsIn(player)));

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
    public void DrawScore()
    {
    }
    public void DrawBackground()
    {
    }

    #endregion Update and Draw

    private void CalculateTranslation()
    {

        var dx = (Globals.WindowSize.X / 2) - player.Position.X;
        var dy = (Globals.WindowSize.Y / 2) - player.Position.Y;

        dx = MathHelper.Clamp(dx, 0, karte.MapSize.X - Globals.WindowSize.X);
        dy = MathHelper.Clamp(dy, 0, karte.MapSize.Y - Globals.WindowSize.Y);

        _translation = Matrix.CreateTranslation(dx, dy, 0f) * Matrix.CreateScale(1f);
    }
}
