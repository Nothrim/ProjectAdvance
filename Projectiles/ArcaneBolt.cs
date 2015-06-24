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
    class ArcaneBolt : ModProjectile
    {
        NPC target=null;
        public override void Initialize()
        {
            projectile.tileCollide = false;
            projectile.scale = 0.9f;

            base.Initialize();
        }
        public void setTarget(NPC n) { target = n; }
        public override void AI()
        {
            if(target!=null)
            {
                if (target.position.X > projectile.position.X)
                    projectile.velocity.X+=0.1f;
                    
                else
                    projectile.velocity.X-=0.1f;
                if (Math.Abs(target.position.Y - projectile.position.Y) < 10) ;
                else if (target.position.Y > projectile.position.Y)

                    projectile.velocity.Y += 0.1f;
                else
                    projectile.velocity.Y -= 0.1f;
                if(target.position.X==projectile.position.X)
                {
                    projectile.velocity = new Vector2(0,0);
                }
            }
           Main.dust[Dust.NewDust(projectile.position, new Vector2(10,10),15,new Vector2(0,0),0,Color.Purple,3f)].noGravity=true;
            base.AI();
        }
       

    }
}
