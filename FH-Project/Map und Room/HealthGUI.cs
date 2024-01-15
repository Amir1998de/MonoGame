using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;



namespace FH_Project;
public class HealthGUI
{
    private Player player;
    private bool isMKeyPressed = false; // Pluss Herz
    private bool isNKeyPressed = false;// Minus Herz
    private Texture2D heartTexture;
    private GameTime  gameTime;


    public HealthGUI(Player player, Texture2D heartTexture) {
        this.player = player;
        this.heartTexture = heartTexture;
        Debug.WriteLine("Haupt Health " + Globals.Player.Health);
    

    }
    private void DrawHeart()
    {

        int heartSpacing = 80;
        int heartPositionX = 500;

        Microsoft.Xna.Framework.Vector2 heartPosition = new Microsoft.Xna.Framework.Vector2(heartPositionX, 10) + Camera.Position;


        // Zeichne die Herzsymbole basierend auf der aktuellen Gesundheit des Spielers
        for (int i = 0; i < Globals.Player.Health; i++)
        {
            GameplayScreen.spriteBatch.Draw(heartTexture, heartPosition, Color.White);
            heartPosition.X += heartSpacing;
        }

    }

    public void UpdateHerz() {
        // this cods are for Test 

        DrawHeart();


        // just for Test 
        KeyboardState keyboardState = Keyboard.GetState();
        // M 
        if (keyboardState.IsKeyDown(Keys.M) && !isMKeyPressed)
        {
            Globals.Player.AddHealth(1);
            Debug.WriteLine("(Plus)Change Health " + Globals.Player.Health);
            isMKeyPressed = true; 
        }

        if (keyboardState.IsKeyUp(Keys.M)) isMKeyPressed = false;

        // N
        if (keyboardState.IsKeyDown(Keys.N) && !isNKeyPressed)
        {
            Globals.Player.ReduceHealth(1);
            Debug.WriteLine("(Minus)Change Health " + Globals.Player.Health);
            isNKeyPressed = true;
        }

        if (keyboardState.IsKeyUp(Keys.N)) isNKeyPressed = false;

    }


}

