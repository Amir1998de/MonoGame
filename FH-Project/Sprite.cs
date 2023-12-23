using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FH_Project;

public class Sprite
{
    private readonly Texture2D _texture;
    public Vector2 Position { get;  set; }
    public Vector2 Origin { get;  set; }

    public Sprite(Texture2D texture, Vector2 position)
    {
        _texture = texture;
        Position = position;
        // Set the origin to the center of the texture.
        Origin = new(_texture.Width / 2, _texture.Height / 2);
    }

    // Draw the sprite on the screen.
    public void Draw()
    {
        // Draw the texture with the specified properties.
        ScreenManager.spriteBatch.Draw(_texture, Position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0f);
       
       // Texture, Position, null, Color.White, 0, new Vector2(0, 0), 0.125f, SpriteEffects.None, 0
    }
}


