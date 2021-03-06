﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using DBZMOD;
using Terraria.ID;
using Terraria.ModLoader;
using Util;

namespace DBZMOD.Projectiles.Auras.Dev
{
    public class SSJSpectrumAuraProj : KaiokenAuraProj
    {
        private int ChargeSoundTimer = 240;
        private int LightningTimer = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 8;
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
            projectile.netUpdate = true;
            ScaledAuraOffset.Y = -30;
			projectile.light = 1f;
        }
		public override void PostAI()
        {
            for (int d = 0; d < 1; d++)
            {
                if (Main.rand.NextFloat() < 0.5f)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, 113, 115, 91, 0f, 0f, 0, new Color(Main.DiscoColor.R, Main.DiscoColor.G, Main.DiscoColor.B), 0.75f);
                    dust.noGravity = true;
                }
				if (Main.rand.NextFloat() < 0.25f)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, 113, 115, 91, 0f, 0f, 0, new Color(Main.DiscoColor.R, Main.DiscoColor.G, Main.DiscoColor.B), 0.25f);
                    dust.noGravity = true;
                }
            }
        }
        public override void AI()
        {

            Player player = Main.player[projectile.owner];
            if (!player.HasBuff(Transformations.SPECTRUM.GetBuffId()))
            {
                projectile.Kill();
            }


            bool shouldPlayAudio = SoundUtil.ShouldPlayPlayerAudio(player, true);
            if (shouldPlayAudio)
            {
                ChargeSoundTimer++;
                if (ChargeSoundTimer > 420)
                {
                    player.GetModPlayer<MyPlayer>().TransformationSoundInfo = SoundUtil.PlayCustomSound("Sounds/SSG", player, .7f, .1f);
                    ChargeSoundTimer = 0;
                }
            }
            base.AI();
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            projectile.scale = Main.GameZoomTarget;
            ScaledAuraOffset.Y = -30 * Main.GameZoomTarget;            
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            return base.PreDraw(spriteBatch, lightColor);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.End();
            spriteBatch.Begin();
        }
    }
}