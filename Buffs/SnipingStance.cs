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
    class SnipingStance : ModBuff
    {
        //Vector2 lastCameraPos=new Vector2(-1,-1);
        //public override void Effects(Player player, int index)
        //{
        //    if (lastCameraPos.X == -1 && lastCameraPos.Y == -1) lastCameraPos = Main.screenPosition;
        //            float speed = 4;
        //            if (player.controlTorch) speed *= 2;
        //            if (player.controlLeft) lastCameraPos.X -= speed;
        //            if (player.controlRight) lastCameraPos.X += speed;
        //            if (player.controlUp) lastCameraPos.Y -= speed;
        //            if (player.controlDown) lastCameraPos.Y += speed;
        //            player.controlTorch = player.controlLeft = player.controlRight = player.controlUp = player.controlDown = false;
                   
        //    base.Effects(player, index);
        //}
        //public override void End(Player player, int index)
        //{
        //    //Main.screenPosition = new Vector2(-1, -1);
        //    base.End(player, index);
        //}
        //public override void ModifyDrawLayerList(Player player, List<PlayerLayer> list)
        //{
        //    if (lastCameraPos.X != -1 || lastCameraPos.Y != -1) Main.screenPosition = lastCameraPos;
        //    base.ModifyDrawLayerList(player, list);
        //}
    }
}
