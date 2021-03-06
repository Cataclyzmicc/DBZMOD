﻿﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using DBZMOD;
using Terraria.ID;
using Terraria.ModLoader;
using Util;

namespace DBZMOD.Projectiles.Auras
{
    public class SSJ1AuraProj : AuraProjectile
    {
        private int ChargeSoundTimer;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 113;
            projectile.height = 115;
            projectile.aiStyle = 0;
            projectile.timeLeft = 10;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.damage = 0;
            IsSSJAura = true;
			projectile.light = 1f;
            HasComplexBlendStates = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (!player.HasBuff(Transformations.SSJ1.GetBuffId()) && !player.HasBuff(Transformations.ASSJ.GetBuffId()) && !player.HasBuff(Transformations.USSJ.GetBuffId()))
            {
                projectile.Kill();
            }

            bool shouldPlayAudio = SoundUtil.ShouldPlayPlayerAudio(player, true);
            if (shouldPlayAudio)
            {
                ChargeSoundTimer++;
                if (ChargeSoundTimer > 22)
                {
                    player.GetModPlayer<MyPlayer>().TransformationSoundInfo = SoundUtil.PlayCustomSound("Sounds/EnergyCharge", player, .7f, .2f);
                    ChargeSoundTimer = 0;
                }
            }
            base.AI();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);          
            
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.End();
            spriteBatch.Begin();
		}
    }
}