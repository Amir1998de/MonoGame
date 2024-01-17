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
    private static List<Item> items = new List<Item>();
    public static int AnzahlPfeile { get; set; } = 5;

    

    //ID gibt an welches Potion es ist
    public static Item GetPotion(int id)
    {
        foreach (var item in items)
        {
            if (item.GetType().ToString().Equals("FH_Project.HealingPotion") || item.GetType().ToString().Equals("FH_Project.ShieldPotion") || item.GetType().ToString().Equals("FH_Project.RandomPotion"))
            {
                Potion potion = (Potion)item;
                if (potion.id == id) return potion;
            }

        }
        return null;
    }

    public static void AddArrow(int number)
    {
        AnzahlPfeile += number;
    }

    public static int ReturnItemCount()
    {
        int count = 0;
        items.ForEach(item => {
            if (item.GetType().ToString().Equals("FH_Project.Enemydrops"))
                count++;
        });


        return count;
    }

    public static void ChangeWeapon(int id)
    {
        foreach(var item in items)
        {
            if (item.GetType().ToString().Equals("FH_Project.Sword") || item.GetType().ToString().Equals("FH_Project.Bow") || item.GetType().ToString().Equals("FH_Project.Hammer"))
            {
                Weapon weapon = (Weapon)item;
                if(weapon.ID == id && !weapon.GetType().ToString().Equals(Globals.Player.Weapon.GetType().ToString()))
                {
                    Globals.Player.Weapon = weapon;
                    return;
                }
            }

        }
    }

    public static void UseItem(Item item)
    {
        if (!items.Contains(item)) return;

        items.Find(i => i.Equals(item)).UseEffect();

    }

    public static void AddItem(Item item)
    {
        items.Add(item);
    }

    public static void RemoveItems(int howMany)
    {
        int count = 0;
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].GetType().ToString().Equals("FH_Project.Enemydrops"))
            {
                RemoveItem(items[i]);
                count++;
            }
            if (count >= howMany) return;
        }

    }

    public static void ResetInventory()
    {
        items.Clear();
        AnzahlPfeile = 5;
    }

    public static void RemoveItem(Item item)
    {
        if (!items.Contains(item)) throw new NoItemFoundException("This Item does not Exist");

        items.Remove(item);
    }
}