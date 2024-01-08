using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public static class Inventory
{
    private const int SPACE = 20;
    private static List<Item> items = new List<Item>();

    

    //ID gibt an welches Potion es ist
    public static Item getPotion(int id)
    {
        foreach (var item in items)
        {
            if (item.GetType().ToString().Equals("FH_Project.HealingPotion") || item.GetType().ToString().Equals("FH_Project.ShieldPotion") || item.GetType().ToString().Equals("FH_Project.RandomPotion"))
            {
                Potion potion = (Potion)item;
                if (potion.id == id) return potion;
            }

        }
        throw new NoItemFoundException("This Item does not Exist");
    }

    public static void changeWeapon(int id)
    {
        foreach(var item in items)
        {
            if (item.GetType().ToString().Equals("FH_Project.Sword") || item.GetType().ToString().Equals("FH_Project.Bow") || item.GetType().ToString().Equals("FH_Project.Hammer"))
            {
                Weapon weapon = (Weapon)item;
                if(weapon.ID==id)
                {
                    Globals.Player.Weapon= weapon;
                    return;
                }
            }

        }
    }

    public static void UseItem(Item item)
    {
        if (!items.Contains(item)) throw new NoItemFoundException("This Item does not Exist");

        items.Find(i => i.Equals(item)).UseEffect();

    }

    public static void AddItem(Item item)
    {
        if (items.Count >= SPACE) throw new InventoryFullException("Inventory is Full");

        items.Add(item);
    }

    public static void RemoveItem(Item item)
    {
        if (!items.Contains(item)) throw new NoItemFoundException("This Item does not Exist");

        items.Remove(item);
    }
}