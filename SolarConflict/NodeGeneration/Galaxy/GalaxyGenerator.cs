using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework.Scenes;
using SolarConflict.Framework.World.MetaGame;
using SolarConflict.GameContent.Activities;
using SolarConflict.GameContent.Activities.Levels;
using SolarConflict.NodeGeneration.NodeProcesess;
using SolarConflict.Session.World.Generation;
using SolarConflict.XnaUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XnaUtils;
using XnaUtils.Framework.Graphics;

namespace SolarConflict.Framework.MetaGame.World
{
    /// <summary>
    /// Generates the galaxy
    /// </summary>
    public class GalaxyGenerator
    {
        private struct FactionInitalPostision
        {
            public Vector2 Position { get; set; }
            public FactionType Faction { get; set; }
            public float Weight { get; set; }
        }
        public const int JUMP_RANGE = 90;
        public static readonly int GALAXY_SEED = 345343; //345341;
        private const int MINIMAL_DISTNACE_NODE = 60;
        private readonly int NUM_OF_PLANETS_TEXTURES = 20;
        private readonly int NUM_OF_GENERATED_NODES = 200;
        private readonly int NUM_OF_CONST_NODES = 20;
        private readonly int NUM_OF_FACTIONS_INITAIL_POSITSION = 10;
        private readonly int MAP_RADIUS = 600;
        private readonly int MAX_NODE_LEVEL = 6;
        float DeadZoneRatio = 0.3f;

        private List<NodeInfo> _spaceNodes;
        private Random _random;
        private List<FactionInitalPostision> _factionsInitalPositions;
        private List<string> _nodesNames;
        private Dictionary<NodeType, float> _nodeTypeFrequencies;

        public GalaxyGenerator()
        {
            _spaceNodes = new List<NodeInfo>(NUM_OF_GENERATED_NODES + NUM_OF_CONST_NODES);
            _random = new Random(GALAXY_SEED);

            _nodeTypeFrequencies = new Dictionary<NodeType, float>() {
                { NodeType.AsteroidField, 0.2f },
                { NodeType.BlueGiant, 0.33f },
                { NodeType.BinaryStar, 1f },
                { NodeType.Nebula, 0.2f },
                { NodeType.RedSun, 0.33f },
                { NodeType.WhiteDwarf, 0.33f },
                { NodeType.Vile, 0.33f },
            };

            _nodesNames = LoadNamesListFromFile();
            CreateFactionsInitalPositions();

            CreateConstNodes();
            CreateNodesFromFiles();
            GenerateNodes();
        }

        public List<NodeInfo> GetSpaceNodes()
        {
            return _spaceNodes;
        }

        private void CreateFactionsInitalPositions()
        {
            // REFACTOR: create faction initial positions randomly.
            _factionsInitalPositions = new List<FactionInitalPostision>(NUM_OF_FACTIONS_INITAIL_POSITSION);



            for (int i = 0; i < 5; i++)
            {
                Vector2 position = CreatePosition();
                AddFactionInitalPostision(position, FactionType.Federation, 1.8f);
            }

            AddFactionInitalPostision(new Vector2(-MAP_RADIUS / 3f, MAP_RADIUS * 0.7f), FactionType.Federation, 1.7f);


            for (int i = 0; i < 5; i++)
            {
                Vector2 position = CreatePosition();
                AddFactionInitalPostision(position, FactionType.Empire, 1.8f);
            }

            AddFactionInitalPostision(new Vector2(MAP_RADIUS / 3f, MAP_RADIUS * 0.7f), FactionType.Empire, 1.7f);

            for (int i = 0; i < 5; i++)
            {
                Vector2 position = CreatePosition();
                AddFactionInitalPostision(position, FactionType.TradingGuild, 1f);
            }

            for (int i = 0; i < 25; i++)
            {
                Vector2 position = CreatePosition();
                AddFactionInitalPostision(position, FactionType.Pirates1, 0.5f);
            }

            AddFactionInitalPostision(Vector2.Zero, FactionType.Void, 1.7f);

        }


        private void AddFactionInitalPostision(Vector2 position, FactionType faction, float weight = 1)
        {
            var factionInitalPostision = new FactionInitalPostision()
            {
                Position = position,
                Faction = faction,
                Weight = weight
            };

            _factionsInitalPositions.Add(factionInitalPostision);
        }


