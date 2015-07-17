using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TAPI;
using Microsoft.Xna.Framework;

namespace ProjectAdvance.Buffs
{
    class EnderLegacy : ModBuff
    {
        public override Color ModifyDrawColor(Player player, Color color)
        {
            return base.ModifyDrawColor(player, Color.IndianRed);
        }
    }
}
