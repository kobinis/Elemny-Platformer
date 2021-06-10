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
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.GameContent.Activities.Tests
{
    class ShowRecipesActivity:SceneActivity
    {
        GuiManager gui;
        public ShowRecipesActivity():base(false)
        { }

        protected override void Init(ActivityParameters parameters)
        {
            base.Init(parameters);
            gui = new GuiManager();
            MakeGui(CraftingStationType.All);
            
        }

        public void MakeGui(CraftingStationType carftingStation = CraftingStationType.All)
        {
            gui.Root = new VerticalLayout(ActivityManager.ScreenCenter);
            var recipes = ContentBank.Inst.GetAllRecipes(carftingStation);

            RadioSelectionGroup radio = new RadioSelectionGroup();
            GridControl stationGrid = new GridControl(9, 2, Vector2.One * 96);
            for (int i = 0; i < 18; i++)
            {
                CraftingStationType craftingStation = (CraftingStationType)(2 << i);
                ImageControl stationToggle = new ImageControl(TextureBank.Inst.GetSprite(craftingStation.GetSpriteID()), Vector2.Zero, Vector2.One * 96);
                stationToggle.Data = craftingStation;
                stationToggle.TooltipText = craftingStation.GetUserName();
                stationToggle.Sprite = TextureBank.Inst.GetSprite(craftingStation.GetSpriteID());
                stationToggle.CursorOn += gui.ToolTipHandler;
                stationToggle.Action += StationToggle_Action;
                radio.AddControl(stationToggle);
                stationGrid.AddChild(stationToggle);
            }
            gui.Root.AddChild(stationGrid);
            ScrollableGrid grid = new ScrollableGrid(8, 6, Vector2.One * 100);
            var itemsCount = new Dictionary<string, int>();
            foreach (var recipe in recipes)
            {
                var recipeControl = new CraftingRecipeControl(recipe, itemsCount, Vector2.Zero, Vector2.One * 64);
                recipeControl.CursorOn += gui.ToolTipHandler;
                grid.AddChild(recipeControl);
            }
            gui.Root.AddChild(grid);
        }

        private void StationToggle_Action(GuiControl source, global::XnaUtils.Input.CursorInfo cursorLocation)
        {
            MakeGui((CraftingStationType)source.Data);
        }

        public override void Update(InputState inputState)
        {
            ActivityManager.GraphicsDevice.Clear(Color.Turquoise);
            base.Update(inputState);
            gui.Update(inputState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            gui.Draw();
            base.Draw(spriteBatch); 
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new ShowRecipesActivity();
        }

    }
}
