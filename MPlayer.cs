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
        #region variables
        bool initialized = false;
        int SkillPoints = 20;
        bool[] SkillsById = new bool[15];
        int SurgeTimer = 0;
        public int getSkillPoints() { return SkillPoints; }
        public void spendSkillPoint() { SkillPoints--; }
        public void grandSkillPoint() { SkillPoints++; }
        #endregion
        #region SaveLoadInitialization
        void initialize()
        {
            for (int i = 0; i < SkillsById.Length; i++) SkillsById[i] = false;
        }
        public override void Load(BinBuffer bb)
        {
            base.Load(bb);
            if(bb.HasLeft)
            SkillPoints = bb.ReadInt();
            for (int i = 0; i < SkillsById.Length; i++)
            {
                if (bb.HasLeft)
                SkillsById[i] = bb.ReadBool();
            }
            if (bb.HasLeft)
                initialized = bb.ReadBool();
        }
        public override void Save(BinBuffer bb)
        {
            base.Save(bb);
            bb.Write(SkillPoints);
            for (int i = 0; i < SkillsById.Length; i++)
                bb.Write(SkillsById[i]);
            bb.Write(initialized);
        }
        #endregion
        public bool checkPreviousSkill(int i) 
        { 
            if(i > 0 && i < SkillsById.Length)
            {
                return SkillsById[i - 1];
            }
            else
            return true;
        }
        public void setSkill(int position)
        {
            if (!SkillsById[position])
            {
                SkillsById[position] = true;
                spendSkillPoint();
            }
        }
        public bool checkSkillAtPosition(int i)
        {
            if (i >= 0 && i < SkillsById.Length) 
                return SkillsById[i];
            else
                return false;
        }
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
            player.velocity.Y += 1;
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
            if (SkillsById[3] && player==Main.localPlayer)
            {
                if (Main.GetKeyState((int)Keys.X) == 1) teleportationCheck();
                if (Main.GetKeyState((int)Keys.X) == -128 && CanTeleport == true) teleportPlayer();
            }
            if(SkillsById[2] && player==Main.localPlayer)
            {
                if (player.HasBuff(BuffDef.byName["ProjectAdvance:PowerSurge"]) == 2)
                    SurgeTimer = 0;
                else
                    SurgeTimer++;
          
                if (SurgeTimer > 360)
                {
                    Main.NewText("Buff Added!");
                    player.AddBuff(BuffDef.byName["ProjectAdvance:PowerSurge"], 6000);
                }
            }
        }  
    }
}
