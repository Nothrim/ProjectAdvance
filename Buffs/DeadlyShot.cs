using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;
namespace ProjectAdvance.Buffs
{
    class DeadlyShot : ModBuff
    {
        bool canUse = false;
        public override void DamageNPC(Projectile projectile, NPC npc, int hitDir, ref int damage, ref float knockback, ref bool crit, ref float critMult)
        {
            if (canUse)
            {
                crit = true;
                Player player = Main.player[projectile.owner];
                player.ClearBuff(BuffDef.byName["ProjectAdvance:DeadlyShot"]);
            }
            else
                canUse = true;
            base.DamageNPC(projectile, npc, hitDir, ref damage, ref knockback, ref crit, ref critMult);
        }
    }
}
