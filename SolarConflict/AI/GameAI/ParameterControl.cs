using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils;
using SolarConflict.AI.Framework;

namespace SolarConflict.AI.GameAI
{
    /// <summary>
    /// AI pramtrize by actions, add ally support
    /// </summary>
    [Serializable]
    public class ParameterControl : IAgentControl 
    {
        public enum GoalTargetType { Enemy, EnemyOrGoalOrAlly, ClosestDanger, Ally };//, ClosestDangerCollideAll, Ally }
        [Serializable]
        class ActionParams
        {
            public ControlSignals Signal;
            public int ItemSlotIndex;
            public float MaxRange;
            public float MinRange;
            public float AngleRange;            
            public Vector2 Rotation;
            public GoalTargetType GoalTargetType;


            public ActionParams(int index, ItemSlot slot, float maxRange = 0, float angleRange = 0.99f, GoalTargetType targetType = GoalTargetType.Enemy)
            {
                Signal = slot.ActivationSignal;
                if (maxRange == 0)
                    MaxRange = slot.Item.Profile.MaximalRange;
                else
                    MaxRange = maxRange;
                AngleRange = ((slot.Type & SlotType.Turret) >0) ? -1.1f : angleRange;
                Rotation = Vector2.UnitX;// FMath.ToCartesian(1,slot.Rotation);
                GoalTargetType = targetType;
                MinRange = -1000;
                ItemSlotIndex = index;
            }

            //public ActionParams(ControlSignals signal, float maxRange = 2000, float angleRange = 0.8f , GoalTargetType targetType = GoalTargetType.Enemy)
            //{
            //    Signal = signal;
            //    MaxRange = maxRange;
            //    AngleRange = angleRange;
            //    Rotation = Vector2.UnitX;
            //    GoalTargetType = targetType;
            //    MinRange = -1000;
            //}

            public ActionParams GetClone()
            {
                return (ActionParams)MemberwiseClone();
            }

        }
        
        public float ShotSpeed = 50;
        public float MinimalDistance = 100;
        public float EvationProb = 1;
        private List<ActionParams> enemyActionParams;
        private List<ActionParams> dangerActionParams;
        private List<ActionParams> goalActionParams; // Or Enemy
        private List<ActionParams> allyActionParams;

        public int ID
        {
            get; set;
        }        

