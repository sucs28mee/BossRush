﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Items.Card
{
    abstract class CardPacketBase : ModItem
    {
        private int countX = 0;
        private float positionRotateX = 0;
        private void PositionHandle()
        {
            if (positionRotateX < 3 && countX == 1)
            {
                positionRotateX += .3f;
            }
            else
            {
                countX = -1;
            }
            if (positionRotateX > 0 && countX == -1)
            {
                positionRotateX -= .3f;
            }
            else
            {
                countX = 1;
            }
        }
        public virtual int PacketType => 0;
        protected void CardDropHandle(Player player, int amount = 1)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            for (int i = 0; i < amount; i++)
            {
                if (Main.rand.NextBool(Card.PlatinumCardDropChance))
                {
                    player.QuickSpawnItem(entitySource, ModContent.ItemType<PlatinumCard>());
                    continue;
                }
                if (Main.rand.NextBool(Card.GoldCardDropChance))
                {
                    player.QuickSpawnItem(entitySource, ModContent.ItemType<GoldCard>());
                    continue;
                }
                if (Main.rand.NextBool(Card.SilverCardDropChance))
                {
                    player.QuickSpawnItem(entitySource, ModContent.ItemType<SilverCard>());
                    continue;
                }
                player.QuickSpawnItem(entitySource, ModContent.ItemType<CopperCard>());
            }
        }
        Color auraColor;
        private void ColorHandle()
        {
            switch (PacketType)
            {
                case 1:
                    auraColor = new Color(255, 100, 0, 30);
                    break;
                case 2:
                    auraColor = new Color(200, 200, 200, 30);
                    break;
                case 3:
                    auraColor = new Color(255, 255, 0, 30);
                    break;
                default:
                    auraColor = new Color(255, 255, 255, 30);
                    break;
            }
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            PositionHandle();
            ColorHandle();
            Main.instance.LoadItem(Item.type);
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            for (int i = 0; i < 3; i++)
            {
                spriteBatch.Draw(texture, position + new Vector2(positionRotateX, positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, position + new Vector2(positionRotateX, -positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, position + new Vector2(-positionRotateX, positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, position + new Vector2(-positionRotateX, -positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
            }
            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (Item.whoAmI != whoAmI)
            {
                return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
            }
            Main.instance.LoadItem(Item.type);
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 drawPos = Item.position - Main.screenPosition + origin;
            for (int i = 0; i < 3; i++)
            {
                spriteBatch.Draw(texture, drawPos + new Vector2(positionRotateX, positionRotateX), null, auraColor, rotation, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, drawPos + new Vector2(positionRotateX, -positionRotateX), null, auraColor, rotation, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, drawPos + new Vector2(-positionRotateX, positionRotateX), null, auraColor, rotation, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, drawPos + new Vector2(-positionRotateX, -positionRotateX), null, auraColor, rotation, origin, scale, SpriteEffects.None, 0);
            }
            return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }
    }

    internal class CardPacket : CardPacketBase
    {
        public override int PacketType => 1;
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Chest);
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 28;
            Item.maxStack = 30;
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override void RightClick(Player player)
        {
            CardDropHandle(player);
        }
    }
    internal class BigCardPacket : CardPacketBase
    {
        public override int PacketType => 2;
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.GoldChest);
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 28;
            Item.maxStack = 30;
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override void RightClick(Player player)
        {
            CardDropHandle(player, 3);
        }
    }
    internal class BoxOfCard : CardPacketBase
    {
        public override int PacketType => 3;
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.ShadowChest);
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 28;
            Item.maxStack = 30;
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override void RightClick(Player player)
        {
            CardDropHandle(player, 5);
        }
    }
}