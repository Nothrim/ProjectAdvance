using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;

namespace ProjectAdvance.Projectiles
{
    class FireballTrail : ModProjectile
    {
        Projectile parrent=null;
        public void setParrent(Projectile parrent) { this.parrent = parrent; }
        public override void AI()
        {
            if (parrent != null)
            {
                projectile.rotation = (float)System.Math.Atan2((double)parrent.velocity.Y, (double)parrent.velocity.X);
                if (parrent.timeLeft % 5 == 0)
                {
                    projectile.frame++;
                    if (projectile.frame % 3 == 0) projectile.frame = 1;
                }

            }
            base.AI();
        }
    }
}
