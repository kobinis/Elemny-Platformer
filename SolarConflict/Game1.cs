using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SolarConflict.AI;
using SolarConflict.Framework;
using SolarConflict.Framework.Emitters;
using SolarConflict.Framework.GameObjects.Emitters;
using SolarConflict.Framework.GUI.ParserCommands;
using SolarConflict.Framework.Scenes.Components;
using SolarConflict.GameContent;
using SolarConflict.GameContent.Activities;
using SolarConflict.GameContent.Activities.Levels;
using SolarConflict.GameContent.ContentGeneration;
using SolarConflict.GameContent.ContentGeneration.Items;
using SolarConflict.GameContent.ContentGeneration.Projectiles;
using SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine.Templates;
using SolarConflict.GameContent.Items.Generated;
using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using SolarConflict.Generation.TemplateGenerationEngine.Templates;
using SolarConflict.Session;
using SolarConflict.XnaUtils;
using SolarConflict.XnaUtils.Files;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using XnaUtils;
using XnaUtils.Framework.Graphics;
using XnaUtils.Graphics;
using XnaUtils.SimpleGui;

namespace SolarConflict
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch sb;

        public static Texture2D cover1;
        public static Texture2D cover2;

        public static SpriteFont font; //Remove;        
        public static SpriteFont orbitron14Black;
        public static SpriteFont orbitron12;
        public static SpriteFont menuFont; //Remove;        
        
        public static int time;    //Remove    
        private string[] _args;

        public static Texture2D planetSurfaceTexture;
        public static Texture2D[] planetHeightTexture;
        
        public Game1(string[] args)
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";

            _args = args;
            SettingsManager.Inst.Load();
            DebugUtils.Mode = ModeType.Release;
#if DEBUG
            DebugUtils.Mode = ModeType.Debug;
