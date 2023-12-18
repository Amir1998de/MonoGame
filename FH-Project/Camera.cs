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

    public Matrix GetViewMatrix()
    {
        float dx = (Globals.WindowSize.X / 7) - player.Position.X;
        float dy = (Globals.WindowSize.Y / 7) - player.Position.Y;

        // Klemme die Werte, um sicherzustellen, dass die Kamera nicht über den Rand der Karte hinausgeht
        //dx = MathHelper.Clamp(dx, 0, karte.MapSize.X - Globals.WindowSize.X);
        //dy = MathHelper.Clamp(dy, 0, karte.MapSize.Y - Globals.WindowSize.Y);

        // Berechne die Transformationsmatrix unter Verwendung von Position und Zoom
        transformMatrix = Matrix.CreateTranslation(new Vector3(dx, dy, 0.0f)) *
                          Matrix.CreateRotationZ(Rotation) *
                          Matrix.CreateScale(new Vector3(Zoom, Zoom, 1.0f));

        return transformMatrix;

    }
}
