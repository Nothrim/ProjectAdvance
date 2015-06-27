using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;
using Microsoft.Xna.Framework;
namespace ProjectAdvance.Buffs
{
    class LightningForm : ModBuff
    {
        public override void Effects(Player player, int index)
        {
            player.noFallDmg = true;
            player.invis = true;
            player.immune = true;
            player.mouseInterface = true;
            if(player.statMana>10)
            player.statMana -= 1;
            Main.dust[Dust.NewDust(player.Center, new Vector2(1, 1), 68,new Vector2(0,0),0,Color.White,4f)].noGravity = true;
            player.velocity = Vector2.Multiply(Vector2.Normalize(new Vector2(Main.mouseWorld.X - player.position.X, Main.mouseWorld.Y - player.position.Y)),16);
            base.Effects(player, index);
        }
    }
}
