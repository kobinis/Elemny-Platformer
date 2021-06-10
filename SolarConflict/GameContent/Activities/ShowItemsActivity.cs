using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.SimpleGui;
using Microsoft.Xna.Framework;
using SolarConflict.XnaUtils.SimpleGui;

namespace SolarConflict.GameContent.Activities
{
    class ShowItemsActivity : Activity
    {
        private GuiManager _gui;
        private List<Item> _items;
        private string _text;

        protected override void Init(ActivityParameters parameters)
        {
            _items = GetAllItems();
            _gui = CreateGui(_items);
            _text = " 1 - All Items,  2- by Category, 3 - by Level";
        }

        public override void Update(InputState inputState)
        {
            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                ActivityManager.Inst.Back();
            }
            if (inputState.IsGKeyPressed(Microsoft.Xna.Framework.Input.Keys.D1))
            {
                _gui = CreateGui(_items);
            }
            if (inputState.IsGKeyPressed(Microsoft.Xna.Framework.Input.Keys.D2))
            {
                _gui = CreateGuiByCategory(_items);
                _text = "Category";
            }

            if (inputState.IsGKeyPressed(Microsoft.Xna.Framework.Input.Keys.D3))
            {
                _gui = CreateGuiByLevel(_items);
                _text = "Level";
            }

            _gui.Update(inputState);
            ActivityManager.Inst.AddToast(_text, 10);

            
        }

        public override void Draw(SpriteBatch sb)
        {
            ActivityManager.GraphicsDevice.Clear(Color.Black);
            _gui.Draw();
        }

        public static Activity ActivityProvider(string param)
        {
            return new ShowItemsActivity();
        }

        private GuiManager CreateGuiByLevel(List<Item> items)
        {
            GuiManager gui = new GuiManager();
            Rectangle screen = ActivityManager.ScreenRectangle;
            Vector2 position = new Vector2(screen.Width / 2, screen.Height / 2);

            Vector2 gridCellSize = Vector2.One * 80;
            Dictionary<int, List<Item>> categories = new Dictionary<int, List<Item>>();
            for (int i = 0; i <= 11; i++)
            {
                categories.Add(i,new List<Item>());
            }
            foreach (Item item in items)
            {
                if ((item.SlotType & (SlotType.Weapon | SlotType.Utility)) > 0 || true)
                {
                    int cat = (int)item.Profile.Level;
                    if (categories.ContainsKey(cat))
                        categories[cat].Add(item);
                    else
                        categories[cat] = new List<Item>() { item };
                }
            }
            int maxItemsInCategory = 0;
            foreach (var entry in categories)
            {
                int numItems = entry.Value.Count;
                maxItemsInCategory = Math.Max(numItems, maxItemsInCategory);
            }
            VerticalLayout layout = new VerticalLayout(position);
            gui.AddControl(layout);
            int itemsInRow = (int)(screen.Width / gridCellSize.X) - 3;
            float gridCellWidth = gridCellSize.X * (itemsInRow + 2);
            float gridCellHeight = (gridCellSize.Y + 10) * (float)Math.Ceiling(maxItemsInCategory / (float)itemsInRow);
            ScrollableGrid grid = new ScrollableGrid(1, (int)(screen.Height / gridCellHeight), new Vector2(gridCellWidth, gridCellHeight));
            layout.AddChild(grid);

            foreach (var entry in categories)
            {
                List<Item> catItems = entry.Value;
                catItems = catItems.OrderBy(x => x.SlotType).ToList();
                var catGrid = new ScrollableGrid(itemsInRow, (int)Math.Ceiling(catItems.Count / (double)itemsInRow), gridCellSize);
                foreach (Item item in catItems)
                {
                    GuiControl itemControl = new ItemControl(item, Vector2.Zero, gridCellSize);
                    itemControl.CursorOn += gui.ToolTipHandler;
                    catGrid.AddChild(itemControl);
                }
                grid.AddChild(catGrid);
            }

            return gui;
        }

        private GuiManager CreateGuiByCategory(List<Item> items)
        {
            GuiManager gui = new GuiManager();
            Rectangle screen = ActivityManager.ScreenRectangle;
            Vector2 position = new Vector2(screen.Width / 2, screen.Height / 2);

            Vector2 gridCellSize = Vector2.One * 80;
            Dictionary<ItemCategory, List<Item>> categories = new Dictionary<ItemCategory, List<Item>>();
            foreach (Item item in items)
            {
                ItemCategory cat = item.Profile.Category;
                if (categories.ContainsKey(cat))
                    categories[cat].Add(item);
                else
                    categories[cat] = new List<Item>() { item };
            }
            int maxItemsInCategory = 0;
            foreach (var entry in categories)
            {
                int numItems = entry.Value.Count;
                maxItemsInCategory = Math.Max(numItems, maxItemsInCategory);
            }
            VerticalLayout layout = new VerticalLayout(position);
            gui.AddControl(layout);
            int itemsInRow = (int)(screen.Width / gridCellSize.X) - 3;
            float gridCellWidth = gridCellSize.X * (itemsInRow + 2);
            float gridCellHeight = (gridCellSize.Y + 10) * (float)Math.Ceiling(maxItemsInCategory / (float)itemsInRow);
            ScrollableGrid grid = new ScrollableGrid(1, (int) (screen.Height / gridCellHeight), new Vector2(gridCellWidth, gridCellHeight));
            layout.AddChild(grid);

            foreach (var entry in categories)
            {
                List<Item> catItems = entry.Value;
                var catGrid = new ScrollableGrid(itemsInRow, (int) Math.Ceiling(catItems.Count / (double)itemsInRow) , gridCellSize);
                foreach (Item item in catItems)
                {
                    GuiControl itemControl = new ItemControl(item, Vector2.Zero, gridCellSize);
                    itemControl.CursorOn += gui.ToolTipHandler;
                   // if(ContentBank.Inst.GetEmitter()
                    catGrid.AddChild(itemControl);
                }
                grid.AddChild(catGrid);
            }

            return gui;
        }

        private GuiManager CreateGui(List<Item> items)
        {
            GuiManager gui = new GuiManager();
            Rectangle screen = ActivityManager.ScreenRectangle;
            Vector2 position = new Vector2(screen.Width / 2, screen.Height / 2);
            HorizontalLayout layout = new HorizontalLayout(position);
            layout.ShowFrame = true;
            gui.AddControl(layout);

            Vector2 gridCellSize = Vector2.One * 80;
            var grid = new ScrollableGrid(10, 10, gridCellSize);
            foreach (Item item in items)
            {
                GuiControl itemControl = new ItemControl(item, Vector2.Zero, gridCellSize);
                if (ContentBank.Inst.TryGetEmitter(item.ID+ "Recipe") == null)
                    itemControl.ControlColor = Color.Yellow;
                itemControl.CursorOn += gui.ToolTipHandler;
                grid.AddChild(itemControl);
            }
            layout.AddChild(grid);

            return gui;
        }

        private List<Item> GetAllItems()
        {
            return ContentBank.Inst.GetAllItemsCopied();
        }
    }
}
