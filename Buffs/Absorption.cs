﻿using System;
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
    class Absorption : ModBuff
    {
        public override void End(Player player, int index)
        {
            player.GetSubClass<MPlayer>().grandSkillPoint();
            base.End(player, index);
        }
    }
}
