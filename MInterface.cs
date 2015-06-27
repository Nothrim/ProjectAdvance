
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;
namespace ProjectAdvance
{
    class MInterface : ModInterface
    {
        MPlayer player = null;
        bool PathChoosen = false;
        bool ChooserInitialized = false;
        public bool isInitialized() { return initialized; }
        public void updateMPlayer(MPlayer m)
        {
            ChooserInitialized = false;
            PathChoosen = false;
            initialized = false;
            player = m;
            tree.updateMPlayer(m);

        }
        public void setupField(int i,bool value)
        {
            if (tree != null)
            {
                SkillSlot temp = tree.getSkillAtPosition(i);
                if (temp != null)
                {
                    temp.setChoosen(value);
                }
            }
            else
                Main.NewText("Tree=null");
        }
        public static SkillTree ChoosenTree=null;
        SkillTree tree;
        Chooser ChPath;
        bool initialized = false;
        void Initialize(SpriteBatch sb)
        {
           tree = new SkillTree( sb);
           ChoosenTree = tree;
           tree.buildSkillTree();
           initialized = true;
        }
        public override bool PreDrawInterface(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            if (!initialized) Initialize(sb);
            if (player != null)
            {
                if (player.getPath() > 0)
                {
                    if (!PathChoosen)
                    {
                        tree.loadSlotsData();
                        PathChoosen = true;
                    }
                    tree.drawTree();
                }
                else
                {
                    if(!ChooserInitialized)
                    {
                        ChPath = new Chooser(sb, player);
                        ChPath.addElement(new Vector2((Main.screenWidth / 2) - 80, Main.screenHeight / 2), "ProjectAdvance:WarriorPath", 1,"Warrior");
                        ChPath.addElement(new Vector2((Main.screenWidth / 2) , Main.screenHeight / 2), "ProjectAdvance:MagePath", 2,"Mage");
                        ChPath.addElement(new Vector2((Main.screenWidth / 2) + 80, Main.screenHeight / 2), "ProjectAdvance:RangePath", 3,"Ranged");
                        ChooserInitialized = true;
                    }
                    ChPath.draw();
                }
            }
           
            return base.PreDrawInterface(sb);
        }
    }
}
