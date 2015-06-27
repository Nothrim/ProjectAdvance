using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    class ChooserElement
    {
        //graphic variables-----------------------------------------
        private Vector2 position;
        private readonly int STANDARD_SIZE=40;
        private String ImageName = "";
        Texture2D SkillImage;
        private Rectangle SkillSlotSurface;
        String description = "";
//mechanic variables-------------------------------------------
        short ElementId;
        MPlayer player;
//---------------------------------------------------
    
        public ChooserElement(Vector2 position,String ImageName,short ElementId,MPlayer player,String description) {
            this.position=position;
            this.ImageName = ImageName;
            SkillSlotSurface = new Rectangle((int)position.X, (int)position.Y, STANDARD_SIZE, STANDARD_SIZE);
            SkillImage = Main.goreTexture[GoreDef.gores[ImageName]];
            this.ElementId = ElementId;
            this.player = player;
            this.description = description;
        }
        public void setPosition(Vector2 position){
            this.position=position;
            SkillSlotSurface.X = (int)position.X;
            SkillSlotSurface.Y = (int)position.Y;
        }

        public void setTexture(string TexturePath) 
        { 
            ImageName = TexturePath;
            SkillImage = Main.goreTexture[GoreDef.gores[ImageName]];
        }
        public void draw(SpriteBatch sb)
        {

            if (SkillSlotSurface.Contains(Main.mouse))
            {
                player.player.mouseInterface = true;
                if (SkillSlotSurface.Contains(Main.mouse) && Main.mouseLeft)
                {
                    sb.Draw(SkillImage, SkillSlotSurface, Color.Peru);
                    if (Main.mouseLeftRelease)
                    {
                        player.setPath(ElementId);
                    }
                }
                else
                    sb.Draw(SkillImage, SkillSlotSurface, Color.Orange);
            }
            else
                sb.Draw(SkillImage, SkillSlotSurface, Color.White);
            if (description != "") { sb.DrawString(Main.fontMouseText, description, new Vector2(position.X,position.Y+45), Color.LightCoral); }

        }
        public Vector2 getPosition() { return position; }
    }
}
