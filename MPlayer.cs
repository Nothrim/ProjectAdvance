using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        Vector2 PreviousPosition;
        int EarthenShellTimer = 0;
        public Dictionary<int, Keys?> Hotkeys;
        int SurgeTimer = 0;
        static MInterface minterface = (MInterface)SuperModsBase.SMB.modInterface;
        public int getPath() { return Path; }
        public void setPath(int path) { Path = path; }
        public int getSkillPoints() { return SkillPoints; }
        //blocking mechanic
        int BlockingTimer = 0;
        bool Blocking = false;
        public bool isBlocking() { return Blocking; }
        public PlayerLayer BlockingLayer = new PlayerLayer.Action("ProjectAdvance:BlockEffect", (layer, drawPlayer, sb) =>
        {
            Texture2D b = Main.goreTexture[GoreDef.gores["ProjectAdvance:BlockEffect"]];
            sb.Draw(b,new Vector2(Main.screenWidth/2-drawPlayer.width,Main.screenHeight/2-drawPlayer.height+11), Color.White);
        });
        public PlayerLayer Barrier = new PlayerLayer.Action("ProjectAdvance:BarrierEffect", (layer, drawPlayer, sb) =>
        {
            int durability=drawPlayer.GetSubClass<MPlayer>().getBarrierDurability();
            if(durability>255)durability/=2;
            Texture2D b = Main.goreTexture[GoreDef.gores["ProjectAdvance:BarrierEffect"]];
            //sb.Draw(b, new Rectangle((int)drawPlayer.position.X,(int)drawPlayer.position.Y,40,60), new Rectangle(0, 0, 40, 60), new Color(Math.Min(255, durability), 0, 130));
            
            sb.Draw(b, new Vector2(Main.screenWidth / 2 - drawPlayer.width, Main.screenHeight / 2 - drawPlayer.height + 11), new Color(Math.Min(255,durability),0,130));
        });
        //-----------------
        //swipe mechanic
        float PlayerMeleeSpeed;
        int ItemUseTime;
        //mirage slash mechanic
        int SlashDamage = 0;
        //barrier mechanic
        int BarrierDurability = 0;
        bool BarrierUp = false;
        int BarrierTimer;
        public int getBarrierDurability() { return BarrierDurability; }
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
                if (Path == 1)
                {
                    if (SkillsById[3]) { CManager.addCooldown(3, "TrollsBlood", "ProjectAdvance:TrollsBlood", 3600); }
                    if (SkillsById[4]) { CManager.addCooldown(4, "MirrorShield", "ProjectAdvance:MirrorShield", 300); }
                    if (SkillsById[5]) { CManager.addCooldown(5, "Throw", "ProjectAdvance:Throw", 300); }
                    if (SkillsById[6]) { CManager.addCooldown(6, "Shockwave", "ProjectAdvance:Shockwave", 300); }
                    if (SkillsById[7]) { CManager.addCooldown(7, "EnderLegacy", "ProjectAdvance:EnderLegacy", 7200); }
                    if (SkillsById[12]) { CManager.addCooldown(12, "Swipe", "ProjectAdvance:Swipe", 300); }
                    if (SkillsById[13]) { CManager.addCooldown(13, "BloodRite", "ProjectAdvance:BloodRite", 3600); }
                    if (SkillsById[14]) { CManager.addCooldown(14, "OverwhelmingPressure", "ProjectAdvance:OverwhelmingPressure", 3600); }
                    if (SkillsById[16] && SkillsById[18]) { CManager.addCooldown(16, "MirageSlash", "ProjectAdvance:MirageSlash", 120); }
                    else if (SkillsById[16]) { CManager.addCooldown(16, "MirageSlash", "ProjectAdvance:MirageSlash", 180); }
                    if (SkillsById[17]) { CManager.addCooldown(17, "Barrier", "ProjectAdvance:Barrier", 3600); }
                    if (SkillsById[18]) { CManager.addCooldown(19, "Shadowstep", "ProjectAdvance:Shadowstep", 30); }
                    if (SkillsById[20]) { CManager.addCooldown(20, "Slash", "ProjectAdvance:Slash", 180); }
                    if (SkillsById[21]) { CManager.addCooldown(21, "ThreeThousandsCuts", "ProjectAdvance:ThreeThousandCuts", 7200); }
                    
                }
                else if (Path == 2)
                {//0-none,1-warrior,2-mage,3-range
                    if (SkillsById[2]) { CManager.addCooldown(2, "PowerSurge", "ProjectAdvance:PowerSurge", 360); }
                    if (SkillsById[7]) { CManager.addCooldown(7, "SageMode", "ProjectAdvance:SageMode", 18000); }
                    if (SkillsById[6]) { CManager.addCooldown(6, "ArcaneBarrage", "ProjectAdvance:ArcaneBarrage", 600); }
                    if (SkillsById[0]) { player.AddBuff(BuffDef.byName["ProjectAdvance:Wizard"], 216000); }
                    if (SkillsById[10]) { CManager.addCooldown(10, "Fireball", "ProjectAdvance:Fireball", 180); }
                    if (SkillsById[13]) { CManager.addCooldown(13, "FlameFury", "ProjectAdvance:FlameFury", 900); }
                    if (SkillsById[14]) { CManager.addCooldown(14, "HellJester", "ProjectAdvance:HellJester", 18000); }
                    if (SkillsById[16]) { CManager.addCooldown(16, "NatureProtection", "ProjectAdvance:NatureProtection", 28800); }
                    if (SkillsById[17]) { CManager.addCooldown(17, "Roots", "ProjectAdvance:Roots", 600); }
                    if (SkillsById[18]) { CManager.addCooldown(18, "Dig", "ProjectAdvance:Dig", 10); }
                    if (SkillsById[19]) { CManager.addCooldown(19, "ThornSword", "ProjectAdvance:ThornSword", 10); }
                    if (SkillsById[20]) { CManager.addCooldown(20, "PoisonZone", "ProjectAdvance:PoisonZone", 900); }
                    if (SkillsById[21]) { CManager.addCooldown(21, "ForceOfNature", "ProjectAdvance:ForceOfNature", 1800); }
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
        public void changeCooldown(int id,int time)
        {
            CManager.setCooldown(id, time);
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
            if(SkillsById[15] && player.position==PreviousPosition)
            {
                EarthenShellTimer++;
            }
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
            if(Path==1)
            {
                if(player==Main.localPlayer)
                {
                    if (Blocking)
                    {
                        if(BlockingTimer++>70)
                        {
                        BlockingTimer = 0;
                        Blocking = false;
                        }
                     }
                    if (SkillsById[12])
                    {
                        if (!CManager.isUsable(12) && CManager.getTime(12) <= 30)
                        {
                            player.meleeSpeed *= 1000;
                            player.heldItem.useTime = 1;
                            player.delayUseItem = false;
                            player.controlUseItem = true;
                            player.releaseUseItem = true;

                        }
                        else if (!CManager.isUsable(12) && CManager.getTime(12) == 31)
                        {
                            player.meleeSpeed = PlayerMeleeSpeed;
                            player.heldItem.useTime = ItemUseTime;
                        }
                    }
                    if(SkillsById[17] && !BarrierUp && CManager.isUsable(17) && player.statMana==player.statManaMax2)
                    {
                        CManager.useSkill(17);
                        BarrierDurability = player.statMana;
                        player.statMana = 0;
                        BarrierUp = true;
                    }
                }
                if(CanUse && player==Main.localPlayer)
                {
                    if(SkillsById[14] && player.controlUseItem && player.releaseUseItem && player.HasBuff(BuffDef.byName["ProjectAdvance:OverwhelmingPressure"])!=-1 && player.heldItem.damage>0)
                    {
                        Projectile.NewProjectile(player.position, new Vector2(Vector2.Normalize(Main.mouseWorld - player.position).X * 25, Main.rand.Next(-2,2)), ProjDef.byName["ProjectAdvance:PressureBolt"].type, player.heldItem.damage, player.heldItem.knockBack, player.whoAmI);
                    }
                    if (SkillsById[1] && player.HasBuff(BuffDef.byName["ProjectAdvance:LayerI"]) == -1 && player.HasBuff(BuffDef.byName["ProjectAdvance:LayerII"]) == -1 && player.HasBuff(BuffDef.byName["ProjectAdvance:LayerIII"]) == -1) 
                    {
                        player.AddBuff(BuffDef.byName["ProjectAdvance:LayerI"], 300);
      
                    }
                    if(SkillsById[4] && Hotkeys.ContainsKey(4) && Hotkeys[4] != null && (Main.GetKeyState((int)Hotkeys[4]) == -127 || Main.GetKeyState((int)Hotkeys[4]) == -128) && (CManager.isUsable(4)))
                    {
                        CManager.useSkill(4);
                        Blocking = true;
                    }
                    if (SkillsById[5] && Hotkeys.ContainsKey(5) && Hotkeys[5] != null && (Main.GetKeyState((int)Hotkeys[5]) == -127 || Main.GetKeyState((int)Hotkeys[5]) == -128) && (CManager.isUsable(5)))
                    {
                       
                        NPC target=null;
                        foreach(NPC n in Main.npc)
                        {
                            if (target==null && n.active && n.Distance(player.position) < 250)
                            {
                                target = n;
                            }
                            else if(target!=null && n.active && n.Distance(player.position)<target.Distance(player.position))
                            {
                                target = n;
                            }
                        }
                        if(target!=null)
                        {
                            CManager.useSkill(5);
                            target.velocity = Vector2.Multiply(Vector2.Normalize(Main.mouseWorld - player.position), 25);
                            Main.projectile[Projectile.NewProjectile(target.position, Vector2.Zero, ProjDef.byName["ProjectAdvance:ThrowDummy"].type, target.life / 150, 0, player.whoAmI)].GetSubClass<Projectiles.ThrowDummy>().setTarget(target);
                        }

                    }
                    if(SkillsById[6] && Hotkeys.ContainsKey(6) && Hotkeys[6] != null && (Main.GetKeyState((int)Hotkeys[6]) == -127 || Main.GetKeyState((int)Hotkeys[6]) == -128) && (CManager.isUsable(6)))
                    {
                        CManager.useSkill(6);
                        Projectile.NewProjectile(new Vector2(player.position.X,player.position.Y+player.height), new Vector2(-7, 0), ProjDef.byName["ProjectAdvance:Shockwave"].type, player.statLife, 3, player.whoAmI);
                        Projectile.NewProjectile(new Vector2(player.position.X, player.position.Y + player.height), new Vector2(7, 0), ProjDef.byName["ProjectAdvance:Shockwave"].type, player.statLife, 3, player.whoAmI);
                    }
                    if (SkillsById[7] && Hotkeys.ContainsKey(7) && Hotkeys[7] != null && (Main.GetKeyState((int)Hotkeys[7]) == -127 || Main.GetKeyState((int)Hotkeys[7]) == -128) && (CManager.isUsable(7)))
                    {
                        CManager.useSkill(7);
                        player.AddBuff(BuffDef.byName["ProjectAdvance:EnderLegacy"], 360);
                    }
                    if (SkillsById[12] && Hotkeys.ContainsKey(12) && Hotkeys[12] != null && (Main.GetKeyState((int)Hotkeys[12]) == -127 || Main.GetKeyState((int)Hotkeys[12]) == -128) && (CManager.isUsable(12)))
                    {
                        CManager.useSkill(12);
                        PlayerMeleeSpeed = player.meleeSpeed;
                        ItemUseTime=player.heldItem.useTime;
                    }
                    if (SkillsById[13] && Hotkeys.ContainsKey(13) && Hotkeys[13] != null && (Main.GetKeyState((int)Hotkeys[13]) == -127 || Main.GetKeyState((int)Hotkeys[13]) == -128) && (CManager.isUsable(13)))
                    {
                        CManager.useSkill(13);
                        player.AddBuff(BuffDef.byName["ProjectAdvance:BloodRite"], 600);
                    }
                    if (SkillsById[14] && Hotkeys.ContainsKey(14) && Hotkeys[14] != null && (Main.GetKeyState((int)Hotkeys[14]) == -127 || Main.GetKeyState((int)Hotkeys[14]) == -128) && (CManager.isUsable(14)))
                    {
                        CManager.useSkill(14);
                        player.AddBuff(BuffDef.byName["ProjectAdvance:OverwhelmingPressure"], 600);
                    }
                    if (SkillsById[16] && Hotkeys.ContainsKey(16) && Hotkeys[16] != null && (Main.GetKeyState((int)Hotkeys[16]) == -127 || Main.GetKeyState((int)Hotkeys[16]) == -128) && (CManager.isUsable(16)))
                    {
                        if(player.statMana>10)
                        {
                            player.statMana -= 10;
                            SlashDamage += 10;
                        }
                    }
                    if (SkillsById[16] && SlashDamage>0 && player.controlUseItem && player.releaseUseItem && (CManager.isUsable(16)))
                    {
                        CManager.useSkill(16);
                        if(player.heldItem.damage>0)
                        {
                            SlashDamage += player.heldItem.damage;
                        }
                        if (SkillsById[18]) SlashDamage += SlashDamage/4;
                        Main.projectile[Projectile.NewProjectile(player.position, Vector2.Multiply(Vector2.Normalize(Main.mouseWorld - player.position), 15), ProjDef.byName["ProjectAdvance:MirageSlash"].type, SlashDamage, 1, player.whoAmI)].scale = Math.Max(1, SlashDamage / 100);
                        SlashDamage = 0;
                    }
                     if (SkillsById[19] && Hotkeys.ContainsKey(19) && Hotkeys[19] != null && (Main.GetKeyState((int)Hotkeys[19]) == -127 || Main.GetKeyState((int)Hotkeys[19]) == -128) && (CManager.isUsable(19) && player.statMana>20))
                     {
                         NPC target = null;
                         float distance = 400;
                         foreach(NPC n in Main.npc)
                         {
                             if (n.active && !n.friendly && n.Distance(Main.mouseWorld) < distance)
                             {
                                 target = n;
                                 distance = n.Distance(Main.mouseWorld);
                             }
                         }
                         if (target != null) 
                         {
                             
                             Vector2 TargetShift=new Vector2(target.center().X+target.width*-target.direction, target.center().Y-player.height/2);
                             Point CheckPosition = TargetShift.ToTileCoordinates();
                             if(Main.tile[CheckPosition.X,CheckPosition.Y].collisionType==0)
                             {
                                 for (int i = 0; i < 7;i++ )
                                 {
                                     Main.dust[Dust.NewDust(player.getRect(), 186)].velocity = new Vector2(Main.rand.Next(-4, 4), Main.rand.Next(-3, 3));
                                     Main.dust[Dust.NewDust(TargetShift,new Vector2(0,0), 186)].velocity = new Vector2(Main.rand.Next(-4, 4), Main.rand.Next(-3, 3));
                                 }
                                 Projectile.NewProjectile(new Vector2(player.position.X,player.position.Y+player.height/2), Vector2.Zero, ProjDef.byName["ProjectAdvance:Shadow"].type, player.heldItem.damage, 0, player.whoAmI);
                                 Main.PlaySound("ProjectAdvance:Shadowstep", player.position.X, player.position.Y);
                                 player.position = TargetShift;
                                 player.velocity.Y += -1;
                                 player.direction = target.direction;
                                 player.controlUseItem = true;
                                 player.statMana -= 20;
                                 CManager.useSkill(19);
                             }
                         }
                     }
                     if (SkillsById[20] && Hotkeys.ContainsKey(20) && Hotkeys[20] != null && (Main.GetKeyState((int)Hotkeys[20]) == -127 || Main.GetKeyState((int)Hotkeys[20]) == -128) && (CManager.isUsable(20)))
                     {
                         CManager.useSkill(20);
                         float distance = Vector2.Distance(Main.mouseWorld, player.position);
                         Vector2 RotationVector;
                         Vector2 EndPoint = player.position - Vector2.Multiply(Vector2.Normalize(player.position - Main.mouseWorld), Math.Max(distance,300));
                         Point EndPointTileCoords=EndPoint.ToTileCoordinates();
                         Vector2 SlashPositon = new Vector2(EndPoint.X + player.width * -player.direction , EndPoint.Y);
                         if (Main.mouseWorld.X > player.position.X)
                             player.direction = 1;
                         else
                             player.direction = -1;
                         if(Main.tile[EndPointTileCoords.X,EndPointTileCoords.Y].active())
                         {
                             for(int i=EndPointTileCoords.X-1;i<=EndPointTileCoords.X+1;i++)
                             {
                                 for(int j=EndPointTileCoords.Y-1;j<=EndPointTileCoords.Y+1;j++)
                                 {
                                     Main.tile[i, j].active(false);
                                     WorldGen.SquareTileFrame(i, j);
                                 }
                             }
                           
                         }
                       
                         RotationVector = Vector2.Divide(Vector2.Normalize(Main.mouseWorld - player.position),100);
                         int max=Math.Max((int)(distance/50),5);
                         for (int i = 0; i <max ; i++)
                         {

                             Projectile.NewProjectile((Vector2.Lerp(SlashPositon, player.position, (float)i/(float)max)), RotationVector, ProjDef.byName["ProjectAdvance:Slash"].type, player.heldItem.damage*3, 1, player.whoAmI);
                         }
                         player.position = new Vector2(EndPoint.X - player.width / 2, EndPoint.Y - player.height / 2);
                         player.velocity.Y -= 1;
                         Main.PlaySound("ProjectAdvance:Sl", player.position.X, player.position.Y);
                     }
                     if (SkillsById[21] && Hotkeys.ContainsKey(21) && Hotkeys[21] != null && (Main.GetKeyState((int)Hotkeys[21]) == -127 || Main.GetKeyState((int)Hotkeys[21]) == -128) && (CManager.isUsable(21)))
                     {
                         CManager.useSkill(21);
                         player.AddBuff(BuffDef.byName["ProjectAdvance:ThreeThousandCuts"], 600);
                     }
                }
            }
            #region mage skills
            else if (Path == 2)
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
                            //doesn't really deal damage and looks ugly compared to dust ;..;
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
                    if(SkillsById[15])
                    {
                        PreviousPosition = player.position;
                        if(EarthenShellTimer>400 )
                        {
                            player.AddBuff(BuffDef.byName["ProjectAdvance:EarthenShell"], 600);
                            EarthenShellTimer = 0;
                        }
                    }
                    if (SkillsById[17] && Hotkeys.ContainsKey(17) && Hotkeys[17] != null && ((Main.GetKeyState((int)Hotkeys[17]) == -127 || Main.GetKeyState((int)Hotkeys[17]) == -128)) && (CManager.isUsable(17)) && player.statMana>60)
                    {
                        CManager.useSkill(17);
                        player.statMana -= 60;
                        Projectile.NewProjectile(Main.mouseWorld, new Vector2(0, 0), ProjDef.byName["ProjectAdvance:RootZone"].type, 1, 0, player.whoAmI);
                    }
                    if (SkillsById[18] && Hotkeys.ContainsKey(18) && Hotkeys[18] != null && ((Main.GetKeyState((int)Hotkeys[18]) == -127 || Main.GetKeyState((int)Hotkeys[18]) == -128)) && (CManager.isUsable(18)) && player.statMana > 2)
                    {
                        CManager.useSkill(18);
                        Main.dust[Dust.NewDust(Main.mouseWorld, new Vector2(10, 10), 15, new Vector2(0, 0), 0, Color.GreenYellow, 3f)].noGravity = true;
                        Point StartingPosition = Main.mouseWorld.ToTileCoordinates();
                        int posX;
                        int posY;
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                if (Main.tile[posX = (StartingPosition.X + i), posY = (StartingPosition.Y + j)].active())
                                {
                                    if (Main.tile[posX, posY].type == 0 || Main.tile[posX, posY].type == 1 || Main.tile[posX, posY].type == 2 || Main.tile[posX, posY].type == 161
                                        || Main.tile[posX, posY].type == 147 || Main.tile[posX, posY].type == 59 || Main.tile[posX, posY].type == 57 || Main.tile[posX, posY].type == 53
                                        || Main.tile[posX, posY].type == 224 || Main.tile[posX, posY].type == 123 || Main.tile[posX, posY].type == 40)
                                    {
                                        WorldGen.SquareTileFrame(posX, posY);
                                        Main.tile[posX, posY].active(false);
                                    
                                        
                                    }
                                }
                            }
                        }
                        Main.PlaySound(0);
                        
                    }
                    if (SkillsById[19] && Hotkeys.ContainsKey(19) && Hotkeys[19] != null && ((Main.GetKeyState((int)Hotkeys[19]) == -127 || Main.GetKeyState((int)Hotkeys[19]) == -128)) && (CManager.isUsable(19)) && player.statMana > 20)
                    {
                        CManager.useSkill(19);
                        float distance = Vector2.Distance(Main.mouseWorld, player.position);
                        player.statMana -= 20;
                        Vector2 RotationVector;
                        Vector2 EndPoint=player.position-Vector2.Multiply(Vector2.Normalize(player.position-Main.mouseWorld),500);           
                        RotationVector=Vector2.Normalize(Main.mouseWorld-player.position);
                        float rotation = (float)System.Math.Atan2((double)RotationVector.X, (double)RotationVector.Y);
                        //TConsole.Print("MouseWorld:" + Main.mouseWorld + " Mouse:" + Main.mouse + " Player:" + player.position + " Calculated:" + EndPoint+" check"+Main.topWorld);
                        Main.projectile[Projectile.NewProjectile(EndPoint, RotationVector, 8, 50, 1, player.whoAmI)].rotation = rotation;
                        for (int i = 1; i <= 15; i++)
                        {
                           Projectile.NewProjectile((Vector2.Lerp(EndPoint, player.position, 0.07f * i)), RotationVector, 7, 50, 1, player.whoAmI);
                        }
                                
                    }
                    if (SkillsById[20] && Hotkeys.ContainsKey(20) && Hotkeys[20] != null && ((Main.GetKeyState((int)Hotkeys[20]) == -127 || Main.GetKeyState((int)Hotkeys[20]) == -128)) && (CManager.isUsable(20)) && player.statMana > 50)
                    {
                        CManager.useSkill(20);
                        player.statMana -= 50;
                        for (int i = 0; i < 20; i++)
                            Projectile.NewProjectile(new Vector2(player.position.X + Main.rand.Next(-200, 200), player.position.Y + Main.rand.Next(-200, 200)), new Vector2(0, 0), ProjDef.byName["ProjectAdvance:PoisonSerpent"].type, 50, 1,player.whoAmI);
                    }
                    if (SkillsById[21] && Hotkeys.ContainsKey(21) && Hotkeys[21] != null && ((Main.GetKeyState((int)Hotkeys[21]) == -127 || Main.GetKeyState((int)Hotkeys[21]) == -128)) && (CManager.isUsable(21)) && player.statMana > 150)
                    {
                        bool gotTarget = false;
                        float prevDistance = 500;
                        float distance;
                        Player PTarget = null;
                        NPC NTarget = null;
                        foreach(Player p in Main.player)
                        {
                            if(p.active && p.statLife<p.statLifeMax2 && (distance=p.Distance(Main.mouseWorld))<300 )
                            {
                                if(distance<prevDistance)
                                {
                                    gotTarget = true;
                                    prevDistance = distance;
                                    PTarget = p;
                                }
                            }
                        }
                        if(gotTarget)
                        {
                            Main.projectile[Projectile.NewProjectile(player.position, Vector2.Zero, ProjDef.byName["ProjectAdvance:HeallingDummy"].type, 70, 2,player.whoAmI)].GetSubClass<Projectiles.HeallingDummy>().setTarget(PTarget, 8);
                        }
                        else
                        {
                            foreach(NPC n in Main.npc)
                            {
                                if(n.active && (distance=n.Distance(Main.mouseWorld))<500)
                                {
                                    if (distance < prevDistance)
                                    {
                                        gotTarget = true;
                                        prevDistance = distance;
                                        NTarget = n;
                                    }
                                }
                            }
                        }
                        if(gotTarget && NTarget!=null)
                        {
                     
                                Main.projectile[Projectile.NewProjectile(player.position, Vector2.Zero, ProjDef.byName["ProjectAdvance:HeallingDummy"].type, 70, 2, player.whoAmI)].GetSubClass<Projectiles.HeallingDummy>().setTarget(NTarget, 8);
                        }
                        if (gotTarget)
                        {
                            CManager.useSkill(21);
                            player.statMana -= 150;
                        }
                    }
                }
            }
            #endregion
        }
        public override void PreHurt(bool pvp, bool quiet, ref bool getHurt, ref bool playSound, ref bool genGore, ref int damage, ref int hitDirection, ref string deathText, ref bool crit, ref float critMultiplier)
        {
            base.PreHurt(pvp, quiet, ref getHurt, ref playSound, ref genGore, ref damage, ref hitDirection, ref deathText, ref crit, ref critMultiplier);
            if(Path==1 && player==Main.localPlayer)
            {
                if(player.HasBuff(BuffDef.byName["ProjectAdvance:EnderLegacy"])!=-1)
                {
                    getHurt = false;
                    playSound = false;
                    player.statLife += damage;
                    CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, 10, 2), Color.Green,"+"+damage);
                }
                if(Blocking)
                {
                    damage = 1;
                    playSound = false;
                    genGore = false;
                    Main.PlaySound("ProjectAdvance:Parry", player.position.X, player.position.Y);
                }
                if (SkillsById[2] && Main.rand.Next(10) == 0)
                {
                   
                    getHurt = false;
                    CombatText.NewText(new Rectangle((int)player.position.X,(int)player.position.Y,10,2), Color.LightBlue, "BLOCKED!");
                }
                if(SkillsById[3] && CManager.isUsable(3) && player.statLife<player.statLifeMax2/2)
                {
                    CManager.useSkill(3);
                    player.AddBuff(BuffDef.byName["ProjectAdvance:TrollRegeneration"], 300);
                }
                if(SkillsById[9])
                {
                    damage =(int)(damage* Math.Max(((float)player.statLife / (float)player.statLifeMax2), 0.7f));
                }
                if(SkillsById[17] && BarrierUp)
                {
                    if(damage<=BarrierDurability)
                    {
                        getHurt = false;
                        player.immune = true;
                        if (BarrierTimer > 17)
                        {
                            BarrierDurability -= damage;
                            BarrierTimer = 0;
                            Main.PlaySound("ProjectAdvance:Barrier", player.position.X, player.position.Y);
                        }
                        else
                            BarrierTimer++;
                    }
                    else
                    {
                        damage -= BarrierDurability;
                        BarrierDurability = 0;
                        BarrierUp = false;
                        Main.PlaySound("ProjectAdvance:BarrierBreak", player.position.X, player.position.Y);
                        for(int i=0;i<10;i++)
                        {
                            Main.dust[Dust.NewDust(player.position, new Vector2(5, 5), 20,Vector2.Zero,0,Color.LightBlue,3f)].velocity = new Vector2(Main.rand.Next(-4, 4), Main.rand.Next(-4, 4));
                        }
                    }
                }
            }
            else if (Path == 2)
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
            if(player==Main.localPlayer)
            {
                if(Blocking)
                {
                    PlayerLayer.Add(list, BlockingLayer, PlayerLayer.LayerHead, false);
                }
                if(BarrierUp)
                {
                    PlayerLayer.Add(list, Barrier, PlayerLayer.LayerHead, false);
                }
            }
            base.ModifyDrawLayerList(list);
        }
        public override void PreUpdate()
        {
           

            base.PreUpdate();
        }
        public override void DamageNPC(Projectile projectile, NPC npc, int hitDir, ref int damage, ref float knockback, ref bool crit, ref float critMult)
        {
            if (player == Main.localPlayer)
            {
                if (Path == 2)
                {
                    if (SkillsById[8] && projectile.magic && npc.HasBuff(BuffDef.byName["ProjectAdvance:BurningSoul"]) == -1)
                        npc.AddBuff(BuffDef.byName["ProjectAdvance:BurningSoul"], 300);
                    if (SkillsById[9] && player.HasBuff(BuffDef.byName["ProjectAdvance:BurningBloodBuff"]) == -1)
                        player.AddBuff(BuffDef.byName["ProjectAdvance:BurningBloodBuff"], 300);
                    if (SkillsById[11] && projectile.magic)
                    {
                        critMult += 1.5f;
                    }
                }
            }
            base.DamageNPC(projectile, npc, hitDir, ref damage, ref knockback, ref crit, ref critMult);
        }
        public override void DealtPlayer(NPC npc, int hitDir, int dmgDealt, bool crit)
        {
            if (player == Main.localPlayer)
            {
                if (Blocking && npc.life > npc.damage)
                {
                    npc.life -= npc.damage / 2;
                    CombatText.NewText(npc.getRect(), Color.Orange, "" + npc.damage / 2);
                }
            }
            base.DealtPlayer(npc, hitDir, dmgDealt, crit);
        }

  
        public override bool? PreKill(double damage, int hitDirection, bool pvp, string deathText)
        {
            if (Path == 2)
            {
                if (player == Main.localPlayer && SkillsById[16] && CManager.isUsable(16))
                {
                    CManager.useSkill(16);
                    player.statLife = player.statLifeMax2 / 4;
                    CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, 10, 5), Color.LightGreen, "Protected by nature! +" + (player.statLifeMax2 / 4) + " Life");
                    return false;
                }
            }
            return base.PreKill(damage, hitDirection, pvp, deathText);
        }
        public override void DamageNPC(NPC npc, int hitDir, ref int damage, ref float knockback, ref bool crit, ref float critMult)
        {
            if (Path == 1)
            {
                if (player == Main.localPlayer)
                {
                    if(SkillsById[0])
                    {
                        damage += (int)((float)damage / 10f);
                    }
                    if (SkillsById[8] && player.statLife < player.statLifeMax2 * 0.75)
                    {
                        for (int i = 0; i < 20; i++)
                            Dust.NewDust(npc.getRect(), 28, new Vector2(Main.rand.Next(-4, 4), Main.rand.Next(-4, 4)));
                        damage += (int)(damage * ((double)(player.statLifeMax2 + 1 - player.statLife) / (double)player.statLifeMax2));
                    }
                    if (SkillsById[9] && npc.life <= damage && Main.rand.Next(4) == 0)
                    {
                        Main.projectile[Projectile.NewProjectile(npc.position, Vector2.Zero, 30, (int)npc.lifeMax / 2, 3, player.whoAmI)].timeLeft = 1;
                    }
                    if (SkillsById[10])
                    {
                        foreach (NPC n in Main.npc)
                        {
                            if (n.active && !n.friendly && n != npc && n.Distance(player.position)<600)
                            {
                                if (Main.netMode != 1) { CombatText.NewText(n.Hitbox, Color.Orange, (int)(damage * 0.15f)+"", false, true); }
                                n.life -= (int)(damage * 0.15f);
                                if (n.life <= 0)
                                {
                                    n.life = 1;
                                    if (Main.netMode != 1)
                                    {
                                        n.StrikeNPC(9999, 0f, 0, false, false);
                                        if (Main.netMode == 2) { NetMessage.SendData(28, -1, -1, "", n.whoAmI, 1f, 0f, 0f, 9999); }
                                      
                                    }
                                }
                            }
                        }
                    }
                    if(SkillsById[15])
                    {
                        int ManaDrain = Math.Min(npc.life,(int)(0.1f * (float)damage));
                        if (ManaDrain > 0)
                        {
                            CombatText.NewText(player.getRect(), Color.Blue, ManaDrain + "");
                            player.statMana += ManaDrain;
                        }
                    }
                }
                if (SkillsById[18]) critMult += 0.5f;
                base.DamageNPC(npc, hitDir, ref damage, ref knockback, ref crit, ref critMult);
            }
        }
    }
}