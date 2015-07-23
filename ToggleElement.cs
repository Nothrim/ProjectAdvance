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
    class ToggleElement
    {
        #region variables
        //graphic variables-----------------------------------------
        private Vector2 position;
        private readonly int STANDARD_SIZE=20;
        private String ImageName = "";
        Texture2D SkillImage;
        private Rectangle SkillSlotSurface;
//mechanic variables-------------------------------------------
        bool Chosen = false;
        #endregion

        public ToggleElement(Vector2 position) {
            this.position=position;
            SkillSlotSurface = new Rectangle((int)position.X, (int)position.Y, STANDARD_SIZE, STANDARD_SIZE);
            SkillImage = Main.goreTexture[GoreDef.gores["ProjectAdvance:ToggleElement0"]];
        }
        
        public void setPosition(Vector2 position){
            this.position=position;
            SkillSlotSurface.X = (int)position.X;
            SkillSlotSurface.Y = (int)position.Y;
        }
        #region draw method
        public void draw(SpriteBatch sb)
        {
            if(isChoosen())
                SkillImage=Main.goreTexture[GoreDef.gores["ProjectAdvance:ToggleElement1"]];
            else
                SkillImage=Main.goreTexture[GoreDef.gores["ProjectAdvance:ToggleElement0"]];
            if (SkillSlotSurface.Contains(Main.mouse))
            {
                Main.localPlayer.mouseInterface = true;
                if (SkillSlotSurface.Contains(Main.mouse) && Main.mouseLeft)
                {
                    sb.Draw(SkillImage, SkillSlotSurface, Color.Peru);
                    if (Main.mouseLeftRelease)
                    {
                        if (isChoosen()) 
                            setChoosen(false);
                        else
                            setChoosen(true);
                    }
                }
                else
                    sb.Draw(SkillImage, SkillSlotSurface, Color.Orange);
            }
            else
                sb.Draw(SkillImage, SkillSlotSurface, Color.White);
        }
        #endregion
        public bool isChoosen() { return Chosen; }
        public void setChoosen(bool chosen) { Chosen = chosen; }
        public Vector2 getPosition() { return position; }
    }
}
