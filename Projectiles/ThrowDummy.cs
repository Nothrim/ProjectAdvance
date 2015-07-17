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
    class ThrowDummy : ModProjectile
    {
        NPC Target = null;
        int id;

        public override void Initialize()
        {
            projectile.tileCollide = false;
            base.Initialize();
        }
        public void setTarget(NPC t) { Target = t; }
        public override void AI()
        {
            if(Target!=null)
            {
                projectile.position = Target.Centre;
                projectile.velocity.Y = -0.5f;

                id = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + Main.rand.Next(40)), new Vector2(5, 5), 9, new Vector2(0, 0), 0, Color.White, 2f);
                Main.dust[id].velocity = Vector2.Zero;
                Main.dust[id].noGravity = true;
                if(Target.velocity.Length()<3)
                {
                    projectile.tileCollide = true;
                    projectile.hurtsTiles = true;
                    projectile.timeLeft = 1;
                }
            }
            base.AI();
        }
        public override bool PreKill()
        {
            if(Target != null)
            {
                int explosions = Math.Max(1, (int)Target.Size.Y / 20);
                for(int i=0;i<explosions;i++)
                {
                    Main.projectile[Projectile.NewProjectile(new Vector2(Target.position.X+Main.rand.Next(-10*i,10*i),Target.position.Y+Main.rand.Next(-10*i,10*i)), Vector2.Zero, 30, (int)(Target.lifeMax * 0.4), 3, projectile.owner)].timeLeft = 1;
                }
            }
            return base.PreKill();
        }
    }
}
