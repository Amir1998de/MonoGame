using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace FH_Project;

public static class CollisionHandler
{
	

	public static bool CollisionEntitys(Entity e1, Entity e2)
	{
		Rectangle enity1 = new((int)e1.Position.X, (int)e1.Position.Y, e1.EntityTexture.Width,e1.EntityTexture.Height);
		Rectangle enity2 = new((int)e2.Position.X, (int)e2.Position.Y, e2.EntityTexture.Width, e2.EntityTexture.Height);

		return enity1.Intersects(enity2);
	}

	public static bool CollisionWithEnviorment(Texture2D texture2D, Vector2 position, Entity entity)
	{
        Rectangle enityRectangle = new((int)entity.Position.X, (int)entity.Position.Y, entity.EntityTexture.Width, entity.EntityTexture.Height);
        Rectangle textureRectangle= new((int) position.X, (int) position.Y, texture2D.Width, texture2D.Height);

		return enityRectangle.Intersects(textureRectangle);
    }

	
}
