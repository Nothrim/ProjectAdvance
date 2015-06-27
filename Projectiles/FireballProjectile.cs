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
    class FireballProjectile : ModProjectile
    {
        public override void Initialize()
        {
            projectile.scale *= 2;
            base.Initialize();
        }
        public override void AI()
        {
            projectile.rotation = (float)System.Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) ;
            if (projectile.timeLeft % 3 == 0)
            {
                projectile.frame++;
                if (projectile.frame % 5==0) projectile.frame = 1;
                Main.dust[Dust.NewDust(projectile.position, new Vector2(3, 3), 55)].noGravity = true;
                Main.projectile[Projectile.NewProjectile(projectile.center(), new Microsoft.Xna.Framework.Vector2(0, 0), ProjDef.byName["ProjectAdvance:FireballTrail"].type, 5, 0, projectile.owner)].GetSubClass<FireballTrail>().setParrent(projectile);
            }
            base.AI();
        }
        public override bool PreKill()
        {
            Main.projectile[Projectile.NewProjectile(projectile.position, new Vector2(0, 0), 30, projectile.damage, 3, projectile.owner)].timeLeft = 1;
            return base.PreKill();
        }
    }
}
