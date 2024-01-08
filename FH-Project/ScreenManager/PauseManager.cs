using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace FH_Project;

public class PauseManager
{
    private bool isPaused;
    private KeyboardState previousKeyboardState;
    private Texture2D pauseOverlay;
    private SpriteFont font;
    private Microsoft.Xna.Framework.Rectangle resumeButtonRect;
    private Microsoft.Xna.Framework.Rectangle quitButtonRect;

    public event EventHandler ResumeClicked;
    public event EventHandler QuitClicked;


    public const int MaxInputs = 4;

    public readonly KeyboardState[] CurrentKeyboardStates;
    public readonly GamePadState[] CurrentGamePadStates;

    public readonly KeyboardState[] LastKeyboardStates;
    public readonly GamePadState[] LastGamePadStates;

    public readonly bool[] GamePadWasConnected;

    public TouchCollection TouchState;

    public readonly List<GestureSample> Gestures = new List<GestureSample>();

    public PauseManager(Texture2D pauseOverlayTexture, SpriteFont pauseFont, Microsoft.Xna.Framework.Rectangle resumeButtonRectangle, Microsoft.Xna.Framework.Rectangle quitButtonRectangle)
    {
        isPaused = false;
        previousKeyboardState = Keyboard.GetState();
        pauseOverlay = pauseOverlayTexture;
        font = pauseFont;
        resumeButtonRect = resumeButtonRectangle;
        quitButtonRect = quitButtonRectangle;
    }

    public void Update()
    {
        KeyboardState currentKeyboardState = Keyboard.GetState();

        if (currentKeyboardState.IsKeyDown(Keys.P) && !previousKeyboardState.IsKeyDown(Keys.P))
        {
            TogglePause(); 
        }

        previousKeyboardState = currentKeyboardState;
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
    {
        if (isPaused)
        {
            spriteBatch.Begin();
          //  spriteBatch.Draw(pauseOverlay, new System.Drawing.Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), Microsoft.Xna.Framework.Color.Black * 0.5f);

            Microsoft.Xna.Framework.Vector2 messagePosition = new Microsoft.Xna.Framework.Vector2(graphicsDevice.Viewport.Width / 2 - 100, graphicsDevice.Viewport.Height / 2 - 40);
            spriteBatch.DrawString(font, "PAUSED", messagePosition, Microsoft.Xna.Framework.Color.Red);

            spriteBatch.DrawRectangle(resumeButtonRect, Microsoft.Xna.Framework.Color.White);
            spriteBatch.DrawString(font, "Resume", new Microsoft.Xna.Framework.Vector2(resumeButtonRect.X + 10, resumeButtonRect.Y + 10), Microsoft.Xna.Framework.Color.White);

            spriteBatch.DrawRectangle(quitButtonRect, Microsoft.Xna.Framework.Color.White);
            spriteBatch.DrawString(font, "Quit", new Microsoft.Xna.Framework.Vector2(quitButtonRect.X + 10, quitButtonRect.Y + 10), Microsoft.Xna.Framework.Color.White);

            spriteBatch.End();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            OnPauseActivated();
        }
        else
        {
            OnPauseDeactivated();
        }
    }

    protected virtual void OnPauseActivated()
    {
     
        ResumeClicked?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnPauseDeactivated()
    {

        QuitClicked?.Invoke(this, EventArgs.Empty);
    }

    public bool IsPaused
    {
        get { return isPaused; }
    }

    public void Updates()
    {
        for (int i = 0; i < MaxInputs; i++)
        {
            LastKeyboardStates[i] = CurrentKeyboardStates[i];
            LastGamePadStates[i] = CurrentGamePadStates[i];

            CurrentKeyboardStates[i] = Keyboard.GetState();
            CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);


            if (CurrentGamePadStates[i].IsConnected)
            {
                GamePadWasConnected[i] = true;
            }
        }

        TouchState = TouchPanel.GetState();

        Gestures.Clear();

        while (TouchPanel.IsGestureAvailable)
        {
            Gestures.Add(TouchPanel.ReadGesture());
        }
    }


    public bool IsNewKeyPress(Keys key, PlayerIndex? controllingPlayer,
                                        out PlayerIndex playerIndex)
    {
        if (controllingPlayer.HasValue)
        {
            // Read input from the specified player.
            playerIndex = controllingPlayer.Value;

            int i = (int)playerIndex;

            return (CurrentKeyboardStates[i].IsKeyDown(key) &&
                    LastKeyboardStates[i].IsKeyUp(key));
        }
        else
        {
            return (IsNewKeyPress(key, PlayerIndex.One, out playerIndex) ||
                    IsNewKeyPress(key, PlayerIndex.Two, out playerIndex) ||
                    IsNewKeyPress(key, PlayerIndex.Three, out playerIndex) ||
                    IsNewKeyPress(key, PlayerIndex.Four, out playerIndex));
        }
    }

    public bool IsNewKeyPress(Keys key)
    {
        return (CurrentKeyboardStates[0].IsKeyDown(key) && LastKeyboardStates[0].IsKeyUp(key));

    }

    public bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer,
                                                 out PlayerIndex playerIndex)
    {
        if (controllingPlayer.HasValue)
        {
            playerIndex = controllingPlayer.Value;

            int i = (int)playerIndex;

            return (CurrentGamePadStates[i].IsButtonDown(button) &&
                    LastGamePadStates[i].IsButtonUp(button));
        }
        else
        {
            return (IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Four, out playerIndex));
        }
    }

    public bool IsMenuSelect(PlayerIndex? controllingPlayer,
                             out PlayerIndex playerIndex)
    {
        return IsNewKeyPress(Keys.Space, controllingPlayer, out playerIndex) ||
               IsNewKeyPress(Keys.Enter, controllingPlayer, out playerIndex) ||
               IsNewButtonPress(Buttons.A, controllingPlayer, out playerIndex) ||
               IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
    }


    public bool IsMenuCancel(PlayerIndex? controllingPlayer,
                             out PlayerIndex playerIndex)
    {
        return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
               IsNewButtonPress(Buttons.B, controllingPlayer, out playerIndex) ||
               IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex);
    }


    public bool IsMenuUp(PlayerIndex? controllingPlayer)
    {
        PlayerIndex playerIndex;

        return IsNewKeyPress(Keys.Up, controllingPlayer, out _) ||
               IsNewButtonPress(Buttons.DPadUp, controllingPlayer, out playerIndex) ||
               IsNewButtonPress(Buttons.LeftThumbstickUp, controllingPlayer, out playerIndex);
    }

    public bool IsMenuDown(PlayerIndex? controllingPlayer)
    {
        PlayerIndex playerIndex;

        return IsNewKeyPress(Keys.Down, controllingPlayer, out playerIndex) ||
               IsNewButtonPress(Buttons.DPadDown, controllingPlayer, out playerIndex) ||
               IsNewButtonPress(Buttons.LeftThumbstickDown, controllingPlayer, out playerIndex);
    }

    public bool IsPauseGame(PlayerIndex? controllingPlayer)
    {
        PlayerIndex playerIndex;

        return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
               IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex) ||
               IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
    }
}
