﻿using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FH_Project;

public class InputState
{
    #region Fields

    public const int MaxInputs = 4;

    public readonly KeyboardState[] CurrentKeyboardStates;
    public readonly GamePadState[] CurrentGamePadStates;

    public readonly KeyboardState[] LastKeyboardStates;
    public readonly GamePadState[] LastGamePadStates;

    public readonly bool[] GamePadWasConnected;

    public TouchCollection TouchState;

    public readonly List<GestureSample> Gestures = new List<GestureSample>();

    #endregion Fields

    #region Initialization

    /// <summary>
    /// Constructs a new input state.
    /// </summary>
    public InputState()
    {
        CurrentKeyboardStates = new KeyboardState[MaxInputs];
        CurrentGamePadStates = new GamePadState[MaxInputs];

        LastKeyboardStates = new KeyboardState[MaxInputs];
        LastGamePadStates = new GamePadState[MaxInputs];

        GamePadWasConnected = new bool[MaxInputs];
    }

    #endregion Initialization

    #region Public Methods

    /// <summary>
    /// Reads the latest state of the keyboard and gamepad.
    /// </summary>
    public void Update()
    {
        for (int i = 0; i < MaxInputs; i++)
        {
            LastKeyboardStates[i] = CurrentKeyboardStates[i];
            LastGamePadStates[i] = CurrentGamePadStates[i];

            CurrentKeyboardStates[i] = Keyboard.GetState();
            CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);

            // Keep track of whether a gamepad has ever been
            // connected, so we can detect if it is unplugged.
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

    /// <summary>
    /// Helper for checking if a key was newly pressed during this update. The
    /// controllingPlayer parameter specifies which player to read input for.
    /// If this is null, it will accept input from any player. When a keypress
    /// is detected, the output playerIndex reports which player pressed it.
    /// </summary>
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
            // Accept input from any player.
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

    /// <summary>
    /// Helper for checking if a button was newly pressed during this update.
    /// The controllingPlayer parameter specifies which player to read input for.
    /// If this is null, it will accept input from any player. When a button press
    /// is detected, the output playerIndex reports which player pressed it.
    /// </summary>
    public bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer,
                                                 out PlayerIndex playerIndex)
    {
        if (controllingPlayer.HasValue)
        {
            // Read input from the specified player.
            playerIndex = controllingPlayer.Value;

            int i = (int)playerIndex;

            return (CurrentGamePadStates[i].IsButtonDown(button) &&
                    LastGamePadStates[i].IsButtonUp(button));
        }
        else
        {
            // Accept input from any player.
            return (IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Four, out playerIndex));
        }
    }

    /// <summary>
    /// Checks for a "menu select" input action.
    /// The controllingPlayer parameter specifies which player to read input for.
    /// If this is null, it will accept input from any player. When the action
    /// is detected, the output playerIndex reports which player pressed it.
    /// </summary>
    public bool IsMenuSelect(PlayerIndex? controllingPlayer,
                             out PlayerIndex playerIndex)
    {
        return IsNewKeyPress(Keys.Space, controllingPlayer, out playerIndex) ||
               IsNewKeyPress(Keys.Enter, controllingPlayer, out playerIndex) ||
               IsNewButtonPress(Buttons.A, controllingPlayer, out playerIndex) ||
               IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
    }

    /// <summary>
    /// Checks for a "menu cancel" input action.
    /// The controllingPlayer parameter specifies which player to read input for.
    /// If this is null, it will accept input from any player. When the action
    /// is detected, the output playerIndex reports which player pressed it.
    /// </summary>
    public bool IsMenuCancel(PlayerIndex? controllingPlayer,
                             out PlayerIndex playerIndex)
    {
        return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
               IsNewButtonPress(Buttons.B, controllingPlayer, out playerIndex) ||
               IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex);
    }

    /// <summary>
    /// Checks for a "menu up" input action.
    /// The controllingPlayer parameter specifies which player to read
    /// input for. If this is null, it will accept input from any player.
    /// </summary>
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

    #endregion Public Methods
}

