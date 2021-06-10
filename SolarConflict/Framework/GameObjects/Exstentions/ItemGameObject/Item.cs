using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Xml.Serialization;
using SolarConflict.Framework;
using XnaUtils;
using XnaUtils.Graphics;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.Utils;

namespace SolarConflict
{

    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum SlotType : uint
    {
        None = 0,
        Generator = 1 << 0,
        Shield = 1 << 1,
        Rotation = 1 << 2,        
        Engine = 1 << 3,
        Weapon = 1 << 4,
        Fabricator = 1 << 5,
        Turret = 1 << 6,
        Utility = 1 << 7,                
        Ammo = 1 << 8,        
        Consumable = 1 << 9,
        Mothership = 1 << 10,
        MainEngine = 1 << 11,
        CraftingStation = 1 << 12,
        Thrusters = 1 << 13,
        All = uint.MaxValue  // 0xFFFFFFFF,
    }

    /// <summary>
    /// ItemCategorey - used for ammo 
    /// </summary>
    [Flags]
    public enum ItemCategory : uint
    {
        None = 0,
        Generator = 1 << 0,
        Engine = 1 << 1,
        ConstructionKit = 1 << 2,
        Rotation = 1 << 3,
        Consumable = 1 << 4,
        Thruster = 1 << 5,
        Mining = 1 << 6,
        Utility = 1 << 7,
        Gun = 1 << 8,
        Shotgun = 1 << 9,
        Shield = 1 << 10,
        Mothership = 1 << 11,
        Cloaking = 1 << 12,
        AsteroidMiningGear = 1 << 13,
        EnergyConsumingWeapon = 1 << 14,
        AmmoWeapon = 1 << 15,
        Material = 1 << 16,        
        Vanity = 1 << 17,
        RepairKit = 1 << 18,
        Mines = 1 << 19,
        Missiles = 1<< 20,
        Boomerang = 1<<21,
        Evasive = 1<<22,
        Core = 1<<24,
        Hotbar = 1 << 25,
        Blueprint = 1<<26,
        NonAI = 1 << 27,
        CraftingMaterial = 1 << 28,
        Imbuing = 1 << 29,
        Final = 1 << 30,
        All = uint.MaxValue,
    }

    public enum CraftingCategory
    {
        Weapon,
        Engine,
        RotationEngine,       
    }

    public enum ItemFlags
    {
        None = 0,
        Imbuable = 1 << 0,
        ShowOnHud = 1 << 1,
        WorkOnAlly = 1 << 2,
        Healing = 1 << 3,
        EnergyRegenerating = 1 << 4,
    }

    [Serializable]
    public sealed class Item : GameObject, IGameObjectFactory, IEmitter
    {
        /// <summary>
        /// To be used when displaying ItemSlots
        /// </summary>
        public static readonly float[] DisplaySizeMultiplyers = new float[] {0.85f, 0.9f, 1.1f, 1.15f, 1.3f, 1.4f };
        static Sprite _backgroundSprite = Sprite.Get("glow128"); //TODO: this is a quick fix for item glow, change                      

        // public enum RarityType { Common = 0, Uncommon, Rare, Epic, Legendary, VoidTech, Level6, Level7, Level8, Level9, Level10} //TODO: remove
       // private static Sprite ring = Sprite.Get("goalmap");


        #region Fields                
        public ItemProfile Profile;        
        public AgentSystem System;                           
        public int Stack;
        //private CollisionSpec impactSpec;   


        private bool wasActive;

        public float CooldownTime
        {
            get
            {
                if(System == null)
                {
                    return 0;
                }
                return System.GetCooldownTime();
            }
        }

        #endregion

        #region Constructors

        public Item()
            : this(null)
        {
            Profile = new ItemProfile();
        }

        public Item(ItemProfile profile)
        {
            this.Profile = profile;
            //systems = new List<IAgentSystem>();            
            Size = 25;
            Stack = 1;
            //impactSpec = new CollisionSpec(); //??
        }
        
        #endregion

        #region Properties
        /// <summary>
        /// Gets item type id
        /// </summary>
        public string ID
        {
            get { return Profile.Id; }
            set { Profile.Id = value; } //maybe remove set content
        }

        public override string Name
        {
            get
            {
                return Profile.Name;
            }

            set
            {                
            }
        }

        public override string Tag
        {
            get { return GetColor().ToTag()+Name + "#dColor{}"; }
        }

