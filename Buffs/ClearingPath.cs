using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TAPI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace ProjectAdvance.Buffs
{
    class ClearingPath : ModBuff
    {
        public override void End(Player player, int index)
        {
            player.GetSubClass<MPlayer>().ClearSkills();
            player.GetSubClass<MPlayer>().setPath(0);
            base.End(player, index);
        }
    }
}
