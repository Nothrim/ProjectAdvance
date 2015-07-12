using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;
using Microsoft.Xna.Framework.Input;
namespace ProjectAdvance
{
    class SkillTree
    {
       
        #region variables
        
        MPlayer ModPlayer = (MPlayer)Main.localPlayer.GetSubClass<MPlayer>();
        private readonly int STANDARD_SKILL_TREE_PATH_SIZE=4;
        private int screenWidth = Main.screenWidth;
        private int screenHeight = Main.screenHeight;
        Vector2 StartingPoint;
        float FinalRowElements;
        static readonly int BRANCH_SIZE=8;
        bool open = false;
        ToggleElement ToggleTree;
        List<SkillSlot> Tree=new List<SkillSlot>();
        SpriteBatch sb;
        #endregion 
        public SkillTree(SpriteBatch sb) {
        this.sb=sb;
        }
        public void updateMPlayer(MPlayer m) { ModPlayer = m;
        for(int i=0;i<Tree.Count;i++)
        {
            Tree[i].updateMPlayer(m);
        }
        }
        public void buildSkillTree()
        {
            
            ToggleTree = new ToggleElement(new Vector2(470,50));
            FinalRowElements =(float) Math.Pow(2, STANDARD_SKILL_TREE_PATH_SIZE - 1);
            StartingPoint = new Vector2((Main.screenWidth/2) - FinalRowElements*30, Main.screenHeight * 0.5f);
            for(int i=0;i<BRANCH_SIZE;i++)
            {
                Tree.Add(new SkillSlot(StartingPoint+new Vector2(i*60,0), "ProjectAdvance:PlaceholderFrame", i));
                Tree.ElementAt(i).setChoosen(ModPlayer.checkSkillAtPosition(i));
            }
            for (int i = 1; i < BRANCH_SIZE; i++)
            {
                Tree.Add(new SkillSlot(StartingPoint + new Vector2(i * 60, 80), "ProjectAdvance:PlaceholderFrame", i+BRANCH_SIZE-1));
                Tree.ElementAt(i + BRANCH_SIZE - 1).setChoosen(ModPlayer.checkSkillAtPosition(i + BRANCH_SIZE - 1));
            }
            for (int i = 1; i < BRANCH_SIZE; i++)
            {
                Tree.Add(new SkillSlot(StartingPoint + new Vector2(i * 60, 160), "ProjectAdvance:PlaceholderFrame", i+2*BRANCH_SIZE-2));
                Tree.ElementAt(i + 2 * BRANCH_SIZE - 2).setChoosen(ModPlayer.checkSkillAtPosition(i + 2 * BRANCH_SIZE - 2));
            }



        }
        public void loadSlotsData()
        {
            if (ModPlayer.getPath() == 1)
            {
                Tree[0].setTexture("ProjectAdvance:MagicElement");
                Tree[0].setTooltip("Grants various attributes required for begginer mages");
                Tree[1].setTexture("ProjectAdvance:LayeredArmor");
                Tree[1].setTooltip("Gain armor layers when not fighting I:(Always On)+5 Armor,II:+10 armor,III:+15 armor");
                Tree[2].setTexture("ProjectAdvance:Block");
                Tree[2].setTooltip("Block incoming damage [10% chance]");
                Tree[3].setTexture("ProjectAdvance:TrollsBlood");
                Tree[3].setTooltip("Provides hastened regeneration when in danger [1 minute cooldown]");
                Tree[3].setCooldownTimer(3600);
                Tree[4].setTexture("ProjectAdvance:MirrorShield");
                Tree[4].setTooltip("Parry incoming hits/projectiles[5 sec cooldown]");
                Tree[4].setCooldownTimer(300);
                Tree[4].usable();
                Tree[5].setTexture("ProjectAdvance:Throw");
                Tree[5].setTooltip("Throw closest enemy to mouse direction");
                Tree[5].usable();
                Tree[5].setCooldownTimer(300);
                foreach (KeyValuePair<int, Keys?> kvp in ModPlayer.Hotkeys)
                {
                    if (Tree[kvp.Key].isUsable())
                        Tree[kvp.Key].setHotkey(kvp.Value);
                    else
                        ModPlayer.Hotkeys.Remove(kvp.Key);
                }
            }
            if (ModPlayer.getPath() == 2)
            {
                Tree[0].setTexture("ProjectAdvance:MagicElement");
                Tree[0].setTooltip("Grants various attributes required for begginer mages");
                Tree[1].setTexture("ProjectAdvance:ManaShield");
                Tree[1].setTooltip("20% of damage from all sources depletes your mana not health points");
                Tree[2].setTexture("ProjectAdvance:PowerSurge");
                Tree[2].setCooldownTimer(360);
                Tree[2].setTooltip("Your next spell will deal double damage");
                Tree[3].setTexture("ProjectAdvance:Blink");
                Tree[3].setTooltip("Blink to cursor[50 mana]");
                Tree[3].usable();
                Tree[4].setTexture("ProjectAdvance:ManaBlast");
                Tree[4].setTooltip("Use all your mana to initiate powerfull blast [min 100 mana required]");
                Tree[4].usable();
                Tree[5].setTexture("ProjectAdvance:Dispersion");
                Tree[5].usable();
                Tree[5].setTooltip("Go into mistform to avoid all damage [60 mana]");
                Tree[6].setTexture("ProjectAdvance:ArcaneBarrage");
                Tree[6].usable();
                Tree[6].setTooltip("Unleash fury of arcane missles![10 s cooldown]");
                Tree[6].setCooldownTimer(600);
                Tree[7].setTexture("ProjectAdvance:SageMode");
                Tree[7].setTooltip("Enter Sage Mode for 10 seconds [5 minutes cooldown]");
                Tree[7].usable();
                Tree[7].setCooldownTimer(18000);
                Tree[8].setTexture("ProjectAdvance:BurningSoul");
                Tree[8].setTooltip("Your concentrated magic can cause enemies to explode![3 hits]");
                Tree[9].setTexture("ProjectAdvance:BurningBlood");
                Tree[9].setTooltip("You are burning for a fight![Speed bonus when in combat]");
                Tree[10].setTexture("ProjectAdvance:Fireball");
                Tree[10].usable();
                Tree[10].setCooldownTimer(180);
                Tree[10].setTooltip("Do you really need description? [30 mana]");
                Tree[11].setTexture("ProjectAdvance:Focus");
                Tree[11].setTooltip("Increases Critical Damage");
                Tree[12].setTexture("ProjectAdvance:LightningForm");
                Tree[12].setTooltip("Turn into Lightning to move at insane speed[Constantly Drains Mana]");
                Tree[12].usable();
                Tree[13].setTexture("ProjectAdvance:FlameFury");
                Tree[13].setTooltip("Unleash Fury of Flames![10 sec cooldown]");
                Tree[13].usable();
                Tree[13].setCooldownTimer(900);
                Tree[14].setTexture("ProjectAdvance:HellJester");
                Tree[14].setTooltip("100% Critical chance for 10 sec[5 minute cooldown]");
                Tree[14].usable();
                Tree[14].setCooldownTimer(18000);
                Tree[15].setTexture("ProjectAdvance:EarthenShell");
                Tree[15].setTooltip("Gain bonus in defence for standing still");
                Tree[16].setTexture("ProjectAdvance:NatureProtection");
                Tree[16].setTooltip("Force of nature protect you from dying [8 minutes cooldown]");
                Tree[16].setCooldownTimer(28800);
                Tree[17].setTexture("ProjectAdvance:Roots");
                Tree[17].setTooltip("Bind enemies in targeted area to one place");
                Tree[17].usable();
                Tree[17].setCooldownTimer(600);
                Tree[18].setTexture("ProjectAdvance:Dig");
                Tree[18].setTooltip("Use your mana to dig stone or dirt arround mouse");
                Tree[18].usable();
                Tree[18].setCooldownTimer(10);
                Tree[19].setTexture("ProjectAdvance:ThornSword");
                Tree[19].setTooltip("Slash your enemies with force of nature!");
                Tree[19].usable();
                Tree[19].setCooldownTimer(5);
                Tree[20].setTexture("ProjectAdvance:PoisonZone");
                Tree[20].setTooltip("Change your surroundings into deadly zone [50 mana][15 sec cooldown]");
                Tree[20].usable();
                Tree[20].setCooldownTimer(900);
                Tree[21].setTexture("ProjectAdvance:ForceOfNature");
                Tree[21].setTooltip("Bouncing projectile, heal your allies and hurt your foes![150 mana][30 sec cooldown] ");
                Tree[21].usable();
                Tree[21].setCooldownTimer(1800);

                foreach (KeyValuePair<int, Keys?> kvp in ModPlayer.Hotkeys)
                {
                    if (Tree[kvp.Key].isUsable())
                        Tree[kvp.Key].setHotkey(kvp.Value);
                    else
                        ModPlayer.Hotkeys.Remove(kvp.Key);
                }
            }
        }
        public void drawTree()
        {
            updateTree();
            ModPlayer.CManager.draw();
            ToggleTree.draw(sb);        
            if (ToggleTree.isChoosen())
            {
                Line.DrawLine(sb, new Vector2(Tree.ElementAt(0).getPosition().X + 20, Tree.ElementAt(0).getPosition().Y + 20), new Vector2(Tree.ElementAt(BRANCH_SIZE-2).getPosition().X + 80, Tree.ElementAt(BRANCH_SIZE-2).getPosition().Y + 20), Color.Purple, 5);
                Line.DrawLine(sb, new Vector2(Tree.ElementAt(0).getPosition().X + 20, Tree.ElementAt(0).getPosition().Y + 20), new Vector2(Tree.ElementAt(0).getPosition().X + 20, Tree.ElementAt(0).getPosition().Y + 180), Color.Purple, 5);
                Line.DrawLine(sb, new Vector2(Tree.ElementAt(0).getPosition().X + 20, Tree.ElementAt(0).getPosition().Y + 100), new Vector2(Tree.ElementAt(BRANCH_SIZE + 6).getPosition().X + 20, Tree.ElementAt(BRANCH_SIZE + 5).getPosition().Y + 20), Color.OrangeRed, 5);
                Line.DrawLine(sb, new Vector2(Tree.ElementAt(0).getPosition().X + 20, Tree.ElementAt(0).getPosition().Y + 180), new Vector2(Tree.ElementAt(BRANCH_SIZE + 13).getPosition().X + 20, Tree.ElementAt(BRANCH_SIZE + 13).getPosition().Y + 20), Color.LawnGreen, 5);
                for (int i = 0; i < (Tree.Count); i++)
                {
                    SkillSlot s = Tree.ElementAt(i);
                    s.draw(sb);
                }
                sb.DrawString(Main.fontMouseText, "Skill Points: " + Convert.ToString(ModPlayer.getSkillPoints()), new Vector2(ToggleTree.getPosition().X + 25, ToggleTree.getPosition().Y), Color.White);
            }
        }
        public SkillSlot getSkillAtPosition(int i)
        {
            if (i > -1 && i < Tree.Count)
                return Tree[i];
            return null;
        }
        public void updateTree()
        {
            if(Main.screenHeight!=screenHeight || Main.screenWidth !=screenWidth)
            {
                screenHeight = Main.screenHeight;
                screenWidth = Main.screenWidth;
                StartingPoint = new Vector2((screenWidth / 2) - FinalRowElements * 30, Main.screenHeight * 0.5f);
                for (int i = 0; i < BRANCH_SIZE; i++)
                {
                    Tree[i].setPosition(StartingPoint + new Vector2(i * 60, 0));               
                }
                for(int i=0;i<BRANCH_SIZE-1;i++)
                {
                    Tree[i+BRANCH_SIZE].setPosition(StartingPoint + new Vector2(i * 60+60, 80));
                    Tree[i+2*BRANCH_SIZE-1].setPosition(StartingPoint + new Vector2(i * 60+60, 160));
                }
            }
        }
    }
}
