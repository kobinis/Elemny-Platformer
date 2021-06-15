
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PaintPlay;
using SolarConflict.Games.PixelGardenGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Activities.Games
{
    public class SlimeEnemy : GameObject
    {
        //public Vector2 Position;
        //public Vector2 Velocity;

        public Spritesheet texture;

        public int SizeX = 5;
        public int SizeY = 10;

        public bool isOnGround;

        int cooldown = 0;
        int weaponMode = 1;

        public override float Mass { get; set; }
        public override string Name { get; set; }

        public override string Tag => "Player";

        public override CollisionSpec CollisionInfo { get; set; }

        public SlimeEnemy()
        {

            CollisionInfo = new CollisionSpec(2, 1);
            Mass = 1;
            texture = new Spritesheet("characters_packed", 24, 24, 18);
            Size = 5;
        }



        public override void Update(GameEngine gameEngine)
        {
            PixelGardenEngine grid = PixelGardenEngine.Inst;


            int maxSpeed = 2;
            float acc = 0.25f;
            var player = gameEngine.player;

            float diff = (player.Position.X - Position.X)/200f;
            


            if (FMath.Bern(diff, FMath.Rand))
            {
                Velocity += Vector2.UnitX * acc;
                if (Velocity.X > maxSpeed)
                    Velocity.X = maxSpeed;
            }

            if (FMath.Bern(-diff, FMath.Rand))
            {
                Velocity -= Vector2.UnitX * acc;
                if (Velocity.X < -maxSpeed)
                    Velocity.X = -maxSpeed;
            }

            if (isOnGround)
            {
                Velocity = -Vector2.UnitY * FMath.Rand.Next(5, 8);
            }

            
            
            


            ////  int cc = CheckCollision(grid);
            //GPixel dpixel = new GPixel();
            //dpixel.type = PixelType.Object;
            //dpixel.value = 3;
            //dpixel.color = Color.Red;
            //for (int dx = -2; dx < 2; dx++)
            //{
            //    if (grid.GetPixelLim((int)Position.X + dx, (int)Position.Y - 3).IsEmpty())
            //        grid.SetPixelLim((int)Position.X + dx, (int)Position.Y - 3, dpixel);
            //}

            MoveX(grid, null);
            MoveY(grid, null);
            Velocity += Vector2.UnitY * 0.2f;
            Velocity *= 0.98f;
            Velocity.X = Velocity.X * 0.8f;
        }

        //public 

        public void MoveX(PixelGardenEngine grid, Action onCollide)
        {
            //xRemainder += amount;
            Point pos = Position.ToPoint();
            int delta = (int)Math.Round(Position.X + Velocity.X - pos.X);
            if (delta != 0)
            {
                int sign = Math.Sign(delta);
                while (delta != 0)
                {
                    if (!CheckCollision(grid, pos.X + sign, pos.Y))
                    {
                        //There is no Solid immediately beside us 
                        pos.X += sign;
                        Position.X += sign;
                        delta -= sign;
                    }
                    else
                    {
                        if (CanMoveUp(grid, pos.X + sign, pos.Y)) //TODO: change
                        {
                            Position.Y -= 1;
                            Position.X += sign;
                        }


                        //Hit a solid!
                        if (onCollide != null)
                            onCollide();
                        break;
                    }
                }
            }
        }

        public void MoveY(PixelGardenEngine grid, Action onCollide)
        {
            isOnGround = false;
            //xRemainder += amount;
            Point pos = Position.ToPoint();
            int delta = (int)Math.Round(Position.Y + Velocity.Y - pos.Y);
            if (delta != 0)
            {
                int sign = Math.Sign(delta);
                while (delta != 0)
                {
                    if (!CheckCollision(grid, pos.X, pos.Y + sign))
                    {
                        //There is no Solid immediately beside us 
                        pos.Y += sign;
                        Position.Y += sign;
                        delta -= sign;
                    }
                    else
                    {
                        Velocity.Y = 0;
                        isOnGround = true;
                        //Hit a solid!
                        if (onCollide != null)
                            onCollide();
                        break;
                    }
                }
            }
        }

        public bool CanMoveUp(PixelGardenEngine grid, int px, int py)
        {
            for (int dx = -SizeX / 6; dx <= SizeX / 6; dx++) //Use mask
            {
                int x = px + dx;
                int y = py + 9;
                //if (grid.GetPixelLim(x, y-1).Value > 0)
                //    return 2;
                if (grid.GetPixelLim(x, y).IsSolid() && !grid.GetPixelLim(x, y - 1).IsSolid())
                    return true;
            }
            return false;
        }

        public bool CheckCollision(PixelGardenEngine grid, int px, int py)
        {
            for (int dx = -SizeX / 6; dx <= SizeX / 6; dx++) //Use mask
            {
                int x = px + dx;
                int y = py + 9;
                //if (grid.GetPixelLim(x, y-1).Value > 0)
                //    return 2;
                if (grid.GetPixelLim(x, y).IsSolid())
                    return true;

                if (grid.GetPixelLim(x, y - 5).IsSolid())
                    return true;
                //else
                //{
                //    grid.SetPixelLim(x, y, new Pixel(Color.Green, 0));
                //}
            }
            for (int dy = -SizeY / 2; dy < -2; dy++) //Use mask
            {
                int x = px + SizeX / 6;
                int y = py + dy;
                if (grid.GetPixelLim(x, y).IsSolid())
                    return true;
                //else
                //{
                //    grid.SetPixelLim(x, y, new Pixel(Color.Green, 0));
                //}
                x = px - SizeX / 6;
                if (grid.GetPixelLim(x, y).IsSolid())
                    return true;
                //else
                //{
                //    grid.SetPixelLim(x, y, new Pixel(Color.Green, 0));
                //}
            }
            return false;
        }

        public override void Draw(Camera camera)
        {
            SpriteEffects effect = SpriteEffects.FlipHorizontally;
            if (Velocity.X < 0)
                effect = SpriteEffects.None;
            int frameIndex = 4;
            if (Velocity.Y > 0.5f)
                frameIndex = 5;
            camera.CameraDraw(texture, frameIndex, Position, 0, 1, Color.White, effect);
        }

        public Point GetPoint()
        {
            return new Point((int)Math.Round(Position.X), (int)Math.Round(Position.Y));
        }

        public override string GetId()
        {
            return "Player";
        }

        public override Sprite GetSprite()
        {
            return null;
        }

        public override void ApplyCollision(GameObject collidingObject, GameEngine gameEngine)
        {
            Vector2 relPos = this.Position - collidingObject.Position;

            Velocity += relPos * 0.5f;
        }

        public override void ApplyForce(Vector2 force, float speedLimit)
        {

        }

        public override GameObjectType GetObjectType()
        {
            return GameObjectType.Ship;
        }        

        public override GameObject GetAgentAncestor()
        {
            return null;
        }

        public override float GetMeterValue(MeterType type)
        {
            return 10;
        }

        public override void SetMeterValue(MeterType type, float value)
        {

        }

        public override GameObject GetTarget(GameEngine gameEngine, TargetType targetType)
        {
            return null;
        }
    }
}
