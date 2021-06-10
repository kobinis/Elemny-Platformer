using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using SolarConflict.XnaUtils.Files;
using SolarConflict.XnaUtils;
using XnaUtils;
using SolarConflict.Framework;
using System.Diagnostics;
using SolarConflict.Framework.Emitters;
using SolarConflict.Framework.Utils;
using SolarConflict.Framework.Agents.Systems.Misc;
using Newtonsoft.Json;
using SolarConflict.Generation.TemplateGenerationEngine;

namespace SolarConflict
{
    /// <summary>
    /// Stores a dictionary with all content (factories and emitters)
    /// </summary>
    class ContentBank
    {
        private static ContentBank bankInstance = null;

        public CharacterBank CharacterBank { get; private set; }

        private Dictionary<string, IGameObjectFactory> gameObjectFactoryBank;
        private Dictionary<string, object> generalBank;
        private Dictionary<string, IEmitter> emitterBank;
        private Dictionary<string, Item> itemBank; //Remove
        private Dictionary<string, AgentLoadout> loadoutBank; //Remove
        private Dictionary<string, Agent> agentBank; //Remove
        private Dictionary<string, Recipe> recipesBank; //Remove      
        

        public static ContentBank Inst
        {
            get
            {
                if (bankInstance == null)
                {
                    bankInstance = new ContentBank();
                }
                return bankInstance;
            }
        }    
        
        public static IEmitter Get(string ID)
        {
            return Inst.GetEmitter(ID);
        }

        //public List<IEmitter> GetAlemit


        private ContentBank()
        {
            gameObjectFactoryBank = new Dictionary<string, IGameObjectFactory>();
            emitterBank = new Dictionary<string, IEmitter>();
            generalBank = new Dictionary<string, object>();

            itemBank = new Dictionary<string, Item>();
            loadoutBank = new Dictionary<string, AgentLoadout>();
            agentBank = new Dictionary<string, Agent>();
            recipesBank = new Dictionary<string, Recipe>();

            CharacterBank = new CharacterBank();
        }

        

        public IGameObjectFactory GetGameObjectFactory(string id)
        {
            if (gameObjectFactoryBank.ContainsKey(id))
            {
                return gameObjectFactoryBank[id];
            }
            else
            {
                throw new Exception("No IGameObjectFactory named: " + id + " was found!");                
            }
        }

        public void AddContent(object content, bool replace = false)
        {            
            if (content is IEmitter)
            {
                AddEmitter((IEmitter)content, replace);
            }
            if (content is IGameObjectFactory)
            {                
                AddGameObjectFactory((IGameObjectFactory)content);
            }
            if (content is Item)
            {
                AddItem((Item)content);
            }
            if (content is AgentLoadout)
            {                
                AgentLoadout loadout = (AgentLoadout)content;
                loadoutBank.Add(loadout.ID, loadout);
            }
            if (content is Agent)
            {             
                Agent agent = (Agent)content;
                agentBank[agent.ID] = agent;
            }
            if (content is Recipe)
            {             
                Recipe recipe = (Recipe)content;
                recipesBank.Add(recipe.ID, recipe);
            }            
        }


        public Item GetItem(string id, bool returnCopy) 
        {
            if(id[0] == '#')
            {
                var splitID = id.Split(':');
                Item toBeImbued = GetItem(splitID[1], true);
                Item imbuingItem = GetItem(splitID[2],false);

                ImbueSystem imbueSys = imbuingItem.System as ImbueSystem;
                imbueSys.ImbueItem(toBeImbued, imbuingItem.ID);
                return toBeImbued;
            }

            if (itemBank.ContainsKey(id))
            {
                if (returnCopy)
                    return itemBank[id].GetWorkingCopy();
                return itemBank[id];
            }
            else
            {
                throw new Exception("No Item named: " + id + " was found!");   //:TODO handle it        
            }
        }

        private void AddItem(Item item)
        {
            Debug.Assert(!itemBank.ContainsKey(item.ID), $"Item named {item.ID} already exists");
            itemBank[item.ID] = item;
        }

        public AgentLoadout GetLoadout(string id)
        {
            var result = TryGetLoadout(id);
            if(result == null)
                throw   new Exception("No AgentLoadout named: " + id + " was found!"); //change it
            return result;
        }


        public AgentLoadout TryGetLoadout(string id) {
            return loadoutBank.Get(id);            
        }

        

        public List<AgentLoadout> GetAllLoadout() 
        {
            return loadoutBank.Values.ToList(); //maybe to Array?
        }

        public List<Agent> GetAllAgents()
        {
            return agentBank.Values.ToList(); //TODO: change
        }

        public List<Item> GetAllItemsCopied(SlotType slotType = SlotType.All, SizeType maxSize = SizeType.Gigantic)
        {
            List<Item> itemList = new List<Item>();
            List<Item> allItems = itemBank.Values.ToList(); 

            foreach (var item in allItems)
            {
                if ( (((item.Profile.SlotType & slotType) >= slotType)  || slotType == SlotType.All)
                    && item.Profile.ItemSize <= maxSize)
                {
                    itemList.Add(item.GetWorkingCopy());
                }
            }

            return itemList;
        }

