﻿using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Utils;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.Zenish
{
    internal class Nadir : SynergyModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Item.BossRushSetDefault(34, 54, 20, 7f, 10, 30, ItemUseStyleID.Shoot, true);
            Item.BossRushSetDefaultSpear(1, 1);
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 5);
        }
        public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer)
        {
            base.ModifySynergyToolTips(ref tooltips, modplayer);
        }
        public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer)
        {
            base.HoldSynergyItem(player, modplayer);
        }
        public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = position.PositionOFFSET(velocity, 30);
        }
        public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ZenishProjectile>(), damage, knockback, player.whoAmI, 0, 0, Main.rand.Next(TerrariaArrayID.AllOreBroadSword));
            CanShootItem = false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CopperBroadsword)
                .AddIngredient(ItemID.TinBroadsword)
                .AddIngredient(ItemID.IronBroadsword)
                .AddIngredient(ItemID.LeadBroadsword)
                .AddIngredient(ItemID.SilverBroadsword)
                .AddIngredient(ItemID.TungstenBroadsword)
                .AddIngredient(ItemID.GoldBroadsword)
                .AddIngredient(ItemID.PlatinumBroadsword)
                .Register();
        }
    }
    class ZenishProjectile : SynergyModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.CopperBroadsword);
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 999;
            Projectile.usesLocalNPCImmunity = true;
        }
        int MouseXPosDirection;
        float MaxLengthX = 0;
        float MaxLengthY = 0;
        public override void SynergyPreAI(Player player, PlayerSynergyItemHandle modplayer, out bool runAI)
        {
            if (Projectile.timeLeft == 999)
            {
                MouseXPosDirection = Main.rand.NextBool().BoolOne() * (Main.MouseWorld.X - player.Center.X > 0 ? 1 : -1);
                MaxLengthX = (Main.MouseWorld - player.Center).Length();
                MaxLengthY = -(MaxLengthX + Main.rand.NextFloat(-10, 80)) * .25f * MouseXPosDirection;
            }
            base.SynergyPreAI(player, modplayer, out runAI);
        }
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            int maxProgress = 50;
            if (Projectile.timeLeft > maxProgress)
            {
                Projectile.timeLeft = maxProgress;
            }
            int halfmaxProgress = (int)(maxProgress * .5f);
            int quadmaxProgress = (int)(maxProgress * .25f);
            float progress;
            if (Projectile.timeLeft > halfmaxProgress)
            {
                progress = (maxProgress - Projectile.timeLeft) / (float)halfmaxProgress;
            }
            else
            {
                progress = Projectile.timeLeft / (float)halfmaxProgress;
            }
            float X = MathHelper.SmoothStep(-60, MaxLengthX, progress);
            ProgressYHandle(Projectile.timeLeft, halfmaxProgress, quadmaxProgress, out float Y);
            Vector2 VelocityPosition = new Vector2(X, Y).RotatedBy(Projectile.velocity.ToRotation());
            Projectile.Center = player.Center + VelocityPosition;
            float rotation = MathHelper.SmoothStep(0, 360, 1 - Projectile.timeLeft / (float)maxProgress) * MouseXPosDirection;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4 + MathHelper.Pi + MathHelper.ToRadians(rotation);
        }
        private void ProgressYHandle(int timeleft, float progressMaxHalf, float progressMaxQuad, out float Y)
        {
            if (timeleft > progressMaxHalf + progressMaxQuad)
            {
                float progressY = 1 - (timeleft - (progressMaxHalf + progressMaxQuad)) / progressMaxQuad;
                Y = MathHelper.SmoothStep(0, MaxLengthY, progressY);
                return;
            }
            if (timeleft > progressMaxQuad)
            {
                float progressY = 1 - (timeleft - progressMaxQuad) / progressMaxHalf;
                Y = MathHelper.SmoothStep(MaxLengthY, -MaxLengthY, progressY);
                return;
            }
            else
            {
                float progressY = 1 - timeleft / progressMaxQuad;
                Y = MathHelper.SmoothStep(-MaxLengthY, 0, progressY);
                return;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = ModContent.Request<Texture2D>(BossRushUtils.GetVanillaTexture<Item>((int)Projectile.ai[2])).Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
            Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos2 = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos2, null, color, Projectile.oldRot[k], origin, Projectile.scale, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}
