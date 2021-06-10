using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XnaUtils;
using XnaUtils.Graphics;
using SolarConflict.Framework;

namespace SolarConflict
{
    [Serializable]
    public class DummyObject : GameObject //TODO: remove
    {

        public string Id
        {
            get { return string.Empty; }            
        }

        public override string GetId()
        {
            return Id;
        }

        public override string Tag
        {
            get { return null; }
        }
        public FactionType Faction;

        public DummyObject(Vector2 position = default(Vector2))
        {
            Size = 20;            
            Position = position;
            Faction = FactionType.Neutral;
        }

        public override FactionType GetFactionType()
        {
            return Faction;
        }

        public override void SetFactionType(FactionType faction)
        {
            Faction = faction;
        }



        public override void ApplyCollision(GameObject spr, GameEngine gameEngine)
        {
            //throw new NotImplementedException();            
        }

        public override void ApplyForce(Vector2 force, float speedLimit) {

        }

        public override void Update(GameEngine engine)
        {
            Position += Velocity;
        }

        public override void Draw(Camera camera)
        {
            
        }

        public override CollisionSpec CollisionInfo
        {
            get
            {
                return new CollisionSpec();
            }
            set
            {
                
            }
        }

        public override Sprite GetSprite()
        {
            return null;
        }

        public override GameObject GetTarget(GameEngine gameEngine, TargetType targetType)
        {
            return null;
        }

        public override CollisionType ListType
        {
            get
            {
                return CollisionType.Collide1;
            }
            set
            {
                base.ListType = value;
            }
        }

        public override float Mass {
            get {
                return 1f;
            }

            set {                
            }
        }

        public override string Name { get { return null; } set {} }



        public override GameObject GetAgentAncestor()
        {
            if (Parent != null)
                return Parent.GetAgentAncestor();
            else
                return null;
        }

        public override void SetMeterValue(MeterType type, float value)
        {
        }

        public override float GetMeterValue(MeterType type)
        {
            return 0;
        }       

        public Item GetWorkingCopy()
        {
            return null;
        }

        public override GameObjectType GetObjectType()
        {
            return GameObjectType.None;
        }
    }
}