        public ParameterControl()
        {
            enemyActionParams = new List<ActionParams>();
            dangerActionParams = new List<ActionParams>();
            goalActionParams = new List<ActionParams>();
            allyActionParams = new List<ActionParams>();
        }

        
        public ControlSignals Update(Agent agent, GameEngine gameEngine, ref Vector2[] analogDirections)
        {            
            ControlSignals controlSignals = ControlSignals.None;
            GameObject targetObject = agent.GetTarget(gameEngine, TargetType.Enemy); //TODO: change 
            if (targetObject == null && agent.targetSelector.AttackDanger && agent.ClosesetDanger != null && agent.ClosesetDanger.ListType == CollisionType.UpdateOnlyOnScreen && agent.ClosesetDanger.IsActive && agent.ClosesetDanger.GetFactionType() != agent.FactionType)
                targetObject = agent.ClosesetDanger;

            GameObject allyTarget = agent.GetTarget(gameEngine, TargetType.Ally);
            bool isAllyItemUsed = false;

            if (allyActionParams.Count > 0 && allyTarget != null)
            {
                
                float targetDis = GameObject.DistanceFromEdge(agent, allyTarget); ;                
                Vector2 relPosReal = allyTarget.Position - agent.Position;

                if (targetDis > 0)
                {
                    relPosReal.Normalize();
                    analogDirections[0] = relPosReal;
                    analogDirections[1] = analogDirections[0];
                }

                foreach (var actionParam in allyActionParams)
                {
                    float life = allyTarget.GetNormlizedHitpointsAndShield();
                    Item item = agent.ItemSlotsContainer[actionParam.ItemSlotIndex].Item;
                    if (targetDis <= actionParam.MaxRange && targetDis >= actionParam.MinRange && 
                        (item != null && ( (item.ItemFlags & ItemFlags.Healing) > 0 && allyTarget.GetNormlizedHitpointsAndShield() < 1 ) ) &&
                        (actionParam.AngleRange <= -1 || Vector2.Dot(relPosReal, FMath.RotateVector(agent.Heading, actionParam.Rotation)) > actionParam.AngleRange))
                    {
                        controlSignals |= actionParam.Signal;
                        isAllyItemUsed = true;
                    }
                }

                
            }

            if (agent.GetMeter(MeterType.Energy)?.Value < 30)
            {
                agent.SetMeterValue(MeterType.AiState, 1);
            }
            if (agent.GetMeter(MeterType.Energy)?.NormalizedValue > 0.9f)
            {
                agent.SetMeterValue(MeterType.AiState, 0);
            }

            if (!isAllyItemUsed && targetObject != null)
            {
                float targetDis = GameObject.DistanceFromEdge(agent, targetObject); ;
                float factor = (float)Math.Cos(agent.Lifetime * 0.05f) * Math.Min(targetDis / (ShotSpeed + (float)Math.Cos(agent.Lifetime * 0.15f) * 5), 1000);
                Vector2 relPosReal = targetObject.Position + (targetObject.Velocity - agent.Velocity) * factor - agent.Position;
 
                if (targetDis > 0) 
                {
                    relPosReal.Normalize();
                    analogDirections[0] = relPosReal;
                    analogDirections[1] = relPosReal;

                    if (agent.GetMeterValue(MeterType.AiState) == 0)
                    {
                        analogDirections[0] = analogDirections[0];
                    }
                    else
                    {
                        analogDirections[0] =  -analogDirections[0];
                    }
                }
                
                foreach (var actionParam in enemyActionParams)
                {
                    if(targetDis <= actionParam.MaxRange && targetDis >= actionParam.MinRange && (actionParam.AngleRange <= -1 || Vector2.Dot(relPosReal, FMath.RotateVector(agent.Heading, actionParam.Rotation)) >  actionParam.AngleRange))
                    {
                        controlSignals |= actionParam.Signal;
                    }
                }
            }

            float distanseOffset = 0;

            if (allyActionParams.Count > 0 && allyTarget != null)
            {
                targetObject = allyTarget;
            }

            if (targetObject == null || agent.targetSelector.PrioritizeGoal)
            {
                targetObject = agent.GetTarget(gameEngine, TargetType.Goal); //TODO: and distance from goal is bigger then offset
                distanseOffset = agent.targetSelector.GoalOffset;
            }

            


            if (targetObject != null)
            {
                float targetDis = GameObject.DistanceFromEdge(agent, targetObject); //TODO: when goal multiply fistance by targetSelector GoalFactor

               // float factor = Math.Max(targetDis/ agent.Velocity.Length(), 60*1.5f);
                Vector2 relPosReal = targetObject.Position - agent.Position;// + (targetObject.Velocity - agent.Velocity) * factor ;

                if (targetDis > 0 && distanseOffset > 0)
                {
                    relPosReal.Normalize();
                    analogDirections[0] = relPosReal;

                    if (agent.GetMeterValue(MeterType.AiState) == 0)
                    {
                        analogDirections[0] = relPosReal;
                    }
                    else
                    {
                        analogDirections[0] = -relPosReal;
                    }

                }
                //targetDis += distanseOffset;
                foreach (var actionParam in goalActionParams)
                {                    
                    if (targetDis <= actionParam.MaxRange
                        && targetDis - distanseOffset >= actionParam.MinRange
                        && (actionParam.AngleRange <= -1 || Vector2.Dot(relPosReal, FMath.RotateVector(agent.Heading, actionParam.Rotation)) >= actionParam.AngleRange))
                    {
                        controlSignals |= actionParam.Signal;
                    }
                }
            }

            targetObject = agent.ClosesetDanger;
            if (targetObject != null && targetObject.IsActive)
            {
                float targetDis = GameObject.DistanceFromEdge(agent, targetObject) ;                
                Vector2 relPosReal = targetObject.Position - agent.Position;

                foreach (var actionParam in dangerActionParams)
                {
                    relPosReal.Normalize(); // Maybe if not zero
                    if (targetDis <= actionParam.MaxRange && targetDis >= actionParam.MinRange && (actionParam.AngleRange <= -1 || Vector2.Dot(relPosReal, FMath.RotateVector(agent.Heading, actionParam.Rotation)) > actionParam.AngleRange))
                    {
                        controlSignals |= actionParam.Signal;
                    }
                }
            }

            return controlSignals;
        }

      
        public IAgentControl GetWorkingCopy()
        {
            return (IAgentControl)MemberwiseClone();
        }

        

