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

    public Enemy()
	{
	}
}
