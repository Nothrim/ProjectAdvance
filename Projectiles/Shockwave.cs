using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAPI;
using Terraria;
using Microsoft.Xna.Framework;
namespace ProjectAdvance.Projectiles
{
    class Shockwave : ModProjectile 
    {
        Vector2 velocity;
        int direction = 0;
        readonly int JUMPINES=3;//more=more jittering animation
        bool top;
        bool down;
        bool left;
        bool right;
        public override void Initialize()
        {
            projectile.tileCollide = false;
            velocity = projectile.velocity;
            direction = (int)Vector2.Normalize(velocity).X;
            base.Initialize();
        }
        public override void AI()
        {
            if(projectile.timeLeft%10==0)
            {
              if(++projectile.frame>4)
              {
                  projectile.frame = 1;
              }

            }
            int id = Dust.NewDust(projectile.getRect(), 45, Vector2.Zero, 0, Color.Cyan, 2f);
            Main.dust[id].velocity = Vector2.Zero;
            Main.dust[id].noGravity = true ;
            //Vector2 shift = projectile.center(); //doesnt work as intended
            //Point Position = projectile.center().ToTileCoordinates();
            //for (int i = 0; i < JUMPINES; i++)
            //{
            //    top = Main.tile[Position.X, Position.Y - 1].active();
            //    down = Main.tile[Position.X, Position.Y + 1].active();
            //    left = Main.tile[Position.X - 1, Position.Y].active();
            //    right = Main.tile[Position.X + 1, Position.Y].active();
            //    if (!top && !down && !left && !right)
            //    {
            //        shift.Y += 1;
            //    }
            //    else if (down && !top && !left && !right)
            //    {
            //        shift.X += direction;
            //    }
            //    else if (!down && !top && !left && right && direction > 0)
            //    {
            //        shift.Y += -1;
            //    }
            //    else if (!down && !top && !left && right && direction < 0)
            //    {
            //        shift.Y += 1;
            //    }
            //    else if (!down && !top && left && !right && direction > 0)
            //    {
            //        shift.Y += 1;
            //    }
            //    else if (!down && !top && left && !right && direction < 0)
            //    {
            //        shift.Y += -1;
            //    }
            //    else if (!down && top && !left && !right)
            //    {
            //        shift.X += -direction;
            //    }
            //    else if (!down && top && !left && right)
            //    {
            //        shift.X += -1;
            //    }
            //    else if (!down && top && left && !right)
            //    {
            //        shift.X += 1;
            //    }
            //    else
            //        shift.Y += 1;
            //    Position = shift.ToTileCoordinates();
            //    TConsole.Print(top + " " + down + " " + left + " " + right);





            //}
            //projectile.position = shift;
            Point CheckCoords = new Vector2(projectile.center().X + projectile.direction, projectile.center().Y).ToTileCoordinates();
            if (Main.tile[CheckCoords.X, CheckCoords.Y].active() && Main.tile[CheckCoords.X, CheckCoords.Y].type != 5)
            {
                int counter = 1;
                while (Main.tile[CheckCoords.X, CheckCoords.Y - counter].active())
                {
                    projectile.velocity.X = 0;
                    counter++;
                }
                projectile.position.Y -= counter;
                projectile.position.X += Vector2.Normalize(velocity).X;
            }
            else
            {

                CheckCoords = new Vector2(projectile.center().X, projectile.center().Y + 1).ToTileCoordinates();
                if (!Main.tile[CheckCoords.X, CheckCoords.Y].active())
                {

                    int counter = 1;
                    while (!Main.tile[CheckCoords.X, CheckCoords.Y + counter].active())
                    {
                        counter++;
                        projectile.velocity.X = 0;
                    }
                    projectile.position.Y += counter;
                    projectile.position.X += Vector2.Normalize(velocity).X;


                }
                else
                    projectile.velocity = velocity;
            }


                base.AI();
        }
    }
}
