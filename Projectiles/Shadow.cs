using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;

namespace ProjectAdvance.Projectiles
{
    class Shadow : ModProjectile
    {
        public override void Initialize()
        {
            projectile.tileCollide = false;
            projectile.velocity = Microsoft.Xna.Framework.Vector2.Zero;
            base.Initialize();
        }
        public override void AI()
        {
            if(projectile.timeLeft%15==0)
            {
                projectile.frame++;
                if (projectile.frame > 2)
                    projectile.frame = 0;
            }
            base.AI();
        }
    }
}
