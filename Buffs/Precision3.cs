using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;
namespace ProjectAdvance.Buffs 
{
    class Precision3 : ModBuff
    {
        bool shoot;
        public override void Start(Player player, int index)
        {
            shoot = false;
            base.Start(player, index);
        }
        public override void DamageNPC(Projectile projectile, NPC npc, int hitDir, ref int damage, ref float knockback, ref bool crit, ref float critMult)
        {
            if(!shoot && projectile.ranged)
            {
                shoot = true;
                damage+=damage*3;
            }
        }
        public override void End(Player player, int index)
        {
            if (shoot) player.AddBuff(BuffDef.byName["ProjectAdvance:Precision0"], 120);
            else
                player.AddBuff(BuffDef.byName["ProjectAdvance:Precision4"], 120);
            base.Effects(player, index);
        }
    }
}
