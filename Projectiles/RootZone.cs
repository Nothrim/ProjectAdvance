using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;
using Microsoft.Xna.Framework;

namespace ProjectAdvance.Projectiles
{
    class RootZone : ModProjectile
    {
        Vector2 BindPoint;
        int id;
        Buffs.Root b = null;
        public override void Initialize()
        {
            BindPoint = projectile.center();
            projectile.tileCollide = false;
            base.Initialize();
        }

        public override void DealtNPC(NPC npc, int hitDir, int dmgDealt, float knockback, bool crit)
        {

            if(npc.HasBuff(BuffDef.byName["ProjectAdvance:Root"])==-1)
            {
                id=npc.AddBuff(BuffDef.byName["ProjectAdvance:Root"], 320);
                b=(Buffs.Root)npc.buffCode[id];
                b.setBoundingPoint(BindPoint);
            }
            base.DealtNPC(npc, hitDir, dmgDealt, knockback, crit);
        }
    }
}
