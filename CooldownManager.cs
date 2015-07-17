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
    class CooldownManager 
    {
        Vector2 anchor;
        static readonly int WIDTH = 100;
        Dictionary<int, CooldownItem> Cooldowns = new Dictionary<int, CooldownItem>();
        SpriteBatch sb;
        public CooldownManager(Vector2 frame_position, SpriteBatch sb) { anchor = frame_position; this.sb = sb; CooldownItem.addSpriteBatch(sb); }
        public void addCooldown(int id, string name, string texture,int cooldown_time) 
        {
            if(!Cooldowns.ContainsKey(id))
            {
                Cooldowns.Add(id, new CooldownItem(id, name, texture, cooldown_time, new Vector2(anchor.X, anchor.Y + Cooldowns.Count * 17)));
            }
        }
        public void setCooldown(int id,int time)
        {
            if (Cooldowns.ContainsKey(id))
                Cooldowns[id].setCooldown(time);
        }
        public void draw() 
        {
            if (Cooldowns.Count > 0)
            {
                int frame_height = 0;
                foreach (KeyValuePair<int, CooldownItem> kvp in Cooldowns)
                {
                    if (!kvp.Value.isActive()) frame_height++;
                }
                if (frame_height > 0)
                {
                    int counter = 0;
                    float x_pos = (Main.screenWidth - WIDTH);
                    Drawing.DrawBox(sb, x_pos, Main.screenHeight - 37 * frame_height, WIDTH, frame_height * 40, Color.LightSeaGreen);

                    foreach (KeyValuePair<int, CooldownItem> kvp in Cooldowns)
                    {
                        if (!kvp.Value.isActive())
                        {
                            kvp.Value.transformPosition(new Vector2(x_pos - 10, (Main.screenHeight - 30 * ++counter)));
                            kvp.Value.Draw();
                            kvp.Value.increment();
                        }
                    }

                }
            }
        }
        public void increment()
        {
            if (Cooldowns.Count > 0)
            {
                foreach (KeyValuePair<int, CooldownItem> kvp in Cooldowns)
                {
                    kvp.Value.increment();
                }
            }
        }
        public void useSkill(int id)
        {
            if(Cooldowns.ContainsKey(id))
            {
                if(Cooldowns[id].isActive())
                Cooldowns[id].setActive(false);
            }
        }
        public void debugContent()
        {
            foreach (KeyValuePair<int, CooldownItem> kvp in Cooldowns)
            {
                Main.NewText("Key " + kvp.Key + " Value:" + kvp.Value);
            }
        }
        public bool isUsable(int id)
        {
            if (Cooldowns.ContainsKey(id))
            {
                return Cooldowns[id].isActive();
            }
            return false;
        }
        public int getTime(int id)
        {
            if(Cooldowns.ContainsKey(id))
            {
                return Cooldowns[id].getCooldownTimer();
            }
            return -1;
        }
        
         
    }
}
