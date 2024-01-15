using FH_Project;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;

public class Player : Entity
{

    #region Variablen
    public bool IsAttacking { get; set; }
    public bool CanGetHit { get; set; }
    private float invincibilityFrames;

    protected int shield;
    private int maxShield = 100;
    public Weapon Weapon { get; set; }
    GameTime gameTime;
    private KeyboardState prevKeyboardState;
    private KeyboardState currentKeyboardState;
    private Room roomPlayerisIn;

    //Sprinten
    private float sprintTimer;
    private bool coolDownForSprint;
    public float Sprint { get; set; }


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
    public PlayerAnimation up;
    public PlayerAnimation down;
    public PlayerAnimation right;
    public PlayerAnimation left;



    //Attack
    private float attackTimer;

    private Vector2 _minPos, _maxPos;
    private float SPEED = 300;

    private Potion potion;
    public Rectangle PlayerBounds { get; set; }
    private Room KorridorPlayerIsIn;



    #endregion Variablen


    public Player(int health, float speed, Vector2 postion, Vector2 velocity, Weapon weapon, int scale) : base(health, speed, postion, velocity, scale)
    {
        LoadContent();
        IsAttacking = false;
        shield = 0;
        Weapon = weapon;
        Weapon.Position = new Vector2(Position.X + EntityTexture.Width * 2, Position.Y + EntityTexture.Height);
        coolDownForSprint = false;
        CanGetHit = true;
        Sprint = 5f;
        up = new PlayerAnimation(9,16);
        down = new PlayerAnimation(25,32);
        right = new PlayerAnimation(1,8);
        left = new PlayerAnimation(17,24);
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



        float distanceToPlayer = 0f;

        float z = (float)EntityTexture.Width;

        if (!KorridorPlayerIsIn.Bereich.Contains(Position))
            distanceToPlayer = Vector2.Distance(new Vector2(roomPlayerisIn.WhichKorridor.Bereich.X, roomPlayerisIn.WhichKorridor.Bereich.Y), Globals.Player.Position);


        if (distanceToPlayer <= 200 && roomPlayerisIn.GetEnemiesInRoomCount() <= 0)
        {
            if (PlayerBounds.Intersects(KorridorPlayerIsIn.Bereich) && roomPlayerisIn.Directions.Equals(RoomDirections.DOWN))
            {
                if (Position.X < KorridorPlayerIsIn.Bereich.Left - z)
                    Position = new Vector2(KorridorPlayerIsIn.Bereich.Left - z, Position.Y);

            }
        }
        else if (Position.X < roomPlayerisIn.Bereich.Left - z)
            Position = new Vector2(roomPlayerisIn.Bereich.Left - z, Position.Y);


    }

    public void MovePlayerRight()
    {
        Position = new Vector2(Position.X + Speed, Position.Y);



        float z = (float)EntityTexture.Width;
        float distanceToPlayer = 0f;
        if(KorridorPlayerIsIn != null)
            if (!KorridorPlayerIsIn.Bereich.Contains(Position))
                distanceToPlayer = Vector2.Distance(new Vector2(roomPlayerisIn.WhichKorridor.Bereich.X, roomPlayerisIn.WhichKorridor.Bereich.Y), Globals.Player.Position);
        

        if (distanceToPlayer <= 200 && roomPlayerisIn.GetEnemiesInRoomCount() <= 0)
        {
            if (PlayerBounds.Intersects(KorridorPlayerIsIn.Bereich) && roomPlayerisIn.Directions.Equals(RoomDirections.DOWN))
            {
                if (Position.X + z > KorridorPlayerIsIn.Bereich.Right - z * 2)
                    Position = new Vector2(KorridorPlayerIsIn.Bereich.Right - z * 2, Position.Y);

            }
        }
        else if (Position.X  > roomPlayerisIn.Bereich.Right - z * 2)
            Position = new Vector2(roomPlayerisIn.Bereich.Right - z * 2, Position.Y);


    }

