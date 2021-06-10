using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework;
using System;
using System.Collections.Generic;
using XnaUtils;
using XnaUtils.SimpleGui;
using SolarConflict.Framework.Utils;
using XnaUtils.Graphics;
using System.Linq; //TODO: remove
using System.Diagnostics;
using System.Runtime.Serialization;
using SolarConflict.XnaUtils;

namespace SolarConflict
{
    public enum CollisionType
    {
        /// <summary>
        /// Collide with all other game objects, including your type game objects.
        /// </summary>
        CollideAll, //change name
        /// <summary>
        /// Collide all, but only if its displayed on screen (good for performance), otherwise don't collide and don't activate update method.
        /// </summary>
        UpdateOnlyOnScreen,
        /// <summary>
        /// Collide with all other game objects that are not of type collode1 (oldie only with CollideAll, Collide2)
        /// </summary>
        Collide1, //change name        
        /// <summary>
        /// Don't collide with anything (including collide all), and don't create off screen.
        /// </summary>
        Effects,
    }

    public enum DrawType
    {
        Planets,
        /// <summary>
        /// Draw the item on the screen as is. (default option)
        /// </summary>
        Alpha,
        /// <summary>
        /// Draw with lighting using normals.
        /// </summary>
        Lit,
        /// <summary>
        /// Add the colors of the item's texture to the screen.
        /// </summary>
        Additive,
        /// <summary>
        /// Like Alpha but will be in the front of the screen, and won't be hidden behind other items in the same place.
        /// </summary>
        AlphaFront,
        /// <summary>
        /// Don't Draw the item.
        /// </summary>   
        Beams,
        None,
        /// <summary>
        /// Draw in Gui (Don't draw potruding itens, like harppons etc..) //Don't work yet!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// </summary>
       // GUI
    }    

    [Serializable]
    public class GameEngine
    {
        public const int MaxFxNumber = 4000; //Read from settings
        /// <remarks>Only affects lighting. Color channels saturate at 1f.</remarks>
        public Vector3 AmbientColor = Vector3.One * 0.5f;  //
        public Random Rand;        
        public string Text;        
        public Camera Camera;
        public int FrameCounter { get; private set; }
        public List<GameObject> RemoveList; //Maybe make it array?
        public List<GameObject> AddList;
        public CollisionManager CollisionManager;
        public Color ScreenOverlayColor { get; set; }        
        public Dictionary<string, string> Messages { get; private set; }
        
        private List<GameObject> _fxParticles;  //don't collide with anything, has a limit on the number of FX objects, add only ones on and near screen       
        public List<GameObject> _collideAllParticles; //collide with self and nonColide; for ships and shots that can be killed        
        public List<GameObject> _collideParticles1; //collide with all but nonColideParticles1, most of shots will be add to this list                
        public List<GameObject> _collideAllCheckList; //TODO: add a getter 

        //public HashSet<GameObject>[] _collisionList;

        // public List<GameObject> PotentialTargets; //TODO: remove

        public int Level;        

        /// <summary>Stuff to draw, sorted by draw type.</summary>
        private Dictionary<DrawType, List<GameObject>> _drawSets;

        public Faction[] Factions;

        public List<GameObject> PermanentLights { get; private set; }
        public List<GameObject> LightsPerFrame { get; private set; }
        //public List<GameObject> _objectsOnScreen;

        public Scene Scene; //can be null

        private HashSet<FactionType> _survivingFactions;
       
       

        public List<GameProcess> GameProcceses;

        [NonSerialized]
        public SoundEngine SoundEngine;

        public Agent PlayerAgent //Remove
        {
            get { return Scene?.PlayerAgent; }
        }

        /// <remarks>Not terribly efficient</remarks>
        public IEnumerable<GameObject> PlayerAgents => _collideAllParticles.Where(p => p.GetFactionType() == FactionType.Player && p.IsControllable());
        
                //Debug
        int maxFXCount = 0;
        int maxNonColideCount = 0;

