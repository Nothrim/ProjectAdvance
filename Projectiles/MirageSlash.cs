using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;
namespace ProjectAdvance.Projectiles
{
    class MirageSlash : ModProjectile
    {
        public override void Initialize()
        {
            projectile.tileCollide = false;
            base.Initialize();
        }
        public override void AI()
        {
            projectile.rotation = (float)System.Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
            base.AI();
        }
    }
}
