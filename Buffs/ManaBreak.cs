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
    class ManaBreak : ModBuff
    {
        public override void Effects(Player player, int index)
        {
            player.statMana =0;
            Dust.NewDust(new Vector2(player.position.X,player.position.Y+Main.rand.Next(10)), 10, 10, 67,0,-0);

            base.Effects(player, index);
        }
        public override void Effects(NPC npc, int index)
        {
            base.Effects(npc, index);
            Dust.NewDust(npc.position, 10, 10, 67, 0, 0);
        }
    }
}
