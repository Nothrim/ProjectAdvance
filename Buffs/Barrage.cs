using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;
namespace ProjectAdvance.Buffs
{
    class Barrage : ModBuff
    {
        public override void Effects(Player player, int index)
        {
            player.rangedDamage = 0.5f;
            base.Effects(player, index);
        }
    }
}
