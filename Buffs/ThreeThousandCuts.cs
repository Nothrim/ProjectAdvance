using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;

namespace ProjectAdvance.Buffs
{
    class ThreeThousandCuts : ModBuff
    {
        public override void Start(Player player, int index)
        {
            player.GetSubClass<MPlayer>().changeCooldown(20, 10);
            base.Start(player, index);
        }
        public override void End(Player player, int index)
        {
            player.GetSubClass<MPlayer>().changeCooldown(20, 180);
            base.End(player, index);
        }
    }
}