        public static List<string> LoadNamesListFromFile(int seed = 23452)
        {
            List<string> nodesNames = new List<string>();

            if (File.Exists(Consts.STAR_NAMES_PATH))
            {
                nodesNames = File.ReadAllLines(Consts.STAR_NAMES_PATH).ToList<string>();
                Random rand = new Random(seed);
                FMath.Shuffle(nodesNames, rand);
            }
            else
            {
                // TODO: catch exception
                throw new FileNotFoundException("Star names file not found.");
            }
            return nodesNames;
        }

        private void CreateNodesFromFiles()
        {

        }

        private void CreateConstNodes()
        {
            var node = new NodeInfo();
            node.ControllingFaction = FactionType.Federation;
            node.FactionsByStrength = new List<FactionType>() { FactionType.Empire, FactionType.Federation };
            node.Level = 1;
            node.Position = Vector2.UnitY * 600;
            node.Name = "Genesis";
            var scene = GameContent.Activities.FirstNode.ActivityProvider() as Scene;
            scene.TryInit(null);
            GalaxyMap.Inst.SaveScene(scene, 0); //Now:
            _spaceNodes.Add(node);

            node = new NodeInfo();
            node.ControllingFaction = FactionType.Empire;
            //node.FactionsByStrength = new List<FactionType>() { FactionType.Empire };
            node.Level = 5;
            node.Position = new Vector2(101, 339);
            node.Name = "Sol";
            _spaceNodes.Add(node);
            //scene = new GenerationManager().Generate(node);
            //scene.GameEngine.AddGameProcces(new TeslaMission(scene.PlayerStartingPoint));
            //Sol.Initialize(scene);
            //GalaxyMap.Inst.SaveScene(scene, 1);

            node = new NodeInfo();
            node.Type = NodeType.RedSun;
            node.ControllingFaction = FactionType.Void;
            node.FactionsByStrength = new List<FactionType>() { FactionType.Void };
            node.Level = 10;
            node.Position = new Vector2(-74, 136);
            node.Name = "Revelation";
            _spaceNodes.Add(node);
        }

        private void GenerateNodes()
        {
            // Create nodes independently
            for (int i = 0; i < NUM_OF_GENERATED_NODES; i++)
            {
                Vector2 position = CreateNodePosition();
                NodeInfo node = CreateGeneratedNode(i, position);
                _spaceNodes.Add(node);
            }

            for (int i = 0; i < 12; i++)
            {
                Vector2 position = CreateNodePosition(MAP_RADIUS * DeadZoneRatio, 0);
                NodeInfo node = CreateGeneratedNode(_spaceNodes.Count + i, position);
                _spaceNodes.Add(node);
            }

            // Set neighbors
            foreach (var galaxyNode in _spaceNodes)
            {
                float distanceThreshold = JUMP_RANGE;
                for (int i = 0; i < _spaceNodes.Count; i++)
                {
                    float distance = Vector2.Distance(galaxyNode.Position, _spaceNodes[i].Position);
                    if (distance < distanceThreshold && galaxyNode != _spaceNodes[i])
                    {
                        galaxyNode.Neighbors.Add(i);
                    }
                }
            }

            // Update based on neighbors
            foreach (var galaxyNode in _spaceNodes)
            {
                var neighborsByFaction = galaxyNode.Neighbors.Select(i => _spaceNodes[i]).Where(n => n.ControllingFaction != galaxyNode.ControllingFaction)
                    .GroupBy(n => n.ControllingFaction)
                    // group neighbors from other factions by their faction
                    .OrderByDescending(g => _random.NextFloat(0f, 0.5f) + g.Count());
                // order by the number of neighbors, plus a random number (to break ties)

                // Set faction strengths based on the above ordering
                neighborsByFaction.Do(g => galaxyNode.FactionsByStrength.Add(g.Key));
            }
        }

        private NodeInfo CreateGeneratedNode(int index, Vector2 position)
        {
            NodeInfo node = new NodeInfo();


            node.Rotation = (float)_random.NextDouble() * MathHelper.TwoPi;
            //node.Index = index;
            node.Name = GetNodeName(index);
            node.Type = GetNodeType(index);
            node.Position = position;
            node.ToolTipPlanetTextureID = "node1";
            //ActivityName = "RareMineralLevel",
            //ActivityParams = string.Empty
            //ActivityName = "LevelFromFile" // Example of loading node from file.
            //ActivityParams = "Levels" + index.ToString()
            SetPropertiesByPosition(position, node);

            return node;
        }

