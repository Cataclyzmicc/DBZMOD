using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Buffs
{
	public class KaiokenBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Kaioken");
			Description.SetDefault("2x Damage, 2x Speed, Drains Life.");
			Main.buffNoTimeDisplay[Type] = true;
		}
        public override void Update(Player player, ref int buffIndex)
        {                                             
            player.lifeRegen = -20;
			player.moveSpeed *= 2f;
            player.meleeDamage *= 2f;
			player.rangedDamage *= 2f;
			player.magicDamage *= 2f;
			player.minionDamage *= 2f;
			player.thrownDamage *= 2f;
			MyPlayer.ModPlayer(player).KiDamage *= 2f;
			Lighting.AddLight(player.Center, 5f, 0f, 0f);
        }	
	}
}
