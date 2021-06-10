//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict
//{
//    [Serializable]
//    public class InitFloatParentParam : BaseInitFloat
//    {
//        public float SizeMult;
//        public float SizeOffset;

//        public InitFloatParentParam()
//        {
//            SizeMult = 1;
//            SizeOffset = 0;
//        }

//        public InitFloatParentParam(float sizeMult, float sizeAdd)
//        {
//            this.SizeMult = sizeMult;
//            this.SizeOffset = sizeAdd;
//        }

//        public override float Init(Projectile projectile)
//        {
//            return projectile.Parent.Param * SizeMult + SizeOffset;
//        }

//    }
//}
