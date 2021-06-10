using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Session.World.MissionManagment;
using SolarConflict.Session.World.MissionManagment.GlobalObjectives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaUtils;

namespace SolarConflict.Framework.InGameEvent.GenericProcess
{

    [Serializable]
    class ReferencePositionProvider
    {
        private AgentType _agentType;
        private GameObject _gameObject;
        private Vector2 _offset;      

        public ReferencePositionProvider(Vector2 offset, AgentType agentType = AgentType.Player, GameObject gameObject = null)
        {
            _offset = offset;
            _agentType = agentType;
            _gameObject = gameObject;
        }

        public Vector2 GetPosition(GameEngine gameEngine) //maybe return a tuple of position and rotation
        {
            switch (_agentType)
            {
                case AgentType.Player:
                    _gameObject = gameEngine.PlayerAgent;
                    break;
                case AgentType.Mothership:
                    _gameObject = gameEngine.GetFaction(FactionType.Player).Mothership;
                    break;
            }
            Vector2 pos = Vector2.Zero;
            if (_gameObject != null)
                pos = _gameObject.Position;
            pos += _offset;

            return pos;
        }

        //public float GetRotation();
    }

    [Serializable]
    class SpwanEnemysProcess : GameProcess, IEmitter
    {
        private List<IEmitter> _emitters;
        private int _numberOfWaves;
        private ReferencePositionProvider _positionProvider;
        private FactionType _factionType;

        private List<GameObject> _targets;
        private int _waveCounter;
        private PlaceHolderObjective _objective;

        public bool IsDropBlueprints;
        
        public string ID { get; set; }

        public SpwanEnemysProcess(string emitterList, ReferencePositionProvider referencePositionProvider, PlaceHolderObjective objective = null)
        {
            _positionProvider = referencePositionProvider;
            _emitters = ContentBank.Inst.GetEmittersFromList(emitterList);
            _targets = new List<GameObject>();
            _factionType = FactionType.Pirates1;
            _objective = objective;
            IsDropBlueprints = true;
        }

        public override void InitProcess(GameEngine gameEngine)
        {
            SpwanEnemies(gameEngine);
        }

        private void SpwanEnemies(GameEngine gameEngine)
        {
            Vector2 pos = _positionProvider.GetPosition(gameEngine);
            for (int i = 0; i < _emitters.Count; i++)
            {
                var refpos = pos + FMath.GetFormationPosition(i) * 300;

                var enemy = _emitters[i].Emit(gameEngine, null, _factionType, refpos, Vector2.Zero, 0, 0, 0, 0, null, gameEngine.Level);
             
                if (IsDropBlueprints && enemy is Agent)
                {
                    var agent = enemy as Agent;
                    agent.AddSystem(new SlotItemDropSystem(ControlSignals.OnDestroyed, 0, true));
                    agent.AddSystem(new Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers.BlueprintEmitterSystem(agent.ID + "BlueprintPart"));                    
                }
                _targets.Add(enemy);
            }
        }


        public override void Update(GameEngine gameEngine)
        {
            bool isDone = _targets.Count > 0;
            GameObject obj = null;
            foreach (var item in _targets)
            {
                if (item.IsActive)
                {
                    obj = item;
                    isDone = false;
                }
            }
            if(isDone)
            {
                Finished = true;
                if(_objective != null)
                    _objective.Status = MissionObjective.ObjectiveStatus.Completed;
            }
            if(obj != null && _objective != null)
            {
                _objective.Position = obj.Position;
            }
            
        }

        public override GameProcess GetWorkingCopy()
        {
            SpwanEnemysProcess clone = MemberwiseClone() as SpwanEnemysProcess;
            clone._waveCounter = 0;
            clone._targets = new List<GameObject>();
            if (clone._objective != null)
                clone._objective.Status = MissionObjective.ObjectiveStatus.Ongoing;
            return clone;         
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            gameEngine.AddGameProcces(GetWorkingCopy());
            return null;
        }
    }
}
