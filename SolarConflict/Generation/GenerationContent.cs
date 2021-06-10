using SolarConflict.GameContent.ContentGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Generation
{
    /// <summary>
    /// Holds data needed for generation
    /// </summary>
    class GenerationContent
    {
        #region Singelton
        private static GenerationContent _instance;
        public static GenerationContent Inst
        {
            get
            {
                if (_instance == null)
                    _instance = new GenerationContent();
                return _instance;
            }
        }
        #endregion

        //private Dictionary<string, List<string>> _listBank;


        public void MakeAgentGenerators()
        {
            var loadouts = ContentBank.Inst.GetAllLoadout();
            foreach (var loadout in loadouts)
            {
                var agentGenerator = AgentGeneratorFactory.MakeAgentGenerator(loadout, null);
                ContentBank.Inst.AddContent(agentGenerator);
            }
        }

        private List<string> GetItemList(string itemID, SizeType size)
        {
            var item = ContentBank.Inst.GetItem(itemID, false);
            return ContentBank.Inst.GetAllItemsCopied(item.Profile.SlotType, size).Select(x => x.ID).ToList();
        }
    }
}
