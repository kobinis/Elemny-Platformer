using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework.Utils;
using SolarConflict.Framework.World.MetaGame;
using SolarConflict.Session;
using SolarConflict.Session.World.Generation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using XnaUtils;
using XnaUtils.Framework.Graphics;
using XnaUtils.Graphics;

namespace SolarConflict.Framework.MetaGame.World
{
    [Serializable]
    public class GalaxyMap
    {
        public static GalaxyMap Inst => Session.GameSession.Inst.GalaxyMap;

        private List<NodeInfo> nodes;
        public List<NodeInfo> Nodes { get { return nodes; } }
        //private int time;
        public int CurrentNodeIndex { get; private set; }
        private Scene currentScene;
        public Scene CurrentScene { get { return currentScene; } private set { currentScene = value; } }

        public GalaxyMap()
        {
        }

        public void GenerateGalaxy()
        {
            CurrentNodeIndex = -1;
        }

        public void Init()
        {
            var gg = new GalaxyGenerator();
            nodes = gg.GetSpaceNodes();
            //_backtexture = gg.CreateFactionTexture();
        }

        public void AddNode(NodeInfo nodeinfo)
        {
            //nodeinfo.Index = nodes.Count;
            nodes.Add(nodeinfo);
        }

        public int? GetNodeIndexInPosition(Vector2 serachPosition)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].IsPositionOverNode(serachPosition))
                {
                    return i;
                }
            }
            return null;
        }

        public NodeInfo GetCurrentNodeInfo()
        {
            if (CurrentNodeIndex < 0)
                return null;
            return nodes[CurrentNodeIndex];
        }



        public bool WarpToValidNode(int dstNodeindex)
        {

            if (DebugUtils.Mode == ModeType.Debug)
            { 
                WarpToNode(dstNodeindex);
                return true;
            }
            else
            {
                if (IsNodeInRange(CurrentNodeIndex, dstNodeindex) || nodes[dstNodeindex].Visibility == NodeVisibility.Visited || nodes[dstNodeindex].Visibility == NodeVisibility.Visible)// || nodes[dstNodeindex].Visibility == NodeVisibility.Visited) || nodes[dstNodeindex].Visted)
                {
                    WarpToNode(dstNodeindex);
                    return true;
                }
                return false;
            }

        }


        public void SaveScene(Scene scene, int index)
        {
            string path = GetNodeFullPath(index);
            new FileInfo(path).Directory.Create();
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.SurrogateSelector = SerializationUtils.MakeSurrogateSelector();
            var file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(file, scene);
            file.Close();
        }

        private Scene LoadeNodeScene(int index)
        {
            string path = GetNodeFullPath(index);
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.SurrogateSelector = SerializationUtils.MakeSurrogateSelector();
            var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var scene = formatter.Deserialize(file) as Scene;
            file.Close();
            return scene;
        }

        private Scene GetSceneFromNodeIndex(int index)
        {
            Scene scene = null;
            if (IsNodeFileExists(index))
            {
                try
                {
                    scene = LoadeNodeScene(index);
                    SaveScene(scene, index); //?? why
                }
                catch (Exception e)
                {
                    ActivityManager.Inst.AddToast(e.ToString(), 100);
                    scene = GenerateScene(nodes[index], index);
                }
               
            }
            else
            {                
                scene = GenerateScene(nodes[index], index);
            }
            return scene;
        }

        private string GetNodeFullPath(int index)
        {
            string path = GameSession.Inst.SavePath;
            return Path.Combine(path, "Node" + index + ".bin");
        }

        private bool IsNodeFileExists(int index)
        {
            string path = GetNodeFullPath(index);
            return File.Exists(path);
        }

        public void RevelNearbyNodes()
        {
            NodeInfo node = nodes[CurrentNodeIndex];
            if (currentScene == null || !currentScene.IsWarpDisabled)
                foreach (var nodeIndex in node.Neighbors)
                    nodes[nodeIndex].Reveal();
        }


        public void WarpToNode(int index)
        {
            if(nodes == null)
            {
                if (CurrentNodeIndex == -1)
                {
                    Init();
                }
            }
            if (index < 0 || index >= nodes.Count)
                throw new Exception("Node index is out of range");
            if(CurrentNodeIndex >= 0)
                nodes[CurrentNodeIndex].PopulateDescription(CurrentScene);
            NodeInfo node = nodes[index];
            node.Visibility = NodeVisibility.Visited;
            //if(currentScene == null || !currentScene.IsWarpDisabled)
            //    foreach (var nodeIndex in node.Neighbors)
            //        nodes[nodeIndex].Reveal();            

            var previousIndex = CurrentNodeIndex;
            var previousScene = currentScene;            
            CurrentNodeIndex = index;
            CurrentScene = GetSceneFromNodeIndex(index);
            
            if (index != previousIndex)
            {
                if (previousScene != null)
                {
                    previousScene.WarpOutPlayerFaction();
                    previousScene.UpdateNodeInfo(Nodes[previousIndex]);
                }
                CurrentScene.UpdateNodeInfo(Nodes[index]);
                CurrentScene.GameEngine.Update(InputState.EmptyState);
                CurrentScene.WarpInPlayerFaction();
                CurrentScene.GameEngine.Update(InputState.EmptyState);
            }
            if (previousScene != null) //Saves current node after you warpout
            {
                SaveScene(previousScene, previousIndex);
                GameSession.Inst.Save();
            }

            ActivityManager.Inst.SwitchActivity(CurrentScene, false);           
        }

        public Scene GenerateScene(NodeInfo info, int nodeIndex)
        {
            

            GenerationManager generator = new GenerationManager();
            var scene = generator.Generate(info);
            MissionContent.AddMissions(scene, info, nodeIndex);
            return scene;
        }

        public void Continue()
        {
            if (currentScene != null)
                ActivityManager.Inst.SwitchActivity(currentScene, false);
            else
                WarpToNode(Math.Max(CurrentNodeIndex, 0));
            
        }      

        public bool IsNodeInRange(int currentNodeIndex, int destNodeIndex)
        {
            Vector2 curPostion = nodes[currentNodeIndex].Position;
            Vector2 destPosition = nodes[destNodeIndex].Position;
            float distance = (curPostion - destPosition).Length();
            return distance <= GalaxyGenerator.JUMP_RANGE;
        }

        public bool WarpToHomeWorld()
        {
            if (MetaWorld.Inst.GetFaction(FactionType.Player).WarpCooldown <= 0)
            {
                MetaWorld.Inst.GetFaction(FactionType.Player).WarpCooldown = Faction.WARP_COOLDOWNTIME;
                WarpToNode(MetaWorld.Inst.GetFaction(FactionType.Player).HomeWorldIndex);
                return true;
            }
            return false;
        }

        private Dictionary<FactionType, int> CountNumberOfNodesPerFaction()
        {
            Dictionary<FactionType, int> counter = new Dictionary<FactionType, int>();
            foreach (var node in nodes)
            {
                FactionType type = node.ControllingFaction;
                int value;
                counter.TryGetValue(type, out value);
                value += 20 - node.Level;
                counter[type] = value;
            }
            return counter;
        }

        private Dictionary<FactionType, float> GetFactionStrength(float time)
        {
            Dictionary<FactionType, int> counter = CountNumberOfNodesPerFaction();
            Dictionary<FactionType, float> strength = new Dictionary<FactionType, float>();
            float sum = 0;
            foreach (var pair in counter)
            {
                sum += pair.Value;
            }

            foreach (var pair in counter)
            {
                strength[pair.Key] = pair.Value / sum;                
            }

            return strength;
               
        }



        public void EveolveGalaxy(int time, Random rand)
        {
            Dictionary<FactionType, float> strengthDic = GetFactionStrength(0);

            foreach (var node in nodes)
            {
                if(rand.Next(10) == 0)
                {
                    float strength = strengthDic[node.ControllingFaction];
                    List<int> neighbors = new List<int>(node.Neighbors);
                    FMath.Shuffle(neighbors, rand);
                    foreach (var neighbor in neighbors)
                    {
                        if(FMath.Bern((2-strength) * 0.25f, rand))
                        {
                            nodes[neighbor].ControllingFaction = node.ControllingFaction;
                        }
                    }
                }
            }
        }

        public void PopolateNodeDescription()
        {
            if(CurrentNodeIndex >= 0 && currentScene != null)
                nodes[CurrentNodeIndex].PopulateDescription(currentScene);
        }

    }
}

