using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;
namespace ProjectAdvance
{
    class MPlayer : ModPlayer
    {
        public override void OnHit(NPC victim, Microsoft.Xna.Framework.Vector2 location)
        {
            Main.NewText(victim.ToString());
            base.OnHit(victim, location);
        }
    }
}
