using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PaintPlay.XnaUtils.MyGui;
using PaintPlay.XnaUtils.Input;
using Microsoft.Xna.Framework.Input.Touch;
using PaintPlay.XnaUtils;
using XnaUtils;
using PaintPlay;
using XnaUtils.Framework.Graphics;

namespace SolarConflict.Games.PixelGardenGame
{
    
    public class PixelGardenEngine 
    {
        public static PixelGardenEngine Inst;
        

        PixelType[] toolsList; //change to ToolProfile               
        int tool;
        Gui gui;       
        RadioControl radio;
        ColorSelectControl colorSelect;
        Color currentColor;
        ClickedControl saveBut, clearControl;
        ToggleControl timeflow, colorRotation;
        int colIndex = 0;

        Texture2D background;
        KeyboardState lastState;

        bool guiOut = false;
        bool guiMoving = false;

        private int effects = 0;
        MCGA mcga;        
        public GardenLogic logic; //engine
        
        public int offsetX;
        public int offsetY;

        //public int sx;
        //public int sy;

        int LimitX;
        int LimitY;

        GPixel emptyPixel;
        public GameEngine gameEngine;
        public Camera camera;
       

        public void GenerateWorld()
        {
            int[] skyLine = new int[LimitX];

            for (int x = 0; x < LimitX; x++)
            {
                skyLine[x] = LimitY / 4 + (int)Math.Round(Math.Cos(x * (0.01f + Math.Sin(x * 0.0013f) * 0.002)) * (Math.Sin(x * 0.0012) * 90 + 10));
            }

            for (int y = 0; y < LimitY; y++)
            {
                for (int x = 0; x < LimitX; x++)
                {
                    //(float)(Math.Cos(y)*FMath.Rand.FloatBetween(0.9f, 1) * 0.2 + 0.3)
                    if (y > skyLine[x])
                    {
                        if (y < skyLine[x] * 1.5)
                        {
                            GPixel pixel = new GPixel();
                            pixel.type = PixelType.WoodWall;
                            pixel.value = 100;
                            pixel.param1 = 100;
                            pixel.color = Color.Aqua;
                            SetPixelLim(x, y, pixel);
                        }
                        else
                        {
                            GPixel pixel = new GPixel();
                            pixel.type = PixelType.WoodWall;
                            pixel.color = Color.Green;
                            SetPixelLim(x, y, pixel);
                        }
                    }
                    else
                    {
                        GPixel pixel = new GPixel();
                        pixel.type = PixelType.Empty;
                        pixel.color = Color.Transparent;
                        SetPixelLim(x, y, pixel);
                    }
                    //if (y == skyLine[x])
                    //{
                    //    SetPixelLim(x, y, new Pixel(new Color(sand.GetPixel(x % sand.Width, y % sand.Height).ToVector3() * new Vector3(0.85f, 0.7f, 1))));
                    //}
                }

            }
        }


