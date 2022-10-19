﻿using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Weapon.MeleeSynergyWeapon.EnchantedOreSword
{
    class EnchantedSilverSwordP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.alpha = 0;
            Projectile.light = 0.65f;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Projectile.alpha += 3;
            if (Projectile.alpha >= 235)
            {
                Projectile.Kill();
            }
        }
    }
}
