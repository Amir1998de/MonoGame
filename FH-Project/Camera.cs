using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FH_Project;
public class Camera
{
    private Matrix transformMatrix;

    public Vector2 Position { get; set; }
    public float Zoom { get; set; }
    public float Rotation { get; set; }
    private Map karte;
    private Player player;

    public Camera(Map karte, Player player)
    {
        Position = Vector2.Zero;
        Zoom = 1.0f;
        Rotation = 0.0f;
        this.karte = karte;
        this.player = player;
    }   

    public Matrix GetViewMatrix(Room currentRoom)
    {
        float dx = player.Position.X * Zoom - (Globals.WindowSize.X / 7);
        float dy = player.Position.Y * Zoom - (Globals.WindowSize.Y / 7);

        // Adjust the camera based on the current room boundaries
        dx = MathHelper.Clamp(dx, currentRoom.Bereich.Left, currentRoom.Bereich.Right - Globals.WindowSize.X * Zoom);
        dy = MathHelper.Clamp(dy, currentRoom.Bereich.Top, currentRoom.Bereich.Bottom - Globals.WindowSize.Y * Zoom);

        // Calculate the transformation matrix using position and zoom
        transformMatrix = Matrix.CreateTranslation(new Vector3(-dx, -dy, 0.0f)) *
                            Matrix.CreateRotationZ(Rotation) *
                            Matrix.CreateScale(new Vector3(Zoom, Zoom, 1.0f));

        return transformMatrix;


    }
}
