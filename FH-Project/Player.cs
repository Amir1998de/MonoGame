using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;


namespace FH_Project;



public class Player : Entity
{

    #region Variablen
    public bool IsAttacking { get; set; }
    protected int shield;
    private int maxShield = 100;
    public Weapon Weapon { get; set; }
    GameTime gameTime;
    private KeyboardState prevKeyboardState;
    private KeyboardState currentKeyboardState;

    //Sprinten
    private float sprintTimer;
    private bool coolDownForSprint;


    private float frameIdleTime = 0.2f;
    private float idleTimer = 0f;

    private float frameRunTime = 0.1f;
    private float runtimer = 0f;

    //Arrays für die jeweiligen Arrays
    private Texture2D playerTexture;
    private Texture2D[] playerIdleTexture;
    private Texture2D[] playerRightTexture;
    private Texture2D[] playerLeftTexture;
    private Texture2D[] playerUpTexture;
    private Texture2D[] playerDownTexture;



    //Attack
    private float attackTimer;

    private Vector2 _minPos, _maxPos;
    private float SPEED = 300;

    private Potion potion;


    #endregion Variablen


    public Player(int health, float speed, Vector2 postion, Vector2 velocity, Weapon weapon) : base(health, speed, postion, velocity)
    {
        LoadContent();
        IsAttacking = false;
        shield = 0;
        Weapon = weapon;
        Weapon.Position = new Vector2(Position.X + EntityTexture.Width * 2, Position.Y + EntityTexture.Height);
        coolDownForSprint = false;

    }

    public void AddShield(int value)
    {
        shield += value;
        if (Health > maxShield)
            Health = maxShield;
    }

    public int GetShield() { return shield; }


    public void ReduceShield(int value)
    {
        shield -= value;
        if (shield < 0)
            shield = 0;
    }

    public bool CanAttack()
    {
        return !(IsAttacking);
    }


    public void MovePlayerLeft()
    {
        Position = new Vector2(Position.X - Speed, Position.Y);

        float z = (float)EntityTexture.Width;

        if (Position.X - z  < Map.GetRoomPlayerIsIn(this).Bereich.Left)
            Position = new Vector2(Map.GetRoomPlayerIsIn(this).Bereich.Left + z, Position.Y);

        Weapon.Position = new Vector2(Position.X + EntityTexture.Width * 2, Position.Y + EntityTexture.Height);
    }

    public void MovePlayerRight()
    {
        Position = new Vector2(Position.X + Speed, Position.Y);

        // Durch 2 weil man unten beim Draw 3 scale hat. 
        float z = (float)EntityTexture.Width;

        /*if (Position.X + z > Map.GetRoomPlayerIsIn(this).Bereich.Right)
            Position = new Vector2(Map.GetRoomPlayerIsIn(this).Bereich.Right - z, Position.Y);*/

        Weapon.Position = new Vector2(Position.X + EntityTexture.Width * 2, Position.Y + EntityTexture.Height);
    }

    public void MovePlayerUp()
    {
        Position = new Vector2(Position.X, Position.Y - Speed);

        float z = (float)EntityTexture.Height;

        if (Position.Y - z < Map.GetRoomPlayerIsIn(this).Bereich.Top)
            Position = new Vector2(Position.X, Map.GetRoomPlayerIsIn(this).Bereich.Top + z);

        Weapon.Position = new Vector2(Position.X + EntityTexture.Width * 2, Position.Y + EntityTexture.Height);

    }

    public void MovePlayerDown()
    {
        Position = new Vector2(Position.X, Position.Y + Speed);

        // Durch 2 weil man unten beim Draw 3 scale hat. 
        float z = (float)EntityTexture.Height;

        if (Position.Y + z >  Map.GetRoomPlayerIsIn(this).Bereich.Bottom)
            Position = new Vector2(Position.X, Map.GetRoomPlayerIsIn(this).Bereich.Bottom - z);

        Weapon.Position = new Vector2(Position.X + EntityTexture.Width * 2, Position.Y + EntityTexture.Height);
    }

    public override void Attack()
    {
        IsAttacking = true;
        GetNotified(PlayerActions.ATTACK);
        Animation();
    }

    public void Animation()
    {

    }

