using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;

namespace SolarConflict.GameContent.Activities.Challenges
{
    class RacingChallengeScene:Scene
    {
        string[] defaultContestents = new[] { "PlayerClone", "SmallShip6A" , "CargoShip1C" };
        private List<GameObject> _checkPoints;        
        private List<GameObject> _contestents;
        
        public RacingChallengeScene(string parameters):base(parameters, false)
        {
           
        }

        public override void InitScript(string parameters = null, ActivityParameters activityParameters = null)
        {
            _contestents = new List<GameObject>();
            _checkPoints = new List<GameObject>();
            MakeTrack(4, 5);
            GameEngine.Update(InputState.EmptyState);
            //if(string.IsNullOrEmpty(parameters))
            //{

            //}
            string[] contestents = defaultContestents;
            for (int i = 0; i < defaultContestents.Length; i++)
            {
                FactionType faction = (FactionType)(i + 1);
                AddContestant(defaultContestents[i], faction, i);
            }
            
        }

        private void AddContestant(string emitterID, FactionType team, int index)
        {
            float rotation = FMath.Rotation(_checkPoints[1].Position - _checkPoints[1].Position);
            Vector2 position = _checkPoints[0].Position+ FMath.RotateVector(FMath.GetFormationPosition(index) * 200, rotation);
            Agent agent = ContentBank.Inst.GetEmitter(emitterID).Emit(GameEngine, null, team, position, Vector2.Zero, rotation) as Agent;
        //    var control = new SmartAI();
            //control.PrioritizeGoal = true;
           // agent.control.controlAi = control; 
            agent.SetMeterValue(MeterType.CheckpointIndex, 1);
            agent.SetTarget(_checkPoints[1], TargetType.Goal);
            _contestents.Add(agent);
        }

        public override void UpdateScript(InputState inpuState)
        {
            foreach (var gameObject in _contestents)
            {
                int checkPointIndex = (int)gameObject.GetMeterValue(MeterType.CheckpointIndex) % _checkPoints.Count;
                GameObject checkpoint = _checkPoints[checkPointIndex];
                if (GameObject.DistanceFromEdge(gameObject, checkpoint) <= 0)
                {
                    gameObject.SetMeterValue(MeterType.CheckpointIndex, checkPointIndex + 1);
                    gameObject.SetTarget(_checkPoints[(checkPointIndex + 1) % _checkPoints.Count], TargetType.Goal);
                }
            }            
        }

        public override void DrawScript(SpriteBatch sb)
        {
            base.DrawScript(sb);
        }

        private void MakeTrack(int mumberOfCheckpoints, int seed)
        {
            IEmitter checkpointEmitter = ContentBank.Inst.GetEmitter("Checkpoint");
            Random random = new Random(seed);
            Vector2 size = Vector2.One * 10000;
            for (int i = 0; i < mumberOfCheckpoints; i++)
            {
                Vector2 postion = new Vector2((float)random.NextDouble() * size.X - size.X * 0.5f, (float)random.NextDouble() * size.Y - size.Y * 0.5f);
                GameObject chekpoint = checkpointEmitter.Emit(this.GameEngine, null, Framework.FactionType.Neutral, postion, Vector2.Zero, 0);
                _checkPoints.Add(chekpoint);
                
            }
        }

        public static Activity ActivityProvider(string parameters) //TODO: change
        {
            return new RacingChallengeScene(parameters);
        }

    }
}
