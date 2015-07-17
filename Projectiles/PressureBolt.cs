using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;
namespace ProjectAdvance.Projectiles
{
    class PressureBolt : ModProjectile
    {
        public override void Initialize()
        {
            projectile.tileCollide = false;
            base.Initialize();
        }
    }
}
