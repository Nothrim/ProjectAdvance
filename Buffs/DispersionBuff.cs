using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;
using Microsoft.Xna.Framework;
namespace ProjectAdvance.Buffs
{
    class DispersionBuff : ModBuff
    {
        public override void Effects(Player player, int index)
        {
            player.invis = true;
            player.noFallDmg = true;
            player.immune = true;
            float randomposition;
            if (Main.rand.Next(1) == 0)
                randomposition = -Main.rand.Next(10);
            else
                randomposition = Main.rand.Next(10);
            int dust;
            int scale=3+Main.rand.Next(6);
            dust=Dust.NewDust(new Vector2(player.position.X+randomposition, player.position.Y + Main.rand.Next(17)), scale,scale, 71, 0, -0);
            Main.dust[dust].noGravity = true;
            base.Effects(player, index);
        }
       
        public override void DamagePlayer(Player player, NPC npc, int hitDir, ref int damage, ref bool crit, ref float critMult)
        {
            damage = 0;
            base.DamagePlayer(player, npc, hitDir, ref damage, ref crit, ref critMult);
        }
    }
}
