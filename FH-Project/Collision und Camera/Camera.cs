using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FH_Project;
public class Camera
{
    private Matrix transformMatrix;

    public static Vector2 Position { get; set; }
    public float Zoom { get; set; }
    public float Rotation { get; set; }
    


    public Camera(Map karte)
    {
        Position = Vector2.Zero;
        Zoom = 1.0f;
        Rotation = 0.0f;
       
    }

    public Matrix GetViewMatrix(Room currentRoom)
    {


        float dx = Globals.Player.Position.X * Zoom - (Globals.WindowSize.X / 7);
        float dy = Globals.Player.Position.Y * Zoom - (Globals.WindowSize.Y / 7);

        // Define a lerp factor (experiment with the value to adjust the smoothness)
        float lerpFactor = 1f;

        // Calculate the target position for the camera
        float targetDx = Globals.Player.Position.X * Zoom - (Globals.WindowSize.X / 5);
        float targetDy = Globals.Player.Position.Y * Zoom - (Globals.WindowSize.Y / 5);

        // Lerp to the target position

        // Adjust the camera based on the current room boundaries


        //dx = MathHelper.Clamp(dx, currentRoom.Bereich.Left, currentRoom.Bereich.Right - Globals.WindowSize.X * Zoom);
        //dy = MathHelper.Clamp(dy, currentRoom.Bereich.Top, currentRoom.Bereich.Bottom - Globals.WindowSize.Y * Zoom);

        dx = MathHelper.Lerp(dx, targetDx, lerpFactor);
        dy = MathHelper.Lerp(dy, targetDy, lerpFactor);


        // Calculate the transformation matrix using position and zoom
        transformMatrix = Matrix.CreateTranslation(new Vector3(-dx, -dy, 0.0f)) *
                            Matrix.CreateRotationZ(Rotation) *
                            Matrix.CreateScale(new Vector3(Zoom, Zoom, 1.0f));

        Position = new(dx, dy);
        return transformMatrix;



    }
}
