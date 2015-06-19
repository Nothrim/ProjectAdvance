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
    class Cooldown 
    {
        private Vector2 position;
        SpriteBatch sb;
        
        private Dictionary<String, int> CooldownDictionary=new Dictionary<String,int>();
        public Cooldown(Vector2 position, SpriteBatch sb) { this.position = position; this.sb = sb; }
        public void add(String key) { 
            if(!CooldownDictionary.ContainsKey(key))
            CooldownDictionary.Add(key, 0); 
        }
        public void update(String key, int value) 
        {
            if(CooldownDictionary.ContainsKey(key))
            {
                CooldownDictionary[key] = value;
            }
        }
        public void Draw()
        {
            foreach (String key in CooldownDictionary.Keys)
            {
                sb.DrawString(Main.fontMouseText, key, position, Color.White);
            }

        }
        public bool contains(String key)
        {
            return CooldownDictionary.ContainsKey(key);
        }

    }
}
