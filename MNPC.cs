using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TAPI;

namespace ProjectAdvance
{
    [GlobalMod]
    class MNPC : ModNPC
    {
        static int WorldSkillCounter = 0;
        public override void PostNPCLoot()
        {
            int random = Main.rand.Next(20);
            if ( npc.lifeMax > 10 * WorldSkillCounter && random == 3 )
            {
                WorldSkillCounter++;
                Item.NewItem(npc.position, npc.Size, ItemDef.byName["ProjectAdvance:PrimeElement"].type);
            }
            base.NPCLoot();
 	        base.PostNPCLoot();
        }
            

    }
}