    public void Update(GameTime gameTime)
    {
        UpdateIdle(gameTime);
        UpdateRun(gameTime);




        prevKeyboardState = Globals.KeyboardState;

        if (!Globals.KeyboardState.IsKeyDown(Keys.Right) &&
            !Globals.KeyboardState.IsKeyDown(Keys.Left) &&
            !Globals.KeyboardState.IsKeyDown(Keys.Up) &&
            !Globals.KeyboardState.IsKeyDown(Keys.Down)) PlayerAnimation(playerIdleTexture, gameTime, idleTimer, frameRunTime, currentIdleFrame, totalIdleFrames);


        if (Globals.KeyboardState.IsKeyDown(Keys.Left))
        {

            PlayerAnimation(playerRightTexture, gameTime, runtimer, frameRunTime, currentRightFrame, totalRightFrames);
            MovePlayerLeft();
        }

        if (Globals.KeyboardState.IsKeyDown(Keys.Right))
        {

            PlayerAnimation(playerRightTexture, gameTime, runtimer, frameRunTime, currentRightFrame, totalRightFrames);
            MovePlayerRight();
        }

        if (Globals.KeyboardState.IsKeyDown(Keys.Up))
        {

            PlayerAnimation(playerRightTexture, gameTime, runtimer, frameRunTime, currentRightFrame, totalRightFrames);
            MovePlayerUp();
        }

        if (Globals.KeyboardState.IsKeyDown(Keys.Down))
        {

            PlayerAnimation(playerRightTexture, gameTime, runtimer, frameRunTime, currentRightFrame, totalRightFrames);
            MovePlayerDown();
        }



        currentKeyboardState = Globals.KeyboardState;


        if (currentKeyboardState.IsKeyDown(Keys.H) && !prevKeyboardState.IsKeyDown(Keys.H)) Inventory.UseItem(Inventory.getPotion(1));
        prevKeyboardState = currentKeyboardState;

        if (currentKeyboardState.IsKeyDown(Keys.J) && !prevKeyboardState.IsKeyDown(Keys.J)) Inventory.UseItem(Inventory.getPotion(2));
        prevKeyboardState = currentKeyboardState;

        if (currentKeyboardState.IsKeyDown(Keys.K) && !prevKeyboardState.IsKeyDown(Keys.K)) Inventory.UseItem(Inventory.getPotion(3));
        prevKeyboardState = currentKeyboardState;


        if (Globals.KeyboardState.IsKeyDown(Keys.LeftShift) && !coolDownForSprint)
        {
            if (sprintTimer <= 0f || sprintTimer <= 3f)
            {
                Speed = 10f;
                sprintTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (sprintTimer >= 1000000f) coolDownForSprint = true;
        }
        else Speed = BaseSpeed;

        if (coolDownForSprint)
        {
            sprintTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (sprintTimer < 0f)
            {
                sprintTimer = 0f;
                coolDownForSprint = false;
            }
        }



        if (Globals.KeyboardState.IsKeyDown(Keys.Space) && CanAttack())
        {
            Attack();
        }

        if (IsAttacking && attackTimer >= 0.5f)
        {
            IsAttacking = false;
            attackTimer = 0f;
        }

        if (IsAttacking) attackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        Sprite.Position += ScreenManager.Direction * Globals.Time * SPEED;
        Sprite.Position = Vector2.Clamp(Sprite.Position, _minPos, _maxPos);
    }

    public void UpdateIdle(GameTime gameTime)
    {
        idleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (idleTimer > frameIdleTime)
        {
            currentIdleFrame = (currentIdleFrame + 1) % totalIdleFrames;
            idleTimer = 0f;
        }
    }

    public override void LoadContent()
    {
        EntityTexture = Globals.Content.Load<Texture2D>("Individual Sprites/adventurer-idle-00");

        playerRightTexture = new Texture2D[totalRightFrames];
        for (int i = 0; i < totalRightFrames; i++)
        {
            playerRightTexture[i] = Globals.Content.Load<Texture2D>($"Individual Sprites/adventurer-run-0{i}");
        }

        playerIdleTexture = new Texture2D[totalIdleFrames];
        for (int i = 0; i < totalIdleFrames; i++)
        {
            playerIdleTexture[i] = Globals.Content.Load<Texture2D>($"Individual Sprites/adventurer-idle-0{i}");
        }

        Sprite = new Sprite(EntityTexture, Position);
    }

    public void UpdateRun(GameTime gameTime)
    {
        runtimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (runtimer > frameRunTime)
        {
            currentRightFrame = (currentRightFrame + 1) % totalRightFrames;
            currentLeftFrame = (currentLeftFrame + 1) % totalLeftFrames;
            currentUpFrame = (currentUpFrame + 1) % totalUpFrames;
            currentDownFrame = (currentDownFrame + 1) % totalDownFrames;
            runtimer = 0f;
        }
    }

    public void PlayerAnimation(Texture2D[] animation, GameTime gameTime, float timer, float frameTime, int currentFrame, int totalFrames)
    {
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (timer > frameTime)
        {
            // Zum nächsten Frame wechseln
            currentFrame = (currentFrame + 1) % totalFrames;
            timer = 0f;
        }
        EntityTexture = animation[currentFrame];
    }

    public void SetBounds(Point mapSize, Point tileSize)
    {
        _minPos = new((-tileSize.X / 2) + Sprite.Origin.X, (-tileSize.Y / 2) + Sprite.Origin.Y);
        _maxPos = new(mapSize.X - (tileSize.X / 2) - Sprite.Origin.X, mapSize.Y - (tileSize.X / 2) - Sprite.Origin.Y);
    }





}