        public List<Item> GetAllItems()
        {
            return itemBank.Values.ToList();
        }

        public bool HasEmitter(string id) {
            return emitterBank.ContainsKey(id);
        }

        public void ClearContent()
        {
            gameObjectFactoryBank.Clear();
            emitterBank.Clear();
            itemBank.Clear();
            loadoutBank.Clear();
            agentBank.Clear();
            recipesBank.Clear();
        }

        /// <summary>
        /// Add/update loadout to the content bank
        /// </summary>
        /// <remarks>TODO: make private</remarks>
        /// <param name="loadout">loadout to add/ update.</param>
        /// <returns>True if loadout was added.</returns>
        public bool AddLoadout(AgentLoadout loadout)
        {
            bool result = false;
            if (emitterBank.ContainsKey(loadout.ID))
            {
                if (GetEmitter(loadout.ID) is AgentLoadout)
                {
                    //TODO: refactor the shit out of this: doesn't make sense to store three copies of the loadout
                    emitterBank[loadout.ID] = loadout;
                    gameObjectFactoryBank[loadout.ID] = loadout;
                    loadoutBank[loadout.ID] = loadout;
                    result = true;
                }
            }
            else
            {
                AddContent(loadout);
                result = true;
            }
            return result;
        } 

        void AddEmitter(IEmitter emitter, bool replace = false)
        {
            Debug.Assert(replace || !emitterBank.ContainsKey(emitter.ID), $"Emitter named {emitter.ID} already exists");
            
            emitterBank[emitter.ID] = emitter;            
        }

        public IEmitter TryGetEmitter(string id)
        {
            if (id == null)
                return null;
            IEmitter res;
            emitterBank.TryGetValue(id, out res);
            return res;            
        }



        int emitterHolderCounter;
        public IEmitter GetEmitter(string id)
        {
            
            if (string.IsNullOrEmpty(id))
                return null;

            if(id.StartsWith("#"))
            {
                string[] parameters = ContentParsingUtil.GetStringBetween(id).Split(',');
                //TODO: change to parse commands
                return new AmountEmitter(parameters[0], ParserUtils.ParseInt(parameters[1]));
            }

            if (emitterBank.ContainsKey(id))
            {
                emitterHolderCounter = 0;
                return emitterBank[id];
            }
            else
            {
                emitterHolderCounter++;
                if (emitterHolderCounter > 50)
                {
#if DEBUG
                    throw new Exception("Emitter: " + id + " was not found!");
#else
                    //TODO: write to log
                    return new EmptyEmitter();
#endif
                }
                return new EmitterIdHolder(id);
            }
        }

        public Recipe GetRecipe(string id) 
        {
            return GetEmitter(id) as Recipe;
        }

        public bool ContainsFactory(string id)
        {            
            return gameObjectFactoryBank.ContainsKey(id);
        }
        

        public bool ContainsEmitter(string id)
        {
            return emitterBank.ContainsKey(id);
        }


        public List<Recipe> GetAllRecipes()
        {
            return recipesBank.Values.ToList<Recipe>(); //change
        }

        public List<Recipe> GetAllRecipes(CraftingStationType craftingStation)
        {
            List<Recipe> list = recipesBank.Values.ToList<Recipe>();
            //(t.CraftingStation & CraftingStationType.All) == 0 &
            return list.Where(t => ( t.CraftingStation & craftingStation) > 0).ToList();
        }

        public List<Recipe> GetCraftableRecipes(Inventory inventory)
        {
            List<Recipe> craftableRecipes = new List<Recipe>();
            foreach (var recipe in recipesBank)
            {
                if (recipe.Value.IsCraftable(inventory))
                {
                    craftableRecipes.Add(recipe.Value);
                }
            }
            return craftableRecipes;
        }

        public List<IEmitter> GetAllEmittersOfType(Type type)
        {
            List<IEmitter> emitters = new List<IEmitter>();

            foreach (var item in emitterBank)
            {
                if(type == null || item.Value.GetType() == type)
                {
                    emitters.Add(item.Value);
                }
            }
            return emitters;
        }


        public void LoadReflectionContent(string nameSpace)
        {            
            LoadEmittersClases(nameSpace + ".Projectiles");
            LoadEmittersClases(nameSpace + ".Emitters");
            LoadEmittersClases(nameSpace + ".Ships");
            LoadEmittersClases(nameSpace + ".Agents");
            LoadEmittersClases(nameSpace + ".Items");
            LoadEmittersClases(nameSpace + ".NewItems");
            LoadEmittersClases(nameSpace + ".Misc");
        }
        
        public void LoadEmittersClases(string nameSpace)
        {
            Type[] types = ReflectionUtils.GetTypesUnderNamespace(Assembly.GetExecutingAssembly(), nameSpace);
            for (int i = 0; i < types.Length; i++) {
                string name = types[i].Name;                
                IEmitter emitter = (IEmitter)types[i].GetMethod("Make").Invoke(null, null);
                if (emitter != null)
                {
                    emitter.ID = name;
                    ContentBank.Inst.AddContent(emitter);
                }
            }
        }

