using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TAPI;

namespace ProjectAdvance.Projectiles
{
    class LightningForm : ModProjectile
    {
        Player Owner = null;
        public void setOwner(Player p) { Owner = p; }
        public override void AI()
        {
            if(Owner!=null)
            {
                projectile.position = Owner.Center;
                projectile.rotation = (float)System.Math.Atan2((double)Owner.velocity.Y, (double)Owner.velocity.X);
                if(projectile.timeLeft%3==0)
                {
                    if(++projectile.frame%4==0)
                    {
                        projectile.frame = 1;
                    }
                }
            }
            base.AI();
        }
    }
}
