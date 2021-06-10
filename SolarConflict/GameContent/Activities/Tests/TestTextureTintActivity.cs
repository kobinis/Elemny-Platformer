using SolarConflict.Framework.Scenes.Activitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.SimpleGui;
using SolarConflict.XnaUtils.SimpleGui;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using XnaUtils.Graphics;
using XnaUtils.Framework.Graphics;
using SolarConflict.XnaUtils.SimpleGui.Controllers;
using SolarConflict.XnaUtils.Files;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.GameContent.Activities.Tests
{
    class  TestTextureActivity : Activity
    {
        private List<string> _imageName;
        private string _outDir;
        private List<Texture2D> _textures;
        private List<Texture2D> _newTextures;
        private ColorSelectorControl _colorSelector;
        HorizontalSliderControl _slider;

        GuiManager gui = new GuiManager();

        public TestTextureActivity()
        {
            Init(null, InputState.EmptyState.Cursor);
        }

        private void Init(GuiControl source, global::XnaUtils.Input.CursorInfo cursorLocation)
        {
            gui = new GuiManager();
            RelativeLayout root = new RelativeLayout();
            root.IsConsumingInput = false;
            root.DefaultAligmentData.Anchor = new GuiControl(ActivityManager.ScreenCenter, ActivityManager.ScreenSize);
            gui.Root = root;
            HorizontalLayout layout = new HorizontalLayout(Vector2.Zero, showFrame: true);
            root.AddChild(layout, HorizontalAlignment.Left, VerticalAlignment.Up);
            _colorSelector = new ColorSelectorControl(200, Game1.sb.GraphicsDevice);
            layout.AddChild(_colorSelector);
            _colorSelector.Action += Apply_Action;

            VerticalLayout hLayout = new VerticalLayout(Vector2.Zero, 5);

            _slider = new HorizontalSliderControl(new Vector2(500, 40), "guif8");
            _slider.Value = 0.4f;
            _slider.CursorOn += _slider_CursorOn;
            _slider.CursorOn += gui.ToolTipHandler;
            _slider.Action += Apply_Action;
            hLayout.AddChild(_slider);
            RichTextControl colorDisplay = new RichTextControl("");
            colorDisplay.LogicFunction = (GuiControl control, InputState inputState) => { (control as RichTextControl).Text = _colorSelector.SelectedColor.ToString(); };
            hLayout.AddChild(colorDisplay);
            layout.AddChild(hLayout);
            
            
            //RichTextControl apply = new RichTextControl("Apply", isShowFrame: true);
            //apply.Action += Apply_Action;
            //layout.AddChild(apply);

            RichTextControl save = new RichTextControl("Save", isShowFrame: true);
            save.Action += Save_Action;
            layout.AddChild(save);

            _imageName = new List<string>();
            _textures = new List<Texture2D>();
            _newTextures = new List<Texture2D>();
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "ImagesToColor");
            _outDir = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "OutImages");
            var files = FileUtils.GetFiles(path, "*.png", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                Texture2D texture = TextureBank.LoadTexture(Game1.sb.GraphicsDevice, file);
                _textures.Add(texture);
                _newTextures.Add(texture);
                _imageName.Add(Path.GetFileNameWithoutExtension(file));
            }
        }

        private void _slider_CursorOn(GuiControl source, global::XnaUtils.Input.CursorInfo cursorLocation)
        {
            source.TooltipText = _slider.Value.ToString();
        }

        private void Save_Action(GuiControl source, global::XnaUtils.Input.CursorInfo cursorLocation)
        {
            for (int i = 0; i < _newTextures.Count; i++)
           {               
                string filename = _imageName[i] + ".png";
                string full_name =Path.Combine(_outDir, filename);
                Stream stream = File.OpenWrite(full_name);
                _newTextures[i].SaveAsPng(stream, _newTextures[i].Width, _newTextures[i].Height);
                stream.Close();
            }
            File.WriteAllText(Path.Combine(_outDir, "color.txt"), "Color: " +  _colorSelector.SelectedColor.ToString() + "Value: " + _slider.Value);
            //WriteAllText(String, String)

            ActivityManager.Inst.AddToast("Saved", 200);
        }

        private void Apply_Action(GuiControl source, global::XnaUtils.Input.CursorInfo cursorLocation)
        {
            for (int i = 0; i < _textures.Count; i++)
            {
                _newTextures[i] = GraphicsUtils.SmartTint(_textures[i], _colorSelector.SelectedColor, _slider.Value);
            }
        }

        public override void Update(InputState inputState)
        {
            gui.Update(inputState);
            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
                ActivityManager.Inst.Back();                           
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 pos = new Vector2(20,300);
            float maxHight = 0;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            foreach (var item in _newTextures)
            {
                spriteBatch.Draw(item, pos, Color.White);
                pos += Vector2.UnitX * item.Width;
                maxHight = Math.Max(maxHight, item.Height);
                if(pos.X > ActivityManager.ScreenSize.X)
                {
                    pos.X = 0;
                    pos.Y = pos.Y + maxHight;
                    maxHight = 0;
                }
            }
            
            spriteBatch.End();
            //spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            //spriteBatch.Draw(texture, Vector2.One * 200, Color.White);
            //spriteBatch.End();
            gui.Draw();

        }

        public static Activity ActivityProvider(string parameters)
        {
            return new TestTextureActivity();
        }

    }
}
