using SolarConflict.Framework.Scenes.Activitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.SimpleGui;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SolarConflict.XnaUtils.SimpleGui;
using SolarConflict.Framework.GUI;
using XnaUtils.SimpleGui.Controllers;
using SolarConflict.Framework;
using XnaUtils.Input;
using XnaUtils.Graphics;
using SolarConflict.Framework.Scenes.HudEngine;
using SolarConflict.AI.GameAI;
using SolarConflict.Framework.Scenes;
using Microsoft.Xna.Framework.Input;
using SolarConflict.Framework.Scenes.Activitys.Shop;

namespace SolarConflict.GameContent.Activities.SceneActivitys
{

    /// <summary>
    /// Shop Activity - get item throw parameters
    /// </summary>
    public class ShopActivity : SceneActivity
    {
        private List<TutorialGoal> tutorialGoals;
        private Sprite guiMarking = Sprite.Get("invArrow");

        private CursorItemControl cursorItem;
        private Agent playerShip;
        private ShopData shopData;
        private RichTextControl moneyControl;
        private GuiManager gui;

        private AgentSlotsControl agentSlotsControl;
        private InventoryControl inventoryControl;
        private ShopInventoryControl allyInventoryControl;        
        private CraftingControl craftingControl;

        public ShopActivity()
        {
            shopData = new ShopData();
            shopData.Inventory = new Inventory(9 * 4);            
        }

        public ShopActivity(ShopData shopData, Scene scene)
        {
            _scene = scene;
            this.shopData = shopData;
            Init(null);
        }

        protected override void Init(ActivityParameters parameters)
        {
            base.Init(parameters);
            tutorialGoals = new List<TutorialGoal>();
            tutorialGoals.AddRange(_scene.GetTutorialGoals());

            gui = new GuiManager();
            playerShip = _scene.PlayerAgent;
            cursorItem = new CursorItemControl(new Vector2(96));
            cursorItem.Sprite = null;
            gui.Root = MakeGui(playerShip, cursorItem, gui);
          
        }

        public void AddItem(string itemID)
        {
            shopData.Inventory.AddItem(itemID);
        }

        

        public GuiControl MakeGui(Agent player, CursorItemControl cursor, GuiManager gui)
        {
            var guiControl = new ControlsGroup();
            HorizontalLayout mainHorizontalLayout = new HorizontalLayout(ActivityManager.ScreenSize / 2);
            guiControl.AddChild(mainHorizontalLayout);
            VerticalLayout verticalLayout = new VerticalLayout(Vector2.Zero);
            mainHorizontalLayout.AddChild(verticalLayout);

            GuiControl playerControl = GuiControlFactory.MakeShopControl(player, shopData, cursor, gui, out allyInventoryControl);                
            verticalLayout.AddChild(playerControl);
            List<Agent> agents = new List<Agent>();
            agents.Add(player);
            List<Inventory> invList = new List<Inventory>();
            cursor.InventoryList = invList;
            invList.Add(player.Inventory);            
            if (player != null)
            {
                agents.Add(player);
                invList.Add(player.Inventory);
                verticalLayout.AddChild(GuiControlFactory.MakeAgentControl(player, cursor, gui, out agentSlotsControl, out inventoryControl));
            }
            mainHorizontalLayout.AddChild(MakeCraftingGui(player, cursor, gui, _scene.GameEngine, agents, invList, _scene, out craftingControl, tutorialGoals.Count == 0));

            moneyControl = new RichTextControl("", isShowFrame:true);
            moneyControl.Position = new Vector2(moneyControl.HalfSize.X + 10, ActivityManager.ScreenSize.Y - moneyControl.HalfSize.Y - 10);
            guiControl.AddChild(moneyControl);

            return guiControl;
        }



        private static Agent GetAllyAgent(Scene scene, Agent agent)
        {
            if (scene?.GameObjectUnderCursor?.GetFactionType() == Framework.FactionType.Player)
            {
                if (scene.GameObjectUnderCursor != agent && scene.GameObjectUnderCursor.GetInventory() != null && scene.GameObjectUnderCursor.GetInventory().Size > 0)
                {
                    return scene.GameObjectUnderCursor as Agent;
                }
            }
            return null;
        }



