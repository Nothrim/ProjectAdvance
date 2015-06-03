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
    public class Tree
    {
        static int testcount = 0;
        public Node root = new Node();

        private void setRoot(Node root) { this.root = root; }

        Node createNode(int size, Node parent)
        {
            if (size <= 0) return null;
            Node node = new Node(parent);
            node.Left = createNode(size - 1, node);
            node.Right = createNode(size - 1, node);
            return node;
        }

        public void createTree(int branches)
        {
            setRoot(createNode(branches, null));
        }

        public void setupNodes(Node node)
        { // Each child of a tree is a root of its subtree.
            if (node.Left != null)
            {
                setupNodes(node.Left);
            }
            node.Id = testcount++;
            Main.NewText(node.Id.ToString());
            if (node.Right != null)
                setupNodes(node.Right);
        }

    }
    public class Node
    {
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
        //tree variables
        private Node left;
        private Node previous;
        private Node right;
        public Node Left { get { return left; } set { left = value; } }
        public Node Right { get { return right; } set { right = value; } }
        public Node Previous { get { return previous; } set { previous = value; } }
        public int Id { get { return id; } set { id = value; } }
        public Node() { previous = null; left = null; right = null; }
        public Node(Node parent) { previous = parent; }
        public void setupNode(int id,Vector2 position,String texturePath)
        {

        }

    }
}