        public string IconTag
        {
            get
            {
                string iconTag = "#image{" + Profile.IconSprite.ID;
                if (Profile.IconSecondarySprite != null)
                    iconTag += "|" + Profile.IconSecondarySprite.ID;
                iconTag += "}";
                return iconTag;
            }
        }

        public string GetTooltipText(bool name = true, bool description = true, bool price = true, bool flavor = true, float priceMult = 1)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("#width{900}"); //TODO: check
            if (name)
                sb.AppendLine(Tag);
            sb.Append("#line{}");
            if (description )
            {
                if(Profile.DescriptionText != null)
                    sb.AppendLine(Profile.DescriptionText);
                if (Profile.StatsText != null)
                {
                    sb.Append("#line{}");
                    sb.AppendLine(Profile.StatsText);
                }
                if (Profile.DescriptionSuffix != null)
                    sb.AppendLine(Profile.DescriptionSuffix);

                if (Level <= 10)
                    sb.AppendLine("Tech Level: " + GetColor().ToTag() + Level.ToString() + "#dColor{}");

            }
            sb.Append("#dcolor{}Item Type: #hcolor{}" + this.ItemSize.ToString());
            if (SlotType != SlotType.None)
                sb.AppendLine(" " + SlotType.ToString()+"#dcolor{}");
            else
                sb.AppendLine();

            if ((Category & ItemCategory.CraftingMaterial) > 0)
                sb.AppendLine("#color{255,255,0}Crafting Material#dcolor{}");
           

            if (price)
            {
                sb.Append("#line{}");
                sb.AppendLine(GetPriceText(priceMult));                
            }
            if(flavor && !string.IsNullOrWhiteSpace(Profile.FlavourText))
            {
                sb.Append("#line{}");
                sb.Append(Profile.FlavourText);
            }         

            //if(DebugUtils.ShowItemID || true)
            //{
            //    sb.Append("#line{}");
            //    sb.Append(Profile.Category);
            //}

            return sb.ToString();
        }

        public string GetPriceText(float priceMult = 1)
        {
            return "\nBuy\\Sell: " + Color.Red.ToTag(GetStackBuyPrice(priceMult).ToString()) + "\\" + Color.Green.ToTag(GetStackSellPrice(priceMult).ToString()) + Sprite.ToTag("coin");           
        }

        public ItemFlags ItemFlags;


        public void SetStack(int stackValue)
        {
            Stack = Math.Min(MaxStack, stackValue);
        }
      

        /// <summary>
        /// True if item is ment to be shown in agent/loadout tooltip
        /// </summary>
        public bool IsShownInAgentTooltip
        {
            get { return Profile.IsActivatable || Profile.IsShownOnHUD; }
        }
        

        public int GetStackBuyPrice(float priceMult = 1)
        {
            return (int)(Profile.BuyPrice * Stack * priceMult);
        }

        public float GetStackSellPrice(float priceMult = 1)
        {
            return (int)(Profile.SellPrice* Stack * priceMult);
        }

        /// <summary>
        /// Gets maximal number of stacks a item can have in the inventory slot
        /// </summary>
        public int MaxStack
        {
            get { return Profile.MaxStack; }            
        }

        /// <summary>
        /// Is item consumed when activated
        /// </summary>
        public bool IsConsumable
        {
            get { return Profile.IsConsumed; }
            //set { profile.IsConsumable = value; }
        }

        public Sprite Sprite
        {
            get { return Profile.IconSprite; }
        }

        public Sprite SecondarySprite
        {
            get { return Profile.IconSecondarySprite; }
        }

        public bool IsStackable
        {
            get
            {
                return Profile.MaxStack > 1;
            }
        }

        public bool IsItemActive
        {
            get
            {
                //mainSystem.
                return false; // do to
            }
        }



        public SlotType SlotType
        {
            get { return Profile.SlotType; }            
        }

        public SizeType ItemSize
        {
            get { return Profile.ItemSize;  }
        }

        public bool WasActive
        {
            get { return wasActive; }
        }

        public override Color GetColor()
        {
            return Profile.GetColor();
        }

        public override int Level
        {
            get { return Profile.Level; }
        }
        
        public ItemCategory Category
        {
            get { return Profile.Category; }
        }

        //public CraftingStationType CraftingStationType { get { return Profile.CraftingStationType; } }

