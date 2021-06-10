//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.Scenes.Activitys;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils;
//using XnaUtils.SimpleGui;
//using XnaUtils.SimpleGui.Controllers;
//using Microsoft.Xna.Framework.Graphics;
//using SolarConflict.Session;
//using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
//using XnaUtils.Graphics;
//using XnaUtils.Input;
//using SolarConflict.Framework.Utils;
//using System.Diagnostics;
//using SolarConflict.XnaUtils.SimpleGui;
//using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;

//namespace SolarConflict.GameContent.Activities.SceneComponents {
//    class FleetManagerActivityOld : SceneActivity {        
//        private GuiManager _gui;
//        private Faction _faction;
//        private FleetSystem _hangar;

//        protected override void Init(ActivityParameters parameters) {
//            _scene = parameters.DataParams["Scene"] as Scene;
//            _gui = new GuiManager();
//            var layout = new HorizontalLayout(ActivityManager.ScreenSize * 0.5f);
//            layout.ShowFrame = true;
//            _gui.AddControl(layout); ;
//            _faction = GameSession.Inst.MetaWorld.GetFaction(Framework.FactionType.Player);
//            _hangar = _faction.MothershipHanger;
//            if (_hangar != null) {
//                foreach (var slot in _hangar.FleetSlots) {
//                    var hangarControl = new FleetSlotControl(_scene, slot, _gui); //TODO: witch to hangerControl
//                    layout.AddChild(hangarControl);
//                }
//            }
//        }

//        public override bool Update(InputState inputState) {
//            base.Update(inputState);
//            _gui.Update(inputState);
//            return false;
//        }

//        public override void Draw(SpriteBatch spriteBatch) {
//            DrawBackground(spriteBatch);
//            base.Draw(spriteBatch);
//            _gui.Draw();
//        }

//        public static Activity ActivityProvider(string parameters) {
//            return new FleetManagerActivityOld();
//        }
//    }

//    public class FleetSlotControl : GuiControl {

//        /// <summary>Index of corresponding slot in the player faction mothership's MotherShipFleetSystem</summary>
//        FleetSystem.FleetSlot _slot;

//        AgentGuiControl _agentControl;
//        GuiControl _destroyButton;
//        GuiControl _inventoryButton;
//        GuiControl _mainLayout;
//        GuiControl _rebuildToggle;

//        Scene _scene;

//        public FleetSlotControl(Scene scene, FleetSystem.FleetSlot slot, GuiManager gui) : base(Vector2.Zero, new Vector2(150, 200)) {
//            // TODO: some frames need hiding, layout bounds are a bit wonky
//            _scene = scene;
//            _slot = slot;

//            var horizontal = new HorizontalLayout(Vector2.Zero);            

//            _inventoryButton = new GuiControl(Vector2.Zero, new Vector2(40, 40));
//            _inventoryButton.CursorOn += OnMouseoverButton;
//            _inventoryButton.ControlColor = new Color(0f, 1f, 0f);
//            _inventoryButton.TooltipText = "Edit ship loadout";
//            _inventoryButton.CursorOn += gui.ToolTipHandler;
//            horizontal.AddChild(_inventoryButton);

//            //_rebuildToggle = new ImageControl(Sprite.Get("replace"), Vector2.Zero, new Vector2(50, 50));
//            _rebuildToggle = new GuiControl(Vector2.Zero, new Vector2(40, 40));
//            _rebuildToggle.IsToggleable = true;
//            _rebuildToggle.IsPressed = _slot.Rebuild;
//            _rebuildToggle.TooltipText += "Toogle Ship Auto-rebuild";
//            _rebuildToggle.CursorOn += gui.ToolTipHandler;
//            _rebuildToggle.PressedControlColor = new Color(1f, 0f, 1f); // TODO: remove this, replace with a ButtonControl that has a separate image for each pressed state
//            horizontal.AddChild(_rebuildToggle);

//            //_destroyButton = new GuiControl(Vector2.Zero, new Vector2(40, 40));
//            //_destroyButton.CursorOn += OnMouseoverButton;
//            //_destroyButton.ControlColor = new Color(1f, 0f, 0f);            
//            //horizontal.AddChild(_destroyButton);

//            _mainLayout = new VerticalLayout(Vector2.Zero);
//            _mainLayout.AddChild(horizontal);

//            _agentControl = new AgentGuiControl(_slot.Agent);
//            _agentControl.CursorOn += OnMouseoverButton;
//            _mainLayout.AddChild(_agentControl);

//            AddChild(_mainLayout);
//        }

//        void OnHullSelected(Agent hull) {
//            var agent = hull.GetWorkingCopy();
//            agent.IsActive = false;
//            _slot.Agent = agent;
//            _agentControl.Agent = _slot.Agent;
//        }

//        void OnMouseoverButton(GuiControl source, CursorInfo cursorInfo) {
//            // Handle left clicks
//            if (cursorInfo.OnPressLeft) {

//                if (_slot.Agent != null) {
//                    // Slot is full, the buttons are enabled

//                    if (source == _inventoryButton) {
//                        // Open ship inventory                        
//                        var parameters = new ActivityParameters();

//                        parameters.DataParams.Add("Scene", _scene);
//                        parameters.DataParams.Add("MainAgent", _slot.Agent);
                        
//                        // Does the ship actually exist?
//                        if (_slot.Agent.IsActive) {
//                            // Yeah. TODO: maybe trade with nearby allies
//                        } else {
//                            // Nope. Give it access to the mothership's inventory
//                            parameters.DataParams.Add("SecondaryAgent", MetaWorld.Inst.GetFaction(Framework.FactionType.Player).Mothership);
//                            parameters.DataParams.Add("ChargeForUnequip", true);
//                        }

