using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Utils
{
    public class EmitterStatUtils //TODO: change name
    {
        public class EmitterData
        {
            public Dictionary<MeterType, float> MeterValue { get; }
            public float Damage {
                get { float val = 0; MeterValue.TryGetValue(MeterType.Damage, out val); return val; }
                set { MeterValue[MeterType.Damage] = value; } }
            public float StunTime { get; set; }
            public float EnergyEffect { get; set; }
            public float Lifetime { get; set; }
            public ImpactType ImpactType { get; set; }

            public EmitterData(float damage =0, float lifetime = 0)
            {
                MeterValue = new Dictionary<MeterType, float>();
                Damage = damage;
                Lifetime = lifetime;
                StunTime = 0;
                EnergyEffect = 0;
                ImpactType = ImpactType.Additive;
            }

        }

        public static void GetEmitterData(IEmitter emitter, EmitterData data, float multiplyer = 1)
        {
          //  EmitterData data = new EmitterData();
            float damageValue = 0;
            float lifetime = 1;
            
            if (emitter is ProjectileProfile)
            {
                
                ProjectileProfile projectileProfile = emitter as ProjectileProfile;

                if (projectileProfile.InitMaxLifetime is InitFloatConst)
                {
                    lifetime = (projectileProfile.InitMaxLifetime as InitFloatConst).value;
                }
                if (projectileProfile.InitMaxLifetime is InitFloatRandom)
                {
                    lifetime = (projectileProfile.InitMaxLifetime as InitFloatRandom).GetAvarageLifetime();
                }
                data.Lifetime = lifetime;

                if (projectileProfile.CollisionType != CollisionType.Effects)
                {
                    if (projectileProfile.CollisionSpec != null)
                    {
                        foreach (var collusionEnetery in projectileProfile.CollisionSpec.ImpactList)
                        {
                            float value = 0;
                            float mult = projectileProfile.IsDestroyedOnCollision ? 1 : lifetime;
                     
                            data.MeterValue.TryGetValue(collusionEnetery.meterType, out value);
                            data.MeterValue[collusionEnetery.meterType] = value  + collusionEnetery.amount*multiplyer;
                        }
                    }
                    if(projectileProfile.ImpactEmitter != null)
                        GetEmitterData(projectileProfile.ImpactEmitter, data, multiplyer);                
                }
            }

            if (emitter is EmitterIdHolder)
            {
                GetEmitterData((emitter as EmitterIdHolder).Emitter, data, multiplyer);
            }

            if (emitter is ParamEmitter)
            {
                ParamEmitter paramEmitter = emitter as ParamEmitter;
                float mult = paramEmitter.MinNumberOfGameObjects + paramEmitter.RangeNumberOfGameObject / 2f;
                GetEmitterData(paramEmitter.Emitter, data, mult * multiplyer);
                if (paramEmitter.LifetimeType == ParamEmitter.EmitterLifetime.Random || paramEmitter.LifetimeType == ParamEmitter.EmitterLifetime.Range)
                    data.Lifetime = paramEmitter.LifetimeMin + paramEmitter.LifetimeRange / 2;
            }
          
        }
    }
}
