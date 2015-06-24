
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
        public bool isInitialized() { return initialized; }
        public void updateMPlayer(MPlayer m)
        {
           
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
            tree.drawTree();
            return base.PreDrawInterface(sb);
        }
    }
}