        private void SetPropertiesByPosition(Vector2 position, NodeInfo node)
        {
            FactionType faction = 0;
            float minimalDistance = float.MaxValue;

            foreach (var factionPosition in _factionsInitalPositions)
            {
                float distance = (factionPosition.Position - position).Length();

                if (distance / factionPosition.Weight < minimalDistance)
                {
                    minimalDistance = distance / factionPosition.Weight;
                    faction = factionPosition.Faction;
                }
            }
            node.ControllingFaction = faction;
            node.FactionsByStrength = new List<FactionType>() { faction }; // controlling faction; we'll set the rest next pass            
            node.Level = Math.Min((int)Math.Round((1f - (node.Position.Length() - MAP_RADIUS * DeadZoneRatio) / (MAP_RADIUS - MAP_RADIUS * DeadZoneRatio)) * MAX_NODE_LEVEL) + 1, 10);
        }

        private float MinimalDistanceFromNode(Vector2 position)
        {
            float minimalDistance = float.MaxValue;

            foreach (var node in _spaceNodes)
            {
                float distance = (node.Position - position).Length();

                if (distance < minimalDistance)
                {
                    minimalDistance = distance;
                }
            }
            return minimalDistance;
        }

        public Vector2 CreateNodePosition()
        {
            return CreateNodePosition(MAP_RADIUS, MAP_RADIUS * DeadZoneRatio);
        }

        private Vector2 CreateNodePosition(float maxRad, float minRad = 0)
        {
            Vector2 position = CreatePosition(maxRad, minRad);
            while (MinimalDistanceFromNode(position) < MINIMAL_DISTNACE_NODE)
            {
                position = CreatePosition(maxRad, minRad);
            }
            return position;
        }

        public Vector2 CreatePosition()
        {
            return CreatePosition(MAP_RADIUS, MAP_RADIUS * DeadZoneRatio);
        }

        private Vector2 CreatePosition(float maxRad, float minRad)
        {
            float radius = FMath.TransformToRadius(_random.NextFloat(), maxRad, minRad);
            float angle = (float)_random.NextDouble() * MathHelper.TwoPi;
            return FMath.ToCartesian(radius, angle);
        }

        private string GetNodeName(int i)
        {
            if (i < _nodesNames.Count)
                return _nodesNames[i % _nodesNames.Count];
            return "Node " + i.ToString();
        }

        private string GetNodeToolTipTextureName(int i)
        {
            int planetID = i % NUM_OF_PLANETS_TEXTURES;
            return string.Format("planet ({0})", planetID);
        }

        private NodeType GetNodeType(int index)
        {
            var type = (NodeType)(index % 6);
            if (type == NodeType.WhiteDwarf)
                type = NodeType.Nebula;
            return type;
        }

        public Texture2D CreateFactionTexture()
        {
            int GalaxyRadius = MAP_RADIUS;
            Canvas canvas = new Canvas(GalaxyRadius * 2 + 1, GalaxyRadius * 2 + 1, GraphicsSettingsUtils.GraphicsDevice);
            //  Canvas canvas2 = new Canvas(rad * 2 + 1, rad * 2 + 1, ActivityManager.SpriteBatch.GraphicsDevice);
            for (int y = -GalaxyRadius; y <= GalaxyRadius; y++)
            {
                for (int x = -GalaxyRadius; x <= GalaxyRadius; x++)
                {
                    if (x * x + y * y < GalaxyRadius * GalaxyRadius)
                    {
                        float minimalDistance = float.MaxValue;
                        Vector2 position = new Vector2(x, y);
                        FactionType faction = FactionType.Neutral;
                        foreach (var factionPosition in _spaceNodes)
                        {
                            float distance = (factionPosition.Position - position).Length();

                            if (distance < minimalDistance)
                            {
                                minimalDistance = distance;
                                faction = factionPosition.ControllingFaction;
                            }
                        }
                        //float distance1 = minimalDistance;                     

                        //    float w1 = distance2 / (distance1 + distance2);
                        // float w2 = distance1 / (distance1 + distance2);
                        //  Color color = new Color( FactionColorIndicator.FactionToColor(faction).ToVector3() * w1 +
                        //    FactionColorIndicator.FactionToColor(secFaction).ToVector3() * w2);
                        if (faction == FactionType.Neutral)
                            canvas.SetPixel(x + GalaxyRadius, y + GalaxyRadius, Color.Transparent);
                        else
                            canvas.SetPixel(x + GalaxyRadius, y + GalaxyRadius, FactionColorIndicator.FactionToColor(faction));
                    }
                }
            }


            canvas.SetData();
            return canvas.GetTexture();
        }
    }
}