        public GameEngine(Camera camera, Scene scene = null, IEnumerable<Faction> factions = null, int seed = 0, int level = 0)
        {
            PermanentLights = new List<GameObject>();
            LightsPerFrame = new List<GameObject>();
            SoundEngine = new SoundEngine(); 
            Level = level;

            GameProcceses = new List<GameProcess>();

            Messages = new Dictionary<string, string>();
            //PotentialTargets = new List<GameObject>();

            
            this.Camera = camera;
            CollisionManager = new CollisionManager(this);
            _survivingFactions = new HashSet<FactionType>();
            //IsBoarderedScreen = false;
            Scene = scene;
            if (seed == 0)
                Rand = new Random(); 
            else
                Rand = new Random(seed);

            Factions = new Faction[(int)FactionType.Size];
            if (factions != null)
            {
                foreach (var item in factions)
                {
                    Factions[(int)item.FactionType] = item;
                }
            }
            
            RemoveList = new List<GameObject>();
            AddList = new List<GameObject>();

            _fxParticles = new List<GameObject>();
            _collideAllParticles =new List<GameObject>();
            _collideParticles1 =  new List<GameObject>();

            _collideAllCheckList = new List<GameObject>();

            _drawSets = new Dictionary<DrawType, List<GameObject>>();
            
            foreach (var t in new DrawType[] {DrawType.Planets,  DrawType.Additive, DrawType.Alpha, DrawType.AlphaFront, DrawType.Lit, DrawType.Beams, DrawType.None })            
                _drawSets[t] = new List<GameObject>();            
            
            //_objectsOnScreen = new HashSet<GameObject>();
            
            FrameCounter = 0;
        }

        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            SoundEngine = new SoundEngine();            
        }

        public void AddGameProcces(GameProcess gameProcces)
        {
            gameProcces.InitProcess(this);
            GameProcceses.Add(gameProcces);
        }

        public void RemoveGameProcces(GameProcess gameProcces)
        {
            GameProcceses.Remove(gameProcces);
        }
        //TODO: add GetGameProccesByType

        public void AddGameObject(GameObject gameObject)
        {
            //gameObject.Init(); //maybe add
            if ((gameObject.ListType != CollisionType.Effects && (gameObject.Flags & GameObjectFlags.AddOnlyOnScreen) == 0) || Camera.IsOnScreen(gameObject.Position, gameObject.Size))
            {
                gameObject.IsNotActive = false;
                switch (gameObject.ListType)
                {
                    case CollisionType.CollideAll:                       
                        _collideAllParticles.Add(gameObject);
                        _drawSets[gameObject.DrawType].Add(gameObject);
                        break;
                    case CollisionType.UpdateOnlyOnScreen:                      
                         _collideAllParticles.Add(gameObject);
                        _drawSets[gameObject.DrawType].Add(gameObject);
                        break;
                    case CollisionType.Collide1:                      
                        _collideParticles1.Add(gameObject);
                        _drawSets[gameObject.DrawType].Add(gameObject);
                        break;
                    case CollisionType.Effects:
                        if (Camera.IsOnScreen(gameObject.Position, gameObject.Size) && _fxParticles.Count < MaxFxNumber)
                        {
                            _fxParticles.Add(gameObject);
                            _drawSets[gameObject.DrawType].Add(gameObject);
                        }
                        
                        break;
                    default:
                        break;
                }

               
            }
        }

        public void AddGameObject1(GameObject gameObject)
        {
            //gameObject.Init(); //maybe add
           
            var draw = false;
            if (gameObject.ListType != CollisionType.Effects) 
            {
                gameObject.IsNotActive = false; //??
                //gameObject.IsActive = true; //Added 9.3.14: mainly for item             

              //  if ((gameObject.GetObjectType() & GameObjectType.PotentialTarget) > 0) //Someday: remove
               //     PotentialTargets.Add(gameObject);

                draw = true;
                switch (gameObject.ListType)  //change to add list
                {
                    case CollisionType.CollideAll:
                        _collideAllParticles.Add(gameObject);
                        break;
                    case CollisionType.Collide1:
                        _collideParticles1.Add(gameObject);
                        break;
                    case CollisionType.UpdateOnlyOnScreen:                     
                        _collideAllParticles.Add(gameObject);
                        break;
                    default:
                        throw new Exception("CollisionType list not found");                        
                }                                
            }
            else
            {
                if (Camera != null && Camera.IsOnScreen(gameObject.Position, gameObject.Size) && _fxParticles.Count < MaxFxNumber) // if Near Screen and count< limit
                {
                    //gameObject.IsActive = true; //Added 9.3.14: mainly for item             
                    _fxParticles.Add(gameObject);
                    draw = true;
                }               
            }

            if (draw && gameObject.DrawType != DrawType.None)
            { 
                _drawSets.Get(gameObject.DrawType).Add(gameObject);                
            }           
        }

