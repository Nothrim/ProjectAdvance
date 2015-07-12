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

    }
}
