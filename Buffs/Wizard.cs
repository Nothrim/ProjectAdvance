using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TAPI;

namespace ProjectAdvance.Buffs
{
    class Wizard : ModBuff
    {
        public override void Effects(Player player, int index)
        {
            player.statManaMax2 += 10;
            base.Effects(player, index);
        }
        public override void End(Player player, int index)
        {
            player.AddBuff(BuffDef.byName["ProjectAdvance:Wizard"], 216000);
            base.End(player, index);
        }
    }
}
