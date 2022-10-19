﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.Audio;

namespace BossRush.ExtraItem
{
	public class CursedDoll : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed totem");
			Tooltip.SetDefault("Spawn skeletron");
			ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 12; // This helps sort inventory know this is a boss summoning item.
			NPCID.Sets.MPAllowedEnemies[NPCID.SkeletronHead] = true;
			NPCID.Sets.MPAllowedEnemies[NPCID.SkeletronHand] = true;
		}

		public override void SetDefaults()
		{
			Item.height = 55;			
			Item.width = 53;
			Item.maxStack = 999;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			return true;
		}

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				// If the player using the item is the client
				// (explicitely excluded serverside here)
				SoundEngine.PlaySound(SoundID.Roar, player.position);

				int type = NPCID.SkeletronHead;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					// If the player is not in multiplayer, spawn directly
					NPC.SpawnBoss((int)player.Center.X, (int)player.Center.Y - 400, type,player.whoAmI);
				}
				else
				{
					// If the player is in multiplayer, request a spawn
					// This will only work if NPCID.Sets.MPAllowedEnemies[type] is true, which we set in this class above
					NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type);
				}
			}

			return true;
		}
    }
}
