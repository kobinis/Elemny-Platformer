using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.XnaUtils.Input;
using XnaUtils;
using XnaUtils.Framework.Graphics;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Activities
{

    
    /*
     * TODO:
     * Go up one block without jumping
     * Repalce sky with background, have paralx background
     * Add Camera
     * Add option to mine
     * Add option to shoot
     * Add Fire logic with inactive flag


    */
    public struct Pixel
    {
        public byte Type;
        public Color Color;
        public int Value;

        public Pixel(Color color, int value = 0, byte type = 0 )
        {
            Color = color;
            Value = value;
            Type = type;
        }

    }

    public struct PixelGrid //chunk
    {
        public Pixel[,] data;
        public bool IsDirty; //Update

        public void Init(int size)
        {
            data = new Pixel[size, size];
        }
    }



    public class Shoot
    {
        public Vector2 position;
        public Vector2 velocity;
        public bool isActive;
    }
    

    /// <summary>
    /// 
    /// </summary>
    public class CellGrid //world
    {
        const int PixelGridSize = 128;
        const int GridSizeX = 100;
        const int GridSizeY = 20;
        Pixel emptyPixel;
        PixelGrid[,] cells;

        public int ViewSizeX = 320;
        public int ViewSizeY = 200;

        public int LimitX { get; private set; }
        public int LimitY { get; private set; }
        Canvas canvas;

        public int Scale = 5;


        public void WorldGeneration()
        {
            Canvas sand = new Canvas(TextureBank.Inst.GetTexture("tilea6"));

         //   List<Canvas> objects = new List<Canvas>();
            for (int i = 1; i <= 4; i++)
            {
           //     objects.Add(new Canvas(TextureBank.Inst.GetTexture("g" + i.ToString())));
            }
            //Canvas treeCan2 = new Canvas(TextureBank.Inst.GetTexture("Tree2"));
            //Canvas treeCan1 = new Canvas(TextureBank.Inst.GetTexture("Tree3"));
            Canvas treeCan = null;// new Canvas(TextureBank.Inst.GetTexture("Tree2"));
            

            int[] skyLine = new int[LimitX];

            for (int x = 0; x < LimitX; x++)
            {
                skyLine[x] = LimitY / 4 + (int)Math.Round(Math.Cos(x * (0.01f + Math.Sin(x * 0.0013f) * 0.002 ))* (Math.Sin(x*0.0012) * 90 + 10 ) );
            }

            for (int y = 0; y < LimitY; y++)
            {
                for (int x = 0; x < LimitX; x++)
                {
                    //(float)(Math.Cos(y)*FMath.Rand.FloatBetween(0.9f, 1) * 0.2 + 0.3)
                    if (y > skyLine[x])
                    {
                        if (y < skyLine[x] * 1.5)
                            SetPixelLim(x, y, new Pixel(sand.GetPixel(x % sand.Width, y % sand.Height), 10));
                        else
                            SetPixelLim(x, y, new Pixel(GraphicsUtils.HsvToRgb(0, FMath.Rand.FloatBetween(0.6f, 0.7f), (float)Math.Cos(y) * 0.3f + 0.4f), 10));
                    }
                    else
                        SetPixelLim(x, y, new Pixel(new Color( sand.GetPixel(x % sand.Width, y % sand.Height).ToVector3() * new Vector3(0.6f,0.5f,1))));
                    if (y == skyLine[x])
                    {
                        SetPixelLim(x, y, new Pixel(new Color(sand.GetPixel(x % sand.Width, y % sand.Height).ToVector3() * new Vector3(0.85f, 0.7f, 1))));
                    }
                }

            }

            for (int x = 0; x < LimitX; x++)
            {
                if( x % 300  == 99)
                {
                   // treeCan = objects[FMath.Rand.Next(objects.Count)];                    

                    int py = 0;
                    for (int i = -5; i <= 5; i++)
                    {
                        py = Math.Max(skyLine[x + i], py);
                    }

                }
                
            }
        }


        public CellGrid()
        {
            emptyPixel = new Pixel(Color.Purple);
            LimitX = PixelGridSize * GridSizeX;
            LimitY = PixelGridSize * GridSizeY;

            cells = new PixelGrid[GridSizeX, GridSizeY];

            canvas = new Canvas(ViewSizeX, ViewSizeY, ActivityManager.GraphicsDevice);            

            for (int y = 0; y < GridSizeY; y++)
            {
                for (int x = 0; x < GridSizeX; x++)
                {
                    cells[x, y].Init(PixelGridSize);
                }
            }

            FireProfile();
            WorldGeneration();



        }

        public static Color[] palette;
        public void FireProfile()
        {
            palette = new Color[256];
            byte r, g, b;
            for (int i = 0; i < 256; i++)
            {
                r = (byte)Math.Min(i * 4, 255);
                g = (byte)Math.Max(Math.Min((i - 64) * 4, 255), 0);
                b = (byte)Math.Max(Math.Min((i - 64 * 2) * 4, 255), 0);
                palette[i] = new Color(r, g, b, Math.Min(i * 4, 255));
                //palette[i] = new Color(r, g, b).PackedValue;
            }
        }

        public void Draw(SpriteBatch sb, Point pos)
        {
            pos -= new Point(ViewSizeX / 2, ViewSizeY / 2);
            for (int y = 0; y < ViewSizeY; y++)
            {
                for (int x = 0; x < ViewSizeX; x++)
                {
                    Pixel pixel = GetPixelLim(x + pos.X, y + pos.Y);
                    if(pixel.Type == 1)                        
                        canvas.SetPixel(x, y, palette[Math.Max(Math.Min(pixel.Value, 255), 0)]);
                    else
                        canvas.SetPixel(x, y, pixel.Color);
                    
                }
            }
            canvas.SetData();
            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp);
            sb.Draw(canvas.GetTexture(), new Rectangle(0,0, ViewSizeX * Scale, ViewSizeY * Scale), Color.White);
            sb.End();

        }

        public void Update(int x, int y)
        {
            int cellIndexX = x / PixelGridSize;            
            int cellIndexY = y / PixelGridSize;

            int radX = 2;
            int radY = 2;

            int fromX = Math.Max(cellIndexX - radX, 0);
            int toX = Math.Min(cellIndexX + radX, GridSizeX-1);
            int fromY = Math.Max(cellIndexY - radY, 0);
            int toY = Math.Min(cellIndexY + radY, GridSizeY - 1);

            for (int cy = fromY; cy <= toY; cy++)
            {
                for (int cx = fromX; cx <= toX; cx++)
                {
                    PixelGrid pixels = cells[cx, cy];
                    if (pixels.IsDirty)
                    {
                        pixels.IsDirty = false;
                        for (int yy = 0; yy < PixelGridSize; yy++)
                        {
                            for (int xx = 0; xx < PixelGridSize; xx++)
                            {
                               

                                if (pixels.data[xx, yy].Type == 1)
                                {
                                    pixels.IsDirty = true;
                                    if (pixels.data[xx, yy].Value > 0)
                                    {
                                        pixels.data[xx, yy].Value -= 1;
                                        int value = pixels.data[xx, yy].Value;
                                      //  int dirVec = FMath.Rand.Next(16);

                                        //if (((dirVec & 1)) > 0 && xx > 0)
                                        //{
                                        //    pixels.data[xx - 1, yy].Type = 1;
                                        //    pixels.data[xx-1, yy].Value += value;
                                        //    pixels.data[xx - 1, yy].Value /= 2;
                                        //}

                                        //if (((dirVec & 2) > 0) && xx < PixelGridSize - 1 )
                                        //{
                                        //    pixels.data[xx + 1, yy].Type = 1;
                                        //    pixels.data[xx + 1, yy].Value += value;
                                        //    pixels.data[xx + 1, yy].Value /= 2;
                                        //}

                                        //if (((dirVec & 4) > 0) && yy > 0)
                                        //{
                                        //    pixels.data[xx, yy - 1].Type = 1;
                                        //    pixels.data[xx, yy - 1].Value += value;
                                        //    pixels.data[xx, yy - 1].Value /= 2;
                                        //}

                                        //if (((dirVec & 8) > 0) && yy < PixelGridSize - 1)
                                        //{
                                        //    pixels.data[xx, yy + 1].Type = 1;
                                        //    pixels.data[xx, yy + 1].Value += value;
                                        //    pixels.data[xx, yy + 1].Value /= 2;

                                        //}

                                    }
                                    else
                                    {
                                        pixels.data[xx, yy].Type = 0;
                                        pixels.data[xx, yy].Color = Color.Black;
                                        pixels.data[xx, yy].Value = 0;
                                    }
                                }

                                
                            }
                        }


                    }
                }
            }

        }

        public Pixel GetPixelLim(int x, int y)
        {
            if (x < 0 || y < 0 || x >= LimitX || y >= LimitY)
                return emptyPixel;
            int cellIndexX = x / PixelGridSize;
            int pixelIndexX = x % PixelGridSize;

            int cellIndexY = y / PixelGridSize;
            int pixelIndexY = y % PixelGridSize;

            return cells[cellIndexX, cellIndexY].data[pixelIndexX, pixelIndexY];
        }

        public void SetPixelLim(int x, int y, Pixel pixel)
        {
            if (x < 0 || y < 0 || x >= LimitX || y >= LimitY)
                return;
            int cellIndexX = x / PixelGridSize;
            int pixelIndexX = x % PixelGridSize;

            int cellIndexY = y / PixelGridSize;
            int pixelIndexY = y % PixelGridSize;
            cells[cellIndexX, cellIndexY].IsDirty = true;
            cells[cellIndexX, cellIndexY].data[pixelIndexX, pixelIndexY] = pixel;
        }

    }

    public class Player
    {
        public Vector2 Position;
        public Vector2 Velocity;

        public Texture2D texture;

        public int SizeX = 32;
        public int SizeY = 32;

        public bool isOnGround;

        List<Shoot> shoots;
        int cooldown = 0;
        int weaponMode = 1;

        public Player()
        {
            texture = TextureBank.Inst.GetTexture("silicon");// "Running");
            shoots = new List<Shoot>();
        }



        public void Update(InputState inputState, CellGrid grid)
        {
            int maxSpeed = 2;
            float acc = 0.25f;
            var controllerState = inputState.CurrentGamePadStates[0];
            if (inputState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) || controllerState.ThumbSticks.Left.X > 0.1f)
            {
                Velocity += Vector2.UnitX * acc;
                if (Velocity.X > maxSpeed)
                    Velocity.X = maxSpeed;
            }

            if (inputState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) || controllerState.ThumbSticks.Left.X < -0.1f)
            {
                Velocity -= Vector2.UnitX * acc;
                if (Velocity.X < -maxSpeed)
                    Velocity.X = -maxSpeed;
            }

            if ((inputState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) || controllerState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A)) && isOnGround)
            {
                Velocity = -Vector2.UnitY * 7;

            }

            if (inputState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
            {
                Velocity += Vector2.UnitY;
            }

            if(inputState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D1))
            {
                weaponMode = 0;
            }

            if (inputState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D2))
            {
                weaponMode = 1;
            }

            var mineVec = inputState.CurrentGamePadStates[0].ThumbSticks.Right;
            mineVec.Y = -mineVec.Y;
            if (mineVec.LengthSquared() > 0.5f && weaponMode == 0)
            {
                var minePos = (Position + mineVec * 15).ToPoint();
                int rad = 15;
                for (int dx = -rad; dx <= rad; dx++)
                {
                    for (int dy = -rad; dy <= rad; dy++)
                    {
                        if ((dx * dx + dy * dy <= rad * rad) && grid.GetPixelLim(minePos.X + dx, minePos.Y + dy - SizeY / 3).Value > 0)
                            grid.SetPixelLim(minePos.X + dx, minePos.Y + dy - SizeY / 3, new Pixel(Color.DarkGray, 0));
                    }

                }
            }
            cooldown--;
            if ((mineVec.LengthSquared() > 0.5f && weaponMode == 1) || inputState.CurrentGamePadStates[0].IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.X))
            {
                if(mineVec.LengthSquared() <= 0.5f)
                {
                    mineVec = FMath.ToCartesian(1, -0.2f);
                    mineVec.X *= Math.Sign(Velocity.X);
                }
                if(cooldown < 0)
                {
                    var shoot = new Shoot();
                    shoot.velocity = mineVec * 8;
                    shoot.position = Position - Vector2.UnitY * 3 + mineVec * 30;
                    shoot.isActive = true;
                    shoots.Add(shoot);
                    cooldown = 30;
                }
            }

            
            MoveX(grid, null);
            MoveY(grid, null);

            List<Shoot> removeList = new List<Shoot>();
            foreach (var item in shoots)
            {
                if(item.isActive)
                {
                    item.velocity += Vector2.UnitY * 0.1f;
                    item.position += item.velocity;

                    var pos = item.position.ToPoint();

                    if(grid.GetPixelLim(pos.X,pos.Y).Value == 0)
                    {
                        grid.SetPixelLim(pos.X, pos.Y, new Pixel(Color.Gold, 110, 1));
                        grid.SetPixelLim(pos.X-1, pos.Y, new Pixel(Color.Gold, 100, 1));
                        grid.SetPixelLim(pos.X + 1, pos.Y, new Pixel(Color.Gold, 100, 1));
                        grid.SetPixelLim(pos.X, pos.Y - 1, new Pixel(Color.Gold, 100, 1));
                        grid.SetPixelLim(pos.X, pos.Y + 1, new Pixel(Color.Gold, 100, 1));

                    }
                    else
                    {
                        removeList.Add(item);
                    }
                }
                else
                {
                    removeList.Add(item);
                }
            }            
            foreach (var item in removeList)
            {
                shoots.Remove(item);
            }
            

            //Position += Velocity;

            //  int cc = CheckCollision(grid);





            //if(cc == 0)
            //{
            //    Velocity += Vector2.UnitY * 0.5f;
            //}
            Velocity += Vector2.UnitY * 0.5f;
            Velocity *= 0.98f;
        }

        //public 

        public void MoveX(CellGrid grid, Action onCollide)
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
                        if(CanMoveUp(grid, pos.X + sign, pos.Y)) //TODO: change
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

        public void MoveY(CellGrid grid, Action onCollide)
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

        public bool CanMoveUp(CellGrid grid, int px, int py)
        {
            for (int dx = -SizeX / 6; dx <= SizeX / 6; dx++) //Use mask
            {
                int x = px + dx;
                int y = py - 2;
                //if (grid.GetPixelLim(x, y-1).Value > 0)
                //    return 2;
                if (grid.GetPixelLim(x, y).Value > 0 && grid.GetPixelLim(x, y-1).Value == 0)
                    return true;
            }
            return false;
        }

        public bool CheckCollision(CellGrid grid, int px, int py)
        {
            for (int dx = -SizeX/6; dx <= SizeX / 6; dx++) //Use mask
            {
                int x = px + dx;
                int y = py - 2;
                //if (grid.GetPixelLim(x, y-1).Value > 0)
                //    return 2;
                if (grid.GetPixelLim(x, y).Value > 0)
                    return true;
                //else
                //{
                //    grid.SetPixelLim(x, y, new Pixel(Color.Green, 0));
                //}
            }
            for (int dy = -SizeY/2; dy < -2; dy++) //Use mask
            {
                int x = px+ SizeX/6;
                int y = py+dy;
                if (grid.GetPixelLim(x, y).Value > 0)
                    return true;
                //else
                //{
                //    grid.SetPixelLim(x, y, new Pixel(Color.Green, 0));
                //}
                x = px - SizeX/6;
                if (grid.GetPixelLim(x, y).Value > 0)
                    return true;
                //else
                //{
                //    grid.SetPixelLim(x, y, new Pixel(Color.Green, 0));
                //}
            }
            return false;
        }

        public void Draw(SpriteBatch sb, CellGrid grid)
        {
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            //Rectangle rect = new Rectangle((int)Math.Round(Position.X * scale), (int)Math.Round(Position.Y * scale), SizeX * scale, SizeY * scale);
            Rectangle rect = new Rectangle((grid.ViewSizeX  - SizeX )* grid.Scale /2, (grid.ViewSizeY / 2 - SizeY) * grid.Scale, SizeX * grid.Scale, SizeY * grid.Scale);
            int spriteInd = (int)(Position.X / grid.Scale) % 4;
            SpriteEffects spriteEffect = SpriteEffects.None;
            if (Velocity.X < 0)
                spriteEffect = SpriteEffects.FlipHorizontally;


            //sb.Draw(texture, rect, new Rectangle(32 * spriteInd, 0, 32 , 32), Color.White, 0, Vector2.Zero, spriteEffect, 0);
            sb.Draw(texture, rect, Color.White);
        //    var mineVec = inputState.CurrentGamePadStates[0].ThumbSticks.Right;
        //   mineVec.Y = -mineVec.Y;

            sb.End();
        }

        public Point GetPoint()
        {
            return new Point((int)Math.Round(Position.X), (int)Math.Round(Position.Y));
        }
    }

    class PlatformerActivity:Activity
    {

        CellGrid grid;
        Player player;
        

        public PlatformerActivity()
        {
            grid = new CellGrid();
            player = new Player();
        }


        public override void Update(InputState inputState)
        {
            player.Update(inputState, grid);
            grid.Update(player.GetPoint().X, player.GetPoint().Y);

            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
                ActivityManager.Inst.Back();
        }

        public override void Draw(SpriteBatch sb)
        {
            
            grid.Draw(sb, player.GetPoint());
            player.Draw(sb, grid);
            
            sb.Begin();
            string text = grid.LimitX.ToString() + " x " + grid.LimitY.ToString();
            sb.DrawString(Game1.font, text, Vector2.One * 20, Color.Black);
            sb.DrawString(Game1.font, player.GetPoint().ToString(), Vector2.One * 20 + Vector2.UnitY * 30, Color.Black);
            sb.End();

        }

        public static Activity ActivityProvider(string parameters)
        {
            return new PlatformerActivity();
        }
    }
}

