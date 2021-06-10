using Microsoft.Xna.Framework;
using SolarConflict;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.GameContent.Emitters;
using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using XnaUtils;

[Serializable]
class LowHitPointsFlames : AgentSystem //TODO: change all class. very bad 
{
    IEmitter fireEmitter, somkeEmitter;

    [NonSerialized]
    BitVector32 fireVector;
    int _serializedFireVector;
    [NonSerialized]
    BitVector32 smokeVector;
    int _serializedSmokeVector;
    int counter;

    public LowHitPointsFlames() //TODO: work only near screen??? //possible issue: if hitpoints skips from low to near full counter will not update
    {
        counter = 0;
        smokeVector = new BitVector32(0);
        fireVector = new BitVector32(0);
        fireEmitter = ContentBank.Inst.GetEmitter(typeof(FireFx).Name); //TODO make a constructor that gets emitters ContentBank.Inst.GetEmitter("SmokePS"); //ContentBank.Inst.GetEmitter("SmokePS"); //
        somkeEmitter = ContentBank.Inst.GetEmitter(typeof(EmitterFxSmoke).Name);
    }

    public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate)
    {
        Meter hitPointsMeter = agent.GetMeter(MeterType.Hitpoints);

        //if ((agent.ControlSig & ControlSignals.QuickUse1) > 0)
        //    hitPointsMeter.Value = 0.5f * hitPointsMeter.MaxValue;

        //if ((agent.ControlSig & ControlSignals.QuickUse2) > 0)
        //    hitPointsMeter.Value = 0.2f * hitPointsMeter.MaxValue;

        float hitPoints = hitPointsMeter.Value / hitPointsMeter.MaxValue;
        float startAt = 0.7f;
        float fireStartAt = 0.7f;
        if (hitPoints < startAt && agent.ItemSlotsContainer != null) {
            int slotNum = agent.ItemSlotsContainer.AgentSlotsCount;
            float numNormalized = (startAt - hitPoints) / startAt;
            numNormalized = numNormalized * numNormalized;
            int num = (int)(numNormalized * slotNum * 2); //only emmit if on screen

            //add
            if (counter < num) {
                int index = gameEngine.Rand.Next(slotNum);
                if (!smokeVector[1 << index]) {
                    counter++;
                    smokeVector[1 << index] = true;
                } else {
                    if (hitPoints < fireStartAt && !fireVector[1 << index]) {
                        counter++;
                        fireVector[1 << index] = true;
                    }
                }
            }

            if (counter > num) {
                int index = gameEngine.Rand.Next(slotNum);
                if (fireVector[1 << index]) {
                    counter--;
                    fireVector[1 << index] = false;
                }

                if (smokeVector[1 << index] && counter > num) {
                    counter--;
                    smokeVector[1 << index] = false;
                }
            }
            if (!agent.IsCloaked) {
                for (int i = 0; i < slotNum; i++) {
                    if (smokeVector[1 << i]) {
                        Vector2 pos = agent.Position + agent.RotateVector(agent.ItemSlotsContainer[i].Position);                        

                        if ((agent.Lifetime + i) % 3 == 0) //Add random speed
                        {
                            somkeEmitter.Emit(gameEngine, agent, 0, pos, agent.Velocity * 0.8f, gameEngine.Rand.Next(512) / 512f * MathHelper.TwoPi);
                        }

                        if (fireVector[1 << i]) {
                            if ((agent.Lifetime + i) % 2 == 0) //maybe counter
                            {
                                fireEmitter.Emit(gameEngine, agent, 0, pos, agent.Velocity * 0.81f, gameEngine.Rand.Next(512) / 512f * MathHelper.TwoPi); //TODO: maybe no need for random rotation
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
    
    public override AgentSystem GetWorkingCopy() {
        LowHitPointsFlames clone = (LowHitPointsFlames)MemberwiseClone();
        clone.fireVector = new BitVector32();
        clone.smokeVector = new BitVector32();
        return clone;
    }


    [OnSerialized]
    void OnSerializedMethod(StreamingContext context) {
        _serializedFireVector = fireVector.Data;
        _serializedSmokeVector = smokeVector.Data;
    }

    [OnDeserialized]
    void OnDeserializedMethod(StreamingContext context) {
        fireVector = new BitVector32(_serializedFireVector);
        smokeVector = new BitVector32(_serializedSmokeVector);
    }
}