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
    class CraftElement
    {
        static Vector2 AnchorPoint;
        public static void setAnchor(Vector2 anchor)
        {
            AnchorPoint = anchor;

        }
        String TextureName;
        Texture2D Texture;
        Rectangle Element;
        int CraftingID;
        int ShiftX;
        int ShiftY;
        int CraftingTimer=0;
        Player Craftsman;
        int amount;
        String name;
        public CraftElement(int ShiftX,int ShiftY,String TexturePath,int CraftingID,Player p,int amount)
        {
            this.amount = amount;
            Craftsman = p;
            this.ShiftX = ShiftX;
            this.ShiftY = ShiftY;
            TextureName="ProjectAdvance:"+TexturePath;
            Texture = Main.goreTexture[GoreDef.gores[TextureName]];
            name = TexturePath;
            this.CraftingID = CraftingID;
            Element = new Rectangle((int)AnchorPoint.X + ShiftX, (int)AnchorPoint.Y + ShiftY, 30, 30);
        }
        public void Draw(SpriteBatch sb)
        {
            Element.X = (int)AnchorPoint.X + ShiftX;
            Element.Y = (int)AnchorPoint.Y + ShiftY;
            if(CraftingTimer>0)
            {
                if(CraftingTimer<120)
                {
                    Vector2 Start=new Vector2((Main.screenWidth/2) -60,Main.screenHeight/2+60);
                    Vector2 End = new Vector2((Main.screenWidth / 2) + 60, Main.screenHeight / 2 + 60);
                    Line.DrawLine(sb,Start,End,Color.DarkGreen,7);
                        Line.DrawLine(sb, Start, Vector2.Lerp(Start, End, CraftingTimer / 120f), Color.GreenYellow, 7);
                    CraftingTimer++;
                }
                else
                {
                    Item.NewItem(Craftsman.getRect(), CraftingID, amount);
                    CraftingTimer = 0;
                }
            }
            if (Element.Contains(Main.mouse))
            {
                
                Craftsman.mouseInterface = true;

                    sb.DrawString(Main.fontMouseText, name, new Vector2(AnchorPoint.X+ShiftX, AnchorPoint.Y+ShiftY + 30), Color.White);
                if (Element.Contains(Main.mouse) && Main.mouseLeft)
                {
                   
                    sb.Draw(Texture, Element, Color.Peru);
                    if (Main.mouseLeftRelease)
                    {
                        CraftingTimer=1;
                       
                    }
                }
                else
                    sb.Draw(Texture, Element, Color.Orange);
            }

            else
            {
                    sb.Draw(Texture, Element, Color.White);
            }
       
        }
        
    }
}
