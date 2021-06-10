//using SolarConflict.Framework.MetaGame.World;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using XnaUtils;
//using Microsoft.Xna.Framework.Graphics;

//namespace SolarConflict.Test {

//    /// <summary>Visits every node in the galaxy, calls Scene.Update() and Scene.Draw() a bunch therein, times them</summary>
//    /// <remarks>KLUDGY</remarks>
//    class GalacticSpeedTest /*: Activity*/ {

//        /// <summary>TEMP KLUDGE</summary>
//        /// <remarks>TODO: make this class an Activity, at the moment we hack it into Scene, instead, in a pretty ugly way. Some commented-out snippets in this class
//        /// from when we tried to make it an Activity, all prepended with "ACTIVITY"</remarks>
//        public static GalacticSpeedTest Inst { get; private set;}

//        GameEngine _gameEngine;

//        int _drawsThisNode;
//        long _drawTimeThisNodeInMillis;

//        /// <summary>{nodeId -> drawsPerSecond</summary>
//        Dictionary<int, float> _nodeDrawRates;
//        /// <summary>{nodeId -> updatesPerSecond (logic only, disregards draws)</summary>
//        Dictionary<int, float> _nodeUpdateRates;

//        float _maxTime;
//        float _maxTimePerUpdate;          
                
//        int _stepEnteredNode;
//        int _stepsPerNode;
//        int _stepStarted;

//        Stopwatch _stopwatch;

//        long _timeEnteredNodeInMillis;

//        int _updatesThisNode;
//        long _updateTimeThisNodeInMillis;

//        /// <param name="stepsPerNode">The test will execute this many Scene.Update() and Scene.Draw() calls in each node of the galaxy map (unless it times out, see maxTime)</param>
//        /// <param name="maxTime">Max real time for the test (timeout condition is an alternative to simulating stepsPerNode steps in each node in the galaxy)</param>
//        /// <param name="maxTimePerUpdate">Max real time, in seconds, per GalacticSpeedTest.Update() and GalacticSpeedTest.Draw() call (each of which will call at least one
//        /// Scene.Update() or Scene.Draw())</param>
//        public static void Start(int stepsPerNode, float maxTime, float maxTimePerUpdate) {
//            if (Inst != null)
//                // Already started
//                return;

//            Inst = new GalacticSpeedTest(stepsPerNode, maxTime, maxTimePerUpdate);
//            //ACTIVITY
//            //ActivityManager.Inst.SwitchActivity(Inst);
//        }
                
//        private GalacticSpeedTest(int stepsPerNode, float maxTime, float maxTimePerUpdate) {
//            Debug.Assert(GalaxyMap.Inst.CurrentScene != null && GalaxyMap.Inst.CurrentNodeIndex >= 0, "Current scene not found in galaxy map");

//            _stepsPerNode = stepsPerNode;
//            _maxTime = maxTime;
//            _maxTimePerUpdate = maxTimePerUpdate;

//            _gameEngine = GalaxyMap.Inst.CurrentScene?.GameEngine;
            
//            _stopwatch = new Stopwatch();
//            _stopwatch.Start();
//            _stepStarted = _gameEngine.FrameCounter;            

//            _nodeDrawRates = new Dictionary<int, float>();
//            _nodeUpdateRates = new Dictionary<int, float>();

//            // KLUDGILY Make player agents sorta-immortal
//            _gameEngine.PlayerAgents.Do(a => {
//                a.MaxHitpoints = int.MaxValue / 2;
//                a.CurrentHitpoints = a.MaxHitpoints;
//                });

//            EnterNode(GalaxyMap.Inst.CurrentNodeIndex);
//        }

//        void EnterNode(int nodeIndex) {
//            GalaxyMap.Inst.WarpToNode(nodeIndex);
//            Debug.Assert(GalaxyMap.Inst.CurrentNodeIndex == nodeIndex, "Node change failed");

//            // Snag the game engine from the scene to which we just switched, then switch back to this activity
//            _gameEngine = GalaxyMap.Inst.CurrentScene.GameEngine;
//            //ACTIVITY
//            //ActivityManager.Inst.SwitchActivity(this);

//            Debug.Assert(!_nodeUpdateRates.ContainsKey(nodeIndex), "Multiply-visited node");
            
//            _stepEnteredNode = _gameEngine.FrameCounter;
//            _timeEnteredNodeInMillis = _stopwatch.ElapsedMilliseconds;

//            _drawsThisNode = 0;
//            _drawTimeThisNodeInMillis = 0;
//            _updatesThisNode = 0;
//            _updateTimeThisNodeInMillis = 0;
//        }

//        //ACTIVITY
//        //public override void Draw(SpriteBatch sb) {
//        public void Draw(SpriteBatch sb) {
//            var drawStartTime = _stopwatch.ElapsedMilliseconds;
//            //while (_drawsThisNode < _stepsPerNode && 0.001f * (_stopwatch.ElapsedMilliseconds - drawStartTime) < _maxTimePerUpdate) {
//            //    _gameEngine.Draw(sb);
//            //    ++_drawsThisNode;
//            //}
//            _gameEngine.Draw(sb); // TEMP
//            _drawTimeThisNodeInMillis += _stopwatch.ElapsedMilliseconds - drawStartTime;
//        }

//        void LeaveNode() {
//            var index = GalaxyMap.Inst.CurrentNodeIndex;
//            Debug.Assert(index >= 0, "Current scene not found in galaxy map");

//            // Note stats
//            var time = 0.001f * (_stopwatch.ElapsedMilliseconds - _timeEnteredNodeInMillis);

//            _nodeUpdateRates[index] = ((float)_updatesThisNode * 1000f) / _updateTimeThisNodeInMillis;
//            _nodeDrawRates[index] = ((float)_drawsThisNode * 1000f) / _drawTimeThisNodeInMillis;
//        }

//        /// <returns>true if test done</returns>
//        bool Step() {
//            if (_stopwatch.ElapsedMilliseconds >= _maxTime * 1000f)
//                // Timed out
//                return true;

//            if (_gameEngine.FrameCounter - _stepEnteredNode >= _stepsPerNode) {
//                // Lingered long enough, warp to next node
//                var nextNode = GalaxyMap.Inst.CurrentNodeIndex + 1;
//                Debug.Assert(nextNode >= 0, "Current node not found");

//                if (nextNode > 5)
//                    return true; // TEMP DEBUG

//                if (GalaxyMap.Inst.Nodes.Count <= nextNode)
//                    // We're done
//                    return true;

//                LeaveNode();
//                EnterNode(nextNode);                
//            }

//            var startTime = _stopwatch.ElapsedMilliseconds;
//            _gameEngine.Update();
//            ++_updatesThisNode;
//            _updateTimeThisNodeInMillis += _stopwatch.ElapsedMilliseconds - startTime;            

//            return false;
//        }

//        //ACTIVITY
//        //public override bool Update(InputState inputState) {            
//        public bool Update(InputState inputState) {            
//            var updateStartTime = _stopwatch.ElapsedMilliseconds;
//            while (0.001f * (_stopwatch.ElapsedMilliseconds - updateStartTime) < _maxTimePerUpdate)
//                if (Step())
//                    return true;                            
            
//            return false;
//        }        
//    }
//}
