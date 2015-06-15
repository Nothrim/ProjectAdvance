
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
