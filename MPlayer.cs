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
        bool CanUse = true;
        public Dictionary<int,Keys?> Hotkeys=new Dictionary<int,Keys?>();
        bool initialized = false;
        int SkillPoints = 0;
        int SkillLevel = 0;
        int DispersionTimer=0;
        int DispersionCount = 0;
        bool[] SkillsById = new bool[15];
        int SurgeTimer = 0;
        public int getSkillPoints() { return SkillPoints; }
        public void spendSkillPoint() { SkillPoints--; }
        public void grandSkillPoint() { SkillPoints++; }
        bool ArcaneBoltOnCooldown;
        int ArcaneBoltCooldown = 0;
        #endregion
        #region SaveLoadInitialization
        public void cantUse() { CanUse = false; }

        void initialize()
        {

            for (int i = 0; i < SkillsById.Length; i++) SkillsById[i] = false;
        }
        public int getArcaneMisslesCd() { return ArcaneBoltCooldown; }
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
            if (bb.HasLeft)
            {
                int key;
                Keys? value;
                int length = bb.ReadInt();
                for (int i = 0; i < length; i++)
                {
                    if (bb.HasLeft)
                    {
                        if (Hotkeys.ContainsKey(key = bb.ReadInt()))
                        {
                            Hotkeys[key] = (value=(Keys?)bb.ReadInt());
                        }
                        //else
                        //Hotkeys.Add(key, value=(Keys?)bb.ReadInt());
                        //Main.NewText("Loaded Key: " + key+" and value: "+value);
                    }

                }
            }
        }
        public override void Save(BinBuffer bb)
        {
            base.Save(bb);
            bb.Write(SkillPoints);
            for (int i = 0; i < SkillsById.Length; i++)
                bb.Write(SkillsById[i]);
            bb.Write(initialized);
            bb.Write(Hotkeys.Count);
            foreach(var kvp in Hotkeys)
            {
                bb.Write(kvp.Key);
                bb.Write((int)kvp.Value);
            }
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
                SkillLevel++;
            }
        }
        public void clearSkill(int position)
        {
            SkillsById[position] = false;
            grandSkillPoint();
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
        private void manaBlast(Player p)
        {
            if (p.statMana >100) {
                p.AddBuff(BuffDef.byName["ProjectAdvance:ManaBreak"], 300);
                Projectile.NewProjectile(player.position, new Vector2(0, 0), ProjDef.byName["ProjectAdvance:ManaWhirl"].type, 0, 13f);
            foreach (NPC n in Main.npc)
            {
                if (!n.friendly)
                {
                    if (Vector2.Distance(n.position, p.position) < 300)
                    {
                        if (p.position.X - n.position.X < 0)
                            n.velocity.X += 15;
                        else
                            n.velocity.X -= 15;
                        n.velocity.Y -= 3;
                        n.life = Math.Max(1, n.life - p.statManaMax);
                        CombatText.NewText(new Rectangle((int)n.position.X, (int)n.position.Y, 40, 10), Color.Cyan, Math.Max(1, n.life - p.statManaMax).ToString());
                        n.AddBuff(BuffDef.byName["ProjectAdvance:ManaBreak"], 300);
                        n.AddBuff(67, 180);
                    }
                }
            }
            Main.PlaySound("ProjectAdvance:BigDrum", player.position.X, player.position.Y);
        }

        }
   
        private void teleportPlayer()
        {
            if (player.statMana > 20)
            {
                player.statMana -= 20;
                Main.PlaySound(6);
                TeleportationPosition.Y -= 10;
                for (int i = 0; i < 25; i++)
                {
                    int d = Dust.NewDust(TeleportationPosition, new Vector2(10, 10), 52, new Vector2(Main.rand.Next(-25, 25), -5));
                    Main.dust[d].noGravity = true;
                }
                player.position = TeleportationPosition;
                TeleportationPosition = player.position;
                CanTeleport = false;
                player.velocity.Y += 1;
            }
        }
        public override void PostUpdate()
        {
            CanUse = true;
            DispersionTimer++;

            if(DispersionTimer>600)
            {
                DispersionTimer = 0;
                DispersionCount = 0;
            }
            if (ArcaneBoltOnCooldown) {
                ArcaneBoltCooldown++;
                if(ArcaneBoltCooldown>600)
                {
                    ArcaneBoltCooldown = 0;
                    ArcaneBoltOnCooldown = false;
                }
            }
            base.PreUpdate();
        }
       

        public override void MidUpdate()
        {
            
            if (!initialized)
            {
                initialize();
                initialized=true;
            }
            base.PostUpdate();
            if (CanUse && player==Main.localPlayer)
            {
                if (SkillsById[3])
                {
                    if (Hotkeys.ContainsKey(3))
                    {
                        if (Main.GetKeyState((int)Hotkeys[3]) == -127 || Main.GetKeyState((int)Hotkeys[3]) == -128) teleportationCheck();
                        if ((Main.mouseRight && Main.mouseRightRelease) && CanTeleport == true) teleportPlayer();
                    }
                }

                if (SkillsById[2])
                {
                    if (player.HasBuff(BuffDef.byName["ProjectAdvance:PowerSurge"]) == 0)
                    {
                        SurgeTimer = 0;
                    }
                    else
                        SurgeTimer++;
                    if (SurgeTimer > 360)
                    {
                        player.AddBuff(BuffDef.byName["ProjectAdvance:PowerSurge"], 6000);
                    }
                }
                if (SkillsById[4])
                {
                    if (Hotkeys.ContainsKey(4))
                    {
                        if (Hotkeys[4] != null)
                        {
                            if ((Main.GetKeyState((int)Hotkeys[4]) == -127 || Main.GetKeyState((int)Hotkeys[4]) == -128))
                            {
                                manaBlast(Main.localPlayer);
                            }

                        }
                    }
                }
                if(SkillsById[5])
                {
                    if (Hotkeys.ContainsKey(5))
                    {
                        if (Hotkeys[5] != null)
                        {
                            if ((Main.GetKeyState((int)Hotkeys[5]) == -127 || Main.GetKeyState((int)Hotkeys[5]) == -128) && player.statMana>=60)
                            {
                                if (player.HasBuff(BuffDef.byName["ProjectAdvance:DispersionBuff"]) == -1)
                                {
                                    player.statMana -= 60;
                                    player.AddBuff(BuffDef.byName["ProjectAdvance:DispersionBuff"], 60);
                                    if(DispersionCount++>3)
                                        player.AddBuff(BuffDef.byName["ProjectAdvance:ManaBreak"], 180);

                                }
                            }

                        }
                    }
                }
                if(SkillsById[6])
                {
                    if(Hotkeys.ContainsKey(6))
                    {
                        if(Hotkeys[6]!=null)
                        {
                            if ((Main.GetKeyState((int)Hotkeys[6]) == -127 || Main.GetKeyState((int)Hotkeys[6]) == -128) && player.statMana >= 60)
                            {
                                if (!ArcaneBoltOnCooldown)
                                {
                                    int counter = 0;
                                    foreach (NPC n in Main.npc)
                                    {

                                        if (!n.friendly && n.active && Vector2.Distance(player.position, n.position) < 1800)
                                        {
                                            counter++;
                                            Main.projectile[Projectile.NewProjectile(player.position, new Vector2(0.3f * Main.rand.Next(-3, 3), -0.3f * Main.rand.Next(3)), ProjDef.byName["ProjectAdvance:ArcaneBolt"].type, 50, 1, player.whoAmI)].GetSubClass<Projectiles.ArcaneBolt>().setTarget(n);
                                        }
                                        if(counter>0)
                                        ArcaneBoltOnCooldown = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public override void PreHurt(bool pvp, bool quiet, ref bool getHurt, ref bool playSound, ref bool genGore, ref int damage, ref int hitDirection, ref string deathText, ref bool crit, ref float critMultiplier)
        {
            base.PreHurt(pvp, quiet, ref getHurt, ref playSound, ref genGore, ref damage, ref hitDirection, ref deathText, ref crit, ref critMultiplier);
            if (player == Main.localPlayer && SkillsById[1] && player.statManaMax2 > (int)(0.2 * damage))
            {
                player.statManaMax2 -= (int)(0.2 * damage);
                damage = (int)(damage*0.8);
                Dust.NewDust(player.position, new Vector2(10, 10), 50);
            }
        }
        public override void ModifyDrawLayerList(List<PlayerLayer> list)
        {
            if (player.HasBuff(BuffDef.byName["ProjectAdvance:DispersionBuff"]) == 1)
            {

                foreach (PlayerLayer p in list)
                    p.visible = false;
            }
            base.ModifyDrawLayerList(list);
        }
    }
}
