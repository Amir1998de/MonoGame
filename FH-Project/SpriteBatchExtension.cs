using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace FH_Project;
public static class SpriteBatchExtensions
{
    public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
    {
        spriteBatch.Draw(Pixel, rectangle, color);
    }

    private static Texture2D Pixel;

    public static void LoadPixel(this GraphicsDevice graphicsDevice)
    {
        Pixel = new Texture2D(graphicsDevice, 1, 1);
        Pixel.SetData(new[] { Color.White });
    }
}