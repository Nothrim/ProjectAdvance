
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
    class SkillTreeImplementation
    {
      
        public class Skill
        {
            //Tree traversing variables
            private Skill parent;
            private Skill left;
            private Skill right;
            //Data variables
            private int id;
            private bool active;
            //Grapic variables
            private Vector2 position;
            private readonly int STANDARD_SIZE = 40;
            Texture2D SkillImage;
            private Rectangle SkillSlotSurface;
            private int Width;
            private int Height;
            //getters
            public int getId() { return id; }
            public Skill Left() { return left; }
            public Skill Right() { return right; }
            public Skill Previous() { return parent; }
            //setter
            public void setLeft(Skill l) { left = l; }
            public void setRight(Skill r) { right = r; }
            //basic tree constructor
            public Skill() { this.parent = null; }
            public Skill(Skill parent)
            {
                this.parent = parent;
                if (parent.left == null) { parent.left = this; }
                else if (parent.right == null) { parent.right = this; }
                else { this.parent = null; }
            }
            //filling with data
            public void setup(int id, Vector2 position, String TexturePath)
            {
                this.id = id;
                this.position = position;
                SkillSlotSurface = new Rectangle((int)position.X, (int)position.Y, STANDARD_SIZE, STANDARD_SIZE);
                SkillImage = Main.goreTexture[GoreDef.gores[TexturePath]];
                Width = Main.screenWidth;
                Height = Main.screenHeight;
            }
            public void update(Vector2 position)
            {
                if (Width != Main.screenWidth || Height != Main.screenHeight)
                {
                    Width = Main.screenWidth;
                    Height = Main.screenHeight;
                    this.position = position;
                    SkillSlotSurface.X = (int)position.X;
                    SkillSlotSurface.Y = (int)position.Y;
                }
            }
            public void draw(SpriteBatch sb)
            {
                if (SkillSlotSurface.Contains(Main.mouse))
                    sb.Draw(SkillImage, SkillSlotSurface, Color.Orange);
                else
                    sb.Draw(SkillImage, SkillSlotSurface, Color.White);


            }
            public int onClick()
            {
                if (SkillSlotSurface.Contains(Main.mouse) && Main.mouseLeft && Main.mouseLeftRelease) return id;
                return -1;
            }

        }
   
        int id = 0;
        private readonly int STANDARD_SKILL_TREE_PATH_SIZE = 4;
        public Skill MAIN_ROOT;
        public SkillTreeImplementation() { MAIN_ROOT = new Skill(); }
       public  Skill generateNode(int size, Skill parent)
        {
            if (size <= 0) return null;
            Skill Node = new Skill(parent);
                parent = Node;
                Node.setLeft(generateNode(size - 1, parent));
                Node.setRight(generateNode(size - 1, parent));
            return Node;
        }
        public void generateTree(int size)
        {
            Skill root = new Skill();
            this.MAIN_ROOT = root;
            generateNode(size, root);


        }
        public void drawTree(Skill node, SpriteBatch sb)
        {
            if (node.Left() != null)
            {
                drawTree(node.Left(), sb);
            }
            //code for node action
            node.draw(sb);
            //-----
            if (node.Right() != null)
            {
                drawTree(node.Right(), sb);
            }
        }

        public void buildTree(Skill node)
        {
            int counter = 0;
            node.setup(counter++, new Vector2(Main.screenWidth / 2, (Main.screenHeight + 10 * counter) / 2), "ProjectAdvance:PlaceholderFrame");
            Queue<Skill> q = new Queue<Skill>();
            q.Enqueue(node);
          
            while (q.Peek()!=null || counter >50)
            {
                counter++;
                node = q.Dequeue();
                Main.NewText("initialising"+node.getId());
                //node.setup(counter++, new Vector2(Main.screenWidth / 2, (Main.screenHeight + 10 * counter) / 2), "ProjectAdvance:PlaceholderFrame");
                if (node.Left() != null) q.Enqueue(node.Left());
                if (node.Right() != null) q.Enqueue(node.Right());
            }

        }
    }
}
