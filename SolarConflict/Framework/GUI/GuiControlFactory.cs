using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
using SolarConflict.Framework.Scenes.Activitys.Shop;
using SolarConflict.XnaUtils.SimpleGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.Input;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.Framework.GUI
{
    /// <summary>
    /// Holds factory methods for GUI controls
    /// </summary>
    public static class GuiControlFactory
    {

        public static GuiControl MakeCharacterControl(Character character, Vector2 imageMaxSize)
        {

            VerticalLayout layout = new VerticalLayout(Vector2.Zero);
            layout.Sprite = Sprite.Get("guiframe");
            layout.Spacing = 4;
            layout.SetAllColors(Palette.GuiBody.Multiply(1.6f), false);
            layout.ShowFrame = true;
            var sprite = Sprite.Get(character.SpriteID);
            if (imageMaxSize == Vector2.Zero)
            {
                imageMaxSize = new Vector2(sprite.Width, sprite.Height);
            }
            ImageControl imageControl = new ImageControl(sprite, Vector2.Zero, imageMaxSize);
            imageControl.IsConsumingInput = false;
            layout.AddChild(imageControl);
            if (character.Name != null)
            {
                TextControl name = new TextControl(character.Name);
                layout.AddChild(name);
            }

            return layout;
        }

        public static Agent GetAllyAgent(Scene scene, Agent agent) //Move it
        {
            if (scene?.GameObjectUnderCursor?.GetFactionType() == Framework.FactionType.Player)
            {
                if (scene.GameObjectUnderCursor != agent && scene.GameObjectUnderCursor.GetInventory() != null && scene.GameObjectUnderCursor.GetInventory().Size > 0)
                {
                    return scene.GameObjectUnderCursor as Agent;
                }
            }
            var gameObjects = new List<GameObject>();
            float minDis = 1500;
            scene.GameEngine.CollisionManager.GetAllObjectPossiblyInRange(agent.Position, minDis, gameObjects);
            GameObject ally = null;

            foreach (var gameObj in gameObjects)
            {
                if (gameObj.GetFactionType() == FactionType.Player && gameObj is Agent && gameObj != agent)
                {
                    float dis = GameObject.DistanceFromEdge(agent, gameObj);
                    if (dis <= minDis)
                    {
                        ally = gameObj;
                        minDis = dis;
                    }
                }

            }
            return ally as Agent;
        }

        public static CraftingStationType GetCraftingStationTypes(Agent agent, GameEngine gameEngine) //Move it
        {
            CraftingStationType craftingStations = agent.GetCraftingStationType();
            List<GameObject> nearGameObjects = new List<GameObject>();
            gameEngine.CollisionManager.GetAllObjectInRange(agent.Position, agent.Size + 1000, nearGameObjects);
            foreach (var gameObject in nearGameObjects)
            {
                craftingStations |= gameObject.GetCraftingStationType();
            }
            return craftingStations;
        }

        
        public static GuiControl MakeCraftingGui(Agent agent, CursorItemControl cursor, GuiManager gui, GameEngine gameEngine, List<Agent> agents, List<Inventory> inventoryList, Scene scene, out CraftingControl craftingControl, bool addRecycle , Point binNum, CraftingStationType craftingStations = CraftingStationType.None, string title = null)
        {
            if (title == null)
                title = "Crafting";
            var layout = new VerticalLayout(Vector2.Zero); //Layout holding categories and crafting control
            layout.Spacing = Math.Max( 3 * GuiManager.Scale,1);
            RichTextControl titleControl = new RichTextControl(title); // #image{helpicon}"); //Add help
            titleControl.IsShowFrame = true;
            layout.AddChild(titleControl);

           
            float itemControlSize = 100 * GuiManager.Scale;

            bool addCategories = true;
            if (craftingStations == CraftingStationType.None)
            {
                craftingStations = GetCraftingStationTypes(agent, gameEngine);
                craftingControl = new CraftingControl(binNum.X, binNum.Y, Vector2.One * itemControlSize, agents, inventoryList, cursor, Vector2.Zero, gui, craftingStations);
            }
            else
            {
                addCategories = false;
                craftingControl = new CraftingControl(binNum.X, binNum.Y, Vector2.One * itemControlSize, agents, inventoryList, cursor, Vector2.Zero, gui, craftingStations, false);
            }

            craftingControl.CursorOn += gui.ToolTipHandler;


            SlotType[] slotTypes = { SlotType.All,    SlotType.Shield,    SlotType.Weapon | SlotType.Turret, SlotType.Utility, SlotType.Consumable | SlotType.Ammo
                                     ,SlotType.CraftingStation ,SlotType.Engine, SlotType.Generator, SlotType.Rotation , SlotType.Mothership};
            string[] slotNames = { "All items", "Shield", "Weapons", "Utility", "Consumables",
                                   "Crafting stations", "Engines", "Generators", "Gyro", "Mothership Utility"};
            string[] icons = {"allIcon","ShieldIcon","WeaponsIcon","UtilIcon", "consumbleIcon",
                               "Crafting1Icon","engineIcon","GeneratorIcon","RotationIcon", "MothershipIcon"};
            int spacing = 10;
            Vector2 butSize = Vector2.One * Math.Min( Math.Max((((craftingControl.Width - spacing) / (slotTypes.Length / 2f)) - spacing), 16),50);
            RadioSelectionGroup radio = new RadioSelectionGroup();
            GridControl categoriesControl = new GridControl(5, 2, butSize);
            for (int i = 0; i < slotTypes.Length; i++) //TODO: GuiControlSelector
            {
                SlotType slotType = slotTypes[i];
                GuiControl control = new GuiControl(Vector2.Zero, butSize);
                control.CursorOn += gui.ToolTipHandler;
                control.Sprite = Sprite.Get("guiframe"); //Sprite.Get(icons[i]); //
                control.PressedControlColor = Color.Yellow;
                control.IsToggleable = true;
                control.TooltipText = slotNames[i];
                ImageControl image = new ImageControl(Sprite.Get(icons[i]), Vector2.Zero, butSize * 0.9f);
                image.IsConsumingInput = false;
                control.AddChild(image);
                
                control.Data = new Tuple<CraftingControl, SlotType>(craftingControl, slotType);
                
                control.Action += delegate (GuiControl source, CursorInfo cursorLocation)
                {
                    var tuple = source.Data as Tuple<CraftingControl, SlotType>;
                    tuple.Item1.FilterBySlotType(tuple.Item2);
                };

                control.LogicFunction = delegate(GuiControl source, InputState inputState)
                {                    
                    //var tuple = source.Data as Tuple<CraftingControl, SlotType>; 
                    //CraftingControl sourceCraftingControl = tuple.Item1;
                    //var list = sourceCraftingControl.GetRecipeList(tuple.Item2, true);
                    //if (list.Count == 0)
                    //{
                    //    source.DisableAll = true;
                        
                    //}
                    //else
                    //{
                    //    source.DisableAll = false;
                    //}
                };

                categoriesControl.AddChild(control);
                radio.AddControl(control);
            }

            if (addCategories) //TODO: check, fix const
                layout.AddChild(categoriesControl);

            layout.AddChild(craftingControl);

            titleControl.HalfSize = new Vector2(Math.Max(craftingControl.HalfSize.X, titleControl.HalfSize.X), titleControl.HalfSize.Y);
            if (addRecycle) //TODO:Move out
            {
                var recycleBin = MakeRecyacleBin(cursor, scene, gui);
                layout.AddChild(recycleBin);
            }

            return layout;
        }

        /// <summary>
        /// A control that shows both agent slots and inventory
        /// </summary>
        public static GuiControl MakeAgentControl(Agent agent, CursorItemControl cursor, GuiManager gui, out AgentSlotsControl slotsControl, out InventoryControl invControl)
        {
            invControl = null;
            slotsControl = null;
            HorizontalLayout layout = new HorizontalLayout(Vector2.Zero);
           // layout.IsConsumingInput = false;
            layout.ShowFrame = true;
            if (agent.ItemSlotsContainer != null)
            {
                var agentSlotcontrol = new AgentSlotsControl(agent, cursor, gui);
                slotsControl = agentSlotcontrol;
                layout.AddChild(agentSlotcontrol);
            }
            if (agent.Inventory != null)
            {
                float itenControlSize = 70 * GuiManager.Scale;

                int numberOflines = Math.Min((int)Math.Ceiling(agent.Inventory.Size / 9f), 5);
                var inventoryControl = new InventoryControl(agent.Inventory, cursor, Vector2.Zero, gui, lineNum: numberOflines, itemSize: itenControlSize);
                invControl = inventoryControl;
                layout.AddChild(inventoryControl);
            }
            return layout;
        }

        public static GuiControl MakeCharacterControl(Character character, Vector2 size, Faction faction = null)
        {
            VerticalLayout layout = new VerticalLayout(Vector2.Zero, 5, false);
            string name = character.Name;
            RichTextControl nameControl = new RichTextControl(name, Game1.menuFont, false);
            size.X = Math.Max(size.X, nameControl.Width);
            size.Y = Math.Max(size.X, size.Y); //??
            ImageControl imageControl = new ImageControl(Sprite.Get(character.SpriteID), Vector2.Zero, size);
            layout.AddChilds(imageControl, nameControl);
            if (faction != null)
                layout.AddChild(MakeFactionInfoControl(faction));
            return layout;
        }

        public static GuiControl MakeFactionInfoControl(Faction faction)
        {
            var layout = new VerticalLayout(Vector2.Zero);
           // RelativeLayout layout = new RelativeLayout();
            RichTextControl factionName = new RichTextControl(faction.ToTag(), Game1.menuFont);

            RichTextControl relations = new RichTextControl(faction.GetPlayerRelationsString());
       
            layout.AddChild(factionName);
            layout.AddChild(relations);
            layout.IsUpdatingPosition = true;
            // layout.AddChild()
            return layout;
        }

        /// <summary>
        /// Makes a control that shows shop inventory and portrait
        /// </summary>
        public static GuiControl MakeShopControl(Agent player, ShopData shopData, ItemControl cursor, GuiManager gui, out ShopInventoryControl invControl)
        {
            invControl = null;            
            HorizontalLayout layout = new HorizontalLayout(Vector2.Zero);
            layout.ShowFrame = true;
            if (shopData.Portrait != null)
            {
                float imageSize = 400;
                if (ActivityManager.ScreenSize.X < 1700)
                    imageSize = 250;
                ImageControl image = new ImageControl(shopData.Portrait, Vector2.Zero, Vector2.One * imageSize);
                layout.AddChild(image);
            }

            if (shopData.Inventory != null)
            {
                //float itemSize = 70;
                //if (ActivityManager.ScreenSize.X < 1700)
                //    itemSize = 50;
                var inventoryControl = new ShopInventoryControl(shopData.Inventory, cursor, gui, player, shopData.ItemPriceMultiplier);
                invControl = inventoryControl;
                layout.AddChild(inventoryControl);
            }                        
            return layout;
        }


        /// <summary>
        /// Makes a control that disposes of an item when placed in it
        /// </summary>       
        public static GuiControl MakeRecyacleBin(ItemControl _cursorItem, Scene scene, GuiManager gui)
        {
            GuiControl control = new GuiControl(Vector2.Zero, Vector2.One * 100);
            control.CursorOn += gui.ToolTipHandler;
            control.Data = scene;
            //control.ControlColor = GuiManager.DefalutGuiColor;
            //control.PressedControlColor = GuiManager.DefalutGuiColor;
            ImageControl imageControl = new ImageControl(Sprite.Get("trash"), Vector2.Zero, Vector2.One * 90);
            imageControl.ControlColor = Color.White;
            imageControl.IsConsumingInput = false;
            control.TooltipText = "Place an item to recycle it.";
            control.AddChild(imageControl);
            control.CursorOn += delegate (GuiControl source, CursorInfo cursorLocation)
            {
                if (_cursorItem.Item != null)
                {
                    float money = _cursorItem.TotalSellPrice * 0.5f;
                    source.TooltipText = "Recycle item for " + Palette.Highlight.ToTag(money.ToString()) + Sprite.Get("coin").ToTag();
                }
                else
                {
                    source.TooltipText = "Place an item to recycle it.";
                }
            };

            control.Action += delegate (GuiControl source, CursorInfo cursorLocation)
            {
                if (_cursorItem != null)
                {
                    float money = _cursorItem.TotalSellPrice * 0.5f;
                    (source.Data as Scene)?.GetPlayerFaction().AddValueToMeter(MeterType.Money, money);
                    _cursorItem.Item = null;
                }
            };

            return control;
        }

        public static GuiControl MakeHangerControl(GuiManager gui, Faction faction, GameEngine gameEngine)
        {
            FleetSystem fleetSystem = faction.MothershipHanger;
            HorizontalLayout layout = new HorizontalLayout(Vector2.Zero);
            foreach (var slot in fleetSystem.FleetSlots)
            {
                layout.AddChild(MakeFleetSlotControl(gui, slot, gameEngine));
            }
            return layout;
        }

        private static GuiControl MakeFleetSlotControl(GuiManager gui, FleetSystem.FleetSlot slot, GameEngine gameEngine)
        {
            VerticalLayout control = new VerticalLayout(Vector2.Zero, 10, true);
            HorizontalLayout bouttons = new HorizontalLayout(Vector2.Zero);

            GuiControl toggleBuild = new GuiControl(Vector2.Zero, Vector2.One * 50);
            toggleBuild.IsToggleable = true;
            toggleBuild.IsPressed = slot.IsRebuild;
            toggleBuild.TooltipText = "Toggle Auto Build";
            toggleBuild.CursorOn += gui.ToolTipHandler;
            toggleBuild.Data = slot;
            toggleBuild.Sprite = Sprite.Get("Ongoing");
            toggleBuild.PressedSprite = Sprite.Get("Completed");            
            toggleBuild.LogicFunction += (GuiControl toggleControl, InputState input) =>
            {
                (toggleControl.Data as FleetSystem.FleetSlot).IsRebuild = toggleControl.IsPressed;
                //toggleBuild.TooltipText = toggleControl.IsPressed.ToString();
            };
            bouttons.AddChild(toggleBuild);

            ImageControl deleteControl = new ImageControl(Sprite.Get("exit"), Vector2.Zero, Vector2.One * 50);
            deleteControl.TooltipText = "Remove ship";
            deleteControl.CursorOn += gui.ToolTipHandler;
            deleteControl.Data = new Tuple<FleetSystem.FleetSlot, GameEngine>(slot, gameEngine);
            deleteControl.Action += (GuiControl source, CursorInfo cursorLocation) => 
            {
                if (gameEngine.GetFaction(FactionType.Player).MothershipHanger.FleetShips.Count > 1)
                {
                    var tuple = (source.Data as Tuple<FleetSystem.FleetSlot, GameEngine>);
                    var fleetSlot = tuple.Item1;
                    var engine = tuple.Item2;
                    FleetSystem.RemoveAgent(fleetSlot, engine);
                }
                else
                {
                    ActivityManager.Inst.AddToast("#color{255,0,0}You must have one ship deployed#dcolor{}", 60);
                }
            };

            bouttons.AddChild(deleteControl);
            control.AddChild(bouttons);

            AgentGuiControl agentControl = new AgentGuiControl(slot.Agent);
            agentControl.Data = slot;
           
            agentControl.LogicFunction = (GuiControl guiControl, InputState inputState) => {
                AgentGuiControl agentSlotControl = guiControl as AgentGuiControl;
                agentSlotControl.Agent = (guiControl.Data as FleetSystem.FleetSlot).Agent;
                agentSlotControl.TooltipText = string.Empty;
                if (agentSlotControl.Agent != null)
                {
                    agentSlotControl.TooltipText = agentSlotControl.Agent.IsActive ? "#color{0,255,0}Deployed#dcolor{}" : "Undeployed";
                    agentSlotControl.TooltipText += "\n" + AgentUtils.DescribeStatsAndAbilities(agentSlotControl.Agent);
                }
                agentSlotControl.TooltipText += "\n" + slot.Command.ToString();
            };
            agentControl.CursorOn += gui.ToolTipHandler;
           // control.TooltipText = "Blaaaaa";
            control.AddChild(agentControl);
          //  control.CursorOn += (source, cursor) => { agentSlotControl };
            control.CursorOn += gui.ToolTipHandler;

            RichTextControl commandControl = new RichTextControl("Command", isShowFrame:true);
            commandControl.TooltipText = slot.GetTooltip();
            commandControl.CursorOn += gui.ToolTipHandler;
            commandControl.SetData("gui", gui);
            commandControl.Action += (s, c) =>
            {
                GuiManager guiManger = s.GetData("gui") as GuiManager;
          //      guiManger.AddControl(MakeCommandSelect(guiManger, s, s.Position, slot));
         //       s.Disable = true;

            };
            control.AddChild(commandControl);
            
            return control;
        }

        public static GuiControl MakeCommandSelect(GuiManager gui, GuiControl callingControl, Vector2 position, FleetSystem.FleetSlot slot)
        {
            VerticalLayout layout = new VerticalLayout(position);
            layout.ControlColor = Color.White;
            layout.ShowFrame = true;
            layout.IsResizeChildrenHorizontally = true;
            RadioSelectionGroup radio = new RadioSelectionGroup();
            radio.SelectedControlIndex = (int)slot.Command;
            for (int i = 0; i < (int)FleetCommandType.Group; i++)
            {
                FleetCommandType commandType = (FleetCommandType)i;
                RichTextControl command = new RichTextControl(commandType.ToString(), isShowFrame:true);
                radio.AddControl(command);
                command.IsToggleable = true;
                command.SetData("calling", callingControl);
                command.SetData("control", layout);
                command.SetData("gui", gui);
                command.SetData("slot", slot);
                command.Index = i;
                command.PressedControlColor = Color.Yellow;
                command.Action += (s,c) => {
                    (s.GetData("slot") as FleetSystem.FleetSlot).Command = (FleetCommandType)i;
                    (s.GetData("calling") as GuiControl).Disable = false;
                    (s.GetData("gui") as GuiManager).RemoveControl( (s.GetData("control") as GuiControl));  
                };
                layout.AddChild(command);
            }
            return layout;
        }

        //private static void CommandControl_Action(GuiControl source, CursorInfo cursorLocation)
        //{
        //    throw new NotImplementedException();
        //}

        public static GuiControl MakeKnownHullControl(GuiManager gui, Faction faction)
        {
            Dictionary<string, int> hullCounter = faction.hullPartsCounter;
            Vector2 binSize = new Vector2(200);
            int xNum = (int)Math.Floor(ActivityManager.ScreenSize.X * 0.7 / binSize.X);
            int yNum = (int)Math.Floor(ActivityManager.ScreenSize.Y * 0.4 / binSize.Y);
            ScrollableGrid grid = new ScrollableGrid(xNum, yNum, binSize);
            foreach (var pair in hullCounter)
            {
                Agent agent = ContentBank.Inst.GetEmitter(pair.Key) as Agent;
                GuiControl control = new GuiControl(Vector2.Zero, binSize);
                control.Sprite = Sprite.Get("guif8");
                control.ControlColor = new Color(Palette.GuiBody.ToVector4() + new Vector4(0.1f, 0.1f, 0.1f, 0));
                control.CursorOverColor = Color.Yellow;
                control.PressedControlColor = new Color(200, 200, 0);
                float size = Math.Min(binSize.X, agent.Sprite.Width);
                ImageControl image = new ImageControl(agent.Sprite, Vector2.Zero, Vector2.One * size); //TODO: change to sizedImage
                control.AddChild(image);
                image.IsConsumingInput = false;
                control.TooltipText = string.Empty;
                if (pair.Value < Consts.NeededHullPart)
                {
                    //control.ControlColor = new Color(Palette.GuiBody.ToVector4() - new Vector4(0.1f, 0.1f, 0.1f, 0));
                    control.TooltipText = Color.Yellow.ToTag("Blueprint parts: " + pair.Value.ToString() + " / " + Consts.NeededHullPart+"\n");
                    image.ControlColor = Color.Black;
                }
                control.TooltipText += "#simage{" + agent.GetSprite().ID + ",300,300}#nl{}" + AgentUtils.DescribeAgentHull(agent);
                control.CursorOn += gui.ToolTipHandler;
                control.Data = faction;
                control.UserData = agent.ID;
                control.Action += (GuiControl source, CursorInfo cursorLocation) => 
                {
                    var cFaction = source.Data as Faction;
                    int? freeSlot = cFaction.MothershipHanger.FindFreeSlotIndex();
                    if (freeSlot != null)
                    {
                        if (cFaction.hullPartsCounter[source.UserData] >= Consts.NeededHullPart)
                            cFaction.MothershipHanger.AddShipCopyToSlot(freeSlot.Value, source.UserData);
                        else
                            ActivityManager.Inst.AddToast("Missing blueprint parts", 20);
                    }
                };                
                grid.AddChild(control);
            }
            return grid;
        }

        public static GuiControl MakeRewardControl(Reward reward, GuiManager gui)
        {
            HorizontalLayout layout = new HorizontalLayout(Vector2.Zero, showFrame: true);
            StringBuilder sb = new StringBuilder();
            sb.Append("Reard\n#line{}");
            if (reward.Money > 0)
                sb.Append(reward.Money.ToString() + " #image{coin}");
            if(reward.ReputationDelta != 0)
            {
                Color col = reward.ReputationDelta > 0 ? Color.Green : Color.Red;
                sb.Append(col.ToTag() + reward.ReputationDelta.ToString() + " " + MetaWorld.Inst.GetFaction(reward.Faction).ToTag());
            }
            RichTextControl title = new RichTextControl("Reward:\n#line{}");

            if(reward.Items.Count > 0)
            {
                HorizontalLayout itemLayout = new HorizontalLayout(Vector2.Zero);
                foreach (var itemTuple in reward.Items)
                {
                    Item item = ContentBank.Inst.GetItem(itemTuple.Item1, true);
                    item.Stack = itemTuple.Item2;
                    ItemControl itemControl = new ItemControl(item, Vector2.Zero, Vector2.One * 50);
                    itemControl.CursorOn += gui.ToolTipHandler;
                    itemLayout.AddChild(itemControl); 
                }
                layout.AddChild(itemLayout);
            }
            
                

            return layout;
        }
    }
}
