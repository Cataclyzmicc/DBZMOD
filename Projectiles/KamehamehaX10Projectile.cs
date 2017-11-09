using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Projectiles
{
	public class KamehamehaX10Projectile : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kamehameha x10");
        }
        public override void SetDefaults()
		{
			projectile.width = 238;
			projectile.height = 800;
            projectile.friendly = true;
            projectile.penetrate = -1;                       
            Main.projFrames[projectile.type] = 1;           
            projectile.hostile = false;
			projectile.magic = true;                        
            projectile.tileCollide = false;                 
			projectile.ignoreWater = true;
		}

		public override void AI()
		{   
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
            projectile.localAI[0] += 1f;
			projectile.alpha = (int)projectile.localAI[0] * 2;
			
			if (projectile.localAI[0] > 130f)
            {
				projectile.Kill();
			}
           
        }
        
    }
}