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
        private readonly int STANDARD_SKILL_TREE_PATH_SIZE=4;
        private int screenWidth = Main.screenWidth;
        private int screenHeight = Main.screenHeight;
        Vector2 StartingPoint;
        float FinalRowElements;
        bool open = false;
        ToggleElement ToggleTree;
      List<SkillSlot> Tree=new List<SkillSlot>();
       //temporary shut down ModPlayer player;
        SpriteBatch sb;
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
            }
            for(int i=0;i<4;i++)
            {

            }
            
              
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
            }
        }
        public void updateTree()
        {
            if(Main.screenHeight!=screenHeight || Main.screenWidth !=screenWidth)
            {
                screenHeight = Main.screenHeight;
                screenWidth = Main.screenWidth;
                StartingPoint = new Vector2((screenWidth / 2) - FinalRowElements * 30, Main.screenHeight * 0.75f);
                //ToggleTree.setPosition(new Vector2(300,screenHeight*0.03f));
                for (int i = 0; i < FinalRowElements; i++)
                {

                    Tree[i].setPosition(StartingPoint + new Vector2(i * 60, 0));
                }
            }
        }
        
       
    
    }
}