//                        // CHANGE OF PLANS: no accessing the inventory of undeployed ships. This is the lazy (but unobtuse) way of avoiding exploits for getting around
//                        // equipment rebuild costs. The less lazy, more obtuse way is to implement ChargeForUnequip
//                        //if (_slot.Agent.IsActive) {
//                        //    var activity = new InventoryActivityPlusPlus();
//                        //    activity.TryInit(parameters);
//                        //    ActivityManager.Inst.SwitchActivity(activity);
//                        //}
//                    }                    

//                    if (source == _destroyButton) {
//                        // Clear agent slot
//                        Debug.Assert(_slot.Agent != null);

//                        var mothership = MetaWorld.Inst.GetFaction(Framework.FactionType.Player).Mothership;
//                        var hangar = MetaWorld.Inst.GetFaction(Framework.FactionType.Player).MothershipHanger;
//                        var money = MetaWorld.Inst.GetFaction(Framework.FactionType.Player).GetMeter(MeterType.Money);

//                        if (_slot.Agent.IsActive) {
//                            // Agent exists, destroy it
//                            _slot.Agent.IsActive = false;

//                            // And refund it (note that we'll repay part of this refund (the item costs) below)
//                            money.Value += hangar.CalcRebuildCost(_slot.Agent);
//                        }

//                        // Return installed equipment to the mothership                    
//                        var failedToAdd = new List<Item>();

//                        _slot.Agent.ItemSlotsContainer.Select(s => s.Item).Where(i => i != null).Do(i => {
//                            // Pay rebuild cost for any reclaimed items (this is to prevent players from avoiding the item rebuild cost by emptying and refilling a slot)
//                            money.Value -= hangar.RebuildCostMultiplier * i.Profile.BuyPrice;
//                            // EXPLOITABLE if the player has no money

//                            if (!mothership.Inventory.AddItem(i))
//                                failedToAdd.Add(i);
//                        });

//                        // If failed to add any items to inventory (no room or other restrictions), just jettison near the mothership
//                        failedToAdd.Do(i => {
//                            // COPYPASTED from CargoEmitterSystem; iffy for large, intact ships
//                            i.Position = mothership.Position;
//                            i.Velocity.X = (_scene.GameEngine.Rand.Next(21) - 10) / 2f; //change
//                            i.Velocity.Y = (_scene.GameEngine.Rand.Next(21) - 10) / 2f;
//                            i.Rotation = (float)_scene.GameEngine.Rand.NextDouble() * MathHelper.TwoPi;
//                            _scene.GameEngine.AddList.Add(i);
//                        });

//                        // Clear the slot
//                        _slot.Agent = null;
//                        _agentControl.Agent = null;
//                    }
//                }
//                else {
//                    // Slot is empty
//                    if (source == _agentControl) {
//                        // Empty fleet slot clicked, bring up hull selection widget
//                        // KLUDGE: we figure out if the widget exists by counting children.
//                        // TODO: hide/reveal instead of removing/adding
//                        if (_mainLayout.GetChildren().Count() == 2)
//                            _mainLayout.AddChild(new HullSelectionControl(_slot.MaxSizeClass, OnHullSelected));
//                    }
//                }
//            }
//        }

//        public override void Update(InputState inputState) {
//            // Update whether to rebuild
//            _slot.Rebuild = _rebuildToggle.IsPressed;

//            // TODO: overlay _agentControl with transparent "UNDEPLOYED" sprite if undeployed
            

//            // Deactivate buttons if appropriate (i.e. if slot is empty)
//            if (_slot.Agent == null) {
//                //_rebuildToggle.
//            }

//            base.Update(inputState);
//        }
//    }



//    public class HullSelectionControl : GuiControl {

//        Agent[] _hulls;
//        GuiControl[] _hullImages;
//        GuiControl _collapseList;
//        public Action<Agent> OnHullSelected;

//        public HullSelectionControl(SizeType size, Action<Agent> OnHullSelected, IEnumerable<Agent> hulls = null) : base(Vector2.Zero, Vector2.Zero) {
//            this.OnHullSelected = OnHullSelected;

//            _hulls = (hulls ?? MetaWorld.Inst.GetFaction(Framework.FactionType.Player).KnownHullIDs.Select(id => ContentBank.Inst.GetEmitter(id) as Agent))
//                .Where(h => h.SizeType <= size) // filter out oversized hulls
//                .ToArray();

//            var grid = new ScrollableGrid(_hulls.Length + 1, 1, new Vector2(50, 50));

//            // Empty slot, for collapsing the list
//            // TODO: image
//            _collapseList = new ImageControl((Sprite)"replace", Vector2.Zero, new Vector2(50, 50));
//            _collapseList.CursorOn += OnMouseoverControl;
//            grid.AddChild(_collapseList);

//            _hullImages = _hulls.Select(h => new ImageControl(h.Sprite, Vector2.Zero, new Vector2(50, 50))).ToArray();

//            _hullImages.Do(i => {
//                i.CursorOn += OnMouseoverControl;
//                grid.AddChild(i);
//                });

//            AddChild(grid);
//        }

//        void OnMouseoverControl(GuiControl source, CursorInfo cursorInfo) {
//            // Handle left clicks
//            if (cursorInfo.OnPressLeft) {
//                if (source == _collapseList) {
//                    // Collapse this control
//                //    Utility.Log($"Removing");
//                    Parent.RemoveChild(this);
//                }

//                for (int i = 0; i < _hullImages.Count(); ++i)
//                    if (source == _hullImages[i]) {
//                        // Hull selected, execute callback and end your needless existence
//                        OnHullSelected(_hulls[i]);
//                        Parent.RemoveChild(this);

//                        break;
//                    }
//            }


//        }
//    }
//}
