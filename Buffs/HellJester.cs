using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using TAPI;
using Terraria;

namespace ProjectAdvance.Buffs
{
    public class HellJester : TAPI.ModBuff
    {     
        public override void DamageNPC(Projectile projectile, NPC npc, int hitDir, ref int damage, ref float knockback, ref bool crit, ref float critMult)
        {
            base.DamageNPC(projectile, npc, hitDir, ref damage, ref knockback, ref crit, ref critMult);
            if (projectile.magic)
            {
                crit = true;
            }
        }
      
		
    }
}