﻿using Terraria;
using Terraria.ID;
using Terraria.Chat;
using Terraria.Audio;
using BossRush.Common;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Toggle
{
    public class CursedSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Make every boss when you fight instant kill you\n" +
                "\"The only way to make god acknowledge you\""); */
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 8));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.height = 60;
            Item.width = 56;
            Item.value = 0;
            Item.rare = ItemRarityID.Purple;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = new TooltipLine(Mod, "ChallengeGod", "A gift from God of challenge" + $"[i:{ModContent.ItemType<CursedSkull>()}]");
            line.OverrideColor = BossRushColor.MultiColor(new List<Color>() { new Color(255,50,255), new Color(100, 50, 100) }, 2);
            tooltips.Add(line);
        }

        public override bool CanUseItem(Player player)
        {
            return !BossRushUtils.IsAnyVanillaBossAlive();
        }
        int count = 0;
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (count == 0)
                    {
                        player.GetModPlayer<ModdedPlayer>().gitGud = true;
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Have fun"), Colors.RarityDarkRed);
                        count++;
                    }
                    else
                    {
                        player.GetModPlayer<ModdedPlayer>().gitGud = false;
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Skill issue"), new Microsoft.Xna.Framework.Color(0, 110, 225));
                        count = 0;
                    }
                }
            }
            return true;
        }
    }
}
