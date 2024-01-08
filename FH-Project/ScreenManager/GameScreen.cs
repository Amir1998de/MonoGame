using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FH_Project;

public enum ScreenState
{
    TransitionOn,
    Active,
    TransitionOff,
    Hidden,
}
public abstract class GameScreen
{
    #region Properties

    /// <summary>
    /// Normally when one screen is brought up over the top of another,
    /// the first screen will transition off to make room for the new
    /// one. This property indicates whether the screen is only a small
    /// popup, in which case screens underneath it do not need to bother
    /// transitioning off.
    /// </summary>
    public bool IsPopup { get; protected set; }

    /// <summary>
    /// Indicates how long the screen takes to
    /// transition on when it is activated.
    /// </summary>
    public TimeSpan TransitionOnTime { get; protected set; } = TimeSpan.Zero;

    /// <summary>
    /// Indicates how long the screen takes to
    /// transition off when it is deactivated.
    /// </summary>
    public TimeSpan TransitionOffTime { get; protected set; } = TimeSpan.Zero;

    /// <summary>
    /// Gets the current position of the screen transition, ranging
    /// from zero (fully active, no transition) to one (transitioned
    /// fully off to nothing).
    /// </summary>
    public float TransitionPosition { get; protected set; } = 1;


    public float TransitionAlpha
    {
        get { return 1f - TransitionPosition; }
    }

    /// <summary>
    /// Gets the current screen transition state.
    /// </summary>
    public ScreenState ScreenState { get; protected set; } = ScreenState.TransitionOn;


    public bool IsExiting { get; protected internal set; }

    /// <summary>
    /// Checks whether this screen is active and can respond to user input.
    /// </summary>
    public bool IsActive
    {
        get
        {
            return !otherScreenHasFocus &&
                   (ScreenState == ScreenState.TransitionOn ||
                    ScreenState == ScreenState.Active);
        }
    }

    private bool otherScreenHasFocus;


    public ScreenManager ScreenManager { get; internal set; }


    public PlayerIndex? ControllingPlayer { get; internal set; }


    public GestureType EnabledGestures
    {
        get { return enabledGestures; }
        protected set
        {
            enabledGestures = value;

    
            if (ScreenState == ScreenState.Active)
            {
                TouchPanel.EnabledGestures = value;
            }
        }
    }

    private GestureType enabledGestures = GestureType.None;

    #endregion Properties

    #region Initialization

    /// <summary>
    /// Load graphics content for the screen.
    /// </summary>
    public virtual void LoadContent() { }

    /// <summary>
    /// Unload content for the screen.
    /// </summary>
    public virtual void UnloadContent() { }

    #endregion Initialization

    #region Update and Draw


    public virtual void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                  bool coveredByOtherScreen)
    {
        this.otherScreenHasFocus = otherScreenHasFocus;

        if (IsExiting)
        {
            // If the screen is going away to die, it should transition off.
            ScreenState = ScreenState.TransitionOff;

            if (!UpdateTransition(gameTime, TransitionOffTime, 1))
            {
                // When the transition finishes, remove the screen.
                ScreenManager.RemoveScreen(this);
            }
        }
        else if (coveredByOtherScreen)
        {
            // If the screen is covered by another, it should transition off.
            if (UpdateTransition(gameTime, TransitionOffTime, 1))
            {
                // Still busy transitioning.
                ScreenState = ScreenState.TransitionOff;
            }
            else
            {
                // Transition finished!
                ScreenState = ScreenState.Hidden;
            }
        }
        else
        {
            // Otherwise the screen should transition on and become active.
            if (UpdateTransition(gameTime, TransitionOnTime, -1))
            {
                // Still busy transitioning.
                ScreenState = ScreenState.TransitionOn;
            }
            else
            {
                // Transition finished!
                ScreenState = ScreenState.Active;
            }
        }
    }

    /// <summary>
    /// Helper for updating the screen transition position.
    /// </summary>
    private bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
    {
        // How much should we move by?
        float transitionDelta;

        if (time == TimeSpan.Zero)
        {
            transitionDelta = 1;
        }
        else
        {
            transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                     time.TotalMilliseconds);
        }

        // Update the transition position.
        TransitionPosition += transitionDelta * direction;

        // Did we reach the end of the transition?
        if (((direction < 0) && (TransitionPosition <= 0)) ||
            ((direction > 0) && (TransitionPosition >= 1)))
        {
            TransitionPosition = MathHelper.Clamp(TransitionPosition, 0, 1);
            return false;
        }

        // Otherwise we are still busy transitioning.
        return true;
    }

    /// <summary>
    /// Allows the screen to handle user input. Unlike Update, this method
    /// is only called when the screen is active, and not when some other
    /// screen has taken the focus.
    /// </summary>
    public virtual void HandleInput(InputState input) { }

    /// <summary>
    /// This is called when the screen should draw itself.
    /// </summary>
    public virtual void Draw(GameTime gameTime) { }

    #endregion Update and Draw

    #region Public Methods

    /// <summary>
    /// Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which
    /// instantly kills the screen, this method respects the transition timings
    /// and will give the screen a chance to gradually transition off.
    /// </summary>
    public void ExitScreen()
    {
        if (TransitionOffTime == TimeSpan.Zero)
        {
            // If the screen has a zero transition time, remove it immediately.
            ScreenManager.RemoveScreen(this);
        }
        else
        {
            // Otherwise flag that it should transition off and then exit.
            IsExiting = true;
        }
    }

    #endregion Public Methods
}

