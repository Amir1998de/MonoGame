using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace FH_Project;
/**
 * 
 *  provides global properties and methods to a game
 */
public static class Globals
{
    // Time elapsed since the last frame update. Useful for time-based calculations.
    public static float Time { get; set; }

    // Content manager for loading game resources like textures and sounds.
    public static ContentManager Content { get; set; }

    // SpriteBatch for rendering textures. Centralized here for global access.
    public static SpriteBatch SpriteBatch { get; set; }

    // The size of the game window. Useful for UI layout and camera adjustments.
    public static Point WindowSize { get; set; }

    public static KeyboardState KeyboardState { get; set; }

    public static MouseState MouseState { get; set; }


    public static Player Player { get; set; }

    public static Map Map { get; set; }
    // Update method, typically called every frame to update the elapsed time.
    public static void Update(GameTime gt)
    {
        // Update the Time property with the time elapsed since the last frame.
        Time = (float)gt.ElapsedGameTime.TotalSeconds;
    }

    
}