        public void RemoveGameObject(GameObject gameObject) {
            if (gameObject == null)
                return;
            gameObject.IsNotActive = true; //???

           // if ((gameObject.GetObjectType() & GameObjectType.PotentialTarget) > 0) //Someday: remove
           //     PotentialTargets.Remove(gameObject);

            switch (gameObject.ListType) {
                case CollisionType.CollideAll:
                    _collideAllParticles.Remove(gameObject);
                    break;
                case CollisionType.Collide1:
                    _collideParticles1.Remove(gameObject);
                    break;
                case CollisionType.Effects:
                    _fxParticles.Remove(gameObject);
                    break;
                case CollisionType.UpdateOnlyOnScreen:
                    _collideAllParticles.Remove(gameObject);
                    break;
                default:
                    break;
            }

            _drawSets.Get(gameObject.DrawType)?.Remove(gameObject);            
        }

        

        public void Update(InputState inputState)
        {
            
            SoundEngine.Update(VolumeSettings.EffectsVolume);
            
            ScreenOverlayColor = Color.Transparent;//???        

            FrameCounter++;

            RemoveGameObjects();
            LightsPerFrame.Clear();
            CollisionManager.Update(_collideAllCheckList);
            CollisionManager.CheckAndApplyWithList(_collideParticles1);            

            _collideAllCheckList.Clear(); //TODO: consider moving after updates

            //add particles  //why is it here ?? // if it is before you draw after the first update
            foreach (GameObject obj in AddList) {
                AddGameObject(obj);
            }
            AddList.Clear();

            // Update
          //  var debugObjectsUpdated = new HashSet<object>();
            foreach (GameObject obj in _collideAllParticles) {

                //Debug.Assert(!debugObjectsUpdated.Contains(obj), $"Multiply-updated object found {obj.GetId()}");
                //if (debugObjectsUpdated.Contains(obj))
                //    throw new Exception($"Multiply-updated object found {obj.GetId()}");

                bool isObjOnScreen = (Camera != null && (obj.ListType == CollisionType.CollideAll || Camera.IsOnScreen(obj.Position, obj.Size)));
                //obj.Flags = isObjOnScreen ? obj.Flags : (obj.Flags | GameObjectFlags.IsOffScreen);
                if (isObjOnScreen) {                    
                    //debugObjectsUpdated.Add(obj);
                    obj.Update(this);
                }

                if (obj.IsNotActive) {                    
                    RemoveList.Add(obj);
                } else {                   
                    if (isObjOnScreen)
                        _collideAllCheckList.Add(obj);
                }           
            }

            foreach (GameObject obj in _collideParticles1) {
             //   Debug.Assert(!debugObjectsUpdated.Contains(obj), $"Multiply-updated object found {obj}");
             //   debugObjectsUpdated.Add(obj);
                obj.Update(this);
                if (obj.IsNotActive)
                    RemoveList.Add(obj);              
            }

            foreach (GameObject obj in _fxParticles) {
             //   Debug.Assert(!debugObjectsUpdated.Contains(obj), $"Multiply-updated object found {obj}");
             //   debugObjectsUpdated.Add(obj);
                obj.Update(this);
                if (obj.IsNotActive)
                    RemoveList.Add(obj);                
            }

            //if(FrameCounter == 1)
            //{
            //    foreach (var procceses in GameProcceses)
            //    {
            //        procceses.InitProcess(this);
            //    }
            //}

            //We are delta dependent so let's assume 60 fps
            PARTICLE_MANAGER.Update(1f / 60f);

            //Game Procces
            for (int i = GameProcceses.Count - 1; i >= 0; i--)
            {
                GameProcceses[i].Update(this);                
                if (GameProcceses[i].Finished)
                {
                    GameProcceses.RemoveAt(i);
                }
            }
            
            foreach (var item in Factions) {
                item?.Update(this);
            }

            
            /*  if (this.Scene != null)
                Scene.SetText(DebugUtils.StopWatch.ElapsedMilliseconds.ToString());*/

            maxFXCount = Math.Max(maxFXCount, _fxParticles.Count); //remove
            maxNonColideCount = Math.Max(maxNonColideCount, _collideParticles1.Count);
        }        



