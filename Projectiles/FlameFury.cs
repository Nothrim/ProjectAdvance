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
    class FlameFury : ModProjectile
    {
        Vector2 Target;
        Vector2 StartingPoint;
        int CosShift=0;
        int maxtime = 0;
        public override void Initialize()
        {
            Target = Vector2.Multiply(Vector2.Normalize(new Vector2(Main.mouseWorld.X - projectile.position.X, Main.mouseWorld.Y - projectile.position.Y)), 10);
            StartingPoint = projectile.position;
            base.Initialize();
            maxtime = projectile.timeLeft;
            CosShift = Main.rand.Next(90);
        }
        public override void AI()
        {
            if (projectile.timeLeft > maxtime - 30)
            {
                projectile.hide = true;

                projectile.tileCollide = false;

            }
            else
            {

            }
            {
                projectile.hide = false;
                if (projectile.timeLeft % 3 == 0)
                {
                    if (projectile.Distance(StartingPoint) > 500)
                        projectile.tileCollide = true;
                    //if (++projectile.frame % 5 == 0)
                    //{
                       
                    //    projectile.frame = 1;
                    //}
                }
                Main.dust[Dust.NewDust(projectile.position, 5, 5, 55,0,0,0,Color.White,2.5f)].noGravity = true;
                //projectile.rotation = (float)System.Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
                
                projectile.velocity = Target;
                projectile.velocity.Y += (float)Math.Cos(CosShift+projectile.position.X+projectile.position.Y)*0.5f;
                base.AI();
            }
        }
        public override bool PreKill()
        {
            Main.projectile[Projectile.NewProjectile(projectile.position, new Vector2(0, 0), 30, projectile.damage, 3, projectile.owner)].timeLeft = 1;
            return base.PreKill();
        }
    }
}