        #endregion

        #region Public Methods

        public bool StakItem(Item item) //Add maxStack, when staking
        {

            if (item != null && this.IsStackable && this.ID == item.ID)
            {
                int amount = Math.Min(item.Stack, Profile.MaxStack - Stack);
                this.Stack += amount;
                item.Stack -= amount;

                if (item.Stack == 0)
                {
                    item.IsActive = false; //??? maybe remove //maybe add Item IsEmpty
                    return true;
                }                                
                return false;
            }
            return false;
        }

        public Item GetItemsFromStack(int amount)
        {
            if (Stack >= 1)
            {
                int realAmount = Math.Min(Stack, amount);
                Item clone = GetWorkingCopy();
                clone.Stack = realAmount;
                Stack -= realAmount;
            }
            return null;
        }



        public int GetCooldown()
        {
            throw new NotImplementedException();
        }

        public override string GetId()
        {
            return ID;
        }

        public override CraftingStationType GetCraftingStationType()
        {
            return Profile.CraftingStationType;
        }

        #endregion

        public static bool CanStack(Item target, string sourceItemID, int numOfSourceItems = 1)
        {
            if (target == null)
                return true;
            if (string.IsNullOrEmpty(sourceItemID))
                return false;            
            return target.Stack + numOfSourceItems < target.MaxStack && target.ID == sourceItemID;
        }

        public static bool StackOne(ref Item target, ref Item source) //Maybe change to stack N
        {
            if (source == null || source.Stack <= 0)
            {
                source = null;
                return true;
            }
            if (target == null)
            {
                target = source.GetWorkingCopy();
                target.Stack = 1;
                source.Stack--;
            }
            else
            {
                if(Item.CanStack(target, source.ID, 1))
                {
                    target.Stack++;
                    source.Stack--;
                }
            }
            if(source.Stack == 0)
            {
                source.IsActive = false;
                source = null;
                return true;
            }
            
            return false;
        }


        #region Update/Draw
       

        public void InventoryUpdate(GameEngine gameEngine, Agent agent, bool tryActivate) //TODO: remove
        {            
            if (Profile.IsWorkingInInventory)
            {
                ItemUpdate(gameEngine, agent, agent.Position, agent.Rotation, tryActivate);
            }
        }

        public void ItemUpdate(GameEngine gameEngine, Agent agent, Vector2 initPosition, float initRotation, bool tryActivate) //maybe change name
        {
            tryActivate = tryActivate & Profile.IsActivatable;       //if(Stack > 0)
            wasActive = false;

            if (System != null)
            {
                wasActive = System.Update(agent, gameEngine, initPosition, initRotation, tryActivate);
            }
           
            if (wasActive && Profile.IsConsumed) //fixIt
            {
                Stack--;
            }

            if (wasActive && Profile.BreaksCloaking) //fixIt
            {
                agent.SetMeterValue(MeterType.Cloak, 0);
            }
            // Debug.Assert(stack > 0, "Stack must be positive");
        }


        /// <summary>
        /// Draw item when equipped in slot
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="agent"></param>
        /// <param name="initPosition"></param>
        /// <param name="initRotation"></param>
        public void DrawEquipped(Camera camera, Agent agent, Vector2 initPosition, float initRotation, DrawType drawType = DrawType.Alpha)
        {
            if (Profile.EquippedSprite != null)             
                camera.CameraDraw(Profile.EquippedSprite, initPosition, initRotation, Profile.EquippedTextureScale, Color.White);

            if (System != null)
                System.Draw(camera, agent, initPosition, initRotation);
        }

        public void DrawEquippedInGUI(SpriteBatch sb, Vector2 position, float scale, float rotation)
        {
            if (Profile.EquippedSprite != null)
                sb.Draw(Profile.EquippedSprite.Texture, position, null, Color.White, rotation, Profile.EquippedSprite.Origin, Profile.EquippedTextureScale * scale, SpriteEffects.None, 0);
        }



        public void DrawInInventory(Camera camera, Agent agent)
        {
            if(Profile.IsWorkingInInventory && System != null)
                System.Draw(camera, agent, agent.Position, agent.Rotation);
        }