        private void AddActionParam(ActionParams param)
        {
            if (param.GoalTargetType == GoalTargetType.Enemy)
                enemyActionParams.Add(param);
            if (param.GoalTargetType == GoalTargetType.EnemyOrGoalOrAlly)
                goalActionParams.Add(param);
            if (param.GoalTargetType == GoalTargetType.ClosestDanger)
                dangerActionParams.Add(param);
            if (param.GoalTargetType == GoalTargetType.Ally)
                allyActionParams.Add(param);

        }

        public static ParameterControl MakeAIFromAgent(Agent agent)
        {
            //Dictionary<ControlSignals, ActionParams> paramDictionery = new Dictionary<ControlSignals, ActionParams>();
            ParameterControl control = new ParameterControl();
           // if(agent.FactionType != SolarConflict.Framework.FactionType.Player)
            control.EvationProb = 0.5f;
            control.ID = 0;
            //agent.ItemSlotsContainer
            //TODO: add slot index
            //TODO: add 
            float distance = 5000;
            int index = 0;
            foreach (var slot in agent.ItemSlotsContainer)
            {
                
                float shootSpeed = 50;
                if (slot.ActivationSignal != ControlSignals.None && slot.ActivationSignal != ControlSignals.AlwaysOn && slot.Item != null)
                {
                    Item item = slot.Item;
                    Vector2 rotation = FMath.ToCartesian(1, slot.rotation);
                    float angle = 0.1f;
                    if ((slot.Type & SlotType.Turret) > 0)
                    {
                        angle = -1;
                    }
                    if (item.Profile.MaximalRange > 0 )//|| (item.SlotType & (SlotType.Weapon | SlotType.Turret)) > 0)  //TODO: 
                    {
                        distance = Math.Min(distance, item.Profile.MaximalRange);
                        ActionParams actionParams = new ActionParams(index, slot, 0, angle);
                        actionParams.Rotation = rotation;
                        if((item.ItemFlags & ItemFlags.WorkOnAlly) > 0)
                            actionParams.GoalTargetType = GoalTargetType.Ally;
                        control.AddActionParam(actionParams);
                    }

                    //Evasive
                    if ((item.Category & ItemCategory.Engine) > 0 && (item.SlotType & (SlotType.MainEngine | SlotType.Engine)) > 0)  //TODO: 
                    {
                        ActionParams actionParams = new ActionParams(index, slot, 1500, 0.7f, GoalTargetType.ClosestDanger);
                        rotation = FMath.ToCartesian(1, slot.rotation);
                        actionParams.Rotation = rotation;
                        control.AddActionParam(actionParams);
                    }

                    control.MinimalDistance = distance * 0.5f; //Get Shot speed
                    control.ShotSpeed = shootSpeed;
                }
                index++;
            }

            index = 0;
            foreach (var slot in agent.ItemSlotsContainer)
            {
                Item item = slot.Item;
                if (item != null)
                {
                    // Main GoalOrTarget (Movement)
                    if ((item.SlotType & (SlotType.MainEngine | SlotType.Engine)) > 0)  //TODO: 
                    {
                        var angleRange = (agent.gameObjectType & GameObjectType.NonRotating) > 0 ? 0.01f : MathHelper.PiOver4;
                        ActionParams actionParams = new ActionParams(index, slot, float.MaxValue * 0.5f, angleRange, GoalTargetType.EnemyOrGoalOrAlly);
                        actionParams.MinRange = Math.Max( control.MinimalDistance,100);  //OR Zero if rammig 
                        Vector2 rotation = FMath.ToCartesian(1, slot.rotation + MathHelper.Pi);
                        actionParams.Rotation = rotation;
                        control.AddActionParam(actionParams);
                    }
                    else
                    {
                        if (item.Profile.MaximalRange == 0 )  //TODO: 
                        {
                            Vector2 rotation = FMath.ToCartesian(1, slot.rotation + MathHelper.Pi);                            
                            ActionParams actionParams = new ActionParams(index, slot, control.MinimalDistance * 1.3f, -1);
                            actionParams.Rotation = rotation;
                            control.AddActionParam(actionParams);
                        }
                    }
                }
                index++;
            }

            //control.MinimalDistance = 130
            control.ShotSpeed = 1000000;
            return control;
        }
    }
}
