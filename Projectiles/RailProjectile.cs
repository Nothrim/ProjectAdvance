using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TAPI;
using Microsoft.Xna.Framework;
namespace ProjectAdvance.Projectiles
{
    class RailProjectile : ModProjectile
    {
        public override void AI()
        {
            projectile.rotation = (float)System.Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
            base.AI();
        }
        public override void DealtNPC(NPC npc, int hitDir, int dmgDealt, float knockback, bool crit)
        {
            Dust.NewDust(npc.getRect(), 6);
            base.DealtNPC(npc, hitDir, dmgDealt, knockback, crit);
        }
    }
}
