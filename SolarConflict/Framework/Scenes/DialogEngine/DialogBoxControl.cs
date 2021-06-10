using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;
using XnaUtils;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using XnaUtils.Input;
using SolarConflict.Framework;
using SolarConflict.XnaUtils.SimpleGui.TextureGeneration;
using XnaUtils.Graphics;
using SolarConflict.Framework.Utils;
using SolarConflict.Framework.Scenes.DialogEngine;
using SolarConflict.Session.World.MissionManagment;
using SolarConflict.Framework.GUI;

namespace SolarConflict
{
    //TODO: maybe add arrow down if text doesn't fit
    [Serializable]
    public class DialogBoxControl : GuiControl //change to group control
    {
        [Serializable]
        public class DialogDesign
        {
            public Color TextColor;
            public Color BackgroundColor;
            public Color QuestionsTextColor;
            public Color QuestionsBackgroundColor;
            public Color QuestionsSelectedBackgroundColor;
            public Vector2 ImageSize;
            public int FadeinTime;
            public int FadeoutTime;
            //SpriteFont font; //?? string FontID;

            public DialogDesign()
            {
                TextColor = Palette.DialogTextColor;
                BackgroundColor = Color.White;// new Color(60,60,200, 255);
                QuestionsTextColor = new Color(167, 187, 174);
                QuestionsBackgroundColor = BackgroundColor;
                QuestionsSelectedBackgroundColor = new Color(90, 90, 200, 200);
                ImageSize = new Vector2(128, 128);// Vector2.Zero;
                FadeinTime = 8;
                FadeoutTime = 20;
                
            }
        }

        public string ID { get; private set; }
        public bool IsBlocking;
        //public bool RemoveFlag { get; set; }
        private RichTextControl _mainTextControl;
        private List<RichTextControl> _questionsTextControls; //No need
        private GuiControl _imageControl;        
        //private int _time;
        private int? _maxLifetime;
        private bool _isSkippable;                                
        private float _spacing;
        private DialogDesign design;
        private DialogManager _dialogManager;

        public void OnQuestionSelect(GuiControl source, CursorInfo cursorLocation)
        {
            _dialogManager.DialogOptiponSelected(source.Index);
        }

        public void OnCountine(GuiControl source, CursorInfo cursorLocation)
        {
            _dialogManager.ContinieHandler();
        }


