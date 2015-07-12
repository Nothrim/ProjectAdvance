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
    class PoisonSerpent : ModProjectile
    {
        Player owner;
        public override void Initialize()
        {
            projectile.tileCollide = false;
            owner = Main.player[projectile.owner];
            base.Initialize();
        }
        public override void AI()
        {
            if (owner != null)
            {
                projectile.velocity = Vector2.Multiply(Vector2.Normalize(Vector2.Subtract(owner.position,projectile.position)),Vector2.Distance(owner.position,projectile.position)/30);
                projectile.velocity = new Vector2(-projectile.velocity.Y, projectile.velocity.X);
                Main.dust[Dust.NewDust(projectile.position, new Vector2(5, 5), 15, new Vector2(0, 0), 0, Color.LawnGreen, 1.5f)].velocity=Vector2.Zero;
            }
            base.AI();
        }
       

    }
}
