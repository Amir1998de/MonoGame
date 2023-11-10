using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public class Inventory
{
	private const int SPACE = 20;
	private static List<Item> items = new List<Item>();

	private Inventory() { }
	

	public static void UseItem(Item item)
	{
        if (!items.Contains(item)) return;

        items.Find(i => i.Equals(item)).UseEffect();
    }

	public static void AddItem(Item item) 
	{
		if(items.Count >= SPACE) throw new InventoryFullException("Inventory is Full");

		items.Add(item);
	}

	public static void RemoveItem(Item item) 
	{
		if (!items.Contains(item)) return;

		items.Remove(item);
	}
}