        public DialogBoxControl(DialogManager dialogManager, string text, string image = null, string soundID = null, IList<DialogOption> dialogOptions = null, string boxID = null, bool isBlocking = false,
        bool isSkippable = false, int? maxLifetime = null, bool isFixedSize = false)
        {
            IsConsumingInput = false;
            _dialogManager = dialogManager;
            IsBlocking = isBlocking;
            design = new DialogDesign();

            _maxLifetime = maxLifetime;
            _isSkippable = isSkippable;
            _spacing = 10;
            ID = boxID;

            ControlColor = Palette.Textbox;
            PressedControlColor = ControlColor;
            CursorOverColor = ControlColor;
                                          
            if (text != null && image != null)
            {
                HorizontalLayout layout = new HorizontalLayout(Vector2.Zero);
                layout.IsConsumingInput = false;
                _imageControl = MakeCharacterControl(image, design.ImageSize);
                
                layout.AddChild(_imageControl);
                _mainTextControl = new RichTextControl(text, Game1.font);
                _mainTextControl.IsConsumingInput = false;
                layout.AddChild(_mainTextControl);
                AddChild(layout);
            }
            else
            {
                if (image != null)
                {
                    _imageControl = MakeCharacterControl(image, design.ImageSize);
                    AddChild(_imageControl);
                }

                if (text != null)
                {
                    _mainTextControl = new RichTextControl(text, Game1.font);
                    _mainTextControl.IsConsumingInput = false;
                    AddChild(_mainTextControl);
                }
            }

            if (_mainTextControl != null)
            {
                _mainTextControl.TextColor = design.TextColor;
            }



            _questionsTextControls = new List<RichTextControl>();

            if (dialogOptions != null)
            {
                //RelativeLayout questionsLayout = new RelativeLayout();
                //AddChild(questionsLayout);
                Color pressedColor = new Color(design.QuestionsBackgroundColor.ToVector4() * 1.3f);
                float maxWidth = 10;
                List<RichTextControl> optionControl = new List<RichTextControl>();
                for (int i = 0; i < dialogOptions.Count; i++)
                {
                    var item = dialogOptions[i];
                    RichTextControl questionControl = new RichTextControl(item.Text, Game1.font);
                    questionControl.TextColor = design.QuestionsTextColor;
                    questionControl.ControlColor = design.QuestionsBackgroundColor;
                    questionControl.CursorOverColor = Color.Yellow; //TODO: Change it to use patlette
                    questionControl.PressedControlColor = pressedColor;
                    questionControl.Index = i;
                    questionControl.IsShowFrame = true;
                    questionControl.Action += OnQuestionSelect;
                    _questionsTextControls.Add(questionControl);
                    if (i == 0)
                        questionControl.ActivationKey = Keys.Space;
                    if (i == dialogOptions.Count-1)
                        questionControl.ActivationKey = Keys.Escape;
                    AddChild(questionControl);
                    optionControl.Add(questionControl);
                    maxWidth = Math.Max(questionControl.Width, maxWidth);
                    //questionsLayout.AddChild(questionControl, HorizontalAlignment.Left, VerticalAlignment.None, _imageControl);
                }
                foreach (var item in optionControl)
                {
                    item.Width = maxWidth;
                }

            }

            if (isSkippable && dialogOptions == null)
            { //Add tooltip
                TextControl skipText = new TextControl(" Continue ", Game1.font);//Change: just "contine" works only when clicked o
                skipText.CursorOn += dialogManager.gui.ToolTipHandler;
                skipText.TooltipText = "Press #color{255,255,0}Space#dcolor{} to continue";
                skipText.ControlColor = new Color(design.BackgroundColor.ToVector3() * 1.3f);                
                skipText.ActivationKey = Keys.Space; //TODO: change
                skipText.TextColor = design.TextColor;
                skipText.IsShowFrame = true;
                //skipText.Sprite =  Sprite.Get("Btn_Button5-2_h"); //Sprite.Get("Small_ui");
                AddChild(skipText);
                skipText.Action += OnCountine;
            }

            Vector2 size = CalculateControlSize();
            this.HalfSize = size * 0.5f;
            if (isFixedSize)
            {
                HalfSize = new Vector2(Math.Max((int)(1068 / 2), HalfSize.X), Math.Max((int)(240 / 2), HalfSize.Y));
            }
            SetPositions();

        }                

        private static GuiControl MakeCharacterControl(string characterID, Vector2 imageMaxSize)
        {
            var ch = ContentBank.Inst.CharacterBank.Get(characterID);
            if (ch == null)
                ch = new Character(characterID, null);
            return GuiControlFactory.MakeCharacterControl(ch,imageMaxSize);
          
            
        }

        private void SetPositions()
        {

            float verticalPos = -HalfSize.Y + _spacing;
            foreach (var item in children)
            {
                item.LocalPosition = new Vector2(item.LocalPosition.X, verticalPos + item.HalfSize.Y);
                verticalPos += item.Height + _spacing;
            }
        }

        private Vector2 CalculateControlSize()
        {
            Vector2 size = Vector2.One * _spacing;
            foreach (var item in children)
            {
                size.Y = size.Y + item.Height + _spacing;
                size.X = Math.Max(size.X, item.Width + 2 * _spacing);
            }
            return size;
        }
        
        public override void AddChild(GuiControl guiController)
        {
            base.AddChild(guiController);
        }

        public override void UpdateLogic(InputState inputState)
        {            
        }

        public override void Draw(SpriteBatch sb, Color? color)
        {
            base.Draw(sb, color);
        }
    }
}
