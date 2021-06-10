using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SolarConflict
{    
    [Serializable]
    public struct MeterCollisionSpec
    {
        public MeterType meterType;
        public float amount;
        public ImpactType impactType;

        public MeterCollisionSpec(MeterType meterType, float amount, ImpactType impactType)
        {
            this.meterType = meterType;
            this.amount = amount;
            this.impactType = impactType;
        }

        public MeterCollisionSpec(MeterType meterType, float amount)
        {
            this.meterType = meterType;
            this.amount = amount;
            this.impactType = ImpactType.Additive;
        }       
    }

    [Flags]
    public enum CollisionSpecFlags : uint
    {
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        IsRotating = 1 << 1,
        IsCollidingWithParent = 1 << 2,
        AffectsAllies = 1 << 3,
        Open1 = 1 << 4,
        IsMassDependent = 1 << 5,   
        IsNotPushingAllies = 1 <<6,
        DoNotEffectNeutral = 1 << 7,
    };

    [Serializable]
    public class CollisionSpec //or struct
    {
        private static CollisionSpec specEmpty = new CollisionSpec();
        private static CollisionSpec specForce = new CollisionSpec(0,1f); //remove
      
        public static CollisionSpec SpecEmpty
        {
            get { return specEmpty; }          
        }

        public static CollisionSpec SpecForce
        {
            get { return specForce; }
        }

        public CollisionSpecFlags Flags;

        public ForceType ForceType;
        public float Force;        

        public List<MeterCollisionSpec> ImpactList;

        public bool IsDamaging; //maybe remove and add to projectile


        #region Constructors
        public CollisionSpec(float damage, float force) : this()
        {
            this.Force = force;
            ImpactList = new List<MeterCollisionSpec>();
            ImpactList.Add(new MeterCollisionSpec(MeterType.Damage, damage));
            IsDamaging = damage > 0;
        }

        public CollisionSpec(float damage, float force, float stunTime) : this()
        {
            this.Force = force;
            ImpactList = new List<MeterCollisionSpec>();
            ImpactList.Add(new MeterCollisionSpec(MeterType.Damage, damage));
            ImpactList.Add(new MeterCollisionSpec(MeterType.StunTime, stunTime, ImpactType.Max));
            IsDamaging = damage > 0 || stunTime > 0;
        }

        public CollisionSpec(float damage, ImpactType type, float force):this()
        {
            this.Force = force;
            ImpactList = new List<MeterCollisionSpec>();
            ImpactList.Add(new MeterCollisionSpec(MeterType.Damage, damage, type));
            IsDamaging = damage > 0;
        }

        public CollisionSpec()
        {
            Flags = CollisionSpecFlags.IsRotating;
            Force = 0;
            IsDamaging = false;
            ImpactList = new List<MeterCollisionSpec>();
        } 

        #endregion


        public void AddEntry(MeterType meterType, float amount)
        {
            if ((meterType == MeterType.Damage || meterType == MeterType.StunTime )&& amount > 0)
                IsDamaging = true;
            if ((meterType == MeterType.Hitpoints || meterType == MeterType.Shield || meterType == MeterType.Energy) && amount < 0)
                IsDamaging = true;

            ImpactList.Add(new MeterCollisionSpec(meterType, amount));
        }

        public void AddEntry(MeterType meterType, float amount, ImpactType impactType)
        {
            if ((meterType == MeterType.Damage || meterType == MeterType.StunTime) && amount > 0)
                IsDamaging = true;
            if ((meterType == MeterType.Hitpoints || meterType == MeterType.Shield || meterType == MeterType.Energy) && amount < 0)
                IsDamaging = true;

            ImpactList.Add(new MeterCollisionSpec(meterType, amount, impactType));
        }        
             
        public CollisionSpec GetWorkingCopy()
        {
            return this;
        }
    }
}
