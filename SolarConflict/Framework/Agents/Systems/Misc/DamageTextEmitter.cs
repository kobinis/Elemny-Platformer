using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Projectiles.Bla;
using XnaUtils;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict
{
    /// <summary>
    /// DamageTextEmitter - Emitts a particle showing the damage taken
    /// </summary>         
    [Serializable]
    class DamageTextEmitter : AgentSystem //REFACTOR: remove this class and add functionality to HudManager compenent 
    {
        float totalDemage = 0;
        //float cooldown = 0;
        int timer = 0;
         static IGameObjectFactory demageProfile = ProjDamageText.Make();
        float prevShield = 0;
        float prevHitpoints = 0;

        static DamageTextEmitter()
        {
            //ProjDamageText
        }

        public DamageTextEmitter()
        {
           
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate)
        {
            float shieldValue = agent.GetMeterValue(MeterType.Shield);
            float hitpointsValue = agent.GetMeterValue(MeterType.Hitpoints);
            totalDemage += Math.Max(0, prevShield - shieldValue);
            totalDemage += Math.Max(0, prevHitpoints - hitpointsValue);
            prevShield = shieldValue;
            prevHitpoints = hitpointsValue;
            //totalDemage += agent.GetDamageTaken();

            if (totalDemage > 1)
            { //|| agent.IsNotActive
                timer++;
                if (timer > 20 || agent.IsNotActive)
                {

                    Vector2 position = agent.Position;
                    //if (agent.lastDamagingObjectToCollide != null)
                    //{
                    //    position = agent.lastDamagingObjectToCollide.Position;
                    //}
                    Projectile gameObject = (Projectile)demageProfile.MakeGameObject(gameEngine, agent, 0, position, -Vector2.UnitY * 5, agent.Rotation, 0, 100);
                    gameObject.Param = totalDemage;
                    gameEngine.AddGameObject(gameObject);
                    timer = 0;
                    totalDemage = 0;
                }
            }
            return false;
        }

        public override AgentSystem GetWorkingCopy()
        {
            return (DamageTextEmitter)MemberwiseClone();
        }

    }
}
