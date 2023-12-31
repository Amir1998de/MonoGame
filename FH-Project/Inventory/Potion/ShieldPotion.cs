﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public class ShieldPotion : Potion
{
    public ShieldPotion(Player player, Texture2D texture) : base(player, texture)
    {
        id = 2;
        Inventory.AddItem(this);
    }



    public override void UseEffect()
    {
        if (player != null)
        {

            player.AddShield(wert);
            Inventory.RemoveItem(this);
        }
    }
}
