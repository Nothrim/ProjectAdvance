using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TAPI;

namespace ProjectAdvance.Buffs
{
    class BurningBloodBuff : ModBuff
    {
        float Haste = 0.5f;
        static readonly float HASTE_LIMIT = 7f;
        public override void Effects(Player player, int index)
        {
         //   TConsole.Print("Step:"+player.stepSpeed+" movement:"+player.moveSpeed+" Run:"+player.maxRunSpeed+" Other"+player.moveSpeedMod+" Velocity"+player.velocity+" Other Data:"+player.runAcceleration);
            player.moveSpeed += Haste;
            player.moveSpeedMax = HASTE_LIMIT;
            player.meleeSpeed += Haste;
            base.Effects(player, index);
        }
        public override void DamageNPC(Projectile projectile, NPC npc, int hitDir, ref int damage, ref float knockback, ref bool crit, ref float critMult)
        {
            base.DamageNPC(projectile, npc, hitDir, ref damage, ref knockback, ref crit, ref critMult);
            if (Haste < HASTE_LIMIT)
            {
                Haste += 0.025f;
                Main.player[projectile.owner].buffTime[Main.player[projectile.owner].HasBuff(BuffDef.byName["ProjectAdvance:BurningBloodBuff"])] += 45;
            }
            //if (Math.Abs(Main.player[projectile.owner].velocity.X) < 8) Main.player[projectile.owner].velocity.X *=2f;



         
    
        }

    }
}
