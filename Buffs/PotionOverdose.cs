using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;


namespace ProjectAdvance.Buffs
{
    class PotionOverdose : ModBuff
    {
        public override void Effects(Player player, int index)
        {
            if(player.HasBuff(21)!=-1)
            {
                player.ClearBuff(21);
            }
            player.potionDelay = 0;
            base.Effects(player, index);
        }
    }
}
