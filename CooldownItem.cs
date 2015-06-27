using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using TAPI;


namespace ProjectAdvance
{
    class CooldownItem
    {
        string name;
        int id;
        Texture2D texture;
        static readonly int RECTANGLE_SIZE=25;
        Rectangle CooldownFrame;
        int cooldown;
        int CooldownTimer = 0;
        static SpriteBatch sb=null;
        Vector2 TextPosition;
        bool active = true;
        public static void addSpriteBatch(SpriteBatch sprite_batch){ sb = sprite_batch;}
        public CooldownItem(int id, string name, string texture_name, int cooldown, Vector2 position) 
        {
            this.id = id;
            this.name = name;
            this.texture = Main.goreTexture[GoreDef.gores[texture_name]];
            this.cooldown = cooldown;
            TextPosition = new Vector2(position.X + 30, position.Y);
            CooldownFrame = new Rectangle((int)position.X, (int)position.Y, RECTANGLE_SIZE, RECTANGLE_SIZE);
        }
        public void Draw()
        {
            if (sb != null) 
            {
                sb.Draw(texture, CooldownFrame, Color.White);

                if (CooldownTimer !=0)
                {
                    sb.DrawString(Main.fontMouseText, CooldownTimer/60+"/"+cooldown/60+" s", TextPosition, Color.White);
                }
                else
                {
                    sb.DrawString(Main.fontMouseText, "READY", TextPosition, Color.White);
                }
            }
        }
        public void increment() 
        {
            if (CooldownTimer < cooldown && !active)
                CooldownTimer++;
            else
            {
                CooldownTimer = 0;
                active = true;
            }

        }
        public void transformPosition(Vector2 transform_position_vector)
        {
            TextPosition.X = transform_position_vector.X+30;
            TextPosition.Y = transform_position_vector.Y;
            CooldownFrame.X = (int)transform_position_vector.X;
            CooldownFrame.Y = (int)transform_position_vector.Y;
        }
        public void setActive(bool value) { active = value; }
        public bool isActive() { return active; }
        public int getCooldownTimer() { return CooldownTimer; }

    }
}
