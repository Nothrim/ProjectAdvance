using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace ProjectAdvance
{
    class Crafting
    {
        List<CraftElement> CraftElements = new List<CraftElement>();
        public void Initialize(Player p)
        {
            CraftElements.Add(new CraftElement(10,0,"Shuriken",42,p,100));
            CraftElements.Add(new CraftElement(50,0,"ThrowingKnife",279,p,50));
            CraftElements.Add(new CraftElement(90,0,"PoisonedKnife",287,p,10));
            CraftElements.Add(new CraftElement(130,0,"SpikyBall",161,p,25));
            CraftElements.Add(new CraftElement(10,60,"Grenade",168,p,10));
            CraftElements.Add(new CraftElement(50,60,"StickyGrenade",2586,p,5));
            CraftElements.Add(new CraftElement(90,60,"Bomb",166,p,2));
        }
        public void Update()
        {
            CraftElement.setAnchor(new Vector2(Main.screenWidth*0.45f, Main.screenHeight * 0.6f));
        }
        public void Draw(SpriteBatch sb)
        {
            Drawing.DrawBox(sb, Main.screenWidth * 0.45f - 10, Main.screenHeight * 0.6f-10, 130, 50);
            for(int i=0;i<4;i++)
            {
               CraftElements[i].Draw(sb);
            }
        }
        public void DrawAll(SpriteBatch sb)
        {
             Drawing.DrawBox(sb,Main.screenWidth * 0.45f-10, Main.screenHeight * 0.6f-10,190,110,Color.DarkOrange);
            foreach (CraftElement c in CraftElements)
            {
                c.Draw(sb);
            }
        }
    }
}