    public void MovePlayerUp()
    {
        //bool directionPlayerCanGo = !roomPlayerisIn.Directions.Equals(RoomDirections.UP) && !roomPlayerisIn.ReverseDircetion.Equals(RoomDirections.DOWN) ? true: false;

        Position = new Vector2(Position.X, Position.Y - Speed);


        float z = (float)EntityTexture.Height;

        float distanceToPlayer = 0f;


        if (!KorridorPlayerIsIn.Bereich.Contains(Position))
            distanceToPlayer = Vector2.Distance(new Vector2(roomPlayerisIn.WhichKorridor.Bereich.X, roomPlayerisIn.WhichKorridor.Bereich.Y), Globals.Player.Position);

        if (distanceToPlayer <= 200 && roomPlayerisIn.GetEnemiesInRoomCount() <= 0)
        {
            if (PlayerBounds.Intersects(KorridorPlayerIsIn.Bereich) && roomPlayerisIn.Directions.Equals(RoomDirections.RIGHT))
            {
                if (Position.Y < KorridorPlayerIsIn.Bereich.Y - z)
                    Position = new Vector2(Position.X, KorridorPlayerIsIn.Bereich.Y - z);

            }
        }
        else if (Position.Y < roomPlayerisIn.Bereich.Y - z)
            Position = new Vector2(Position.X, roomPlayerisIn.Bereich.Y - z);



    }

    public void MovePlayerDown()
    {
        Position = new Vector2(Position.X, Position.Y + Speed);




        float distanceToPlayer = 0f;

        float z = (float)EntityTexture.Height * 3;

        if(!KorridorPlayerIsIn.Bereich.Contains(Position))
            distanceToPlayer = Vector2.Distance(new Vector2(roomPlayerisIn.WhichKorridor.Bereich.X, roomPlayerisIn.WhichKorridor.Bereich.Y), Globals.Player.Position);


        if (distanceToPlayer <= 200 && roomPlayerisIn.GetEnemiesInRoomCount() <= 0)
        {
            if (PlayerBounds.Intersects(KorridorPlayerIsIn.Bereich) && roomPlayerisIn.Directions.Equals(RoomDirections.RIGHT))
            {
                if (Position.Y > KorridorPlayerIsIn.Bereich.Bottom - z)
                    Position = new Vector2(Position.X, KorridorPlayerIsIn.Bereich.Bottom - z);
            }
        }
        else if (Position.Y > roomPlayerisIn.Bereich.Bottom - z)
            Position = new Vector2(Position.X, roomPlayerisIn.Bereich.Bottom - z);


        //Weapon.Position = new Vector2(Position.X + EntityTexture.Width * 2, Position.Y + EntityTexture.Height);


    }


    public override void Attack(GameTime gameTime, PlayerActions data)
    {
        IsAttacking = true;
        GetNotified(data);
        Animation();
    }

    public void MausRichtiung()
    {
        Vector2 mousePosition = new Vector2(Globals.MouseState.X, Globals.MouseState.Y);

        float horizontalDistance = mousePosition.X + Camera.Position.X;

        if (horizontalDistance > Position.X)
            Weapon.Position = new Vector2(Position.X + EntityTexture.Width * 2, Position.Y + EntityTexture.Height);
        else
            Weapon.Position = new Vector2(Position.X - EntityTexture.Width / 2 + 10, Position.Y + EntityTexture.Height);


    }

    public void Animation()
    {

    }