        /// <summary>
        /// Draws item icon, to be used in gui and tooltips
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="rectangle"></param>
        public void DrawItemIcon(SpriteBatch sb, Rectangle rectangle, Color? color = null, bool isCenterd = true)
        {
            color = color ?? Color.White;
            if (Profile.IconSecondarySprite != null)
                Sprite.DrawIcon(sb, rectangle, color.Value, isCenterd, 0.9f, Profile.IconSprite, Profile.IconSecondarySprite);
            else
                Sprite.DrawIcon(sb, rectangle, color.Value, isCenterd, 0.9f, Profile.IconSprite);
        }

        public void DrawOnHud(SpriteBatch spriteBatch, Rectangle rectangle, ControlSignals activation, float activationAlpha)
        {
            
            DrawItemIcon(spriteBatch, rectangle, Color.DarkGray);
            if (System != null) {
                float cooldown = System.GetCooldown();
                if (cooldown > 0 && Math.Max(cooldown, System.GetCooldownTime()) > Utility.Frames(0.1f)) {
                    // Display cooldown. Note that we only do this if CooldownTime is greater than some small, arbitrary value (0.1s)
                    float timeInSec = (int)(cooldown / 6) /10f;
                    string text = timeInSec.ToString("f1");
                    Vector2 stringSize = Game1.font.MeasureString(text);
                    Vector2 textPosition = new Vector2(rectangle.X, rectangle.Y) - (stringSize * 0.5f) + Vector2.UnitY * 1;
                    spriteBatch.DrawString(Game1.font, text, textPosition, Color.White);
                }
                var iconSize = 30;
                ItemIndicator.DrawKeyBinding(spriteBatch, new Vector2(rectangle.X + (rectangle.Width - iconSize) / 2, rectangle.Y + (rectangle.Height - iconSize) / 2), activation, true, iconSize);
                
            }

        }

        #endregion

        #region Private Methods
        #endregion


        #region GameObject
        public override void ApplyCollision(GameObject collidingObj, GameEngine gameEngine)
        {
            Vector2 appliedForce = Vector2.Zero;
            if (collidingObj.IsActive && this.Parent != collidingObj)  //add another chack if(Or Colide With parent) 
            {

                Vector2 reletivePos = (this.Position - collidingObj.Position);


                switch (collidingObj.CollisionInfo.ForceType)
                {
                    case ForceType.FromCenter:
                        if (reletivePos != Vector2.Zero)
                        {
                            reletivePos.Normalize();
                            appliedForce += reletivePos * collidingObj.CollisionInfo.Force;
                        }
                        break;
                    case ForceType.Velocity:
                        break;
                    case ForceType.Rotation:
                        //applayedForce += collidingObj.ImpactSpec.Force * new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
                        appliedForce += collidingObj.CollisionInfo.Force * new Vector2((float)Math.Cos(collidingObj.Rotation), (float)Math.Sin(collidingObj.Rotation));
                        break;
                    case ForceType.Gravity:
                        Vector2 relPos = (this.Position - collidingObj.Position);
                        if (relPos != Vector2.Zero)
                        {
                            float lengthSquerd = relPos.LengthSquared();
                            appliedForce += relPos * collidingObj.CollisionInfo.Force / (lengthSquerd + 1) * Stack; //*profle.mass
                        }
                        break;
                    case ForceType.Mult:
                        Velocity *= collidingObj.CollisionInfo.Force;
                        break;
                }


                Velocity += appliedForce / (1);
                /*
                if (Velocity.X == float.NegativeInfinity || Velocity.X == float.PositiveInfinity)
                    throw new Exception();*/
            }
            
            
            if (this.IsActive && (Lifetime >= ItemProfile.ParentPickupCooldown || this.Parent != collidingObj)) //add if parent
            {
                if (collidingObj.AddItemToInventory(this))
                {
                    this.IsNotActive = true;
                    if (Profile.CollectedEmitter != null)
                        Profile.CollectedEmitter.Emit(gameEngine, this, this.GetFactionType(), this.Position, this.Velocity, this.Rotation);
                }
            }
        }
        
        /// <remarks>TODO: implement</remarks>
        public override void ApplyForce(Vector2 force, float speedLimit) {
            
        }

        public override void Update(GameEngine engine)
        {
            Lifetime++;
            Position += Velocity;
            Rotation += RotationSpeed;
            Velocity *= 0.98f;
            RotationSpeed *= 0.99f;

            //if (Lifetime >= Profile.MaxLifetime)
            //    // Expired
            //    IsActive = false;
        }