        public PixelGardenEngine()//getResolution;
        {
            Inst = this;
            //touchForm.

            // sizeX = graphicsDevice.Viewport.Width / 2; ;
            // sizeY = graphicsDevice.Viewport .Height/2;
            //RealObjectEngine.Init();
            this.background = MyGraphics.GetTexture("back6");
            var profiles = GardenLogic.MakeProfiles();
            logic = new GardenLogic(profiles);
            
            mcga = new MCGA(logic.sx, logic.sy);
            toolsList = new PixelType[10];
            toolsList[0] = PixelType.Sand;
            toolsList[1] = PixelType.WoodWall;
            toolsList[2] = PixelType.Fire;
            toolsList[3] = PixelType.Plant;
            toolsList[4] = PixelType.Water;
            toolsList[5] = PixelType.Tnt;
            toolsList[6] = PixelType.Snow;
            toolsList[7] = PixelType.Object;                              

            tool = 1;

            GuiControlDesign design = new GuiControlDesign();
            design.frameNumber = 15;
            design.colorGradient = 1f;
            design.color = 0.7f;
            GuiControl bot = new MoveGuiControl(new Vector2(), 150, 150, design, new Norma(ShapeBank.CircleNorma));
            gui = new Gui();
            gui.controls.Add(bot);
            design.colorGradient = 1f;
            design.frameNumber = 5;
            design.color = 0.75f;
            radio = new RadioControl();
            ToggleControl radioBot;
            int toolCounter = 0;
            for (int i = -2; i <= 2; i++)
            {

                radioBot = new ToggleControl(new Vector2(i * 53, 38), 25, 25, design, new Norma(ShapeBank.RoundedRectNorma));
                radioBot.AddIcon(MyGraphics.GetTexture(toolsList[toolCounter].ToString()));     
                radio.AddControl(radioBot);
                toolCounter++;
            }

            for (int i = -1; i <= 1; i++)
            {
                radioBot = new ToggleControl(new Vector2(i * 53, 91), 25, 25, design, new Norma(ShapeBank.RoundedRectNorma));
                radioBot.AddIcon(MyGraphics.GetTexture(toolsList[toolCounter].ToString()));  
                radio.AddControl(radioBot);
                toolCounter++;
            }
            gui.controls.Add(radio);
            colorSelect = new ColorSelectControl(new Vector2(0, -60), 60);
            gui.controls.Add(colorSelect);

            timeflow = new ToggleControl(new Vector2(-1.7f * 53, -70), 21, 21, design, new Norma(ShapeBank.CircleNorma));
            timeflow.AddIcon(MyGraphics.GetTexture("timeflow"));
            gui.controls.Add(timeflow);

            colorRotation = new ToggleControl(new Vector2(-1.8f * 53, -20), 25, 25, design, new Norma(ShapeBank.CircleNorma));
            colorRotation.AddIcon(MyGraphics.GetTexture("colRot"));
            gui.controls.Add(colorRotation);

            clearControl = new ClickedControl(new Vector2(1.7f * 53, -70), 21, 21, design, new Norma(ShapeBank.RoundedRectNorma)); //clear
            clearControl.AddIcon(MyGraphics.GetTexture("newpage"));
            gui.controls.Add(clearControl);

            saveBut = new ClickedControl(new Vector2(1.8f * 53, -20), 25, 25, design, new Norma(ShapeBank.RoundedRectNorma)); //clear
            saveBut.AddIcon(MyGraphics.GetTexture("save"));
            gui.controls.Add(saveBut);

            gui.Position = new Vector2(200, 200);

            LimitX = logic.sizeX;
            LimitY = logic.sizeY;

            emptyPixel = new GPixel();
            emptyPixel.type = PixelType.Wall;
            emptyPixel.color = Color.Red;

            GenerateWorld();
            


            camera = new Camera();
            camera.Zoom = 6;
            gameEngine = new GameEngine(camera);
            
        }

        public void Init()
        {
           // logic.Cls();
           // logic.Load("open.dat");
            gui.Position = new Vector2(200, 200);
        }


        public ref GPixel GetPixelLim(int x, int y)
        {
            
            if (x < 0 || y < 0 || x >= LimitX || y >= LimitY)
                return ref emptyPixel;
            return ref logic.grid[x, y];
        }

        public void SetPixelLim(int x, int y, GPixel pixel)
        {
            if (x < 0 || y < 0 || x >= LimitX || y >= LimitY)
                return;            
            logic.grid[x, y] = pixel;
        }

        private void UpdateGird()
        {
            logic.Update(offsetX, offsetY);
        }


        public void UpdateLogic(List<TouchState> inputList)
        {

            UpdateGird();
            
            //RealObjectEngine.Update(logic, inputList);

            //input.Update();
            //List<TouchState> inputList = input.GetTouchColection();
            gui.Update(inputList);
           

            tool = (int)toolsList[radio.GetValue()];

            if (colorRotation.IsPressed())
            {
                //currentColor = Painter.palette[colIndex % Painter.palette.Length];
                currentColor.A = colorSelect.GetSelectedColor().A;
                colIndex++;
            }
            else
                currentColor = colorSelect.GetSelectedColor();

            

            
            foreach (var touch in inputList)
            {

                int px = mcga.GetMcgaX((int)touch.Position.X) + offsetX;
                int py = mcga.GetMcgaY((int)touch.Position.Y) + offsetY;                
                GPixel.profiles[tool].Init( px,  py, px ,  py , currentColor, logic.grid);                                
            }


         /*   if (GestureUtils.GetGesture() == PXCMGesture.Gesture.Label.LABEL_POSE_PEACE)
                radio.SetValue((radio.GetValue() + 1) % 8);*/

            KeyboardState keyboardState = Keyboard.GetState();
           

            if (guiOut && guiMoving)
            {
                gui.Position = new Vector2(gui.Position.X - 6, gui.Position.Y);

                if (gui.Position.X < -200)
                    guiMoving = false;
            }

            if (!guiOut && guiMoving)
            {
                gui.Position = new Vector2(gui.Position.X +6, gui.Position.Y);

                if (gui.Position.X > 200)
                    guiMoving = false;
            }
                
           

            
            lastState = keyboardState;
         
           
        }

