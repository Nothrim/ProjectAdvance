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
    class BindingRoot : ModProjectile
    {
        NPC target;
        public void setTarget(NPC npc) { target = npc; }
        public override void Initialize()
        {
            base.Initialize();
            projectile.tileCollide = false;

        }
        public override void AI()
        {

            if (target != null)
            {
                projectile.rotation = (float)System.Math.Atan2((double)target.position.Y, (double)target.position.X);
            }
            base.AI();
        }    
    }
}
