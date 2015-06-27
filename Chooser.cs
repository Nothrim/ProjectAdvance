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
    class Chooser
    {
        short ChoosenElement = -1;
        MPlayer player;
        public short getChoosenElement() { return ChoosenElement; }
        SpriteBatch sb;
        List<ChooserElement> Elements=new List<ChooserElement>();
        public Chooser(SpriteBatch sb, MPlayer player) { this.sb = sb; this.player = player; }
        public void draw() 
        {
            foreach(ChooserElement ch in Elements)
            {
                ch.draw(sb);
            }
        }
        public void addElement(Vector2 position, String texture, short ID,string description) { Elements.Add(new ChooserElement(position, texture, ID, player,description)); }
    }
}
