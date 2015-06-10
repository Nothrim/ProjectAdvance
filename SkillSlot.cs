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
    class SkillSlot 
    {
        static readonly MPlayer player = (MPlayer)Main.localPlayer.GetSubClass<MPlayer>();
//graphic variables-----------------------------------------
        private Vector2 position;
        private readonly int STANDARD_SIZE=40;
        private String ImageName = "";
        Texture2D SkillImage;
        private Rectangle SkillSlotSurface;
//mechanic variables-------------------------------------------
        bool Chosen = false;
        int SkillId;
//---------------------------------------------------
    
        public SkillSlot(Vector2 position,String ImageName,int SkillId) {
            this.position=position;
            this.ImageName = ImageName;
            SkillSlotSurface = new Rectangle((int)position.X, (int)position.Y, STANDARD_SIZE, STANDARD_SIZE);
            SkillImage = Main.goreTexture[GoreDef.gores[ImageName]];
         
            this.SkillId = SkillId;
        }

        public void setPosition(Vector2 position){
            this.position=position;
            SkillSlotSurface.X = (int)position.X;
            SkillSlotSurface.Y = (int)position.Y;
        }

        public void draw(SpriteBatch sb)
        {
            if (SkillSlotSurface.Contains(Main.mouse))
            {
                if (SkillSlotSurface.Contains(Main.mouse) && Main.mouseLeft)
                {
                    sb.Draw(SkillImage, SkillSlotSurface, Color.Peru);
                    if (Main.mouseLeftRelease)
                    {
                        if (!Chosen)
                        {
                            if (player.checkPreviousSkill(getID()))
                            {
                                player.setSkill(getID());
                                Chosen = true;
                            }
                            else
                            {
                                Main.PlaySound("ProjectAdvance:beep", Main.localPlayer.position.X, Main.localPlayer.position.Y);
                            }
                        }
                    }
                }
                else
                    sb.Draw(SkillImage, SkillSlotSurface, Color.Orange);
            }
            else
                if (Chosen) 
                    sb.Draw(SkillImage, SkillSlotSurface, Color.White);
                else
                    sb.Draw(SkillImage, SkillSlotSurface, Color.Gray);
        }

        public bool isChoosen() { return Chosen; }
        public void setChoosen(bool chosen) { Chosen = chosen; }
        public int getID() { return SkillId; }
        public Vector2 getPosition() { return position; }
    }
}
