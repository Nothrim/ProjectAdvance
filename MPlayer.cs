﻿using System;
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
        public CooldownManager CManager;
        bool loaded = false;
        bool TreeCleared = false;
        int DispersionTimer = 0;
        int DispersionCount = 0;
        bool[] SkillsById;
        int SkillPoints = 0;
        int SkillLevel = 0;
        int Path=0;
        public Dictionary<int, Keys?> Hotkeys;
        int SurgeTimer = 0;
        static MInterface minterface = (MInterface)SuperModsBase.SMB.modInterface;
        public int getPath() { return Path; }
        public void setPath(int path) { Path = path; }
        public int getSkillPoints() { return SkillPoints; }
        public void spendSkillPoint()
        {
            if (SkillPoints > SkillLevel) SkillPoints = SkillLevel;
            if (SkillPoints < 0) SkillPoints = 0;
            SkillPoints--;
        }
        public void grandSkillPoint()
        {
            if (SkillLevel < 9)
            {
                SkillLevel++;
                SkillPoints++;
            }
        }
        #endregion
        #region SaveLoadInitialization
        public void cantUse() { CanUse = false; }
        public override void Initialize()
        {
            SkillsById = new bool[22];
            SkillPoints = 0;
            SkillLevel = 0;
            loaded = false;
            TreeCleared = false;
            Path = 0;
            Hotkeys = new Dictionary<int, Keys?>();
            base.Initialize();
        }

        public override void Load(BinBuffer bb)
        {
            if (bb.Size > 0)
            {
                Path = bb.ReadInt();
                SkillPoints = bb.ReadInt();
                SkillLevel = bb.ReadInt();
                bool read;
                for (int i = 0; i < SkillsById.Length; i++)
                {
                    read = bb.ReadBool();
                    if (read)
                    {
                        SkillsById[i] = read;
                    }
                }
                int key;
                int length = bb.ReadInt();
                for (int i = 0; i < length; i++)
                {
                    key = bb.ReadInt();
                    if (Hotkeys.ContainsKey(key))
                    {
                        Hotkeys[key] = (Keys)bb.ReadInt();
                    }
                    else
                    {
                        Hotkeys.Add(key, (Keys)bb.ReadInt());
                    }
                }
            }
            if (!loaded)
            {

                loaded = true;
                CManager = new CooldownManager(new Vector2(Main.screenWidth, Main.screenHeight), Main.spriteBatch);
                if (Path == 2)
                {//0-none,1-warrior,2-mage,3-range
                    if (SkillsById[2]) { CManager.addCooldown(2, "PowerSurge", "ProjectAdvance:PowerSurge", 360); }
                    if (SkillsById[7]) { CManager.addCooldown(7, "SageMode", "ProjectAdvance:SageMode", 18000); }
                    if (SkillsById[6]) { CManager.addCooldown(6, "ArcaneBarrage", "ProjectAdvance:ArcaneBarrage", 600); }
                    if (SkillsById[0]) { player.AddBuff(BuffDef.byName["ProjectAdvance:Wizard"], 216000); }
                    if (SkillsById[10]) { CManager.addCooldown(10, "Fireball", "ProjectAdvance:Fireball", 180); }
                    if (SkillsById[13]) { CManager.addCooldown(13, "FlameFury", "ProjectAdvance:FlameFury", 900); }
                    if (SkillsById[14]) { CManager.addCooldown(14, "HellJester", "ProjectAdvance:HellJester", 18000); }
                }
            }
        }
        public override void Save(BinBuffer bb)
        {
            bb.Clear();
            bb.Write(Path);
            bb.Write(SkillPoints);
            bb.Write(SkillLevel);
            for (int i = 0; i < SkillsById.Length; i++)
            {
                bb.Write(SkillsById[i]);
            }
            bb.Write(Hotkeys.Count);
            foreach (var kvp in Hotkeys)
            {
                bb.Write(kvp.Key);
                bb.Write((int)kvp.Value);
            }

        }
        #endregion
        public bool checkPreviousSkill(int i)
        {
            if (i > 0 && i < SkillsById.Length)
            {
                if (i ==8 || i==15) return SkillsById[0];
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
        public void setCooldown(int position, int time, string texture, string name)
        {
            CManager.addCooldown(position, name, texture, time);
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
            if (p.statMana > 100)
            {
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
            if (player.statMana > 50)
            {
                player.statMana -= 50;
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
            if (Path == 2)
            {//0-none,1-warrior,2-mage,3-range
                DispersionTimer++;


                if (DispersionTimer > 600)
                {
                    DispersionTimer = 0;
                    DispersionCount = 0;
                }
            }
            base.PreUpdate();
        }


        public override void MidUpdate()
        {
            if (!TreeCleared && minterface.isInitialized())
            {
                minterface.updateMPlayer(this);
                for (int i = 0; i < SkillsById.Length; i++)
                {
                    minterface.setupField(i, SkillsById[i]);
                }
                TreeCleared = true;
            }
            base.PostUpdate();
            if (Path == 2)
            {//0-none,1-warrior,2-mage,3-range
                if(!CManager.isUsable(13) && CManager.getTime(13)<=100 && CManager.getTime(13)%5==0)
                {
                    for (int i = 0; i < Main.rand.Next(1, 4); i++)
                    {
                       Projectile.NewProjectile(new Vector2(player.position.X, player.position.Y + Main.rand.Next(-100, 100)), new Vector2(0, 0), ProjDef.byName["ProjectAdvance:FlameFury"].type, 39, 3, player.whoAmI);
                    }
                }
                if (CanUse && player == Main.localPlayer)
                {
                    if (SkillsById[3] && (Hotkeys.ContainsKey(3)))
                    {
                        if (Main.GetKeyState((int)Hotkeys[3]) == -127 || Main.GetKeyState((int)Hotkeys[3]) == -128) teleportationCheck();
                        if ((Main.mouseRight && Main.mouseRightRelease) && CanTeleport == true) teleportPlayer();
                    }

                    if (SkillsById[2])
                    {
                        if (player.HasBuff(BuffDef.byName["ProjectAdvance:PowerSurge"]) != -1)
                        {
                            SurgeTimer = 0;

                        }
                        else
                        {
                            CManager.useSkill(2);
                            SurgeTimer++;
                        }
                        if (SurgeTimer > 360)
                        {

                            player.AddBuff(BuffDef.byName["ProjectAdvance:PowerSurge"], 6000);
                        }
                    }
                    if (SkillsById[4] && Hotkeys.ContainsKey(4) && (Hotkeys[4] != null) && ((Main.GetKeyState((int)Hotkeys[4]) == -127 || Main.GetKeyState((int)Hotkeys[4]) == -128)))
                    {
                        manaBlast(Main.localPlayer);
                    }
                    if (SkillsById[5] && Hotkeys.ContainsKey(5) && Hotkeys[5] != null && ((Main.GetKeyState((int)Hotkeys[5]) == -127 || Main.GetKeyState((int)Hotkeys[5]) == -128) && player.statMana >= 60))
                    {
                        if (player.HasBuff(BuffDef.byName["ProjectAdvance:DispersionBuff"]) == -1)
                        {
                            player.statMana -= 60;
                            player.AddBuff(BuffDef.byName["ProjectAdvance:DispersionBuff"], 60);

                            if (DispersionCount++ > 3)
                                player.AddBuff(BuffDef.byName["ProjectAdvance:ManaBreak"], 180);
                        }
                    }
                    if (SkillsById[6] && Hotkeys.ContainsKey(6) && Hotkeys[6] != null && ((Main.GetKeyState((int)Hotkeys[6]) == -127 || Main.GetKeyState((int)Hotkeys[6]) == -128) && player.statMana >= 60) && (CManager.isUsable(6)))
                    {

                        int counter = 0;
                        foreach (NPC n in Main.npc)
                        {
                            if (!n.friendly && n.active && Vector2.Distance(player.position, n.position) < 1800)
                            {
                                counter++;
                                Main.projectile[Projectile.NewProjectile(player.position, new Vector2(0.3f * Main.rand.Next(-3, 3), -0.3f * Main.rand.Next(3)), ProjDef.byName["ProjectAdvance:ArcaneBolt"].type, 50, 1, player.whoAmI)].GetSubClass<Projectiles.ArcaneBolt>().setTarget(n);
                            }
                            if (counter > 0)
                            {
                                CManager.useSkill(6);
                            }
                        }
                    }
                    if (SkillsById[7] && Hotkeys.ContainsKey(7) && Hotkeys[7] != null && ((Main.GetKeyState((int)Hotkeys[7]) == -127 || Main.GetKeyState((int)Hotkeys[7]) == -128)) && (CManager.isUsable(7)))
                    {
                        CManager.useSkill(7);
                        player.AddBuff(BuffDef.byName["ProjectAdvance:SageMode"], 600);
                    }
                    if (SkillsById[10] && Hotkeys.ContainsKey(10) && Hotkeys[10] != null && ((Main.GetKeyState((int)Hotkeys[10]) == -127 || Main.GetKeyState((int)Hotkeys[10]) == -128)) && (CManager.isUsable(10)) && player.statMana >= 30)
                    {
                        CManager.useSkill(10);
                        player.statMana -= 30;
                        Main.projectile[Projectile.NewProjectile(player.position, Vector2.Multiply(Vector2.Normalize((new Vector2((Main.mouseWorld.X - player.position.X), (Main.mouseWorld.Y - player.position.Y)))),5f), ProjDef.byName["ProjectAdvance:FireballProjectile"].type, 50, 3, player.whoAmI)].tileCollide = false;
                    }
                    if (SkillsById[12] && Hotkeys.ContainsKey(12) && Hotkeys[12] != null && ((Main.GetKeyState((int)Hotkeys[12]) == -127 || Main.GetKeyState((int)Hotkeys[12]) == -128) && player.statMana >= 50))
                    {
                        player.mouseInterface = true;
                        if (player.HasBuff(BuffDef.byName["ProjectAdvance:LightningForm"]) == -1)
                        {
                            player.AddBuff(BuffDef.byName["ProjectAdvance:LightningForm"], 60);
                            //doesn't really deal damage and looks ugly compared to particles ;..;
                            //Main.projectile[Projectile.NewProjectile(player.position, new Vector2(0, 0), ProjDef.byName["ProjectAdvance:LightningForm"].type, 50, player.whoAmI)].GetSubClass<Projectiles.LightningForm>().setOwner(player);
                        }
                    }
                    if (SkillsById[13] && Hotkeys.ContainsKey(13) && Hotkeys[13] != null && ((Main.GetKeyState((int)Hotkeys[13]) == -127 || Main.GetKeyState((int)Hotkeys[13]) == -128)) && (CManager.isUsable(13)))
                    {
                        CManager.useSkill(13);
                       
                    }
                    if (SkillsById[14] && Hotkeys.ContainsKey(14) && Hotkeys[14] != null && ((Main.GetKeyState((int)Hotkeys[14]) == -127 || Main.GetKeyState((int)Hotkeys[14]) == -128)) && (CManager.isUsable(14)))
                    {
                        CManager.useSkill(14);
                        player.AddBuff(BuffDef.byName["ProjectAdvance:HellJester"], 900);

                    }
                }
            }
        }
        public override void PreHurt(bool pvp, bool quiet, ref bool getHurt, ref bool playSound, ref bool genGore, ref int damage, ref int hitDirection, ref string deathText, ref bool crit, ref float critMultiplier)
        {
            base.PreHurt(pvp, quiet, ref getHurt, ref playSound, ref genGore, ref damage, ref hitDirection, ref deathText, ref crit, ref critMultiplier);
            if (Path == 2)
            {//0-none,1-warrior,2-mage,3-range
                if (player == Main.localPlayer && SkillsById[1] && player.statManaMax2 > (int)(0.2 * damage))
                {
                    player.statManaMax2 -= (int)(0.2 * damage);
                    damage = (int)(damage * 0.8);
                    Dust.NewDust(player.position, new Vector2(10, 10), 50);
                }
            }
        }
        public override void ModifyDrawLayerList(List<PlayerLayer> list)
        {
            if (player.HasBuff(BuffDef.byName["ProjectAdvance:DispersionBuff"]) != -1 || player.HasBuff(BuffDef.byName["ProjectAdvance:LightningForm"]) != -1)
            {

                foreach (PlayerLayer p in list)
                    p.visible = false;
            }
            base.ModifyDrawLayerList(list);
        }
        public override void PreUpdate()
        {

            base.PreUpdate();
        }
        public override void DamageNPC(Projectile projectile, NPC npc, int hitDir, ref int damage, ref float knockback, ref bool crit, ref float critMult)
        {
            if (SkillsById[8]&& projectile.magic && npc.HasBuff(BuffDef.byName["ProjectAdvance:BurningSoul"]) == -1 )
                npc.AddBuff(BuffDef.byName["ProjectAdvance:BurningSoul"], 300);
            if (SkillsById[9] && player.HasBuff(BuffDef.byName["ProjectAdvance:BurningBloodBuff"]) == -1)
                player.AddBuff(BuffDef.byName["ProjectAdvance:BurningBloodBuff"], 300);
            if (SkillsById[11] && projectile.magic) 
            {
                critMult += 1.5f;
            }
            base.DamageNPC(projectile, npc, hitDir, ref damage, ref knockback, ref crit, ref critMult);
        }
    }
}