        public override void Update(InputState inputState)
        {            
            base.Update(inputState);
            //cursorItem.Data = null;
            if (playerShip != null)
            {
                int money = (int)playerShip.GetMeterValue(MeterType.Money);
                moneyControl.Text = money.ToString() + "#image{coin}";
                moneyControl.Position = new Vector2(moneyControl.HalfSize.X + 10, ActivityManager.ScreenSize.Y - moneyControl.HalfSize.Y - 10);
            }
            cursorItem.Position = _scene.InputState.Cursor.Position;
            gui.Update(_scene.InputState);            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawBackground(spriteBatch);
            if (gui.Root != null)
            {
                gui.Draw();
                spriteBatch.Begin();
                cursorItem.Draw(spriteBatch);
                spriteBatch.End();
            }

            base.Draw(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            DrawAllGoals();
            spriteBatch.End();
        }

        private void DrawAllGoals()
        {
            bool isDone = true;
            foreach (var goal in tutorialGoals)
            {
                isDone &= goal.DrawTutorialGoal(craftingControl, cursorItem, agentSlotsControl, inventoryControl, null, allyInventoryControl);
            }

            if (isDone && tutorialGoals.Count > 0)
            {

                ActivityManager.Inst.AddToast("Press ESC to go back", 10);  //Change    
                HudUtils.DrawArrow(Game1.sb, guiMarking, _back.Position);
            }
        }

        public override ActivityParameters OnBack()
        {
            playerShip.control.controlAi = ParameterControl.MakeAIFromAgent(playerShip);
            if (cursorItem.Item != null) //TODO: add if shift is held drop the item or maybe move the item do scene cursour item
            {
                ////_scene.CursorItem = cursorItem.Item;
                if (playerShip.Inventory.HasRoom(cursorItem.Item.ID, cursorItem.Item.Stack))// and not drop to space
                {
                    playerShip.AddItemToInventory(cursorItem.Item);
                }
                else
                {
                    cursorItem.Item.Position = _scene.GameEngine.Camera.GetWorldPos(_scene.InputState.Cursor.Position);
                    _scene.GameEngine.AddGameObject(cursorItem.Item);
                }
                //_cursorItem.Item.Position = _scene.GameEngine.Camera.GetWorldPos(_scene.InputState.Cursor.Position);
                //_scene.GameEngine.AddGameObject(_cursorItem.Item);
            }

            return base.OnBack();
        }

        private static GuiControl MakeCraftingGui(Agent agent, CursorItemControl cursor, GuiManager gui, GameEngine gameEngine, List<Agent> agents, List<Inventory> inventoryList, Scene scene, out CraftingControl craftingControl , bool addRecycle)
        {
            var layout = new VerticalLayout(Vector2.Zero);
            RichTextControl titleControl = new RichTextControl("Crafting");// #image{helpicon}");
            titleControl.IsShowFrame = true;
            layout.AddChild(titleControl);

            CraftingStationType craftingStations = agent.GetCraftingStationType();
            List<GameObject> nearGameObjects = new List<GameObject>();
            gameEngine.CollisionManager.GetAllObjectInRange(agent.Position, agent.Size + 1000, nearGameObjects);
            foreach (var gameObject in nearGameObjects)
            {
                craftingStations |= gameObject.GetCraftingStationType();
            }
            float itemControlSize = 100;
            if (ActivityManager.ScreenSize.X < 1700)
                itemControlSize = 60;
            //Inventory inventory = agent.Inventory;
            craftingControl = new CraftingControl(2, 5, Vector2.One * itemControlSize, agents, inventoryList, cursor, Vector2.Zero, gui, craftingStations);
            craftingControl.CursorOn += gui.ToolTipHandler;
            layout.AddChild(craftingControl);

            titleControl.HalfSize = new Vector2(Math.Max(craftingControl.HalfSize.X, titleControl.HalfSize.X), titleControl.HalfSize.Y);
            if (addRecycle)
            {
                var recycleBin = GuiControlFactory.MakeRecyacleBin(cursor, scene, gui);             
                layout.AddChild(recycleBin);
            }

            return layout;
        }

        private GuiControl MakeCommandsControl(Inventory mainInventory, Agent allyAgent, float width)
        {
            RelativeLayout layout = new RelativeLayout();            
            //layout.Spacing = 4;
            //layout.ShowFrame = true;

            RichTextControl control = new RichTextControl("Sort");
            control.IsShowFrame = true;
            control.Data = mainInventory;
            control.Action += (GuiControl source, CursorInfo cursorLocation) => { (source.Data as Inventory)?.Sort(); };
            layout.AddChild(control, HorizontalAlignment.Left, VerticalAlignment.Center);

            //Drop all
            //Loot all
            //Auto equip
            if (allyAgent != null && allyAgent.Inventory != null)
            {
                control = new RichTextControl("Stash");
                control.IsShowFrame = true;
                control.Data = new Tuple<Inventory, Inventory>(mainInventory, allyAgent.Inventory);
                control.Action += (GuiControl source, CursorInfo cursorLocation) =>
                {
                    Tuple<Inventory, Inventory> invTuple = source.Data as Tuple<Inventory, Inventory>;
                    invTuple.Item1.TryTransferExcept(invTuple.Item2, ItemCategory.None);
                };
                layout.AddChild(control, HorizontalAlignment.Left, VerticalAlignment.Center, layout.LastChildAdded);

                control = new RichTextControl("Loot");
                control.IsShowFrame = true;
                control.Data = new Tuple<Inventory, Inventory>(mainInventory, allyAgent.Inventory);
                control.Action += (GuiControl source, CursorInfo cursorLocation) =>
                {
                    Tuple<Inventory, Inventory> invTuple = source.Data as Tuple<Inventory, Inventory>;
                    invTuple.Item2.TryTransferExcept(invTuple.Item1, ItemCategory.None);
                };
                layout.AddChild(control, HorizontalAlignment.Left, VerticalAlignment.Center, layout.LastChildAdded);
            }

            layout.Width = width;
            return layout;
        }


        public static Activity ActivityProvider(string parameters)
        {
            return new InventoryActivity();
        }
    }
}