    public void Update(GameTime gameTime)
    {
        UpdateIdle(gameTime);
        UpdateRun(gameTime);
        if (Map.GetRoomPlayerIsIn() != null)
        {
            if (roomPlayerisIn == null || (!roomPlayerisIn.Equals(Map.GetRoomPlayerIsIn())))
            {
                if (!Map.GetRoomPlayerIsIn().IsKorridor)
                {
                    roomPlayerisIn = Map.GetRoomPlayerIsIn();
                    KorridorPlayerIsIn = roomPlayerisIn.WhichKorridor;
                }


            }


        }

        prevKeyboardState = Globals.KeyboardState;

        if (!Globals.KeyboardState.IsKeyDown(Keys.Right) &&
            !Globals.KeyboardState.IsKeyDown(Keys.Left) &&
            !Globals.KeyboardState.IsKeyDown(Keys.Up) &&
            !Globals.KeyboardState.IsKeyDown(Keys.Down)) PlayerAnimation(playerIdleTexture, gameTime, idleTimer, frameRunTime, currentIdleFrame, totalIdleFrames);


        if (Globals.KeyboardState.IsKeyDown(Keys.A))
        {

            PlayerAnimation(playerRightTexture, gameTime, runtimer, frameRunTime, currentRightFrame, totalRightFrames);
            MovePlayerLeft();
            left.Update();
        }

        if (Globals.KeyboardState.IsKeyDown(Keys.D))
        {

            PlayerAnimation(playerRightTexture, gameTime, runtimer, frameRunTime, currentRightFrame, totalRightFrames);
            MovePlayerRight();
            right.Update();
        }

        if (Globals.KeyboardState.IsKeyDown(Keys.W))
        {

            PlayerAnimation(playerRightTexture, gameTime, runtimer, frameRunTime, currentRightFrame, totalRightFrames);
            MovePlayerUp();
            up.Update();
        }

        if (Globals.KeyboardState.IsKeyDown(Keys.S))
        {

            PlayerAnimation(playerRightTexture, gameTime, runtimer, frameRunTime, currentRightFrame, totalRightFrames);
            MovePlayerDown();
            down.Update();
        }



        prevKeyboardState = currentKeyboardState;
        currentKeyboardState = Globals.KeyboardState;



        if (currentKeyboardState.IsKeyDown(Keys.H) && !prevKeyboardState.IsKeyDown(Keys.H))
        {
            Inventory.UseItem(Inventory.GetPotion(1));

        }


        if (currentKeyboardState.IsKeyDown(Keys.J) && !prevKeyboardState.IsKeyDown(Keys.J))
            Inventory.UseItem(Inventory.GetPotion(2));


        if (currentKeyboardState.IsKeyDown(Keys.K) && !prevKeyboardState.IsKeyDown(Keys.K))
            Inventory.UseItem(Inventory.GetPotion(3));


        if (currentKeyboardState.IsKeyDown(Keys.L) && !prevKeyboardState.IsKeyDown(Keys.L))
            Inventory.ChangeWeapon(1);

        if (currentKeyboardState.IsKeyDown(Keys.O) && !prevKeyboardState.IsKeyDown(Keys.O))
            Inventory.ChangeWeapon(2);



        if (Globals.KeyboardState.IsKeyDown(Keys.LeftShift) && !coolDownForSprint)
        {
            if (sprintTimer <= 0f || sprintTimer <= 3f)
            {
                Speed = Sprint;
                sprintTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (sprintTimer >= 3f) coolDownForSprint = true;
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

        if (CheckIfDead())
        {
            Debug.WriteLine("dsad");
        }
        MausRichtiung();

        if (Globals.MouseState.LeftButton == ButtonState.Pressed && !IsAttacking && Globals.Player.Weapon.GetType().ToString().Equals("FH_Project.Sword"))
        {

            Attack(gameTime, PlayerActions.ATTACK);
        }
        Globals.MouseState = Mouse.GetState();

        if (Globals.MouseState.LeftButton == ButtonState.Pressed && Globals.Player.Weapon.GetType().ToString().Equals("FH_Project.Bow"))
        {
            Attack(gameTime, PlayerActions.SHOOT);
        }

        //Debug.WriteLine(IsAttacking);

        if (IsAttacking && attackTimer >= 0.5f)
        {
            IsAttacking = false;
            attackTimer = 0f;
        }

        if (IsAttacking) attackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (!CanGetHit) invincibilityFrames += (float)gameTime.ElapsedGameTime.TotalSeconds;

        //Debug.WriteLine("Attack Timer: " + attackTimer);

        if (invincibilityFrames >= 3)
        {
            CanGetHit = true;
            invincibilityFrames = 0;
        }

        Sprite.Position += ScreenManager.Direction * Globals.Time * SPEED;
        Sprite.Position = Vector2.Clamp(Sprite.Position, _minPos, _maxPos);

        PlayerBounds = new Rectangle((int)Position.X + 50, (int)Position.Y + 25, EntityTexture.Width, EntityTexture.Height * 2);



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
