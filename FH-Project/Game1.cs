using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FH_Project;

public class Game1 : Game
{

    private ContentManager content;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch spriteBatch;
    private Texture2D playerTexture;
    private KeyboardState keyboardState;
    private Player player;
    private Viewport viewport;
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
        viewport = GraphicsDevice.Viewport;
        player = new Player(100,10,new Vector2(viewport.Width / 2, viewport.Height - 100),new Vector2(0,0));
        
   
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Left))
            player.MovePlayerLeft(playerTexture);
        else if(keyboardState.IsKeyDown(Keys.Right))
            player.MovePlayerRight(playerTexture);
        else if(keyboardState.IsKeyDown(Keys.Up))
            player.MovePlayerUp(playerTexture);
        else if(keyboardState.IsKeyDown(Keys.Down))
            player.MovePlayerDown(playerTexture);

        

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        spriteBatch.Begin();
        player.DrawPlayer(spriteBatch, playerTexture);
        spriteBatch.End();

        base.Draw(gameTime);
    }
}

