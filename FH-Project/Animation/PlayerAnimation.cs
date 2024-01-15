using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FH_Project;
public class PlayerAnimation
{
    private static Texture2D _texture;
    private Vector2 _position;
    private readonly Animation _anim;

    public PlayerAnimation(int frameX, int frameY)
    {
        _texture = Globals.Content.Load<Texture2D>("sPlayerRun_strip32");
        _anim = new(_texture, frameX, frameY, 0.1f);
        _position = Globals.Player.Position;
    }

    public void Update()
    {
        _anim.Update();
        _position = Globals.Player.Position;
    }

    public void Draw()
    {
        _anim.Draw(_position);
    }

}
