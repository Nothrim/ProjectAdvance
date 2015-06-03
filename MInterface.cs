
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
        SkillTree tree;
        SkillTreeImplementation skilltree;
        Tree testree;
        
        bool initialized = false;
        void Initialize(SpriteBatch sb)
        {
            //not really working right now
            //skilltree = new SkillTreeImplementation();
            //skilltree.generateTree(3);
            //skilltree.buildTree(skilltree.MAIN_ROOT);
            tree = new SkillTree( sb);
           tree.buildSkillTree();
            initialized = true;
            testree = new Tree();
           
            testree.createTree(4);

            
        }
        //static ItemSlot i = new ItemSlot(new ModBase(), "Slot", 0, i.ActionSet, i.ActionGet);
        public override bool PreDrawInterface(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            if (!initialized) Initialize(sb);
            tree.drawTree();
            //testree.setupNodes(testree.root);
            //skilltree.drawTree(skilltree.MAIN_ROOT,sb);
            return base.PreDrawInterface(sb);
        }
       
    }
}
