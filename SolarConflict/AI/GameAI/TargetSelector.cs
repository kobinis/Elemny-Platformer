using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict
{       
    //To Do:    
    //Select the closest target not in neutral faction and not in your faction
    //if no target select the target that attacks you

    public enum TargetType
    {
        Enemy,
        Ally,
        Goal,
    }

    public enum AgentCommand
    {
        None,
        FollowFlagShip,
        Attack,
        Move,
        Follow,
        Defend,
    }

    /// <summary>
    /// Manages the target
    /// </summary>
    [Serializable]
    public class TargetSelector
    {
        

        [Serializable]
        public struct Target //change to class?
        {
            public GameObject target;
            public float aggroRange;
            public float aggroLostRange;
            public int cooldown;            

            public Target(TargetType Type)
            {
                target = null;
                aggroRange = 6500;
                aggroLostRange = 17500;
                cooldown = 0;             
            }

        }

        //TODO: replace with flags?
        public bool AttackDanger;
        public bool PrioritizeGoal;
        public float GoalOffset = 500;

        public int cooldownTime = 15;

        public AgentCommand command; 


        public Target[] targets;

        public GameObjectType DefaultTarget { get; set; }

        private int timeSinceAttack = 0;

        public void SetTarget(GameObject target, TargetType targetType, int cooldown = 0)
        {
            targets[(int)targetType].target = target;
            if(cooldown != 0)
                targets[(int)targetType].cooldown = cooldown;
        }

        public GameObject GetTarget(TargetType targetType)
        {
            return targets[(int)targetType].target;
        }

        public void ClearAllTargets()
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].target = null;
            }   
        }

        public void SetAggroRange(float aggroRange, float aggroLostRange, TargetType targetType)
        {
            targets[(int)targetType].aggroRange = aggroRange;
            targets[(int)targetType].aggroLostRange = aggroLostRange;

        }

        public TargetSelector()
        {
            AttackDanger = true;
            Init();
        }

        public void Init()
        {
            int targetNum = 3;
            targets = new Target[targetNum];
            for (int i = 0; i < targetNum; i++)
            {
                targets[i] = new Target((TargetType)i);
            }
            targets[(int)TargetType.Ally].aggroLostRange = float.MaxValue;
            targets[(int)TargetType.Goal].aggroLostRange = float.MaxValue;
        }
        

        public GameObject GetPotentialTargets(GameEngine gameEngine, GameObjectType type, Vector2 position)
        {
            GameObject target = null;
            float minimalDistance = 1000000;
            List<GameObject> targetList = new List<GameObject>();
            gameEngine.CollisionManager.GetAllObjectPossiblyInRange(position, 10000, targetList);
            foreach (var item in targetList)
            {
                if ((item.GetObjectType() & type) > 0)
                {
                    float distance = (position - item.Position).Length();

                    if (distance < minimalDistance)
                    {
                        minimalDistance = distance;
                        target = item;
                    }
                }
            }

            return target;
        }

        public void Update(GameEngine gameEngine, Agent agent)
        {
            GameObject goal = GetTarget(TargetType.Goal);
            var faction = gameEngine.GetFaction(agent.FactionType);

            UpdateGoal(gameEngine, agent, goal);
                        

                        
            //TODO: change?
            UpdateTarget(ref targets[(int)TargetType.Enemy], gameEngine, agent);

            
            if (GetTarget(TargetType.Goal) == null)
            {
                var target = GetPotentialTargets(gameEngine, DefaultTarget, agent.Position);
                SetTarget(target, TargetType.Goal);
            }

            UpdateTarget(ref targets[(int)TargetType.Ally], gameEngine, agent, false);

            timeSinceAttack++;            
            if ((agent.ControlSignal & ControlSignals.OnTakingDamage) > 0 && agent.lastDamagingObjectToCollide != null && agent.lastDamagingObjectToCollide.GetAgentAncestor() != null) //&& agent.faction != 0
            {
                timeSinceAttack = 0;
                FactionType attackerFaction = agent.lastDamagingObjectToCollide.GetAgentAncestor().GetFactionType();                         
                if ( attackerFaction != agent.FactionType &&  //attackerFactionIndex != 0 &&
                    (faction.GetRelationToFaction(attackerFaction) < 0.5f || ((agent.ControlSignal & ControlSignals.OnLowHitpoints) > 0)))
                {       
                    agent.SetTarget(agent.lastDamagingObjectToCollide.GetAgentAncestor(), TargetType.Enemy);
                    faction.CallForHelp(gameEngine, agent.GetTarget(gameEngine, TargetType.Enemy), agent, Math.Max(targets[(int)TargetType.Enemy].aggroLostRange, 2000));
                }                                
            }

            var enemy = GetTarget(TargetType.Enemy);
            if (enemy != null)
            {
                if (timeSinceAttack > 30 && faction.GetRelationToFaction(enemy.GetFactionType()) >= 0)
                {
                    SetTarget(null, TargetType.Enemy);
                }
                if (enemy.IsVisible(agent) == false)
                {
                    SetTarget(null, TargetType.Enemy);
                }
            }

        }

        private void UpdateGoal(GameEngine gameEngine, Agent agent, GameObject goal)
        {
            if (goal != null && goal.IsNotActive)
            {
                SetTarget(null, TargetType.Goal);
            }
            else
            {                
                             
            }
        }

        private void UpdateTarget(ref Target target, GameEngine gameEngine, Agent agent, bool isEnemy = true ) //change
        {
            if (target.target != null && (target.target.IsNotActive || target.target.IsCloaked))
            {
                target.target = null;
                target.cooldown = 0;
            }

            if (agent.GetFactionType() != 0) //change
            {
                if ((target.target == null & target.cooldown <= cooldownTime/2) || (target.target != null && target.cooldown <= 0 &&  target.target.IsNotActive || target.target != null && (target.target.Position - agent.Position).Length() > target.aggroLostRange
                    || target.target != null && target.target.GetFactionType() == agent.FactionType))
                {
                    //Or Getting hit by a target with higher priorety
                    target.cooldown = cooldownTime;
                    if(isEnemy)
                        target.target = FindClosestEnemy(gameEngine, agent, target.aggroRange);
                    else
                        target.target = FindClosestTargetInFaction(gameEngine, agent, target.aggroRange);
                }
            }

            target.cooldown--;
        }
        
         
        public static GameObject FindClosestEnemy(GameEngine gameEngine, Agent agent, float aggroRange)
        {
            GameObject target = null;
            List<GameObject> potentialTargets = new List<GameObject>();
            gameEngine.CollisionManager.GetAllObjectPossiblyInRange(agent.Position, aggroRange, potentialTargets);
            Faction agentFaction = gameEngine.GetFaction(agent.GetFactionType());

            float minDistance = aggroRange * aggroRange;
            for (int i = 0; i < potentialTargets.Count; i++)
            {
                FactionType targetFaction = potentialTargets[i].GetFactionType();
                if (targetFaction != 0 && agentFaction.GetRelationToFaction(targetFaction) < 0 && !potentialTargets[i].IsCloaked && (potentialTargets[i].GetObjectType() & GameObjectType.PotentialTarget) > 0 ) 
                {
                    float distance = (potentialTargets[i].Position - agent.Position).LengthSquared();                    
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        target = potentialTargets[i];
                    }
                }
            }

            return target;
        }

        public static GameObject FindClosestTargetInFaction(GameEngine gameEngine, Agent agent, float aggroRange)
        {
            FactionType agentFaction = agent.GetFactionType();
            GameObject target = null;
            List<GameObject> potentialTargets = new List<GameObject>();
            gameEngine.CollisionManager.GetAllObjectInRange(agent.Position, aggroRange, potentialTargets);
               // gameEngine.PotentialTargets; //replave with gameEngine.GetTargetsInRadius(position, radius);    
            
            float minDistance = aggroRange * aggroRange;
            for (int i = 0; i < potentialTargets.Count; i++)
            {
                
                if (agentFaction == potentialTargets[i].GetFactionType() && potentialTargets[i] != agent)
                {
                    float distance = (potentialTargets[i].Position - agent.Position).LengthSquared();             
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        target = potentialTargets[i];
                    }
                }
            }            
            return target;
        }


        //private GameObject FindClosestTarget(GameEngine gameEngine, Agent agent, float aggroRange, int allyEnemy)
        //{
        //    int agentFaction = agent.GetFaction();
        //    GameObject target = null;
        //    List<GameObject> potentialTargets = gameEngine.potentialTargets; //replave with gameEngine.GetTargetsInRadius(position, radius);    
        //    float minDistance = aggroRange * aggroRange;

        //    for (int i = 0; i < potentialTargets.Count; i++)
        //    {
        //        int targetFaction = potentialTargets[i].GetFaction();
        //        if (targetFaction != 0 && (Math.Abs(Math.Sign(targetFaction - agentFaction)) == allyEnemy) && potentialTargets[i] != agent) //replace with a Ally Matrix
        //        {
        //            float distance = (potentialTargets[i].Position - agent.Position).LengthSquared();                    
        //            if (potentialTargets[i].IsCloaked)
        //                distance *= 3;
        //            if (distance < minDistance)
        //            {
        //                minDistance = distance;
        //                target = potentialTargets[i];
        //            }
        //        }
        //    }

        //    if (target != null && target.IsCloaked)
        //    {
        //        //dummyTarget.Parent = target;
        //       // target = dummyTarget;                
        //    }

        //    return target;
        //}


        /*   private GameObject FindFarthestAlly(GameEngine gameEngine, Agent agent)
           {
               float maxDistance = 0;
                   int faction = agent.GetFaction();
                   GameObject gameObject = null;
                   for (int i = 0; i < gameEngine.potentialTargets.Count; i++)
                   {
                       int targetFaction = gameEngine.potentialTargets[i].GetFaction();
                       if (targetFaction == agent.faction && gameEngine.potentialTargets[i] != agent) //change
                       {
                           float distance = (gameEngine.potentialTargets[i].Position - agent.Position).LengthSquared();                       
                           if (distance > maxDistance)
                           {
                               maxDistance = distance; 
                               gameObject = gameEngine.potentialTargets[i];
                           }
                       }
                   }

                   if (gameObject != null)
                   {
                       farAlly = gameObject;
                   }

               return farAlly;
           }

           private GameObject FindClosestAlly(GameEngine gameEngine, Agent agent)
           {
               float maxDistance = float.MaxValue;
               int faction = agent.GetFaction();
               GameObject gameObject = null;
               for (int i = 0; i < gameEngine.potentialTargets.Count; i++)
               {
                   int targetFaction = gameEngine.potentialTargets[i].GetFaction();
                   if (targetFaction == agent.faction && gameEngine.potentialTargets[i] != agent) //change
                   {
                       float distance = (gameEngine.potentialTargets[i].Position - agent.Position).LengthSquared();
                       if (distance < maxDistance)
                       {
                           maxDistance = distance;
                           gameObject = gameEngine.potentialTargets[i];
                       }
                   }
               }

               if (gameObject != null)
               {
                   farAlly = gameObject;
               }

               return farAlly;
           }*/

        public TargetSelector GetWorkingCopy()
        {
            TargetSelector clone = (TargetSelector)MemberwiseClone();
            clone.Init();
            return clone;            
        }
    }
}