        private void ComputeLightsForDraw() 
        {           
            if (!GraphicsSettings.UseLighting)
                return;

            Camera.AmbientColor = AmbientColor;

            var screenSize = new Rectangle(0, 0, ActivityManager.ScreenWidth, ActivityManager.ScreenHeight);
            var rangeModifier = -(float)Math.Sqrt(screenSize.Width * screenSize.Width + screenSize.Height * screenSize.Height) / Camera.Zoom; // distance from center of screen to corner, in world units
            var screenCenter = Camera.GetWorldPos(ActivityManager.ScreenCenter);

           
            // Get all pertinent lights and lightable objects     //TODO:        //Not good                         
            var lightsInRange = new List<GameObject>();

            //lightsInRange.AddRange(PermanentLights);
            foreach (var gameObject in PermanentLights)
                if (gameObject.Light != null && gameObject.Light.InRange(gameObject.Position, screenCenter))
                    lightsInRange.Add(gameObject);

            //lightsInRange.AddRange(LightsPerFrame);
            foreach (var gameObject in LightsPerFrame)
                if (gameObject.Light != null && gameObject.Light.InRange(gameObject.Position, screenCenter))
                    lightsInRange.Add(gameObject);

            foreach (var gameObject in _collideAllParticles)
                if (gameObject.Light != null && gameObject.Light.InRange(gameObject.Position, screenCenter))
                    lightsInRange.Add(gameObject);

            foreach (var gameObject in _collideParticles1)
                if (gameObject.Light != null && gameObject.Light.InRange(gameObject.Position, screenCenter))
                    lightsInRange.Add(gameObject);

            foreach (var gameObject in _fxParticles)
                if (gameObject.Light != null && gameObject.Light.InRange(gameObject.Position, screenCenter))
                    lightsInRange.Add(gameObject);

         //   ActivityManager.Inst.AddToast(" Lights Num: " + lightsInRange.Count.ToString(), 10);
            // ^ O(numLights), so shouldn't be an issue at all, but we could optimize this by having lights provide effect bounding boxes,
            // and tracking those in our collision manager. 'course we presently rebuild the collision grid every step, so this'd actually imperceptibly
            // hurt performance                      

            // TODO: this chunk (everything until we pas the actual lights) belongs in Camera.PrepareForLitDraw(), but moving it affects the draw outcome (it shouldn't). Investigate
            Action<EffectParameterCollection> passArgs = (paramCollection) => 
            {
                paramCollection["AmbientColor"].SetValue(AmbientColor * 0.5f);
                paramCollection["Zoom"].SetValue(Camera.Zoom);
                paramCollection["HalfResolution"].SetValue(ActivityManager.ScreenCenter);
                //paramCollection["ProjectionMatrix"].SetValue(Matrix.CreateOrthographicOffCenter(0, ActivityManager.ScreenWidth, ActivityManager.ScreenHeight, 0, 0, 1));
                //paramCollection["TransformationMatrix"].SetValue(ActivityManager.ScreenCenter);
            }; 
            
            passArgs(Camera.NormalMapEffect.Parameters);           
            Camera.NormalMapEffect.Parameters["Tilt"].SetValue(LightingGlobals.Tilt);            
            Camera.IncomingLightsObjects = lightsInRange.GetRange(0, Math.Min( LightingGlobals.MaxLightsPerObject, lightsInRange.Count));            
        }

