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
    class HeallingDummy : ModProjectile
    {
        Player PTarget=null;
        Player owner;
        NPC NTarget=null;
        float distance = 0;
        int jumpTimes = 0;
        bool isSet = false;
        public void setTarget(Player p, int jumps) { PTarget = p; jumpTimes = jumps; }
        public void setTarget(NPC n, int jumps) { NTarget = n; jumpTimes = jumps; }
        public override void Initialize()
        {
            projectile.tileCollide = false;
            owner = Main.player[projectile.owner];
            base.Initialize();
        }
        public override void AI()
        {
            if(!isSet)
            {
                if(NTarget!=null)
                {
                    isSet = true;
                    projectile.hostile = false;
                }
                if(PTarget!=null)
                {
                    isSet = true;
                    projectile.hostile = true;
                }
            }
            if (PTarget != null)
            {
                if (!PTarget.active) projectile.timeLeft = 1;
                projectile.velocity = Vector2.Multiply(Vector2.Normalize(Vector2.Subtract(PTarget.position,projectile.position)),6);
                distance=Vector2.Distance(PTarget.position, projectile.position);
                if (distance > 150) 
                projectile.velocity.Y -=distance  / 300;
                Main.dust[Dust.NewDust(projectile.position, new Vector2(5, 5), 15, new Vector2(0, 0), 0, Color.DarkOliveGreen, 2f)].velocity=new Vector2(0,0.5f);
            }
            else if(NTarget!=null)
            {
                if (!NTarget.active) projectile.timeLeft = 1;
                projectile.velocity = Vector2.Multiply(Vector2.Normalize(Vector2.Subtract(NTarget.position, projectile.position)), 6);
                distance=Vector2.Distance(NTarget.position, projectile.position);
                if (distance > 150) 
                projectile.velocity.Y -=distance  / 220;
                Main.dust[Dust.NewDust(projectile.position, new Vector2(5, 5), 15, new Vector2(0, 0), 0, Color.DarkOliveGreen, 2f)].velocity = new Vector2(0, 0.5f);
            }
            base.AI();
        }
        public override void DealtNPC(NPC npc, int hitDir, int dmgDealt, float knockback, bool crit)
        {
            if(NTarget!=null)
            {
                if(npc.whoAmI==NTarget.whoAmI)
                {
                    projectile.timeLeft = 1;
                    jumpTimes--;
                }
                else
                {
                    projectile.penetrate++;
                }
            }
            base.DealtNPC(npc, hitDir, dmgDealt, knockback, crit);
        }
        public override void DamagePlayer(Player p, int hitDir, ref int damage, ref bool crit, ref float critMult)
        {

            base.DamagePlayer(p, hitDir, ref damage, ref crit, ref critMult);
        }
        public override bool? CanHitPlayer(Player p)
        {

            if (PTarget != null)
            {
                int lifeHeal;
                if (p.whoAmI == PTarget.whoAmI)
                {
                    jumpTimes--;
                    lifeHeal = Main.rand.Next(p.statLifeMax2 - p.statLife);
                    p.statLife += lifeHeal;
                    if (lifeHeal > 0)
                        CombatText.NewText(p.getRect(), Color.LightGreen, "+" + lifeHeal + " HP");
                    projectile.timeLeft = 1;
                    return false;
                }
                else
                {
                    projectile.penetrate++;
                }
            }
            return base.CanHitPlayer(p);
        }
        public override bool PreKill()
        {
            if (jumpTimes > 0)
            {
                bool gotTarget = false;
                float prevDistance = 500;
                float distance;
                if (PTarget != null)
                {
                    foreach (Player p in Main.player)
                    {
                        if (p.active && p.whoAmI != PTarget.whoAmI && p.statLife < p.statLifeMax2 && (distance = p.Distance(projectile.position)) < 500)
                        {
                            if (distance < prevDistance)
                            {
                                gotTarget = true;
                                prevDistance = distance;
                                PTarget = p;
                            }
                        }
                    }
                }
                else
                {
                    foreach (Player p in Main.player)
                    {
                        if (p.active && p.statLife < p.statLifeMax2 && (distance = p.Distance(projectile.position)) < 500)
                        {
                            if (distance < prevDistance)
                            {
                                gotTarget = true;
                                prevDistance = distance;
                                PTarget = p;
                            }
                        }
                    
                    }
                }
                if (gotTarget)
                {
                    Main.projectile[Projectile.NewProjectile(projectile.position, Vector2.Zero, ProjDef.byName["ProjectAdvance:HeallingDummy"].type, 70, 2, owner.whoAmI)].GetSubClass<Projectiles.HeallingDummy>().setTarget(PTarget, jumpTimes--);
                }
                else
                {
                    if (NTarget != null)
                    {
                        foreach (NPC n in Main.npc)
                        {
                            if (n.active && n.whoAmI != NTarget.whoAmI && (distance = n.Distance(projectile.position)) < 500)
                            {
                                if (distance < prevDistance)
                                {
                                    gotTarget = true;
                                    prevDistance = distance;
                                    NTarget = n;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (NPC n in Main.npc)
                        {
                            if (n.active && (distance = n.Distance(projectile.position)) < 500)
                            {
                                if (distance < prevDistance)
                                {
                                    gotTarget = true;
                                    prevDistance = distance;
                                    NTarget = n;
                     
                                }
                            }
                        }
                    }
                }
                if (gotTarget && NTarget != null)
                {
                    Main.projectile[Projectile.NewProjectile(projectile.position, Vector2.Zero, ProjDef.byName["ProjectAdvance:HeallingDummy"].type, 70, 2, owner.whoAmI)].GetSubClass<Projectiles.HeallingDummy>().setTarget(NTarget, jumpTimes--);
                }
            }
            return base.PreKill();
        }
       

    }
}
