using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
namespace ProjectAdvance
{
    class MPlayer : ModPlayer
    {
        bool initialized = false;
        int SkillPoints = 0;
        bool[] SkillsById = new bool[15];
        void initialize()
        {
            for (int i = 0; i < SkillsById.Length; i++) SkillsById[i] = false;
        }
        public void SetSkill(int position){SkillsById[position]=true;}
        Vector2 TeleportationPosition = new Vector2();
        bool CanTeleport = false;
        private void teleportationCheck()
        {
            if (Math.Abs(TeleportationPosition.X - Main.mouseWorld.X) > 25)
                TeleportationPosition = Main.mouseWorld;
            if (Main.tile[TeleportationPosition.ToTileCoordinates().X, TeleportationPosition.ToTileCoordinates().Y].collisionType == 1)
            {
                TeleportationPosition.Y -= 10;
                CanTeleport = false;
              
            }
            else if (Main.tile[TeleportationPosition.ToTileCoordinates().X, TeleportationPosition.ToTileCoordinates().Y].collisionType == 0 || Main.tile[TeleportationPosition.ToTileCoordinates().X, TeleportationPosition.ToTileCoordinates().Y].collisionType == -1)
            {
                Dust.NewDust(TeleportationPosition, player.Size / 10, 12, new Microsoft.Xna.Framework.Vector2(0, 0));
                CanTeleport = true;
            }
        }
        private void teleportPlayer()
        {
            TeleportationPosition.Y -= 10;
            for (int i = 0; i < 25; i++)
            {
                int d = Dust.NewDust(TeleportationPosition, new Vector2(10, 10), 52, new Vector2(Main.rand.Next(-25, 25), -5));
                Main.dust[d].noGravity = true;
            }
            player.position = TeleportationPosition;
        }
   
        public override void PostUpdate()
        {
            if (!initialized)
            {
                initialize();
                    initialized=true;
                    Main.NewText("Skill tree initialized!");
            }
            
            base.PostUpdate();
            if (SkillsById[3])
            {
                if (Main.GetKeyState((int)Keys.X) == 1) teleportationCheck();
                if (Main.GetKeyState((int)Keys.X) == -128 && CanTeleport == true) teleportPlayer();
            }
        }
      
    }
}
