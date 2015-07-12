using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TAPI;
namespace ProjectAdvance.Buffs
{
    class LayerII : ModBuff
    {
        int stacks;
        public override void Start(Player player, int index)
        {
            stacks = 3;
            base.Start(player, index);
        }
        public override void DealtPlayer(Player player, NPC npc, int hitDir, int dmgDealt, bool crit)
        {
            if (stacks-- <= 0)
            {
                player.ClearBuff(BuffDef.byName["ProjectAdvance:LayerII"]);
            }
            base.DealtPlayer(npc, player, hitDir, dmgDealt, crit);
        }
        public override void Effects(Player player, int index)
        {
            player.statDefense += 10;
            base.Effects(player, index);
        }
        public override void End(Player player, int index)
        {
            if (stacks < 3)
            {
                player.AddBuff(BuffDef.byName["ProjectAdvance:LayerI"], 300);
            }
            else
                player.AddBuff(BuffDef.byName["ProjectAdvance:LayerIII"], 300);
            base.End(player, index);
        }
    }
}