        public void Draw(SpriteBatch sb)
        {
            
            Camera.UpdateMatrix();
            ComputeLightsForDraw();
            Camera.PrepareForLitDraw();

       

            //To Do: Camera
            //Projection matrix needs to be changed only on resolution change and at start of the game, replace 1920 and 1200 with screen width and height respectively
            //I suggest to move it for example to camera, or leave it in this class, just dont do it per cycle in draw code
            Matrix projection;
            Matrix.CreateOrthographicOffCenter(0, sb.GraphicsDevice.Viewport.Width, sb.GraphicsDevice.Viewport.Height, 0, 0, -1, out projection);

       

            //This needs to be done only once per rendering cycle so let's do it here at start
            //Here we do what monogame does in spritebatch with transformation matrix, for details you can see Setup(...) in Spritebatch code, we are just multiplying view matrix with
            //projection, keep it here, MatrixTransformation parameter can be cached


            Camera.NormalMapEffect.Parameters["MatrixTransform"].SetValue(Camera.Transform * projection);
            Camera.NormalMapEffect.Parameters["Viewport"].SetValue(new Vector2(ActivityManager.GraphicsDevice.Viewport.Width, ActivityManager.GraphicsDevice.Viewport.Height));

            DrawPlanets(_drawSets[DrawType.Planets]);

            // Lit with normals   
            if (GraphicsSettings.UseLighting)
            {
                sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, Camera.NormalMapEffect, transformMatrix: Camera.Transform);
                DrawLitObjects(_drawSets[DrawType.Lit]);
                sb.End();
            }
            else
            {
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, transformMatrix: Camera.Transform);
                foreach (GameObject obj in _drawSets[DrawType.Lit])
                {
                    if (Camera.IsOnScreen(obj.Position, obj.Size))
                        obj.Draw(Camera); //dont draw if out of screen
                }
                sb.End();
            }

            

            // Alpha
            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, transformMatrix: Camera.Transform);
            foreach (GameObject obj in _drawSets[DrawType.Alpha])
            {
                if (Camera.IsOnScreen(obj.Position, obj.Size))
                    obj.Draw(Camera); //dont draw if out of screen
            }
            sb.End();

            // Beam
            Camera.NormalMapEffect.CurrentTechnique = Camera.NormalMapEffect.Techniques["BeamEffect"];
            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearWrap, null, null, Camera.NormalMapEffect, transformMatrix: Camera.Transform);
            foreach (GameObject obj in _drawSets[DrawType.Beams] )
            {
                //if (true ||Camera.IsOnScreen(obj.Position, obj.Size)) //temp
                   obj.Draw(Camera); //dont draw if out of screen              
            }            
            sb.End();
           
            // Additive
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, transformMatrix: Camera.Transform);
            foreach (GameObject obj in _drawSets[DrawType.Additive])
            {
                if (Camera.IsOnScreen(obj.Position, obj.Size))
                   obj.Draw(Camera); //dont draw if out of screen              
            }            
            sb.End();
            
            // AlphaFront
            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, transformMatrix: Camera.Transform);
            foreach (GameObject obj in _drawSets[DrawType.AlphaFront])
            {
                if (Camera.IsOnScreen(obj.Position, obj.Size))
                    obj.Draw(Camera); //dont draw if out of screen
            }
            sb.End();

            //We should render GPU particles on top, their blending mode is resolved internally
            PARTICLE_MANAGER.Draw(Camera.Transform, projection);
            //DrawLitObjects(_drawSets[DrawType.AlphaFront]);            
            // We send to postprocessing here and flush to screen
            if (GraphicsSettings.IsPostprocessing)
                PostProcessing.Apply(GraphicsSettingsUtils.renderTargetFullA);

            if (DebugUtils.Mode == ModeType.Test)
            {
                sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                Camera.SpriteBatch.DrawString(Game1.font, "FX:" + _fxParticles.Count + " Max:" + maxFXCount, new Vector2(20, 300), Color.White);
                Camera.SpriteBatch.DrawString(Game1.font, "Collide:" + _collideAllParticles.Count.ToString(), new Vector2(20, 350), Color.White);
                Camera.SpriteBatch.DrawString(Game1.font, "Non Collide:" + _collideParticles1.Count + " Max:" + maxNonColideCount, new Vector2(20, 400), Color.White);
                Camera.SpriteBatch.DrawString(Game1.font, (GetFaction(FactionType.Player).GetHashCode() == MetaWorld.Inst.GetFaction(FactionType.Player).GetHashCode()).ToString(), new Vector2(20, 450), Color.Red);
                int i = 0;
                foreach (var item in Factions)
                {
                    if (item != null)
                    {
                        string text = item.FactionType + " : " + (int)(item.GetRelationToFaction(FactionType.Player)*100)+" "+ Faction.RelationsString(item.GetRelationToFaction(FactionType.Player));
                        Camera.SpriteBatch.DrawString(Game1.font, text, new Vector2(20, 550 + i * 50), Color.White);
                        i++;
                    }
                }                
                sb.End();
            }

            
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
           // Text = Camera.Zoom.ToString();
            if (Text != null && Text != "")
            {
                Vector2 textLength = Game1.font.MeasureString(Text);
                Vector2 textPosition = new Vector2((ActivityManager.ScreenWidth - textLength.X) / 2, 10);
                sb.DrawString(Game1.font, Text, textPosition, Color.White);
            }
            sb.End();


            // Text = MetaWorld.Inst._tempName;

        }

        private void DrawPlanets(List<GameObject> objects) //TODO: change name
        {

            Camera.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, Camera.NormalMapEffect, Camera.Transform);

            //Camera.NormalMapEffect.CurrentTechnique = Camera.NormalMapEffect.Techniques["PlanetRandomSurfaceBarren"];
            
            foreach (var obj in objects)
            {
                Camera.NormalMapEffect.CurrentTechnique = obj.GetEffectTechnique();
                if (Camera.IsOnScreen(obj.Position, obj.Size))
                    obj.Draw(Camera);
            }                        
            Camera.SpriteBatch.End();
        }

        void DrawLitObjects(List<GameObject> objects)
        {
            if (GraphicsSettings.UseLighting)
            {
                Camera.DrawLit = true;
                Camera.NormalMapEffect.CurrentTechnique = Camera.NormalTechniqueWithLights;
                foreach (var obj in objects)
                    if (Camera.IsOnScreen(obj.Position, obj.Size))
                        obj.Draw(Camera);
                Camera.DrawLit = false;
            }
            else

            {
                Camera.DrawLit = false;
                Camera.NormalMapEffect.CurrentTechnique = Camera.NormalTechniqueBasicSprite;
                foreach (var obj in objects)
                    if (Camera.IsOnScreen(obj.Position, obj.Size))
                    {
                        obj.Draw(Camera);
                    }
            }

        }

        public Faction GetFaction(FactionType factionType)
        {
            if (Factions[(int)factionType] == null)
            {
                Factions[(int)factionType] = new Faction(factionType);
            }
            return Factions[(int)factionType];
        }

        private void RemoveGameObjects() {
            foreach (GameObject obj in RemoveList) {
                RemoveGameObject(obj);
            }
            RemoveList.Clear();
        }
       
        /// <summary>
        /// Checks if there is only 1 surviving faction
        /// </summary>
        /// <returns>Factions.None if number of surviving factions != 1, otherwise returns faction name</returns>
        public FactionType GetSoleFaction() {
            FactionType foundFaction = FactionType.None;
            if (_collideAllCheckList.Count == 0)
                return FactionType.Neutral;
            foreach (GameObject go in _collideAllCheckList) {
                if (go.GetFactionType() != FactionType.Neutral) {
                    if (go.GetFactionType() == foundFaction)
                        continue;
                    if (foundFaction == FactionType.None) {
                        foundFaction = go.GetFactionType();
                    } else {
                        return FactionType.None;
                    }
                }
            }
            return foundFaction;
        }

        public HashSet<FactionType> GetSurvivingFactions()
        {
            _survivingFactions.Clear();
            foreach (var gameObject in _collideAllCheckList)
            {
                if (gameObject.GetFactionType() != 0)
                    _survivingFactions.Add(gameObject.GetFactionType());
            }
            return _survivingFactions;
        }

        public Color GetFactionColor(FactionType faction) {
            Color color = Color.Red;
            if (Factions[(int)faction] != null) {
                color = Factions[(int)faction].Color;
            }
            return color;
        }

        public void Clear()
        {
          //  PotentialTargets.Clear();
            _fxParticles.Clear();
            _collideAllParticles.Clear();
            _collideParticles1.Clear();            
            _collideAllCheckList.Clear();
            _drawSets.Values.Do(s => s.Clear());
            FrameCounter = 0;
        }


        public GameObject AddGameObject(string id, FactionType faction, Vector2 position, float rotation = 0, AgentControlType controlType = AgentControlType.AI, float param = 0) //add AI index?
        {
            rotation = MathHelper.ToRadians(rotation);

            if (ContentBank.Inst.ContainsEmitter(id))
            {
                IEmitter emitter = ContentBank.Inst.GetEmitter(id);                
                GameObject gameObject = emitter.Emit(this, null, faction, position, Vector2.Zero, rotation, param: param);
                if (gameObject != null)
                {
                    gameObject.SetControlType(controlType);
                }
                return gameObject;
            }

            throw new Exception($"Emitter named: {id} was not found!");
        }


        public GameObject FindGameObjectInPosition(Vector2 position, float radius, GameObjectType type = GameObjectType.All)
        {
            List<GameObject> objects = new List<GameObject>();
            CollisionManager.GetAllObjectPossiblyInRange(position, radius, objects);
            foreach (var gameObject in objects)
            {
                if ((gameObject.GetObjectType() & type) > 0 && GameObject.DistanceFromEdge(position, gameObject.Position, 0, gameObject.Size) <= radius)
                {
                    return gameObject;
                }
            }            
            foreach (var gameObject in _collideParticles1)
            {
                if ((gameObject.GetObjectType() & type) > 0 && GameObject.DistanceFromEdge(position, gameObject.Position, 0, gameObject.Size) <= radius)
                {
                    return gameObject;
                }
            }
            return null;
        }

    }
}
