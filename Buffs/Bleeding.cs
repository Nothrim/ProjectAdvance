using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;
namespace ProjectAdvance.Buffs
{
    class Bleeding : ModBuff
    {
        int bleeding_timer = 0;
        public override void Start(NPC npc, int index)
        {
            int chance = Main.rand.Next(20, 50);
            npc.life -= Math.Max(1,(int)((float)npc.lifeMax /(float)chance));
            CombatText.NewText(npc.getRect(), Microsoft.Xna.Framework.Color.OrangeRed, "" + Math.Max(1, (int)((float)npc.lifeMax / (float)chance)));
            if (npc.life <= 0)
            {
                npc.life = 1;
                if (Main.netMode != 1)
                {
                    npc.StrikeNPC(9999, 0f, 0, false, false);
                    if (Main.netMode == 2) { NetMessage.SendData(28, -1, -1, "", npc.whoAmI, 1f, 0f, 0f, 9999); }

                }
            }
            base.Start(npc, index);
        }
        public override void Effects(NPC npc, int index)
        {
            if(bleeding_timer++>60)
            {
                bleeding_timer = 0;
                CombatText.NewText(npc.getRect(), Microsoft.Xna.Framework.Color.Orange, (int)Math.Max(1, (npc.lifeMax / 100f)) + "");
                npc.life -= (int)Math.Max(1,npc.lifeMax / 100f);
                if (npc.life <= 0)
                {
                    npc.life = 1;
                    if (Main.netMode != 1)
                    {
                        npc.StrikeNPC(9999, 0f, 0, false, false);
                        if (Main.netMode == 2) { NetMessage.SendData(28, -1, -1, "", npc.whoAmI, 1f, 0f, 0f, 9999); }

                    }
                }
            }
            base.Effects(npc, index);
        }
    }
}
