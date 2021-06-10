//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.Framework.Agents
//{
//    /// <summary>Class for agents which are disrupted by damage, and recover over time</summary>
//    [Serializable]
//    public struct Integrity
//    {
//        private const float damageThreshold = 10;
//        private const int integrityCompromiseDuration = 60 * 10;
//        private int cooldown;
//        public bool Update(float damgeThisFrame)
//        {
//            if (damgeThisFrame >= damageThreshold)
//            {
//                cooldown = integrityCompromiseDuration;
//            }
//            cooldown--;
//            return cooldown <= 0;
//        }
//    }
//}
