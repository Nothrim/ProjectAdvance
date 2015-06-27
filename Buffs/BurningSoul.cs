using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;
using Microsoft.Xna.Framework;

namespace ProjectAdvance.Buffs
{
    class BurningSoul : ModBuff
    {
        int owner = 255;
        int Stack = 0;
        int DrawingCounter = 0;
        public override void Effects(NPC npc, int index)
        {
            if ((Stack>2) && DrawingCounter++ == 60)
            {
                DrawingCounter = 0;
                CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y - npc.height, 10, 10), Color.Violet, Stack.ToString());
            }
            base.Effects(npc, index);
        }
        public override void DealtNPC(NPC npc, Projectile projectile, int hitDir, int dmgDealt, float knockback, bool crit)
        {
            if(owner==255)
            { owner = projectile.owner; }
            if(projectile.magic)
            Stack++;
            base.DealtNPC(npc, projectile, hitDir, dmgDealt, knockback, crit);
        }
        public override void End(NPC npc, int index)
        {
            if(Stack>2)
            Main.projectile[Projectile.NewProjectile(npc.position, new Vector2(0, 0),30, 10 * Stack, 1,owner)].timeLeft=1;
            base.End(npc, index);
        }
    }
}
