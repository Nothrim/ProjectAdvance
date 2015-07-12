using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;

namespace ProjectAdvance.Buffs
{
    class TrollRegeneration : ModBuff
    {
        int RegenTimer = 0;
        int LifeBonus = 0;
        public override void Effects(Player player, int index)
        {
            if(RegenTimer++>10)
            {
                RegenTimer = 0;
                LifeBonus = Main.rand.Next(5, 10);
                if(player.statLife<player.statLifeMax-LifeBonus)
                {
                    player.statLife += LifeBonus;
                    CombatText.NewText(new Microsoft.Xna.Framework.Rectangle((int)player.position.X, (int)player.position.Y, 10, 5), Microsoft.Xna.Framework.Color.Green, "+" + LifeBonus);
                }
            }
            base.Effects(player, index);
        }
    }
}
