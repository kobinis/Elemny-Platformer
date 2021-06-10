using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using SolarConflict.Framework;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Scenes;
using SolarConflict.Framework.Scenes.HudEngine.Components;

namespace SolarConflict.GameContent.Activities.Challenges
{
    class LadderChallenge:Scene
    {
        private class FactionData
        {
            public FactionType FactionType;
            public int AgentIndex;
            public Vector2 Position;
            public float Rotation;
            public Agent agent;

            public FactionData(FactionType factionType, Vector2 position, float rotation)
            {
                FactionType = factionType;
                Position = position;
                Rotation = rotation;
            }
        }

        const float RADIUS = 2000;
        private IEmitter warpEffect; //Matador
        private List<string> loadoutsID = new List<string> { "PrologShip1", "Kemron", "PrologFirstEnemy", "SmallShip9B", "SmallShip5A", "MediumShip14A", "MediumShip9A" };
        private List<AgentLoadout> loadouts;
        private List<FactionType> factions = new List<FactionType> { FactionType.Player, FactionType.Empire };
        private Dictionary<FactionType, FactionData> factionData;
        

        public LadderChallenge():base(null, false, 0)
        {
            CameraManager.ZoomType = CameraZoomType.Auto;
            HudManager.AddComponent(new TargetIndicator());
            //HudManager.AddComponent(new AnnouncerEffects());
        }
            

        public override void InitScript(string parameters = null, ActivityParameters activityParameters = null)
        {
            loadouts = new List<AgentLoadout>();
            foreach (var id in loadoutsID)
            {
                loadouts.Add(ContentBank.Inst.GetLoadout(id));
            }            
            loadouts = loadouts.OrderBy(o => o.Cost).ToList();

            factionData = new Dictionary<FactionType, FactionData>();
            for (int i = 0; i < factions.Count; i++)
            {
                float angle = i / (float)factions.Count * MathHelper.TwoPi;
                Vector2 pos = FMath.ToCartesian(RADIUS, angle);
                factionData.Add(factions[i], new FactionData(factions[i], pos, -angle));
            }

            AddFactionShips();
        }

        private void AddFactionShips()
        {
            foreach (var factionDataKeyValue in factionData) //TODO: add reswapn cooldown
            {
                if(factionDataKeyValue.Value.agent == null || factionDataKeyValue.Value.agent.IsNotActive)
                {
                    AddFactionShip(factionDataKeyValue.Value);
                }
            }
        }

        private void AddFactionShip(FactionData factionData)
        {
            if (factionData.AgentIndex < loadouts.Count)
            {
                var loadout = loadouts[factionData.AgentIndex];
                factionData.AgentIndex++;
                factionData.agent = loadout.Emit(GameEngine, null, factionData.FactionType, factionData.Position, Vector2.Zero, factionData.Rotation) as Agent;
                if (factionData.FactionType == FactionType.Player)
                    factionData.agent.SetControlType(AgentControlType.Player);
            }
        }

        public override void UpdateScript(InputState inpuState)
        {
            AddFactionShips();
        }

        public override void DrawScript(SpriteBatch sb)
        {
            string text = string.Empty;
            foreach (var factiontype in factions)
            {
                var faction = GameEngine.GetFaction(factiontype);
                text += faction.Name + " : " + faction.GetMeter(MeterType.FactionKills).ToString()
                +"      ";
            }
        }

        public static Activity ActivityProvider(string parameters) //TODO: change
        {
            return new LadderChallenge();
        }
    }
}
