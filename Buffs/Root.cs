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
    class Root : ModBuff
    {
        Vector2 BoundingPoint;
        float distance;
        public void setBoundingPoint(Vector2 point) { BoundingPoint = point; }
        int timer = 0;
        public override void Effects(NPC npc, int index)
        {
            if(timer++>30)
            {
           
                    timer = 0;
                //Main.projectile[Projectile.NewProjectile((BoundingPoint), new Vector2(0, 0), ProjDef.byName["ProjectAdvance:BindingRoot"].type, 10, 0)].GetSubClass<Projectiles.BindingRoot>().setTarget(npc);
                if((distance=npc.Distance(BoundingPoint))>40)
                {
                       Main.dust[ Dust.NewDust(npc.position,new Vector2(5,5),15,Vector2.Zero,0,Color.Green,3f)].velocity=Vector2.Multiply(Vector2.Normalize(Vector2.Subtract(BoundingPoint,npc.position)),3);
                    npc.velocity = Vector2.Multiply(Vector2.Normalize(new Vector2(BoundingPoint.X - npc.position.X, BoundingPoint.Y - npc.position.Y)), distance / 20);
                }
            }
            base.Effects(npc, index);
        }
    }
}
