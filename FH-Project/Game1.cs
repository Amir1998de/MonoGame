using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
    private Texture2D enemyTexture;
    private Enemy enemy;
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

        viewport = GraphicsDevice.Viewport;
        player = new Player(100,2,new Vector2(0, 0),new Vector2(0,0), playerTexture, viewport.Height, viewport.Width);
        enemy = new Slime(100, 2, new Vector2(viewport.Height/2, viewport.Width/3), new Vector2(0, 0), enemyTexture, viewport.Height, viewport.Width,player);
        
   
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Left))
            player.MovePlayerLeft();
        if(keyboardState.IsKeyDown(Keys.Right))
            player.MovePlayerRight();
        if(keyboardState.IsKeyDown(Keys.Up))
            player.MovePlayerUp();
        if(keyboardState.IsKeyDown(Keys.Down))
            player.MovePlayerDown();
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
        if(!enemy.IsDestroyed)
            enemy.Draw(spriteBatch);
        player.Draw(spriteBatch);
        spriteBatch.End();

        base.Draw(gameTime);
    }
}

