using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict.Framework.Scenes.Components
{
    public class ItemSlotControl:ItemControl // REFACTOR: remove this class
    {
        public float Rotation;
        public ItemSlot ItemSlot { get; set; }

        public ItemSlotControl(ItemSlot itemSlot, Vector2 pos, Vector2 size)
            : base(itemSlot.Item, pos, size)
        {            
            this.ItemSlot = itemSlot;
            //Sprite = Sprite.Get("guif8");
        }

        public override void UpdateLogic(InputState inputState)
        {
            base._item = ItemSlot.Item;

            StringBuilder sb = new StringBuilder();

            if (IsCursorOn) 
            {
                if (_item != null)
                {
                    sb.Append(_item.GetTooltipText(flavor:false, price:true));
                    sb.Append("#line{}");                   
                }
                else
                {
                    
                }
               
                //if(DebugUtils.ShowItemID)
                //    sb.AppendLine(this.Index.ToString()); //Debug           

                sb.Append("Activation: ");                
                sb.Append("(#action{");
                sb.Append(ItemSlot.ActivationSignal.ToString()); //Change
                sb.AppendLine("})");
                sb.AppendLine($"Slot Type: {ItemSlot.Size} {ItemSlot.Type.ToString()}");
                if(_item == null)
                {
                    sb.Append("#line{}");
                    sb.Append("#image{rmb}#color{255,255,0}Right button#dcolor{} - Change activiation");
                }

                if (DebugUtils.Mode == ModeType.Debug)
                {
                    sb.AppendLine(Color.Yellow.ToTag("\nIndex: " + this.Index.ToString()));
                }
                TooltipText = sb.ToString();
            }
        }

        protected override void DrawItem(SpriteBatch sb)
        {
            Rectangle rectangle = new Rectangle((int)(Position.X), (int)(Position.Y), (int)Width, (int)Height);           
            
            if ((ItemSlot.Type & ~(SlotType.Utility | SlotType.Mothership)) != 0) {
                // Slot has a type besides Utility, MothershipUtility, and turret, draw facing arrow
                Sprite arrowTexture = Sprite.Get("WeaponSlot"); // change it
                if ((this.ItemSlot.Type & SlotType.Turret) > 0)
                    arrowTexture = Sprite.Get("TurretSlot");
                if ((this.ItemSlot.Type & (SlotType.Engine | SlotType.MainEngine)) > 0)
                    arrowTexture = Sprite.Get("EngineSlot");
                if ((this.ItemSlot.Type & (SlotType.Engine | SlotType.Weapon) ) >= (SlotType.Engine | SlotType.Weapon))
                    arrowTexture = Sprite.Get("WeaponEngineSlot");
                if ((this.ItemSlot.Type & SlotType.Generator) > 0)
                    arrowTexture = Sprite.Get("GeneratorGUI");
                else
                {
                    if ((this.ItemSlot.Type & SlotType.Shield) > 0)
                        arrowTexture = Sprite.Get("ShieldGUI");
                    else
                    {
                        if ((this.ItemSlot.Type & SlotType.Rotation) > 0)
                            arrowTexture = Sprite.Get("RotationGUI");
                    }
                }
                
                sb.Draw(arrowTexture, rectangle, null, new Color(100,100,100,200), MathHelper.ToRadians(Rotation + ItemSlot.Rotation),
                  new Vector2(arrowTexture.Width / 2, arrowTexture.Height / 2), SpriteEffects.None, 0);
            }                      
            if (base._item != null)
            {
                base._item.DrawItemIcon(sb, rectangle);
                        
                if (base._item.Stack > 1)
                {
                    SpriteFont font = Game1.font; //TODO:FromFont Bank, maybe IFont
                    Vector2 pos = Position + new Vector2(halfWidth, halfHeight) * 0.8f;

                    string text = base._item.Stack.ToString();
                    Vector2 size = font.MeasureString(text);
                    sb.DrawString(font, text, pos - size * 0.5f, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                }
            }

            if (ItemSlot.ActivationSignal != ControlSignals.None && ItemSlot.ActivationSignal != ControlSignals.AlwaysOn) {
                // KLUDGE: draw text keybindings centered, icons in the bottom right, maybe paint a diffrent marking to always on
                if (ItemSlot.ActivationSignal != ControlSignals.None)
                {
                    var key = KeysSettings.Data.KeyBindings[ItemSlot.ActivationSignal];
                    var centered = key.GetIcon() == null;
                    ItemIndicator.DrawKeyBinding(Game1.sb, Position, ItemSlot.ActivationSignal, centered);
                }
            }

            //if (DebugUtils.Mode == ModeType.Debug)
            //{
            //    SpriteFont font = Game1.font; //TODO:FromFont Bank, maybe IFont
            //    Vector2 pos = Position - new Vector2(halfWidth, halfHeight) * 0.8f;

            //    string text = Index.ToString();
            //    Vector2 size = font.MeasureString(text);
            //    for (int dx = -1; dx <= 1; dx++)
            //    {
            //        for (int dy = -1; dy <= 1; dy++)
            //        {
            //            sb.DrawString(font, text, pos - size * 0.5f + new Vector2(dx,dy), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            //        }
            //    }

            //    sb.DrawString(font, text, pos - size * 0.5f, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            //}
        }

        protected override void DrawLogic(SpriteBatch sb, Color? color = null)
        {
            //add stuff
            base.DrawLogic(sb, color);
        }
    }
}
