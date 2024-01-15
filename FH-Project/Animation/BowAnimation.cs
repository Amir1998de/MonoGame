using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FH_Project;
public class BowAnimation
{
    private static Texture2D _texture;
    private Vector2 _position;
    private readonly Animation _anim;

    public BowAnimation(Vector2 pos, int number, int count)
    {
        _texture = Globals.Content.Load<Texture2D>("neue Sprites/sArrow_strip4");
        _anim = new(_texture, 4, 1, 0.1f,number,count);
        _position = pos;
    }

    public void Update()
    {
        _anim.Update();
    }

    public void Draw()
    {
        _anim.Draw(_position);
    }

}
