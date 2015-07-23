using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TAPI;
using Microsoft.Xna.Framework;

namespace ProjectAdvance
{
    [GlobalMod]
    class MNPC : ModNPC
    {
        static int WorldSkillCounter = 0;
        public override void PostNPCLoot()
        {
            int random = Main.rand.Next(40);
            if ( npc.lifeMax > 10 * WorldSkillCounter && random == 3 )
            {
                WorldSkillCounter++;
                Item.NewItem(npc.position, npc.Size, ItemDef.byName["ProjectAdvance:PrimeElement"].type);
            }
            base.NPCLoot();
 	        base.PostNPCLoot();
        }
        public override void DealtPlayer(Player player, int hitDir, int dmgDealt, bool crit)
        {
            if(player.GetSubClass<MPlayer>().isBlocking() && npc.aiStyle==9)
            {
                npc.friendly = true;
                npc.velocity=Vector2.Multiply(npc.velocity, -1);
            }
            base.DealtPlayer(player, hitDir, dmgDealt, crit);
        }
            

    }
}
