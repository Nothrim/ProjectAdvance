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
    class SkillTree
    {
        #region variables
        private readonly int STANDARD_SKILL_TREE_PATH_SIZE=4;
        private int screenWidth = Main.screenWidth;
        private int screenHeight = Main.screenHeight;
        Vector2 StartingPoint;
        float FinalRowElements;
        bool open = false;
        ToggleElement ToggleTree;
        static readonly MPlayer player = (MPlayer)Main.localPlayer.GetSubClass<MPlayer>();
        List<SkillSlot> Tree=new List<SkillSlot>();
        SpriteBatch sb;
        #endregion 
        public SkillTree(SpriteBatch sb) {
        this.sb=sb;
        }
        public void buildSkillTree()
        {
            ToggleTree = new ToggleElement(new Vector2(470,50));
            FinalRowElements =(float) Math.Pow(2, STANDARD_SKILL_TREE_PATH_SIZE - 1);
            StartingPoint = new Vector2((Main.screenWidth/2) - FinalRowElements*30, Main.screenHeight * 0.75f);
            Main.NewText(((Main.screenWidth / 2) - (float)(Math.Pow(2, STANDARD_SKILL_TREE_PATH_SIZE) * 60)).ToString());
            for(int i=0;i<FinalRowElements;i++)
            {
                Tree.Add(new SkillSlot(StartingPoint+new Vector2(i*60,0), "ProjectAdvance:PlaceholderFrame", i));
                Tree.ElementAt(i).setChoosen(player.checkSkillAtPosition(i));
            }
            Tree[0].setTexture("ProjectAdvance:MagicElement");
            Tree[1].setTexture("ProjectAdvance:ManaShield");
            Tree[2].setTexture("ProjectAdvance:PowerSurge");
            Tree[3].setTexture("ProjectAdvance:Blink");
            Tree[3].usable();
            Tree[4].setTexture("ProjectAdvance:ManaBlast");
            Tree[4].usable();
        }
        public void drawTree()
        {
            updateTree();
            ToggleTree.draw(sb);
            
            if (ToggleTree.isChoosen())
            {
                Line.DrawLine(sb, new Vector2(Tree.ElementAt(0).getPosition().X + 20, Tree.ElementAt(0).getPosition().Y + 20), new Vector2(Tree.ElementAt(Tree.Count - 2).getPosition().X + 80, Tree.ElementAt(Tree.Count - 2).getPosition().Y + 20), Color.Blue, 3);
                for (int i = 0; i < (Tree.Count); i++)
                {
                    SkillSlot s = Tree.ElementAt(i);
                    s.draw(sb);
                }
                sb.DrawString(Main.fontMouseText, "Skill Points: " + Convert.ToString(player.getSkillPoints()), new Vector2(ToggleTree.getPosition().X + 25, ToggleTree.getPosition().Y), Color.White);
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
                StartingPoint = new Vector2((screenWidth / 2) - FinalRowElements * 30, Main.screenHeight * 0.75f);
                for (int i = 0; i < FinalRowElements; i++)
                {
                    Tree[i].setPosition(StartingPoint + new Vector2(i * 60, 0));
                }
            }
        }
    }
}
