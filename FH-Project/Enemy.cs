using FH_Projekt;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public abstract class Enemy : Entity, IObserver
{
    #region Variablen
    private float attackPower;
    

    #endregion Variablen

    public Enemy(int health, float speed, Vector2 pos, Vector2 velocity, Texture2D texture, float maxH, float maxW
        ) : base(health, speed,pos,velocity, texture, maxH, maxW)
	{
	}
}
