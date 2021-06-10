using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PaintPlay.XnaUtils.MyGui;
using PaintPlay.XnaUtils.Input;
using SolarConflict.GameContent.Activities.Games;
using SolarConflict.Games.PixelGardenGame.Profiles;

namespace PaintPlay
{
    public enum PixelType
    {
        Empty = 0,
        Sand,
        Snow,
        Wall,
        WoodWall,
        Fire,
        BlueFire,
        TorchProfile,
        Plant,
        Flower,
        Water,
        Oill,
        Tnt,
        Tree,
        Structure,
        ColoredFire,
        MusicFireworks,
        RealWall,
        Object,
        Copper,
        Iron,
        Gold,
        Diamond,
        Vapor,
        Earth,
        EaterBug,
        Fly,
        JumperBug,

        Last
    }

    //cloudes, torch, funtine, snowWithTexture, flys, etar bugs, jumper bugs
    //opjects
    // house, stickPeople, balls, monster, portal...


    public struct GPixel
    {
        public static PixelProfile[] profiles;

        public PixelType type; 
        public Int32 value; //change to Int16
        public Int32 param1; //change to Int16
        public Int32 param2; //change to Int16     
        public Color color;

        public bool IsEmpty()
        {
            return type == PixelType.Empty;
        }

        public bool IsSolid()
        {                      
            return profiles[(int)type].IsSolid(this);
        }


        public int ConvertToBytes(byte[] buffer, int offset)
        {
            byte bType = (byte)type;
            byte[] bValue = BitConverter.GetBytes(value);
            byte[] bParam1 = BitConverter.GetBytes(param1);
            byte[] bParam2 = BitConverter.GetBytes(param2);
            byte[] bColor = BitConverter.GetBytes(color.PackedValue);
            
            


            
            Buffer.BlockCopy(bValue, 0, buffer, offset, bValue.Length);
            int addOffset = bValue.Length;
            Buffer.BlockCopy(bParam1, 0, buffer, offset + addOffset, bParam1.Length);
            addOffset += bParam1.Length;
            Buffer.BlockCopy(bParam2, 0, buffer, offset + addOffset, bParam2.Length);
            addOffset += bParam2.Length;
            Buffer.BlockCopy(bColor, 0, buffer, offset + addOffset, bColor.Length);
            addOffset += bColor.Length;
            buffer[offset + addOffset] = bType;
            return addOffset + 1;

            /*buffer[offset] = bType;
            Buffer.BlockCopy(bColor, 0, buffer, offset+1, bColor.Length);
           
            return bColor.Length+1;*/
        }

        public int FromBytes(byte[] buffer, int offset)
        {
            value = BitConverter.ToInt32(buffer, offset);
            int addOffset = sizeof(Int32);
            param1 = BitConverter.ToInt32(buffer, offset + addOffset);
            addOffset += sizeof(Int32);
            param2 = BitConverter.ToInt32(buffer, offset + addOffset);
            addOffset += sizeof(Int32);
            uint packedColor = BitConverter.ToUInt32(buffer, offset + addOffset);
            color.PackedValue = packedColor;
            addOffset += sizeof(UInt32);
            type = (PixelType)buffer[offset+addOffset];
            return addOffset + 1;          

            //type = PixelType.Sand;
           /* type = (PixelType)buffer[offset];
            color.PackedValue = BitConverter.ToUInt32(buffer, offset+1);
            
            return sizeof(UInt32)+1;*/
        }

        public static int GetSize()
        {
            int addOffset = 3*sizeof(Int32) + sizeof(UInt32) +1;            
            return addOffset;
        }

        
    }
    


    public class GardenLogic
    {
        public static int time;
        public GPixel[,] grid;// nextGrid;
        public int sizeX, sizeY;
        PixelProfile[] pixelProfiles;
        //public MCGA mcga;
        int dx;
        int dy;
        public Color dayTimeColor;

        public int sx, sy;

        public static PixelProfile[] MakeProfiles()
        {
            PixelProfile[]  pixelProfiles = new PixelProfile[(int)PixelType.Last];
            pixelProfiles[(int)PixelType.Empty] = new EmptyProfile();
            pixelProfiles[(int)PixelType.Wall] = new WallProfile();
            pixelProfiles[(int)PixelType.WoodWall] = new WoodProfile();
            pixelProfiles[(int)PixelType.Sand] = new SandProfile();
            pixelProfiles[(int)PixelType.Snow] = new SnowProfile();
            pixelProfiles[(int)PixelType.Plant] = new PlantProfile();
            pixelProfiles[(int)PixelType.Fire] = new FireProfile();
            pixelProfiles[(int)PixelType.BlueFire] = new BlueFireProfile();
            pixelProfiles[(int)PixelType.Flower] = new FlowerProfile();
            pixelProfiles[(int)PixelType.Water] = new WaterProfile();
            pixelProfiles[(int)PixelType.Oill] = new OillProfile();
            pixelProfiles[(int)PixelType.Tnt] = new TntProfile();
            pixelProfiles[(int)PixelType.Structure] = new StructureProfile();
            pixelProfiles[(int)PixelType.ColoredFire] = new ColoredFireProfile();
            pixelProfiles[(int)PixelType.MusicFireworks] = new MusicFireworks();
            pixelProfiles[(int)PixelType.RealWall] = new RealWall();
            pixelProfiles[(int)PixelType.Object] = new ObjectProfile();
            
            GPixel.profiles = pixelProfiles; //a static profiles array - not good but fast     
            return pixelProfiles;
        }

