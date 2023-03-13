﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using BossRush.Texture;

namespace BossRush.Items.Weapon.SummonerSynergyWeapon.StickySlime
{
    internal class StickyFlower : ModItem, ISynergyItem
    {
        public override string Texture => ItemTexture.MISSINGTEXTURE;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Quite a sticky ghost");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; 
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.knockBack = 3f;
            Item.mana = 10; 
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(gold: 30);
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item44; 
          
            Item.noMelee = true; 
            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<StickyFriend>();

            Item.shoot = ModContent.ProjectileType<GhostSlime>(); 
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
        
            player.AddBuff(Item.buffType, 2);

           
            var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage;

           
            return false;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SlimeStaff)
                .AddIngredient(ItemID.AbigailsFlower)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
}