#endif

            if (args.Length == 1)
            {
                if (args[0].StartsWith("-"))
                {
                    args[0] = args[0].Remove(0, 1);
                    DebugUtils.Mode = ParserUtils.ParseEnum<ModeType>(args[0], ModeType.Release);
                    _args = new string[0];
                }
            }

            DebugTemplate debugTemplate = new DebugTemplate();
            debugTemplate.LoadDirectory(SolarConflict.Framework.Consts.TEMPLATES_PATH);

            if (DebugUtils.Mode == ModeType.Debug )
            {
                GraphicsSettings.IsFullscreen = false;
                //GraphicsSettings.IsBorderless = true;
            }

            System.Drawing.Rectangle rect = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            if (GraphicsSettings.ResWidth == 0f || GraphicsSettings.ResHeight == 0f || rect.Width < GraphicsSettings.ResWidth || rect.Height < GraphicsSettings.ResHeight)
            {             
                GraphicsSettings.ResWidth = rect.Width;
                GraphicsSettings.ResHeight = rect.Height;                
            }
            graphics.PreferredBackBufferWidth = GraphicsSettings.ResWidth;
            graphics.PreferredBackBufferHeight = GraphicsSettings.ResHeight;
            
            IsFixedTimeStep = true;
            float targetFPS = 61;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / targetFPS);
            graphics.IsFullScreen = GraphicsSettings.IsFullscreen;
            graphics.HardwareModeSwitch = GraphicsSettings.IsHardwareModeSwitch;

            //IsFixedTimeStep = false;
            //graphics.SynchronizeWithVerticalRetrace = false;
            GraphicsSettingsUtils.InitStatic(graphics);
            if (GraphicsSettings.IsBorderless)
                GraphicsSettingsUtils.Borderless(this);
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
           
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            sb = new SpriteBatch(GraphicsDevice);
            
            
            IsMouseVisible = false;
            font = Content.Load<SpriteFont>("MainFont");// Game.Content.Load<SpriteFont>("Font/SpriteFont1");
            font.DefaultCharacter = '*';
            FontBank.DefaultFont = font;
            orbitron14Black = Content.Load<SpriteFont>("Orbitron14Black");
            orbitron14Black.DefaultCharacter = '*';
            orbitron12 = Content.Load<SpriteFont>("Orbitron12");
            orbitron12.DefaultCharacter = '*';
            menuFont = Content.Load<SpriteFont>("MainFont");
            menuFont.DefaultCharacter = '*';

            TextureBank.Inst.AddTexture("cursor", Content.Load<Texture2D>("Images/Cursor32"));
            //cover1 = Content.Load<Texture2D>("Images/Cover");
            //cover2 = Content.Load<Texture2D>("Images/PromoB");
            //TextureBank.Inst.AddTexture("cover", cover1);
            //TextureBank.Inst.AddTexture("cover2", cover2);
            //TextureBank.Inst.AddTexture("coverA", Content.Load<Texture2D>("Images/Cover3"));
            //TextureBank.Inst.AddTexture("coverA1", Content.Load<Texture2D>("Images/Cover4"));
            TextureBank.Inst.AddTexture("missing", Content.Load<Texture2D>("Images/missing"));
            //   cover1 = TextureBank.Inst.GetTexture("coverA");
           // TextureBank.Inst.AddTexture("guiframe", Content.Load<Texture2D>("Images/grey_panel"));

            TextureBank.Inst.AddSprite(new Sprite9Sliced("guiframe", 10, 10, 10, 10));
            GuiManager.BackTexture = Sprite.Get("guiframe");
            GuiManager.ScrollTexture = Sprite.Get("guiframe");

            // Custom parser commands
            RichTextParser.AddCommand(typeof(ActionCommand));
            RichTextParser.AddCommand(typeof(ItemIconCommand));
            RichTextParser.AddCommand(typeof(ItemNameCommand));
            RichTextParser.AddCommand(typeof(SectorNameCommand));
            RichTextParser.AddCommand(typeof(WriteToBoardCommand));
            RichTextParser.AddCommand(typeof(HighlightCommand));

            Camera.Init(Content.Load<Effect>("Shaders/normalmap"), Content.Load<Effect>("Shaders/background"));

            ShadersEffects.MiniMapEffect = Content.Load<Effect>("Shaders/minimap");
            PostProcessing.processingEffect = Content.Load<Effect>("Shaders/processing");

            //This is loaded here as placehoder atm for testing purposes

            ActivityManager.CursorTexture = Sprite.Get("cursor").Texture;
            //MakeGuiDesign();

            InitMusic();
            //    MusicEngine.Instance.PlaySong(MusicEngine.MENU_SONG);
            ActivityManager.Init(this);
            GraphicsSettingsUtils.UpdateScreenSizeFields();

            ActivityManager.Inst.LoadActivityProviders(Assembly.GetExecutingAssembly(), "SolarConflict.GameContent.Activities");
            ActivityManager.Inst.AddActivityProvider("SaveGame", SaveGameProvider);
            ActivityManager.Inst.AddActivityProvider("PopupMenu",Menu.PopupMenuActivityProvider);

            if (DebugUtils.LoadWithThred)
            {
                Thread initThread = new Thread(new ThreadStart(InitGame));
                initThread.Start();
                ActivityManager.Inst.SwitchActivity(new DisclaimerActivity(initThread), false);
            }
            else
            {
                InitGame();
                if (_args.Length > 0)
                {
                    if (_args.Length == 1)
                        ActivityManager.Inst.SwitchActivity(_args[0], string.Empty);
                    else
                        ActivityManager.Inst.SwitchActivity(_args[0], _args[1]);
                }
                else
                {
                    var menuActivity = ActivityManager.Inst.SwitchActivity("PixelGardenActivity", string.Empty, false);
                    menuActivity.OnEnter(null);
                    //(menuActivity as Menu).IsConfirmQuitNeeded = true;
                    ActivityManager.Inst.DefaultActivity = menuActivity;
                }
            }           
        }


        private void InitGame()
        {
            System.Console.WriteLine("Load content Game1");
            StringBuilder sb = new StringBuilder();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();      
          //  TextureBank.Inst.LoadContent(Content, "content\\Images", false);
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts1 = stopWatch.Elapsed;
            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts1.Hours, ts1.Minutes, ts1.Seconds, ts1.Milliseconds);
            sb.AppendLine("Textures: " + elapsedTime);

            stopWatch.Reset();
            stopWatch.Start();
            AudioBank.Inst.LoadContent(Content, "content\\Sound");
            stopWatch.Stop();
            TimeSpan ts2 = stopWatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",ts2.Hours, ts2.Minutes, ts2.Seconds, ts2.Milliseconds);
            sb.AppendLine("Sounds: " + elapsedTime);
            // ContentBank.Inst.lo
            //AudioBank.Inst.LoadSounds(Path.Combine(Consts.GAME_DATA_PATH, "Sounds")); //TOTO: add most sounds to xna content pipeline                                                    

            stopWatch.Reset();
            stopWatch.Start();
            LoadGameContent();
            PostProcessing.Init();
            stopWatch.Stop();
            TimeSpan ts3 = stopWatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts3.Hours, ts3.Minutes, ts3.Seconds, ts3.Milliseconds);
            sb.AppendLine("Emitters & Misc: " + elapsedTime);
            if (DebugUtils.Mode == ModeType.Debug)
            {
                File.WriteAllText("Loading times.txt", sb.ToString());
                //ContentBank.Inst.ExportItemsCSV();
            }                            
        }

        public void LoadGameContent(bool ignoreTextureDuplication = false)
        {

            List<string> errors = new List<string>();
            string errorType = string.Empty;

            AIBank.Inst.Clear();
            ContentBank.Inst.ClearContent();
            errorType = "Text assets error!";
            TextBank.Inst.Clear();
            TextAssetsTemplate textAssets = new TextAssetsTemplate();
            textAssets.LoadDirectory(SolarConflict.Framework.Consts.TEMPLATES_PATH);
            TextBank.Inst.LinkAllAssets();


            errorType = "Sound emitters error!";
            AddSoundEmitters();
            SoundEmitter se = (SoundEmitter)(ContentBank.Inst.GetEmitter("sound_shotgun"));
            se.Volume = 0.3f;




            TextureBank.Inst.LoadTextures(GraphicsDevice, Path.Combine(Consts.GAME_DATA_PATH, "Images"), true);

            PARTICLE_MANAGER.Initialize(Content, graphics.GraphicsDevice);

            planetSurfaceTexture = TextureBank.Inst.GetTexture("TerrainCombined1"); //Move
            planetHeightTexture = new Texture2D[2];
            planetHeightTexture[0] = TextureBank.Inst.GetTexture("PlanetHeight1");
            planetHeightTexture[1] = TextureBank.Inst.GetTexture("PlanetHeight2");

            GraphicsUtils.Init();

            

            ContentBank.Inst.AddContent(new PlayerCloneEmitter());
            ContentBank.Inst.AddContent(new PlayerEmitter());
            ContentBank.Inst.AddContent(new TeleportAncestor());
            ContentBank.Inst.AddContent(new RemoveGameObjectEmitter());
            ContentBank.Inst.AddContent(new CargoDropEmitter());
            ContentBank.Inst.AddContent(new AgentSlotDropEmitter());
            //Loads content *************************************************************************************************            
            ContentBank.Inst.LoadReflectionContent("SolarConflict.NewContent");//Remove
            ContentBank.Inst.LoadReflectionContent("SolarConflict.GameContent");
            ContentBank.Inst.LoadEmittersClases("SolarConflict.GameContent.ShipHulls");
            LoadAllAgentsFromXML();

            //MiningLaserGeneration.Make();

            //********************************************CSV Content ********************************************************               

            var csvContentReader = new TemplateGenerationManager();
            AddCSVContent(csvContentReader);
            csvContentReader.GenerateContent();
            ContentBank.Inst.LoadEmittersClases("SolarConflict.GameContent.Loadouts");
            AsteroidGeneration.GenerateAsteroidsProfiles();
            ResourceMineGeneration.MakeMines();


            // Items prepped, do loadouts
            HullBlueprintGenerator.Generate();
            ContentBank.Inst.LoadLoadouts("Loadouts");
            ContentBank.Inst.LoadLoadouts("UserLoadouts");
            // Load AgentGenerators late, they depend on a bunch of prior content
            ContentBank.Inst.LoadEmittersClases("SolarConflict.GameContent.AgentGenerators");
            ShipConstructionKitGenerator.MakeAll();
            AIUtils.GenerateAIForLoadouts();
            GenerationContent.Inst.MakeAgentGenerators();
            var items = ContentBank.Inst.GetAllItems().Where(i => i.Level == 0);
            foreach (var item in items)
            {
                item.Profile.BuyPrice = 0;
                item.Profile.SellPrice = 0;
            }


            var itemInfo = new ItemInfoTemplate();
            itemInfo.LoadDirectory(SolarConflict.Framework.Consts.TEMPLATES_PATH);

            //ShipGenerator.MakeAllShipGenerators();
            ShipGenerator.LoadShipGenerators();

            FactionContent.LoadFactionData();
            //GameSession.Inst.GalaxyMap.GenerateAllScenes();  

            //ContentBank.Inst.ExportCSV(typeof(Item));
            if (!ignoreTextureDuplication)
            {
                TextureBank.Inst.SetSprite(new Sprite9Sliced("guif8", 10, 10, 10, 10));
                TextureBank.Inst.SetSprite(new Sprite9Sliced("guif10", 10, 10, 10, 10));
                TextureBank.Inst.AddSprite(new Sprite9Sliced("guiframe2", 10, 10, 10, 10));
                TextureBank.Inst.AddSprite(new Sprite9Sliced("Btn_Menu1_n", 35, 35, 35, 35));
                TextureBank.Inst.AddSprite(new Sprite9Sliced("Small_ui", 8, 8, 8, 8, margine: 0));
                //  TextureBank.Inst.AddSprite(new Sprite9Sliced("guiframe2", 35, 35, 35, 35));

                GuiManager.BackTexture = Sprite.Get("Btn_Menu1_n");
            }
          
            GuiManager.DefalutGuiColor = Palette.GuiColor;


            var recipeMatrials = ContentBank.Inst.GetAllRecipeMatrials().Keys;
            ContentBank.Inst.MarkItemsAsMaterial(recipeMatrials);
            //PlayerMouseAndKeys.LoadOrDefaultBindings(true);
            ContentBank.Inst.GetItem("FireTrailSprint", false).Profile.Category |= ItemCategory.NonAI;
            LoadSceneSpecificContent();
            //ContentBank.Inst.TestCrafting();
            //ContentBank.Inst.TestEmitters();



            try
            {
            }
            catch (Exception e)
            {
                errors.Add(e.ToString());
                throw e;
            }
            finally
            {
                try
                {
                    string filename = "BuggyMcBugBug" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".txt";
                    //string fullpath = Path.Combine(Consts.GAME_DATA_PATH, filename);
                    if (errors.Count > 0)
                        File.WriteAllLines(filename, errors);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            //ConvertLoadouts();
            if (DebugUtils.ShowFPS)
                this.Components.Add(new FrameRateCounter(this));

           
        }
        
        //todo: remove
        void LoadSceneSpecificContent()
        {
            Sol.AddSceneSpecificContent();
        }

        private void InitMusic()
        {
            //ours            
            //  MusicEngine.Instance.Songs.Add(Game.Content.Load<Song>("Music/5"));
            MusicEngine.Instance.Songs.Add(Content.Load<Song>("Music/Chill Carrier - Back In The Days/01 - Chemical Reactions"));
            MusicEngine.Instance.Songs.Add(Content.Load<Song>("Music/Chill Carrier - Back In The Days/02 - Changing Times"));
            MusicEngine.Instance.Songs.Add(Content.Load<Song>("Music/Chill Carrier - Back In The Days/03 - Waves Of Tension (Pt. I)"));
            MusicEngine.Instance.Songs.Add(Content.Load<Song>("Music/Chill Carrier - Back In The Days/04 - Into The Dark (Beeing Quiet Version)"));
            MusicEngine.Instance.Songs.Add(Content.Load<Song>("Music/Chill Carrier - Back In The Days/06 - Back In The Days"));
            // less good music ours
            //  MusicEngine.Instance.Songs.Add(Game.Content.Load<Song>("Music/2"));            
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            if (Steam.IsSteamRunning())
            {
                SteamAPI.RunCallbacks();
                SteamAchievements.Update();
            }

#if DEBUG
            if (ActivityManager.Inst.InputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.F11))
            {
                ReLoadData();
            }
#endif
            if (ActivityManager.Inst.InputState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftAlt) && ActivityManager.Inst.InputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                //_game.GR.IsFullScreen = !this.graphics.IsFullScreen;
                //graphics.ApplyChanges();                
            }
            if (ActivityManager.Inst.Update(gameTime, this))
                Exit();
            MusicEngine.Instance.Update(gameTime, VolumeSettings.MusicVolume);
            time++;

            base.Update(gameTime);

            //ActivityManager.Inst.AddToast(GameSession.Inst.GetHashCode().ToString(), 60);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
             GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            ActivityManager.Inst.Draw(sb);
            base.Draw(gameTime);
        }

        public void ReLoadData()
        {
            DebugUtils.IsReload = true;
            //LoadGameContent(true);
            try
            {
                //TextAssetsTemplate textAssets = new TextAssetsTemplate();
                //textAssets.LoadDirectory(SolarConflict.Framework.Consts.TEMPLATES_PATH);

                LoadGameContent(true);
                
                ActivityManager.Inst.AddToast(Color.Green.ToTag("Data was loaded"), 100, null ,false);

            }
            catch (Exception e)
            {
                ActivityManager.Inst.AddToast(Color.Red.ToTag(e.ToString()), 300);
                //throw;
            }
        }

        public static void AddSoundEmitters()
        {
            ContentBank cb = ContentBank.Inst;
            foreach (var item in AudioBank.Inst.GetSoundDictionary())
            {
                SoundEmitter se = new SoundEmitter(item.Key, 1f);
                se.ID = "sound_" + item.Key;
                cb.AddContent(se);
            }
        }

        private void AddCSVContent(TemplateGenerationManager csvContentReader)
        {
            csvContentReader.Add(new CharacterTemplate());
            csvContentReader.Add(new PaletteTemplate());
            //generationTemplates.Add(new ShipTemplate());
            csvContentReader.Add(new ShotTemplate());
            //generationTemplates.Add(new ShopTemplate());
            //generationTemplates.Add(new ShopInventoryTemplate());
            csvContentReader.Add(new RawMaterialsTemplate());
            csvContentReader.Add(new GeneratorTemplate());
            csvContentReader.Add(new ShieldTemplate());
            csvContentReader.Add(new BatteryTemplate());
            csvContentReader.Add(new EngineTemplate());
            csvContentReader.Add(new RotationEngineTemplate());
            //generationTemplates.Add(new AsteroidTemplate());
            // generationTemplates.Add(new StoryItemsTemplate());
            //  generationTemplates.Add(new AmmoTemplate());
            csvContentReader.Add(new WeaponTemplate());
            csvContentReader.Add(new KitsTemplate());
            //    generationTemplates.Add(new WarHeadTemplate());
            csvContentReader.Add(new ParamEmitterTemplate());
            csvContentReader.Add(new LootEmitterTemplate());
            csvContentReader.Add(new RecipeTemplate()); //needs to be last 
                                                        // csvContentReader.Add(new ReadPathData());
        }

        public void LoadAllAgentsFromXML()
        {
            string path = Path.Combine(Consts.GAME_DATA_PATH, "Agents");
            if (Directory.Exists(path))
            {
                var allfiles = FileUtils.GetFiles(path, "*.xml");
                foreach (var item in allfiles)
                {
                    ContentBank.Inst.AddContent(ShipQuickStart.LoadFromXML(item));
                }
            }
        }


        public static Dictionary<string, T> LoadListContent<T>(ContentManager contentManager, string contentFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "/" + contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
            Dictionary<String, T> result = new Dictionary<String, T>();

            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);


                result[key] = contentManager.Load<T>(contentFolder + "/" + key);
            }
            return result;
        }

        

        public static Activity SaveGameProvider(string parameters)
        {
            PersistenceManager.Inst.Save();
            return null;
        }

        
    }
}
