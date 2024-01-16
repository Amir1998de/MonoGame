using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using System.Diagnostics;


namespace FH_Project;

public class ScreenManager : DrawableGameComponent
{
    #region Fields

    private List<GameScreen> screens = new List<GameScreen>();
    private List<GameScreen> screensToUpdate = new List<GameScreen>();

    private InputState input = new InputState();

    private SpriteFont font;
    private Texture2D blankTexture;

    private bool isInitialized;

    private bool traceEnabled;

    // 
    public static ContentManager content;

    public static SpriteBatch spriteBatch;
    private KeyboardState keyboardState;
    public static Viewport viewport;
    private Player player;



    private Map karte;

    private Camera camera;

    private Matrix _translation;

    private static Microsoft.Xna.Framework.Vector2 _direction;
    public static Microsoft.Xna.Framework.Vector2 Direction => _direction;

    #endregion Fields

    #region Properties

    /// <summary>
    /// A default SpriteBatch shared by all the screens. This saves
    /// each screen having to bother creating their own local instance.
    /// </summary>
    public SpriteBatch SpriteBatch
    {
        get { return spriteBatch; }
    }



    /// <summary>
    /// A default font shared by all the screens. This saves
    /// each screen having to bother loading their own local copy.
    /// </summary>
    public SpriteFont Font
    {
        get { return font; }
    }

    /// <summary>
    /// If true, the manager prints out a list of all the screens
    /// each time it is updated. This can be useful for making sure
    /// everything is being added and removed at the right times.
    /// </summary>
    public bool TraceEnabled
    {
        get { return traceEnabled; }
        set { traceEnabled = value; }
    }

    #endregion Properties

    #region Initialization

    /// <summary>
    /// Constructs a new screen manager component.
    /// </summary>
    public ScreenManager(Game game)
        : base(game)
    {

        TouchPanel.EnabledGestures = GestureType.None;
    }

    /// <summary>
    /// Initializes the screen manager component.
    /// </summary>
    public override void Initialize()
    {

        base.Initialize();
        isInitialized = true;

    }

    /// <summary>
    /// Load your graphics content.
    /// </summary>
    protected override void LoadContent()
    {

        // Load content belonging to the screen manager.
        ContentManager content = Game.Content;

        spriteBatch = new SpriteBatch(GraphicsDevice);
        font = content.Load<SpriteFont>("menufont");

        blankTexture = content.Load<Texture2D>("blank");

        Globals.SpriteBatch = spriteBatch;
        GraphicsDevice.LoadPixel();

        // Tell each of the screens to load their content.
        foreach (GameScreen screen in screens)
        {
            screen.LoadContent();
        }

    }

    /// <summary>
    /// Unload your graphics content.
    /// </summary>
    protected override void UnloadContent()
    {
        // Tell each of the screens to unload their content.
        foreach (GameScreen screen in screens)
        {
            screen.UnloadContent();
        }
    }

    #endregion Initialization

    #region Update and Draw

    /// <summary>
    /// Allows each screen to run logic.
    /// </summary>
    public override void Update(GameTime gameTime)
    {
        // Read the keyboard and gamepad.
        input.Update();

        // Make a copy of the master screen list, to avoid confusion if
        // the process of updating one screen adds or removes others.
        screensToUpdate.Clear();

        foreach (GameScreen screen in screens)
            screensToUpdate.Add(screen);

        bool otherScreenHasFocus = !Game.IsActive;
        bool coveredByOtherScreen = false;

        // Loop as long as there are screens waiting to be updated.
        while (screensToUpdate.Count > 0)
        {
            // Pop the topmost screen off the waiting list.
            GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];

            screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

            // Update the screen.
            screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (screen.ScreenState == ScreenState.TransitionOn ||
                screen.ScreenState == ScreenState.Active)
            {
                // If this is the first active screen we came across,
                // give it a chance to handle input.
                if (!otherScreenHasFocus)
                {
                    screen.HandleInput(input);

                    otherScreenHasFocus = true;
                }

                // If this is an active non-popup, inform any subsequent
                // screens that they are covered by it.
                if (!screen.IsPopup)
                    coveredByOtherScreen = true;
            }
        }

        // Print debug trace?
        if (traceEnabled)
            TraceScreens();
       
    }

    /// <summary>
    /// Prints a list of all the screens, for debugging.
    /// </summary>
    private void TraceScreens()
    {
        List<string> screenNames = new List<string>();

        foreach (GameScreen screen in screens)
            screenNames.Add(screen.GetType().Name);

        //Debug.WriteLine(string.Join(", ", screenNames.ToArray()));
    }

    /// <summary>
    /// Tells each screen to draw itself.
    /// </summary>
    public override void Draw(GameTime gameTime)
    {
        foreach (GameScreen screen in screens)
        {
            if (screen.ScreenState == ScreenState.Hidden)
                continue;

            screen.Draw(gameTime);
        }


        
    }

    #endregion Update and Draw

    #region Public Methods

    /// <summary>
    /// Adds a new screen to the screen manager.
    /// </summary>
    public void AddScreen(GameScreen screen, PlayerIndex? controllingPlayer)
    {
        screen.ControllingPlayer = controllingPlayer;
        screen.ScreenManager = this;
        screen.IsExiting = false;

        // If we have a graphics device, tell the screen to load content.
        if (isInitialized)
        {
            screen.LoadContent();
        }

        screens.Add(screen);

        // update the TouchPanel to respond to gestures this screen is interested in
        TouchPanel.EnabledGestures = screen.EnabledGestures;
    }

    /// <summary>
    /// Removes a screen from the screen manager. You should normally
    /// use GameScreen.ExitScreen instead of calling this directly, so
    /// the screen can gradually transition off rather than just being
    /// instantly removed.
    /// </summary>
    public void RemoveScreen(GameScreen screen)
    {
        // If we have a graphics device, tell the screen to unload content.
        if (isInitialized)
        {
            screen.UnloadContent();
        }

        screens.Remove(screen);
        screensToUpdate.Remove(screen);

        // if there is a screen still in the manager, update TouchPanel
        // to respond to gestures that screen is interested in.
        if (screens.Count > 0)
        {
            TouchPanel.EnabledGestures = screens[screens.Count - 1].EnabledGestures;
        }
    }

    /// <summary>
    /// Expose an array holding all the screens. We return a copy rather
    /// than the real master list, because screens should only ever be added
    /// or removed using the AddScreen and RemoveScreen methods.
    /// </summary>
    public GameScreen[] GetScreens()
    {
        return screens.ToArray();
    }

    /// <summary>
    /// Helper draws a translucent black fullscreen sprite, used for fading
    /// screens in and out, and for darkening the background behind popups.
    /// </summary>
    public void FadeBackBufferToBlack(float alpha)
    {
        Viewport viewport = GraphicsDevice.Viewport;

        spriteBatch.Begin();

        spriteBatch.Draw(blankTexture,
                         new Rectangle(0, 0, viewport.Width, viewport.Height),
                         Color.Black * alpha);

        spriteBatch.End();
    }

    #endregion Public Methods


    private void CalculateTranslation()
    {

        var dx = (Globals.WindowSize.X / 2) - player.Position.X;
        var dy = (Globals.WindowSize.Y / 2) - player.Position.Y;

        dx = MathHelper.Clamp(dx, 0, karte.MapSize.X - Globals.WindowSize.X);
        dy = MathHelper.Clamp(dy, 0, karte.MapSize.Y - Globals.WindowSize.Y);

        _translation = Matrix.CreateTranslation(dx, dy, 0f) * Matrix.CreateScale(1f);
    }
}





