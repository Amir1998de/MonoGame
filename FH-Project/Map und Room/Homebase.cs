using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace FH_Project;

public class Homebase : Room
{
    private Perklist[] perks;
    Random random = new();
    private SpriteFont spriteFont;
    private List<Perklist> perksList = new List<Perklist>();
    private Rectangle[] itemChoices = new Rectangle[3];
    private List<Rectangle> shopChoices = new List<Rectangle>();
    private List<Texture2D> shopItems = new List<Texture2D>();
    private Vector2 position;
    public bool ChoicesBeenMade { get; private set; }
    private Texture2D shopKeeperTexture;
    private Rectangle shopKeeperBounds;
    private MouseState mouseState;
    private bool openShop = false;
    private Texture2D healingPotion;
    private Texture2D shieldPotion;
    private Texture2D randomPotion;
    private Texture2D arrowTexture;
    private bool mouseClicked = false;
    private Rectangle done;


    public bool AreWeDone { get; set; }
    public Homebase(int x, int y, int breite, int höhe, bool isKorridor) : base(x, y, breite, höhe, isKorridor)
    {
        ChoicesBeenMade = false;
        
        perks = (Perklist[])Enum.GetValues(typeof(Perklist));
        spriteFont = Globals.Content.Load<SpriteFont>("Verdana");
        AreWeDone = true;
        healingPotion = Globals.Content.Load<Texture2D>("Items/Potions/PotionRed");
        shieldPotion = Globals.Content.Load<Texture2D>("Items/Potions/PotionBlue");
        randomPotion = Globals.Content.Load<Texture2D>("Items/Potions/PotionGreen");
        arrowTexture = Globals.Content.Load<Texture2D>("neue Sprites/sArrowDrop");
        shopItems.Add(healingPotion);
        shopItems.Add(shieldPotion);
        shopItems.Add(randomPotion);
        shopItems.Add(arrowTexture);

    }

