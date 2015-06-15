using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TAPI;

namespace ProjectAdvance.Projectiles
{
    class ManaWhirl:ModProjectile
    {
        double angle = 0;
        public override void AI()
        {
            if(projectile.timeLeft%2==0)
            projectile.rotation = (float)Math.Cos(angle++);
            base.AI();
        }
    }
}