        public GardenLogic(PixelProfile[] pixelProfiles) //remove add sizeX sizeY
        {
            this.pixelProfiles = pixelProfiles;
            sx = MyGraphics.gdm.GraphicsDevice.Viewport.Width / 6;
            sy = MyGraphics.gdm.GraphicsDevice.Viewport.Height / 6;
            sizeX = sx  * 20; //from input
            sizeY =  sy * 10;

            dx = 600;
            dy = 200;



            //mcga = new MCGA(MyGraphics.gdm.GraphicsDevice, MyGraphics.sb, sizeX, sizeY); //possibly change to bigger res
            grid = new GPixel[sizeX, sizeY];

            dayTimeColor = Color.White;

      

            
            for (int x = 1; x < sizeX - 1; x++) //zero the grid
            {
                for (int y = 1; y < sizeY - 1; y++)
                {
                    grid[x, y].type = PixelType.Empty;
                  
                }
            }            

            for (int x = 0; x < sizeX; x++) //possibly in every loop
            {
                grid[x, 0].type = PixelType.Wall;
                grid[x, sizeY - 1].type = PixelType.Wall;
            }

            for (int y = 0; y < sizeY; y++)
            {
                grid[0, y].type = PixelType.Wall;
                grid[sizeX - 1, y].type = PixelType.Wall;
            }

            time = 0;           
        }



        public void Update(int cx, int cy)
        {            
            time++;
            //float col = (float)(0.6 + 0.4*Math.Cos(time/200.0));
            //dayTimeColor = new Color(col , col, col);

            if (time % 2 == 0)
            {
                for (int xx = Math.Max(cx - dx, 1); xx < Math.Min(cx + dx, sizeX - 1); xx++)
                {
                    //int x;
                    //if (time % 2 == 0)
                    //    x = xx;
                    //else
                    //    x = sizeX - xx - 1;
                    int x = xx;

                    for (int y = Math.Min(sizeY - 2, cy + dy); y > Math.Max(0, cy - dy); y--)
                    {
                        if (grid[x, y].type != PixelType.Empty)
                        {
                            pixelProfiles[(int)grid[x, y].type].Logic(x, y, grid);
                        }
                    }
                }
            }
            else
            {
                for (int xx = Math.Min(cx + dx, sizeX - 2) ; xx > Math.Max(cx - dx, 1); xx--)
                {
                    //int x;
                    //if (time % 2 == 0)
                    //    x = xx;
                    //else
                    //    x = sizeX - xx - 1;
                    int x = xx;

                    for (int y = Math.Min(sizeY - 2, cy + dy); y > Math.Max(0, cy - dy); y--)
                    {
                        if (grid[x, y].type != PixelType.Empty)
                        {
                            pixelProfiles[(int)grid[x, y].type].Logic(x, y, grid);
                        }
                    }
                }
            }

        }

        


        public void Draw(MCGA mcga, int offsetX, int offsetY)
        {
           // MyGraphics.sb.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
            for (int x = 1; x < sizeX - 1; x++)
            {
                for (int y = 1; y < sizeY - 1; y++)
                {
                    if (grid[x, y].type != PixelType.Empty)
                    {
                        //mcga.Putpixel(x, y, grid[x, y].color);
                        pixelProfiles[(int)grid[x, y].type].Draw(x, y, grid, mcga);
                    }
                    else
                      mcga.Putpixel(x, y, Color.Transparent);
                }
            }            

            mcga.SetData();
            int shadeX = 6;
            int shadeY = 6;
            Rectangle newRect = new Rectangle(-shadeX,-shadeY, MyGraphics.screenRect.Width, MyGraphics.screenRect.Height);
            MyGraphics.sb.Draw(mcga.mcgaTexture, newRect, new Color(00,00,00,200));
            mcga.Draw(dayTimeColor);

           // MyGraphics.sb.End();
            //mcga.Cls(0);
        }

        public void Save(String filename)
        {
            int offset = 0;
            byte[] buffer = new byte[sizeX * sizeY * GPixel.GetSize()];
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    offset += this.grid[x, y].ConvertToBytes(buffer, offset);
                  //  grid[x, y].color = Color.Green;
                }
            }

            offset = 0;
            
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    offset += this.grid[x, y].FromBytes(buffer, offset);
                    
                }
            } 
            FileUtils.SaveByte(buffer, filename);            
        }

        public void Cls()
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    grid[x, y].type = PixelType.Empty;
                    //grid[x, y].color = Color.Red;
                }
            }       
        }

        public void Load(String filename)
        {           
            int offset = 0;
            byte[] buffer = FileUtils.LoadByte(filename);            
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    offset += this.grid[x, y].FromBytes(buffer, offset);
                    //grid[x, y].color = Color.Red;
                }
            }            
        }



    }
}
