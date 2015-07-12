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
    class EarthenShell : ModBuff
    {
        int healTimer=0;
        public override void Effects(Player player, int index)
        {
            player.statDefense += 10;
            if (++healTimer > 30)
            {
                healTimer = 0;
                if (player.statLife < player.statLifeMax2)
                {
                    CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, 10, 5), Color.LightGreen, "+1");
                    player.statLife += 1;

                }
            }
            base.Effects(player, index);
        }
        public override Microsoft.Xna.Framework.Color ModifyDrawColor(Player player, Microsoft.Xna.Framework.Color color)
        {
            return base.ModifyDrawColor(player, Color.Green);
        }
    }
}