    public override void Draw()
    {
        for (int y = 0; y < tileSize.Y; y++)
        {
            for (int x = 0; x < tileSize.X; x++)
                tiles[x, y].Draw();
        }

        if (!ChoicesBeenMade)
        {
            int count = 0;
            int diff = 300;
            foreach (var item in perksList)
            {

                Globals.SpriteBatch.DrawRectangle(itemChoices[count], Color.White);
                Globals.SpriteBatch.DrawString(spriteFont, item.ToString(), new Vector2(diff, Globals.WindowSize.Y / 2) + Camera.Position, Color.Red);
                diff += 300;
                count++;
            }
        }
        else
            Globals.SpriteBatch.Draw(shopKeeperTexture, new Vector2(Globals.WindowSize.X / 2, Globals.WindowSize.Y / 2), null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);

        if (openShop)
        {
            int count = 0;
            shopChoices.ForEach(item =>
            {
                Globals.SpriteBatch.DrawRectangle(item, Color.White);
                Globals.SpriteBatch.Draw(shopItems[count], new Vector2(item.Center.X, item.Center.Y), null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                count++;
            });
        }

        Globals.SpriteBatch.DrawRectangle(done,Color.White);
    }
    public void Update()
    {
        int count = 0;
        int diff = 300;
        Debug.WriteLine("Gems: " + Inventory.ReturnItemCount());
        

        if (!ChoicesBeenMade)
        {
            perksList.ForEach(item =>
            {
                position = new Vector2(diff, Globals.WindowSize.Y / 2) + Camera.Position;
                itemChoices[count] = new Rectangle((int)position.X, (int)position.Y, 200, 300);
                count++;
                diff += 300;

            });
        }

        mouseState = Mouse.GetState();
        Point mousePosition = new Point(mouseState.X + (int)Camera.Position.X, mouseState.Y + (int)Camera.Position.Y);

        if (shopKeeperBounds.Contains(mousePosition) && mouseState.LeftButton == ButtonState.Pressed && !openShop)
        {
            openShop = true;
            diff = 500;
            for (int i = 0; i < 4; i++)
            {
                position = new Vector2(Globals.WindowSize.X / 2 + 100, diff);
                shopChoices.Add(new Rectangle((int)position.X, (int)position.Y, 150, 100));
                diff += 101;
            }
        }
        count = 0;
        if (mouseState.LeftButton == ButtonState.Released) mouseClicked = false;

        shopChoices.ForEach(item =>
        {
            if (openShop && mouseState.LeftButton == ButtonState.Pressed && item.Contains(mousePosition) && !mouseClicked)
            {
                mouseClicked = true;
                if (count == 0)
                {
                    
                    if (Inventory.ReturnItemCount() >= 5)
                    {
                        Inventory.RemoveItems(5);
                        new HealingPotion(Globals.Player, healingPotion);
                    }


                }
                else if (count == 1)
                {
                   
                    if (Inventory.ReturnItemCount() >= 5)
                    {
                        Inventory.RemoveItems(5);
                        new ShieldPotion(Globals.Player, shieldPotion);
                    }

                }
                else if (count == 2)
                {
                    
                    if (Inventory.ReturnItemCount() >= 5)
                    {
                        Inventory.RemoveItems(5);
                        new RandomPotion(Globals.Player, randomPotion);
                    }

                }
                else if(count == 3)
                {
                    if(Inventory.ReturnItemCount() >= 2)
                    {
                        Inventory.RemoveItems(2);
                        Inventory.AddArrow(1);
                    }
                }
            }
            count++;
            
        });


        if (done.Contains(mousePosition) && mouseState.LeftButton == ButtonState.Pressed)
        {
            AreWeDone = true;

        }

        position = new Vector2(Globals.WindowSize.X / 2, Globals.WindowSize.Y / 2 - 550) + Camera.Position;
        done = new Rectangle((int)position.X, (int)position.Y, 100, 100);
    }


    public void LoadContent()
    {
        int count = 0;
        shopKeeperTexture = Globals.Content.Load<Texture2D>("Hero");
        shopKeeperBounds = new Rectangle(Globals.WindowSize.X / 2, Globals.WindowSize.Y / 2, shopKeeperTexture.Width, shopKeeperTexture.Height);

        

        while (count < 3)
        {
            int randomZahl = random.Next(0, perks.Length - 1);

            if (!perksList.Any())
            {
                perksList.Add(perks[randomZahl]);
            }
            else
            {
                if (perksList.Contains(perks[randomZahl]))
                {
                    continue;
                }
                perksList.Add(perks[randomZahl]);

            }

            count++;
        }
        AreWeDone = false;

    }

    public void HandleInput()
    {
        MouseState mouseState = Mouse.GetState();
        bool mouseClicked = false;

        Point mousePosition = new Point(mouseState.X + (int)Camera.Position.X, mouseState.Y + (int)Camera.Position.Y);

        if (mouseState.LeftButton == ButtonState.Pressed && !mouseClicked && !ChoicesBeenMade)
        {


            for (int i = 0; i < itemChoices.Length; i++)
            {

                if (itemChoices[i].Contains(mousePosition))
                {

                    if (perksList[i].Equals(Perklist.PLAYERHP))
                    {
                        Globals.Player.MaxHealth += 1;
                        Globals.Player.Health = Globals.Player.MaxHealth;
                    }
                    if (perksList[i].Equals(Perklist.PLAYERPOTION)) new HealingPotion(Globals.Player, healingPotion);
                    if (perksList[i].Equals(Perklist.PLAYERSPEED))
                    {
                        Globals.Player.Speed += 1;
                        Globals.Player.BaseSpeed += 1;
                        Globals.Player.Sprint += 1;
                    }
                    if (perksList[i].Equals(Perklist.PLAYERATTACK)) Globals.Player.Weapon.Damage += 1;
                    if (perksList[i].Equals(Perklist.PLAYERDEFENSE)) new ShieldPotion(Globals.Player, shieldPotion);
                    if (perksList[i].Equals(Perklist.PLAYERDOPPELDMG))
                    {
                        Enemy.Damage *= 2;
                        Globals.Player.Weapon.Damage *= 2;
                    }

                    mouseClicked = true;
                    ChoicesBeenMade = true;
                }
            }

        }
        if (mouseState.LeftButton == ButtonState.Released)
        {
            mouseClicked = false;
        }


    }


}
