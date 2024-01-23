using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Input;
using System.Numerics;

namespace FH_Project;
public class InventoryDraw
{

    //
    private bool isUKeyPressed = false; // add Item
    private bool isLKeyPressed = false;// remove Item

    private Potion ShieldPotion2;


    private List<Item> items = new List<Item>();
    private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
    // string name , string count
    private Dictionary<Texture2D, int> currItems = new Dictionary<Texture2D, int>();


    public InventoryDraw()
    {
        items = Inventory.GetAll();
    }

    public void Draw()
    {
        items = Inventory.GetAll();

        int PositionX = 10;
        int PositionY = 40;
        int Spacing = 80;
        int count = 0;

        Microsoft.Xna.Framework.Vector2 itemPosition = new Microsoft.Xna.Framework.Vector2(PositionX, PositionY) + Camera.Position;

        //GameplayScreen.spriteBatch.Draw(textures[0], heartPosition, Microsoft.Xna.Framework.Color.White);

        // Debug.WriteLine("textures[0]" + textures[0].Name);

        for (int i = 0; i < items.Count; i++)
        {
            bool sameItem = false;
            string sameItemName = "";
            if (items[i] != null)
            {

                //Debug.WriteLine("items : " + items[i].GetType().ToString());

                textures.TryGetValue(items[i].GetType().ToString(), out Texture2D retrievedTexture);
                if (retrievedTexture != null)
                {
                    //   Debug.WriteLine("textures : " + retrievedTexture.GetType().ToString());

                    for (int j = 0; j < items.Count; j++)
                    {
                        for (int n = j + 1; n < items.Count; n++)
                        {
                            if (AreItemsSimilar(items[j], items[n]))
                            {
                                sameItem = true;
                                sameItemName = items[j].GetType().ToString();
                            }

                        }
                    }
                    GameplayScreen.spriteBatch.Draw(retrievedTexture, itemPosition, Microsoft.Xna.Framework.Color.White);

                    itemPosition.Y += Spacing;

                }



            }

        }
        // Debug.WriteLine("count Inventory : " + count++);


    }
    //
    public void AddTexture(Texture2D texture2D, string name)
    {

        textures[name] = texture2D;

    }
    //

    public void TestInventoryDraw(Texture2D shieldPotionTexture)
    {


        // just for Test 
        KeyboardState keyboardState = Keyboard.GetState();
        // U
        if (keyboardState.IsKeyDown(Keys.U) && !isUKeyPressed)
        {
            ShieldPotion2 = new ShieldPotion(Globals.Player, shieldPotionTexture);
            isUKeyPressed = true;
        }

        if (keyboardState.IsKeyUp(Keys.U)) isUKeyPressed = false;

        // L
        if (keyboardState.IsKeyDown(Keys.L) && !isLKeyPressed)
        {
            // Globals.Player.AddShield(2);
            isLKeyPressed = true;
        }

        if (keyboardState.IsKeyUp(Keys.L)) isLKeyPressed = false;

    }

    private bool AreItemsSimilar(Item item1, Item item2)
    {
        return item1.GetType().ToString().Equals(item2.GetType().ToString());
    }


}


