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
    class GorgonSentry : ModProjectile
    {
        Player p;
        int AttackSpeed=30;
        int AttackTimer=0;
        int BoredomTimer=0;
        int ShootingTimer=0;
        NPC CurrentTarget=null;
        public override void Initialize()
        {

            projectile.velocity = Vector2.Zero;
            base.Initialize();
            p = Main.player[projectile.owner];
        }
        NPC findTarget(float distance)
        {
            NPC Target=null;
            foreach (NPC n in Main.npc)
            {
                if(n.active && !n.friendly && n.Distance(p.position)<distance)
                {
                    Target = n;
                    distance = Target.Distance(p.position);
                }
            }
            return Target;
        }
        public override void AI()
        {
            if(CurrentTarget!=null && CurrentTarget.active && CurrentTarget.life>0)
            {
                if (AttackTimer < AttackSpeed)
                {
                    AttackTimer++;
                }
                else
                {
                    if (ShootingTimer < 15)
                    {
                        ShootingTimer++;
                        if (ShootingTimer % 3 == 0)
                        {
                            if (projectile.frame < 2)
                                projectile.frame++;
                            else if (projectile.frame == 2)
                                projectile.frame = 4;
                            else
                                projectile.frame = 0;
                        }
                       
                    }
                    else
                    {
                        if (projectile.frame == 2)
                            projectile.frame = 3;
                        else
                            projectile.frame = 2;
                        AttackTimer = 0;
                        ShootingTimer = 0;
                        Projectile.NewProjectile(projectile.center(), Vector2.Normalize(CurrentTarget.position - projectile.position) * 22, ProjDef.byName["ProjectAdvance:GorgonBeam"].type, projectile.damage, 1, projectile.owner);
                    }
                }
            }
            else
            {
                if(BoredomTimer<Main.rand.Next(30,60))
                {
                    if (BoredomTimer % 8 == 0)
                    {
                        if (projectile.frame < 2)
                            projectile.frame++;
                        else if (projectile.frame == 2)
                            projectile.frame = 4;
                        else
                            projectile.frame = 0;
                    }
                    BoredomTimer++;
                }
                else
                {
                    BoredomTimer=0;
                    CurrentTarget = findTarget(900);
                }
            }
            base.AI();
        }
        public override void DamageNPC(NPC npc, int hitDir, ref int damage, ref float knockback, ref bool crit, ref float critMult)
        {
            knockback = 10;
            base.DamageNPC(npc, hitDir, ref damage, ref knockback, ref crit, ref critMult);
        }
    }
}
