using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TAPI;
using Microsoft.Xna.Framework;
namespace ProjectAdvance
{
    [GlobalMod]
    class MProjectile :ModProjectile
    {
        public override void DamagePlayer(Player p, int hitDir, ref int damage, ref bool crit, ref float critMult)
        {
           if(p.GetSubClass<MPlayer>().isBlocking())
           {
               damage = 0;
               Projectile.NewProjectile(p.position, Vector2.Multiply(projectile.velocity, -1), projectile.type, projectile.damage, projectile.knockBack, p.whoAmI);
               projectile.Kill();
           }
            base.DamagePlayer(p, hitDir, ref damage, ref crit, ref critMult);
        }
        public override void OnSpawn()
        {
            if (Main.player[projectile.owner].GetSubClass<MPlayer>().getPath() == 3 )
            {
                if (Main.player[projectile.owner].GetSubClass<MPlayer>().checkSkillAtPosition(0))
                    projectile.penetrate++;
                if (Main.player[projectile.owner].GetSubClass<MPlayer>().checkSkillAtPosition(2))
                projectile.penetrate += 3;
                if (Main.player[projectile.owner].GetSubClass<MPlayer>().checkSkillAtPosition(6) && Main.player[projectile.owner].HasBuff(BuffDef.byName["ProjectAdvance:SnipingStance"])!=-1) 
                    projectile.velocity=Vector2.Multiply(projectile.velocity,3);
                if (Main.player[projectile.owner].GetSubClass<MPlayer>().checkSkillAtPosition(13) && Main.player[projectile.owner].GetSubClass<MPlayer>().SkillIsUsed(13) && Main.player[projectile.owner].GetSubClass<MPlayer>().getCooldownTime(13)<60)
                {
                    projectile.velocity.Y += Main.rand.Next(-4, 4);
                    projectile.velocity.X += Main.rand.Next(-2, 2);
                }
            }
            base.OnSpawn();
        }
        public override bool PreKill()
        {
            if (Main.player[projectile.owner].GetSubClass<MPlayer>().getPath() == 3)
            {
                if (Main.player[projectile.owner].HasBuff(BuffDef.byName["ProjectAdvance:ChainReaction"])!=-1 && Main.rand.Next(5) == 0)
                {
                    Projectile.NewProjectile(projectile.position, new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), projectile.type, projectile.damage / 2, projectile.knockBack, projectile.owner);
                }
                if (Main.player[projectile.owner].GetSubClass<MPlayer>().checkSkillAtPosition(9) && Main.rand.Next(2)==0)
                {
                    projectile.velocity.Y *= -0.8f;
                    projectile.velocity.X *= 1.3f;
                    if (projectile.penetrate > 0 && Math.Abs(projectile.velocity.X)>0.5f)
                    {
                        projectile.penetrate--;
                        return false;
                    }  
                }
            }
            return base.PreKill();
        }

    }
}
