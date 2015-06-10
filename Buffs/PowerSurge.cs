using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using TAPI;
using Terraria;

namespace ProjectAdvance.Buffs
{
    public class PowerSurge : TAPI.ModBuff
    {     
        public override void DamageNPC(Projectile projectile, NPC npc, int hitDir, ref int damage, ref float knockback, ref bool crit, ref float critMult)
        {
            base.DamageNPC(projectile, npc, hitDir, ref damage, ref knockback, ref crit, ref critMult);
            if (projectile.magic)
            {
                damage *= 2;
                Player player = Main.player[projectile.owner];
                player.ClearBuff(BuffDef.byName["ProjectAdvance:PowerSurge"]);
            }
        }
      
		
    }
}