        public override void Draw(Camera camera)
        {
            /*  if (DebugUtils.IsDebug)
                  camera.CameraCircle(Position, size, Color.Multiply(DebugUtils.DebugColor, 1)); //Remove Multiply*/

            if (_backgroundSprite != null) //TODO: maybe add a blinking background?
            {
                camera.CameraDraw(_backgroundSprite, Position, Rotation + Profile.DisplayRotation, Profile.BackgroundScale * 1.25f, Profile.BackgroundColor);
            }

            //Draws a circle around item
            //if ((this.Profile.Category & (ItemCategory.Gun | ItemCategory.Shield)) > 0)
            //{
            //    float scale = (Game1.time % 50) / 100f;
            //    camera.CameraDraw(ring, Position, Rotation + Profile.DisplayRotation, scale, new Color(255, 255, 255, 100));
            //}

            camera.CameraDraw(Profile.IconSprite, Position, Rotation + Profile.DisplayRotation, Profile.TextureScale, Profile.TextureColor);
            //if (Profile.IconSecondarySprite != null)
            //    camera.CameraDraw(Profile.IconSecondarySprite, Position, Rotation + Profile.DisplayRotation, Profile.TextureScale, Profile.TextureColor);

            /* if (DebugUtils.IsDebug)
             {
                 camera.spriteBatch.DrawString(Game1.font, stack.ToString(), camera.GetScreenPos(Position), Color.White);
             }*/
        }

        public override CollisionSpec CollisionInfo
        {
            get
            {
                return Profile.CollisionInfo;
            }
            set
            {
                if (DebugUtils.Mode == ModeType.Debug)
                    throw new Exception("Setting CollisionInfo for item is not supported");
            }
        }

        public override Sprite GetSprite()
        {
            return Profile.IconSprite;
        }

        public override GameObject GetTarget(GameEngine gameEngine, TargetType targetType)
        {
            return null;
        }

        public override CollisionType ListType
        {
            get
            {
                return CollisionType.Collide1; //TODO: change
            }
            set
            {                
            }
        }

        public override float Mass {
            get {
                return 1f;
            }

            set {
                throw new NotImplementedException();
            }
        }

        public override GameObject GetAgentAncestor()
        {
            return this;
        }        

        public override void SetMeterValue(MeterType type, float value)
        {
        }

        public override float GetMeterValue(MeterType type)
        {
            return 0;
        }

        #endregion


        public Item GetWorkingCopy()
        {
            Item copy = (Item)MemberwiseClone();        
            if(System != null)
                copy.System = System.GetWorkingCopy();            
            return copy;
        }


        public GameObject MakeGameObject(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0,
            int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            Item item = GetWorkingCopy();
            item.Parent = parent;
            item.Position = refPosition;
            item.Velocity = refVelocity;
            item.Rotation = refRotation;
            item.RotationSpeed = refRotationSpeed;
            item.Param = 0;
            if (size != null && size>=1)
            {
                item.Stack = Math.Min((int)size.Value, Profile.MaxStack);
            }
            return item;
        }
        
        
        public GameObject MakeGameObject(GameEngine gameEngine, GameObject parent = null, FactionType faction = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            return MakeGameObject(gameEngine, parent, faction, Vector2.Zero, Vector2.Zero, 0, 0, maxLifetime, size, color, param);
        }

        
    
           
     
        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0,
            int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            GameObject item = MakeGameObject(gameEngine, parent, faction, refPosition, refVelocity, refRotation, refRotationSpeed, maxLifetime, size, color);            
            gameEngine.AddList.Add(item);
            return item;
        }

        public static float GetItemPriceMult(Item item, Dictionary<string, float> itemPriceMultiplier)
        {
            if (item == null || itemPriceMultiplier == null || !itemPriceMultiplier.ContainsKey(item.ID))
            {
                return 1;
            }
            return itemPriceMultiplier[item.ID];
       } 

        public static bool IsItemInCategory(Item item, ItemCategory category, ItemCategory notInCategory)
        {
            return (item.Category  & category) > 0 & (item.Category & notInCategory) ==0; //TODO: test
        }

        public override string ToString()
        {
            //TODO: add, if id == null returen, generated + category
            return Profile.Id;
        }

        public override DrawType DrawType
        {
            get
            {
                return DrawType.Lit;
            }
        }

        public override GameObjectType GetObjectType()
        {
            return GameObjectType.Item;
        }
    }
}
