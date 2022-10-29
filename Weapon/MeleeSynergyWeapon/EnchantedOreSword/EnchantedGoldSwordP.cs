﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace BossRush.Weapon.MeleeSynergyWeapon.EnchantedOreSword
{
    internal class EnchantedGoldSwordP : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.GoldShortsword;
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.alpha = 0;
            Projectile.light = 0.75f;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 4;
        }

        public override void AI()
        {
            Projectile.velocity += (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.UnitX);
            if (Projectile.velocity.X > 10)
            {
                Projectile.velocity.X = 10;
            }
            else if (Projectile.velocity.X < -10)
            {
                Projectile.velocity.X = -10;
            }
            if (Projectile.velocity.Y > 10)
            {
                Projectile.velocity.Y = 10;
            }
            else if (Projectile.velocity.Y < -10)
            {
                Projectile.velocity.Y = -10;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Projectile.alpha += 2;
            if (Projectile.alpha >= 235)
            {
                Projectile.Kill();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + origin + new Vector2(Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)(Projectile.oldPos.Length));
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}
