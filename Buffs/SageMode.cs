using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TAPI;
namespace ProjectAdvance.Buffs
{
    class SageMode : ModBuff
    {
        public override void Effects(Player player, int index)
        {
            player.statMana = player.statManaMax2;
            if (player.HasBuff(BuffDef.byName["ProjectAdvance:ManaBreak"]) != -1)
                player.buffTime[player.HasBuff(BuffDef.byName["ProjectAdvance:ManaBreak"])]=1;
            base.Effects(player, index);
        }
    }
}