        public void Draw(Rectangle rectangle, Color color)
        {
            MyGraphics.sb.Draw(background, rectangle, color);
         //   logic.Draw(rectangle, color);
          //  data.SetData();
           // MyGraphics.sb.Draw(data.mcgaTexture, rectangle, color);
            //frontTexture.Draw();          
        }


        public void DrawGird()
        {
            // MyGraphics.sb.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
            for (int x = 1; x < logic.sx - 1; x++)
            {
                for (int y = 1; y < logic.sy - 1; y++)
                {
                    GPixel pixel = GetPixelLim(x + offsetX, y + offsetY);
                    if (pixel.type != PixelType.Empty)
                    {
                        //mcga.Putpixel(x, y, pixel.color);
                        GPixel.profiles[(int)pixel.type].Draw(x, y, pixel, mcga);
                    }
                    else
                        mcga.Putpixel(x, y, Color.Transparent);
                }
            }

            mcga.SetData();
            int shadeX = 6;
            int shadeY = 6;
            Rectangle newRect = new Rectangle(-shadeX, -shadeY, MyGraphics.screenRect.Width, MyGraphics.screenRect.Height);
            MyGraphics.sb.Draw(mcga.mcgaTexture, newRect, new Color(00, 00, 00, 200));
            mcga.Draw();

            // MyGraphics.sb.End();
            //mcga.Cls(0);
        }


        public void Draw()
        {
            MyGraphics.sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied) ;
          
         /*   if (MyPipeline.texture != null)
                background = MyPipeline.texture;*/

          //  MyGraphics.sb.Draw(background, MyGraphics.screenRect, newColor);
            MyGraphics.sb.Draw(background, MyGraphics.screenRect, null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 1);
            DrawGird();




            MyGraphics.sb.End();
            MyGraphics.sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied,  samplerState: SamplerState.PointClamp);
            gui.Draw();


            if (guiOut)
            {
                String text = toolsList[radio.GetValue()].ToString(); //""GestureUtils.GetGesture().ToString()";
                Vector2 size = MyGraphics.font.MeasureString(text);
                MyGraphics.sb.DrawString(MyGraphics.font, text, new Vector2((MyGraphics.screenRect.Width - size.X) / 2, size.Y/2 + 5), Color.CornflowerBlue);
            }
            //input.Draw();
            //RealObjectEngine.Draw();
            MyGraphics.sb.End();
        }

        public void Update(InputState inputState)
        {

            if (inputState.IsKeyDown(Keys.D))
                offsetX++;
            if (inputState.IsKeyDown(Keys.A))
                offsetX--;
            if (inputState.IsKeyDown(Keys.S))
                offsetY++;
            if (inputState.IsKeyDown(Keys.W))
                offsetY--;



            var cursor = ActivityManager.Inst.InputState.Cursor;

            List<TouchState> touchList = new List<TouchState>();
            TouchState touch = new TouchState();
            touch.Position = cursor.Position;
            touch.IsFingerOrPen = true;
            touch.PreviousPosition = cursor.PreviousPosition;
            touch.OnPress = cursor.IsPressedLeft;
            touch.OnRelease = cursor.OnReleaseLeft;
            touch.FirstPosition = cursor.FirstPosition;
            if(cursor.IsPressedLeft || cursor.IsLastPressedLeft)
                touchList.Add(touch);

            gameEngine.Update(inputState);

           

            UpdateLogic(touchList);
        }

        public void Draw(SpriteBatch sb)
        {
            GraphicsSettings.IsPostprocessing = false;
            Draw();
            gameEngine.Draw(sb);
        }


    }
}
