﻿﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Weapons.Tier_2
{
	public class DestructoDisk : KiItem
	{
        private Player player;

        public override void SetDefaults()
		{
			item.shoot = mod.ProjectileType("DestructoDiskProjectile");
			item.shootSpeed = 20f;
			item.damage = 42;
			item.knockBack = 5f;
			item.useStyle = 5;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 28;
			item.useTime = 28;
			item.width = 20;
			item.noUseGraphic = true;
			item.height = 20;
			item.autoReuse = false;
			item.value = 4000;
			item.rare = 2;
            KiDrain = 40;
			WeaponType = "Disk";
			if(!Main.dedServ)
            {
                item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/DiscFire").WithPitchVariance(.3f);
            }
	    }
	    public override void SetStaticDefaults()
		{
		Tooltip.SetDefault("'Its a frizbee I swear.'");
		DisplayName.SetDefault("Destructo Disk");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "CalmKiCrystal", 30);
            recipe.AddIngredient(null, "AstralEssentia", 12);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
	        recipe.AddRecipe();
		}
	}
}
