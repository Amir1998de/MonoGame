using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FH_Project;

public class Animation
{
    private readonly Texture2D _texture;
    private readonly List<Rectangle> _sourceRectangles = new();
    private readonly int _frames;
    private int _frame;
    private readonly float _frameTime;
    private float _frameTimeLeft;
    private bool _active = true;
    private int frameNumber;
    private int frameCount;
    public Animation(Texture2D texture, int framesX, int framesY, float frameTime, int number,int count, int row = 1)
    {
        _texture = texture;
        _frameTime = frameTime;
        _frameTimeLeft = _frameTime;
        _frames = framesX;
        frameNumber = number;
        frameCount = count;
        var frameWidth = _texture.Width / framesX;
        var frameHeight = _texture.Height / framesY;

        for (int i = 0; i < _frames; i++)
        {
            _sourceRectangles.Add(new(i * frameWidth, (row - 1) * frameHeight, frameWidth, frameHeight));
        }
    }

    public void Stop()
    {
        _active = false;
    }

    public void Start()
    {
        _active = true;
    }

    public void Reset()
    {
        _frame = 0;
        _frameTimeLeft = _frameTime;
    }

    public void Update()
    {
        if (!_active) return;

        _frameTimeLeft -= Globals.Time;

        if (_frameTimeLeft <= 0)
        {
            _frameTimeLeft += _frameTime;
            _frame = (_frame + 1) % frameCount;
        }
    }

    public void Draw(Vector2 pos)
    {
        Globals.SpriteBatch.Draw(_texture, pos, _sourceRectangles[_frame+frameNumber], Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 1);
    }
}
