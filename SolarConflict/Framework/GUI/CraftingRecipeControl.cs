using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict
{
    [Serializable] 
    class CraftingRecipeControl : GuiControl
    {
        public static Texture2D highlight = TextureBank.Inst.GetTexture("HighlightGreen");
        public Color HighlightColor;

        public Recipe CraftingRecipe { get; set; }
        Dictionary<string, int> _itemCount;        
        public static Color _uncraftableItemColor = new Color(70, 70, 70, 255);
        public float blinkAlpha; //Used for crafting feedbak 

        public int RightInputPressed;
        public bool IsDisabled { get; set; }
        public float DisableMult;

        public bool IsShowCraftingStation;
        

        public CraftingRecipeControl(Recipe craftingRecipe, Dictionary<string, int> itemCount, Vector2 pos, Vector2 size)
            : base(pos, size)
        {
            this.CraftingRecipe = craftingRecipe;
            _itemCount = itemCount;
            this.CursorOverColor = new Color(240, 240, 20, 200);
            TooltipText = craftingRecipe.GetRecipeText(_itemCount);
            DisableMult = 0.3f;
        }

        public override void Update(InputState inputState)
        {
            IsDisabled = !CraftingRecipe.IsCraftable(_itemCount);

            base.Update(inputState);

            if(IsCursorOn)
            {
                TooltipText = CraftingRecipe.GetRecipeText(_itemCount, IsShowCraftingStation);
                if (inputState.Cursor.IsPressedRight)
                {
                    RightInputPressed++;
                }
                else
                {
                    RightInputPressed = 0;
                }
            }
            else
            {
                RightInputPressed = 0;
            }

            blinkAlpha = Math.Max(0, blinkAlpha - 0.05f);
            //blinkAlpha *= 0.9f;
        }     

        protected override void DrawLogic(SpriteBatch sb, Color? color = null)
        {
            base.DrawLogic(sb, color);
            Color itemColor = Color.White;
            ControlColor = Palette.GuiColor * 1.2f ;
            if (IsDisabled)
            {
                itemColor = _uncraftableItemColor;
                ControlColor = ControlColor * DisableMult;
            }
            if (CraftingRecipe != null)
            {
                float sizeMult = 0.9f;
                Rectangle rect = new Rectangle((int)Position.X, (int)Position.Y, (int)(Width* sizeMult), (int)(Height* sizeMult));
                CraftingRecipe.CraftedItem.DrawItemIcon(sb, rect, itemColor);
            }
            if (blinkAlpha > 0)
            {
                var feedbackSprite = Sprite.Get("lightGlow");
                sb.Draw(feedbackSprite, GetRectangle(), new Color(1f, 1f, 1f, blinkAlpha * 0.6f));
            }

            Rectangle rect1 = GetRectangle();
            sb.Draw(highlight, rect1, HighlightColor);
        }
    }
}

