using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;

namespace DBZMOD.Items.Weapons.Fist
{
	public class KiFist1 : KiItem
	{
        private string tooltip;
		public override void SetDefaults()
		{
			item.damage = 10;
			item.knockBack = 5f;
			item.useStyle = 3;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 30;
			item.useTime = 1;
			item.width = 12;
			item.noUseGraphic = true;
			item.height = 12;
			item.autoReuse = false;
            item.channel = true;
			item.value = 0;
			item.rare = 1;
	    }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fist");
            Tooltip.SetDefault("The most essential skill of any martial artist.");
        }

        private const string FistLine1 = "\nLeft click to punch, has no cooldown between attacks.";
        private const string FistLine2 = "\nHolding Left click allows you to do a flurry attack.";
        private const string FistLine3 = "\nRight click to do a ki infused punch, this sends anything flying.";
        private const string ZanzokenLine1 = "\nDouble tap in any direction to do a short ranged teleport, teleporting towards a enemy makes you teleport to them.";
        private const string ZanzokenHeavyLine1 = "\nHeavy hitting after teleporting to a enemy will send them flying even harder.";
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            string tooltip = FistLine1;
            if (MyPlayer.ModPlayer(player).CanUseFlurry)
            {
                tooltip += FistLine2;
            }
            if (MyPlayer.ModPlayer(player).CanUseHeavyHit)
            {
                tooltip += FistLine3;
            }
            if (MyPlayer.ModPlayer(player).CanUseZanzoken)
            {
                tooltip += ZanzokenLine1;
            }
            if (MyPlayer.ModPlayer(player).CanUseHeavyHit && MyPlayer.ModPlayer(player).CanUseZanzoken)
            {
                tooltip += ZanzokenHeavyLine1;
            }

            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.mod == "Terraria" && line2.Name == "Tooltip0")
                {
                    line2.text = line2.text + "\nScales with your progression." + tooltip;
                }
            }
            base.ModifyTooltips(tooltips);
        }
        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "StableKiCrystal", 20);
            recipe.AddTile(null, "ZTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }*/
        /* public override void HoldItem(Player player)
         {
             Projectile.NewProjectile(player.Center.X - 40, player.Center.Y + 90, 0, 0, mod.ProjectileType("KiFistProj"), 0, 0, player.whoAmI);
         }*/
    }
}
