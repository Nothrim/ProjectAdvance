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
    public sealed class SuperModsBase : TAPI.ModBase
    {
        Vector2 lastCameraPos;
        public static SuperModsBase SMB
        {
            get;
            private set;
        }
        public SuperModsBase()
            : base()
        {
            SMB = this;
        }
        public override void PreGameDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            if (!Main.gameMenu)
            {
                if (Main.localPlayer.HasBuff(BuffDef.byName["ProjectAdvance:SnipingStance"]) != -1)
                {
                    lastCameraPos = Main.localPlayer.GetSubClass<MPlayer>().lastCameraPos;
                    if (lastCameraPos.X != -1 || lastCameraPos.Y != -1) Main.screenPosition = lastCameraPos;
                }
            }
            base.PreGameDraw(sb);
        }
    }
}
