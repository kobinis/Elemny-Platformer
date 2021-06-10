using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SolarConflict.Framework;
using SolarConflict.Framework.CameraControl.Movment;
using SolarConflict.Framework.CameraControl.Zoom;
using SolarConflict.Framework.MetaGame.World;
using SolarConflict.Framework.Scenes.Activitys;
using SolarConflict.Session;
using SolarConflict.XnaUtils.Input;
using SolarConflict.XnaUtils.SimpleGui.Controllers;
using System;
using XnaUtils;
using XnaUtils.Framework.Graphics;
using XnaUtils.Graphics;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.GameContent.Activities
{
    public class GalaxyMapActivity :SceneActivity
    {        
        private Sprite linePixelSprite = Sprite.Get("Pixel");
        private Sprite galaxyTexture = Sprite.Get("galaxy");
        private Sprite starTexture = Sprite.Get("light");
        private Sprite currentLocationMarker = Sprite.Get("locationmarker");
        private Sprite warpTargetMarker = Sprite.Get("selectgui");
        private Sprite missionMarker = Sprite.Get("goalmarker");

        //private RichTextControl mapHelp;
        private Camera camera;
        private GalaxyMap galaxyMap;
        private NodeInfo selectedNode;
        private int? selectedNodeIndex;
        private Vector2 cursorPosInWorld;
        private ManualMovement cameraMovment;
        private ManualZoomToPoint cameraZoom;
      //  private ManualZoom cameraZoom;
        //private Vector2 cameraSpeed = new Vector2();

        //Temp
        float _starRotation1;
        float _starRotation2;
        Background background;
        private GuiManager _gui;

        public GalaxyMapActivity()          
        {
            background = new Background(2, false);
        }

        protected override void Init(ActivityParameters parameters)
        {
            base.Init(parameters);
            cameraMovment = new ManualMovement();
            cameraZoom = new ManualZoomToPoint();
            cameraZoom.MaxZoom = 10;
            //cameraZoom = new ManualZoom();

            camera = new Camera();
            galaxyMap = GameSession.Inst.GalaxyMap;
            galaxyMap.PopolateNodeDescription();
            IsPopup = true;

            //    camera.Position = (galaxyMap.Nodes[galaxyMap.CurrentNodeIndex].Position + Vector2.Zero) * 0.5f;
            //   cameraZoom.TargetZoom = 1.5f;

            cameraZoom.TargetZoom = 0.75f * GuiManager.Scale;
            camera.Zoom = cameraZoom.TargetZoom;

            cameraZoom.MinZoom = 0.5f;
            GalaxyMap.Inst.RevelNearbyNodes();

            // GUI
            CreateGui();
        }

        void CreateGui() {
            _gui = new GuiManager();

            if (galaxyMap.CurrentNodeIndex != MetaWorld.Inst.GetFaction(FactionType.Player).HomeWorldIndex) {
                // Not at the homeworld, add a button for warping back to it
                var warpHome = new RichTextControl("Warp Home");
                warpHome.Position = new Vector2(ActivityManager.ScreenCenter.X, warpHome.HalfSize.Y);
                warpHome.Action += (source, cursorInfo) => {
                    GalaxyMap.Inst.WarpToHomeWorld();
                };
                warpHome.IsShowFrame = true;

                _gui.Root = warpHome;
            }
            AddHelp(TextBank.Inst.GetString("HelpGalaxyMap"));
        }

        private int? GetNodeIndexUnderCursor(InputState inputState)
        {
            Vector2 cursorPositionOnMap = camera.GetWorldPos(inputState.Cursor.Position);
            return galaxyMap.GetNodeIndexInPosition(cursorPositionOnMap);            
        }

        private NodeInfo GetNodeUnderCursor(InputState inputState)
        {
            Vector2 cursorPositionOnMap = camera.GetWorldPos(inputState.Cursor.Position);
            var index = galaxyMap.GetNodeIndexInPosition(cursorPositionOnMap);
          //  ActivityManager.Inst.AddToast(index.ToString(), 100);
            if(index != null)
            {
                return galaxyMap.Nodes[index.Value];
            }
            return null;
        }        

        public override void Update(InputState inputState)
        {
            base.Update(inputState);            
            _gui.Update(inputState);

            cursorPosInWorld = camera.GetWorldPos(inputState.Cursor.Position);

            if (inputState.Cursor.OnReleaseLeft && (inputState.Cursor.FirstPosition - inputState.Cursor.Position).Length() < 5 && inputState.Cursor.IsActive)
            {
                var nodeIndex = GetNodeIndexUnderCursor(inputState);
                if (nodeIndex == galaxyMap.CurrentNodeIndex)
                {
                    ActivityManager.Inst.Back();
                }
                else
                {
                    if (nodeIndex != null)
                    {

                        if (DebugUtils.Mode == ModeType.Debug && galaxyMap.CurrentScene.IsWarpDisabled)
                        {
                            ActivityManager.Inst.AddToast("Warp inhibitor active. DEBUG WARP ENGAGED", 120);
                            galaxyMap.CurrentScene.IsWarpDisabled = false;
                        }

                        if (galaxyMap.CurrentScene.IsWarpDisabled)
                        {
                            if (nodeIndex.Value == _scene.GetPlayerFaction().HomeWorldIndex)
                            {
                                if(!galaxyMap.WarpToHomeWorld())
                                {
                                    ActivityManager.Inst.AddToast(Color.Red.ToTag("Home warp on cooldown:")+ (_scene.GetPlayerFaction().WarpCooldown/60).ToString(), 60);
                                   
                                }
                            }
                            else
                            {
                                ActivityManager.Inst.AddToast(Color.Red.ToTag("Warp disabled !!") + "\n"
                                    + Color.Yellow.ToTag("Destroy warp inhibitor"), 60 * 2);

                            }
                        }
                        else
                        {
                            if (!galaxyMap.WarpToValidNode(nodeIndex.Value))
                            {
                                ActivityManager.Inst.AddToast(Color.Red.ToTag("Can't warp to node")+"\n"
                                    + "Node out of range", 60*2); //TODO: change messege (not using toast)
                            }
                        }
                    }
                }
            }

            _starRotation1 += 0.01f;
            _starRotation2 -= 0.011f;

            selectedNodeIndex = GetNodeIndexUnderCursor(inputState);
            selectedNode =  GetNodeUnderCursor(inputState);

            //_scene.SceneComponentSelector.Update(inputState);
            

            CameraLogic(inputState);

            
        }


        public override void Draw(SpriteBatch spriteBatch)
        {

           // _scene.Draw(spriteBatch);

            background.Draw(camera);
            camera.UpdateMatrix();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, transformMatrix: camera.Transform);
            camera.CameraDraw(galaxyTexture, Vector2.Zero, 0, 2, Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, transformMatrix: camera.Transform);
            //camera.CameraDraw(galaxyMap._backtexture, Vector2.Zero, 0, 1, Color.White);
            
            DrawNodes(camera);
            DrawMarkings(camera);
            spriteBatch.End();

            camera.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            
            //camera.CameraDraw(galaxyMap._backtexture, Vector2.Zero, 0, 1, new Color(255, 255, 255, 150));



            if (selectedNode != null)
            {
                //// Details in bottom-left
                //selectedNode.DrawTooltip(camera, new Vector2(10, ActivityManager.ScreenSize.Y - 10), string.Empty,
                //    selectedNode.Visibility == NodeVisibility.Hidden);

                //// Brief tooltip at cursor position
                //selectedNode.DrawTooltip(camera, InputState.Inst.Cursor.Position, string.Empty,
                //    true);
                // Never mind, everything at cursor position
                selectedNode.DrawTooltip(camera, ActivityManager.Inst.InputState.Cursor.Position, selectedNodeIndex.Value,
                    selectedNode.Visibility == NodeVisibility.Hidden);
            }

            // spriteBatch.DrawString(Game1.font, cursorPosInWorld.ToString(), Vector2.One * 20, Color.White);

            camera.SpriteBatch.End();
            _gui.Draw();



            base.Draw(spriteBatch);
        }

        private void DrawMarkings(Camera camera)
        {
            var nodes = galaxyMap.Nodes;
            var currentNodeIndex = galaxyMap.CurrentNodeIndex;

           
            camera.CameraDraw(currentLocationMarker, (Vector2)nodes[currentNodeIndex].Position, Game1.time * 0.01f, 0.1f, Color.Green);
           
            if(!galaxyMap.CurrentScene.IsWarpDisabled)
            foreach (var node in nodes)
            {
                if(node.Visibility == NodeVisibility.Visible)
                    camera.CameraDraw(warpTargetMarker, node.Position, 0, 0.05f, Color.Red);
            }

            float sizeMult = (float)(Math.Sin(Game1.time * 0.1f) + 1)*0.9f;
            var missions = MetaWorld.Inst.GlobalMissionManager.GetSelectetMissions();
            foreach (var mission in missions)
            {
                // Mission destination
                if(mission.DestenationNode != null)
                {
                    int index = mission.DestenationNode.Value;
                    camera.CameraDraw(missionMarker, (Vector2)nodes[index].Position, 0, 0.1f * sizeMult, mission.Color);
                }

             
                // Mission objective targets
                var indices = mission.Objective.GetTargetNodeIndices();
                if (indices != null)
                    foreach (var i in mission.Objective.GetTargetNodeIndices())
                        camera.CameraDraw(missionMarker, (Vector2)nodes[i].Position, 0, 0.1f * sizeMult, mission.Color);
            }

        }


        private void DrawNodes(Camera camera) //?move to GalaxyMapActivity
        {
            var nodes = galaxyMap.Nodes;
            var currentNodeIndex = galaxyMap.CurrentNodeIndex;

            for (int i = 0; i < nodes.Count; i++)
            {
                foreach (var j in nodes[i].Neighbors)
                {
                    if (i > j )
                    {
                        GraphicsUtils.Line(camera.SpriteBatch, (Vector2)nodes[i].Position, (Vector2)nodes[j].Position, Color.LightBlue, 10, this.PutPixel);
                    }
                }
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Draw(camera, 0, false);
            }            
        }

        public void PutPixel(SpriteBatch sb, Vector2 point, Color color)
        {
            camera.CameraDraw(linePixelSprite, point, 0, 0.2f, color);
        }




        private void CameraLogic(InputState inputState) //TODO: use camera logic
        {
            cameraMovment.Update(camera, null, null, _scene.GameEngine, inputState);
            cameraZoom.Update(camera, null, null, _scene.GameEngine, inputState);
            //cameraZoom.Update(camera, null, null, _scene.GameEngine, inputState);
            //float targetZoom = camera.Zoom;
            //float zoomValue = targetZoom * MathHelper.Clamp(((50f + MouseUtils.Inst.GetDScroolWheel()) / 50f), 0.9f, 1.1f);
            //targetZoom = MathHelper.Clamp(zoomValue, 0.17f, 8.5f);
            //camera.Zoom = targetZoom;
        }

        private string CrateMapHelpText()
        {
            string factionString = string.Empty;
            foreach (var faction in _scene.GameEngine.Factions)
            {
               
                if (faction != null && faction.FactionType != FactionType.Neutral && faction.FactionType != FactionType.Player) 
                    factionString += faction.ToTag() +"\n";
            }
            string helpText = Color.LightYellow.ToTag("Help:") + "\n" +
                Color.LightYellow.ToTag("Factions:") + "\n" + factionString+"\n";/* +

                Color.LightYellow.ToTag("\nMap Signs:") + "\n" +
                currentLocationMarker.ToTag(Color.Green) + " - Current fleet location\n" +
                warpTargetMarker.ToTag(Color.Yellow) + " - Nodes in warp range\n" +
                missionMarker.ToTag(Color.Red) + " - Mission target nodes (You need to go there to complete the mission)";
                */

            //mapHelp = new RichTextControl(helpText);
            //mapHelp.IsShowFrame = true;
            return helpText;
        }



        public static Activity ActivityProvider(string parameters)
        {                      
            return new GalaxyMapActivity();
        }

        //TODO:
    }
}
