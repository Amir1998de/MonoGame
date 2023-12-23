using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;



namespace FH_Project;


    public class Game1 : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager _graphics;
    private ScreenManager screenManager;


    private static readonly string[] preloadAssets =
{
            "gradient",
        };

    public Game1()
    {
        Content.RootDirectory = "Content";

        _graphics = new GraphicsDeviceManager(this);
        
        IsMouseVisible = true;

        // Create the screen manager component.
        screenManager = new ScreenManager(this);

        Components.Add(screenManager);


      // Activate the first screens.
      screenManager.AddScreen(new BackgroundScreen(), null);
      screenManager.AddScreen(new MainMenuScreen(), null);
    }


    protected override void Initialize()
    {

        Globals.WindowSize = new(1920, 1080);
      //  _graphics.PreferredBackBufferWidth = 1920;
      //  _graphics.PreferredBackBufferHeight = 1080;
         _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
          _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
        _graphics.ApplyChanges();



        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Loads graphics content.
        foreach (string asset in preloadAssets)
        {
            Content.Load<object>(asset);
        }

       
    }

    // Draw
    protected override void Draw(GameTime gameTime)
    {
        _graphics.GraphicsDevice.Clear(Color.Black);
        base.Draw(gameTime);
    }

    internal static class Program
    {
        private static void Main()
        {
            using (Game1 game = new Game1())
            {
                game.Run();

            }
        }


        }
 }