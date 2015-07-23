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
    class RainDummy : ModProjectile
    {
        bool IsSetup=false;
        int ProjectileType;
        int damage;
        public void setup(int Ptype, int Pdamage) { ProjectileType = Ptype; damage = Pdamage; IsSetup = true; }
        public override void Initialize()
        {
            projectile.tileCollide = false;
            projectile.light = 1;
            base.Initialize();
        }
        public override void AI()
        {
            if(IsSetup)
            {
                if(projectile.timeLeft%3==0)
                {
                    if (projectile.frame < 4)
                        projectile.frame++;
                    else
                        projectile.frame = 0;
                    Projectile.NewProjectile(projectile.center(), new Vector2(Main.rand.Next(-6, 6), Main.rand.Next(10, 19)), ProjectileType, damage, 1, projectile.owner);
                }
            }
            base.AI();
        }
    }
}
