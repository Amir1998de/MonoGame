using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FH_Project;

public class Homebase : Room
{
    private Perklist[] perks;
    Random random = new();
    private SpriteFont spriteFont;
    private List<Perklist> list = new List<Perklist>();
    public bool AreWeDone { get; set; }
    public Homebase(int x, int y, int breite, int höhe, bool isKorridor) : base(x, y, breite, höhe, isKorridor)
    {
        perks = (Perklist[])Enum.GetValues(typeof(Perklist));
        spriteFont = Globals.Content.Load<SpriteFont>("Verdana");
        AreWeDone = true;
    }

    public override void Draw()
    {
        for (int y = 0; y < tileSize.Y; y++)
        {
            for (int x = 0; x < tileSize.X; x++)
                tiles[x, y].Draw();
        }
        int diff = 600;
        foreach (var item in list)
        {
            Globals.SpriteBatch.DrawString(spriteFont, item.ToString(), new Vector2(diff, Globals.WindowSize.Y / 2) + Camera.Position, Color.Red);
            diff += 300;
        }
    }

    public void Update()
    {

    }

    public void Shop()
    {
        int count = 0;


        while (count < 3)
        {
            int randomZahl = random.Next(0, perks.Length - 1);

            if (!list.Any())
            {
                list.Add(perks[randomZahl]);
                count++;
            }
            else
            {
                if (list.Contains(perks[randomZahl]))
                {
                    continue;
                }
                list.Add(perks[randomZahl]);
                count++;
            }

        }
        AreWeDone = false;

    }
}
