using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;


namespace FH_Project;

public class CollisionWrapper
{
    public bool IsCollision { get; private set; }
    public Rectangle Collision { get; private set; }
    public static HashSet<Rectangle> CollisionWrappers { get; set; } = new HashSet<Rectangle>();


    public CollisionWrapper(bool isCollision, Rectangle collision) 
    { 
        IsCollision = isCollision;
        Collision = collision;
        
    }

}

