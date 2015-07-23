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
    class GorgonBeam:ModProjectile
    {
        public override void Initialize()
        {
            projectile.tileCollide = false;
            base.Initialize();
            projectile.light = 1;
        }
        public override void AI()
        {
            projectile.rotation = (float)System.Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
            base.AI();
        }
        public override void Kill()
        {
            Dust.NewDust(projectile.getRect(), 20, Vector2.Zero, 0, Color.White, 1f);
            base.Kill();
        }
    }
}
