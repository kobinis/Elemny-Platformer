using System;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace SolarConflict
{
    [Serializable]
    public class ItemSlot
    {
        const SlotType NON_UTIL_SLOT_TYPE = SlotType.Shield | SlotType.Rotation | SlotType.Generator | SlotType.MainEngine;

        Vector2? _displayPosition;
        /// <summary>The slot's position in the inventory/editor UI. Defaults to its position on the ship.</summary>
        [XmlIgnoreAttribute]
        public Vector2 DisplayPosition {
            get {
                return _displayPosition ?? position;
            }
            set {
                _displayPosition = value;
            }
        }

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        [XmlIgnoreAttribute]
        public float rotation;
        public float Rotation
        {
            get { return MathHelper.ToDegrees(rotation); }
            set { rotation = MathHelper.ToRadians(value); }
        }
        
        public SlotType Type { get; set; }
        public SizeType Size {get; set;}

        [XmlIgnoreAttribute]
        private Item item;
        [XmlIgnoreAttribute]
        public Item Item
        {
            get { return item; }
            set { item = value; }
        }

        public ControlSignals ActivationSignal;

        public ItemSlot()
        {
        }

        public ItemSlot(SlotType type, SizeType size, Vector2 position, float rotation, ControlSignals activationSignal, Vector2? displayPos = null)
        {
            this.Type = type;
            this.Size = size;
            this.position = position;
            this.rotation = rotation;
            this.ActivationSignal = activationSignal;
            _displayPosition = displayPos;     
        }



        public bool CanEquip(Item item)
        {
            if (item == null)
                return true;
            SlotType type = Type;
            if ((type & NON_UTIL_SLOT_TYPE) == 0)
                type = type | SlotType.Utility;
            if((type & SlotType.MainEngine) > 0)
            {
                type |= SlotType.Engine;
            }
            return ((type & item.Profile.SlotType) > 0) && (item.ItemSize <= Size);
        }

        public Item EquipItem(Item item)
        {
            if (CanEquip(item))
            {
                Item returendItem = this.item;
                this.item = item;
                return returendItem;
            }
            return item;
        }

        public void Update(GameEngine gameEngine,Agent agent)
        {           
            if (item != null)
            {
                if (item.Stack == 0)
                {
                    item = null;
                }
                else
                {
                    float initRotation = agent.Rotation + rotation;
                    if ((Type & SlotType.Turret) > 0)
                    {
                        initRotation = (float)Math.Atan2(agent.analogDiractions[1].Y, agent.analogDiractions[1].X);
                    }
                    Vector2 pos = new Vector2(agent.Position.X + position.X * agent.Heading.X - position.Y * agent.Heading.Y, agent.Position.Y + position.X * agent.Heading.Y + position.Y * agent.Heading.X);
                    item.ItemUpdate(gameEngine, agent, pos, initRotation, (ActivationSignal & agent.ControlSignal) > 0);
                }
            }            
        }

        public void Draw(Camera camera, Agent agent, DrawType drawType = DrawType.Alpha) //changeName 
        {
            // camera.CameraCircle(agent.Position + agent.RotateVector(Position), 10, Color.Red * 0.5f);
            if (item != null && item.Profile.SlotType != SlotType.Shield && item.Profile.SlotType != SlotType.Generator)
            {
                if ((Type & SlotType.Turret) > 0)
                {
                    float initRotation = (float)Math.Atan2(agent.analogDiractions[1].Y, agent.analogDiractions[1].X); //Change shit
                    item.DrawEquipped(camera, agent, agent.Position + agent.RotateVector(Position), initRotation, drawType);
                }
                else
                {
                    item.DrawEquipped(camera, agent, agent.Position + agent.RotateVector(Position), agent.Rotation + rotation, drawType);
                }
            }
        }

        public void DrawInGui(SpriteBatch sb, Vector2 pos, float scale, float rot) 
        {
            // camera.CameraCircle(agent.Position + agent.RotateVector(Position), 10, Color.Red * 0.5f);
            
            if (item != null && item.Profile.SlotType != SlotType.Shield && item.Profile.SlotType != SlotType.Generator )
            {
                if ((Type & SlotType.Turret) > 0)
                {
                    
                    item.DrawEquippedInGUI(sb, pos + FMath.RotateVector(Position,rot) * scale, scale, -MathHelper.PiOver2);
                }
                else
                {
                   // if(( Type & SlotType.Weapon) > 0)
                        item.DrawEquippedInGUI(sb, pos + FMath.RotateVector(Position, rot) * scale, scale, rot+rotation);
                    
                }
            }
        }

        public ItemSlot GetWorkingCopy()
        {
            ItemSlot clone = (ItemSlot)MemberwiseClone();
            if (this.item != null)
                clone.item = this.item.GetWorkingCopy();
            return clone;
        }
                
        public string ToCode()
        {            
            StringBuilder res = new StringBuilder();            
            res.Append("ship.ItemSlotsContainer.AddAgentSlot(");
            string typeToSave = "SlotType." + Type.ToString().Replace(",", "| SlotType.");
            res.Append(typeToSave);
            res.Append(", ");
            res.Append("ship.SizeType");
            res.Append(", ");
            res.Append("new Vector2(");
            res.Append(position.X);
            res.Append(", ");
            res.Append(position.Y);
            res.Append("), ");
            res.Append(Rotation);
            res.Append(", ");
            res.Append("ControlSignals.");
            res.Append(ActivationSignal.ToString());
            res.Append(");");
            return res.ToString();
        }

    }
}
