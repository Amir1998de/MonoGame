using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public class HealingPotion : Potion
{
    public HealingPotion(Player player, Texture2D texture) : base(player, texture)
    {
        id = 1;
        Inventory.AddItem(this);

    }

    public override bool Equals(object obj)
    {
        return obj is HealingPotion potion &&
               base.Equals(obj) &&
               wert == potion.wert &&
               EqualityComparer<Texture2D>.Default.Equals(potionTexture, potion.potionTexture) &&
               EqualityComparer<Player>.Default.Equals(player, potion.player) &&
               pos.Equals(potion.pos) &&
               id == potion.id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), wert, potionTexture, player, pos, id);
    }

    public override void UseEffect()
    {
        if (Globals.Player != null)
        {

            Globals.Player.AddHealth(wert);
            Inventory.RemoveItem(this);
        }
    }

    
}
