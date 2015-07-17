using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TAPI;

namespace ProjectAdvance.Buffs
{
    class BloodRite : ModBuff
    {
        public override void DamageNPC(Player player, NPC npc, int hitDir, ref int damage, ref float knockback, ref bool crit, ref float critMult)
        {
                for (int i = 0; i < 6; i++)
                {
                 Main.dust[Dust.NewDust(player.getRect(), 12)].velocity=new Microsoft.Xna.Framework.Vector2(Main.rand.Next(-3,3),Main.rand.Next(-1,3));
                }
            int heal=(int)(damage * 0.1f);
            CombatText.NewText(player.getRect(), Microsoft.Xna.Framework.Color.Green, "+" + heal);
            player.statLife += heal;
            base.DamageNPC(player, npc, hitDir, ref damage, ref knockback, ref crit, ref critMult);
        }
    }
}
