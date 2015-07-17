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
    class SkillSlot 
    {
        MPlayer player = (MPlayer)Main.localPlayer.GetSubClass<MPlayer>();
//graphic variables-----------------------------------------
        private Vector2 position;
        private readonly int STANDARD_SIZE=40;
        private String ImageName = "";
        private String name="";
        Texture2D SkillImage;
        private Rectangle SkillSlotSurface;
//mechanic variables-------------------------------------------
        bool Chosen = false;
        int SkillId;
        bool Usable = false;
        Keys? Hotkey = null;
        String Tooltip = "";
        int CooldownTimer = 0;
//---------------------------------------------------
    
        public SkillSlot(Vector2 position,String ImageName,int SkillId) {
            this.position=position;
            this.ImageName = ImageName;
            SkillSlotSurface = new Rectangle((int)position.X, (int)position.Y, STANDARD_SIZE, STANDARD_SIZE);
            SkillImage = Main.goreTexture[GoreDef.gores[ImageName]];

         
            this.SkillId = SkillId;
        }
        public void updateMPlayer(MPlayer m) { player = m; }
        public void setHotkey(Keys? hotkey) 
        {
            Hotkey = hotkey; 
        }
        public void setTooltip(String Tooltip) { this.Tooltip = Tooltip; }
        public void usable() { Usable = true; }
        public bool isUsable() { return Usable; }
        public void setPosition(Vector2 position){
            this.position=position;
            SkillSlotSurface.X = (int)position.X;
            SkillSlotSurface.Y = (int)position.Y;
        }

        public void setTexture(string TexturePath) 
        { 
            ImageName = TexturePath;
            name = ImageName.Split(':')[1];
            SkillImage = Main.goreTexture[GoreDef.gores[ImageName]];
        }
        public void setCooldownTimer(int time) { CooldownTimer = time; }

        public void draw(SpriteBatch sb)
        {
            if (SkillSlotSurface.Contains(Main.mouse))
            {
                player.player.mouseInterface = true;
                player.cantUse();
                if (Tooltip != "")
                {
                    sb.DrawString(Main.fontMouseText, name, new Vector2(position.X, position.Y + STANDARD_SIZE), Color.Purple);
                    sb.DrawString(Main.fontMouseText, Tooltip, new Vector2(position.X, position.Y + STANDARD_SIZE + 20), Color.LightGray);
                }
                if (Chosen && Usable && Keyboard.GetState().GetPressedKeys().Length > 0)
                {
                    if (Keyboard.GetState().GetPressedKeys()[0] == Keys.Back)
                    {

                        //player.Hotkeys.Remove(getID());-didnt really work well with my save system
                        player.Hotkeys[getID()] = Keys.Help; //something doesnt work? Hack the $@#% Ha! what fool has a help key Kappa ? :D
                        Hotkey = null;
                    }
                    else
                    {
                        if (player.Hotkeys.ContainsKey(getID())) player.Hotkeys[getID()] = Keyboard.GetState().GetPressedKeys()[0];
                        else
                            player.Hotkeys.Add(getID(), Keyboard.GetState().GetPressedKeys()[0]);
                        Hotkey = Keyboard.GetState().GetPressedKeys()[0];
                    }

                }
                if (SkillSlotSurface.Contains(Main.mouse) && Main.mouseLeft)
                {
                    sb.Draw(SkillImage, SkillSlotSurface, Color.Peru);
                    if (Main.mouseLeftRelease)
                    {
                        if (!Chosen)
                        {
                            if (player.checkPreviousSkill(getID()) && player.getSkillPoints() > 0)
                            {
                                player.setSkill(getID());
                                if (CooldownTimer != 0)
                                {
                                    player.setCooldown(getID(), CooldownTimer, ImageName, ImageName);
                                }
                                Chosen = true;
                            }
                            else
                            {
                                Main.PlaySound("ProjectAdvance:beep", Main.localPlayer.position.X, Main.localPlayer.position.Y);
                            }
                        }
                    }
                }
                //temporary dev funcionality to clear and test

                else
                    sb.Draw(SkillImage, SkillSlotSurface, Color.Orange);
            }

            else
            {
                if (Chosen)
                    sb.Draw(SkillImage, SkillSlotSurface, Color.White);
                else
                    sb.Draw(SkillImage, SkillSlotSurface, Color.Gray);

            }
            if (Usable)
            {
                if (Hotkey != null && Hotkey!=Keys.Help)//Hack it Kappa
                {
                    sb.DrawString(Main.fontMouseText, Hotkey.ToString(), position, Color.LightYellow);
                }
                else
                    sb.DrawString(Main.fontMouseText, "-", position, Color.LightYellow);
            }
        }

        public bool isChoosen() { return Chosen; }
        public void setChoosen(bool chosen) { Chosen = chosen; }
        public int getID() { return SkillId; }
        public Vector2 getPosition() { return position; }
    }
}