        public void LoadLoadouts(string path)
        {
            path = Consts.GetLoadoutPath(path);
            //if (path == null)
            //    path = Consts.AGENTS_SAVE_PATH;                                    
            string[] files = FileUtils.GetFiles(path, "*.json", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                string ID = Path.GetFileNameWithoutExtension(file);
                try
                {

                    var text = File.ReadAllText(file);
                    var agentLoadout = JsonConvert.DeserializeObject<AgentLoadout>(text);
                    agentLoadout.ID = ID;
                    this.AddContent(agentLoadout);

                    //var agentLoadout = (AgentLoadout)SaveLoadManager.Instance().Load(file); // BUG: here's where we misplace the mining lasers                
                    //agentLoadout.ID = ID;                                        
                    //this.AddContent(agentLoadout);
                }
                catch (Exception e)
                {
                    throw new Exception("Cant load: " + file + " Exception: " + e.Message);                    
                }
            }
        }

        private void AddGameObjectFactory(IGameObjectFactory factory)
        {
            gameObjectFactoryBank[factory.ID] = factory;
        }


        public Dictionary<string, List<Recipe>> GetAllRecipeMatrials()
        {
            var recipeMatrials = new Dictionary<string, List<Recipe>>();
            foreach (var recipe in recipesBank.Values)
            {
                foreach (var cost in recipe.CraftingCostList)
                {
                    if(recipeMatrials.ContainsKey(cost.ItemNeeded.Id))
                    {
                        recipeMatrials[cost.ItemNeeded.Id].Add(recipe);
                    }
                    else
                    {
                        var recipeList = new List<Recipe>() { recipe };
                        recipeMatrials[cost.ItemNeeded.Id] = recipeList;
                    }                        
                }
            }
            return recipeMatrials;
        }
        
        public void MarkItemsAsMaterial(IEnumerable<string> IDList)
        {
            foreach (var id in IDList)
            {
                itemBank[id].Profile.Category |= ItemCategory.CraftingMaterial;
            }
        }


        public bool TestCrafting() {
            var scene = new Scene();
            var gameEngine = scene.GameEngine;

            GetAllRecipes().Do(r => {
                // Create inventory, add materials thereto
                var inventory = new Inventory(10);
                Debug.Assert(r.AmountRecived <= r.CraftedItem.Profile.MaxStack, "Crafted amount exceeds stack limit"); // we assume it doesn't elsewhere,
                                                                                                                       // rather sloppily
                r.CraftingCostList.Do(c => Utility.Range(0, c.Amount).Do(_ => inventory.AddItem(c.ItemNeeded.Id)));

                // Craft
                r.Craft(gameEngine, inventory);
            });
            gameEngine.Update(InputState.EmptyState);

            return true;
        }

        public bool TestEmitters()
        {
            Scene scene = new Scene();
            scene.TryInit(null);

            GameEngine gameEngine = scene.GameEngine; //new GameEngine(new Camera(ActivityManager.SpriteBatch)); 
            GameObject root = new Agent();
            int i = 0;
            foreach (var emitter  in emitterBank)
            {
                i++;
                emitter.Value.Emit(gameEngine, root, 0, Vector2.One * i * 300, Vector2.Zero, 0);
                string textLine = emitter.Key + "," + emitter.Value.GetType().Name;                
            }
            gameEngine.Update(InputState.EmptyState);            
            return true;
        }

        public List<IEmitter> GetEmittersFromList(string emitterList)
        {
            List<IEmitter> list = new List<IEmitter>();
            var emitterArray = emitterList.Split(',');
            foreach (var emitterId in emitterArray)
            {
                var emitter = TryGetEmitter(emitterId);
                list.Add(emitter);
            }
            return list;
        }


        public void ExportItemsCSV()
        {
            //ID, Name, "Description", Size, Level, Slot, price, FlavourText 
            var items = GetAllItems();
            string hader = "Id,Name*,Description,Flavour*,Comments,StatsText,Size, Level, Slot, price";

            List<string> lines = new List<string>();
            lines.Add(hader);                     
            foreach (var item in items)
            {
                string description = item.Profile.DescriptionText;
                if (description == null)
                    description = string.Empty;
                description = description.Replace("\n", @"\n").Replace("\r", @"\n");
                string statText = item.Profile.StatsText;
                if (statText == null)
                    statText = string.Empty;
                statText = statText.Replace("\n", @"\n").Replace("\r", @"\n");
                string flavour = item.Profile.FlavourText;
                if (flavour == null)
                    flavour = string.Empty;
                flavour = flavour.Replace("\n", @"\n").Replace("\r", @"\n");

                string textLine = $"{item.ID},{item.Name},\u0022{description}\u0022,\u0022{flavour}\u0022,,\u0022{statText}\u0022,{item.Profile.ItemSize},{item.Level},\u0022{item.SlotType}\u0022,{item.Profile.BuyPrice}";
                lines.Add(textLine);
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"all items.csv"))
            {
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
            }
        }

    }
}
