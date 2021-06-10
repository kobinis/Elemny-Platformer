//using SolarConflict.Framework;
//using SolarConflict.XnaUtils;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Utils
//{
//    class ContentRefactoring
//    {
//        public static void ReplaceItem(AgentLoadout loadout, string toFind, string toReplace, bool fromStart)
//        {
//            for (int i = 0; i < loadout.LoadoutEntryList.Count; ++i)
//            {
//                var entry = loadout.LoadoutEntryList[i];

//                if (entry.ItemId == null)
//                    continue;

//                if (fromStart)
//                {
//                    if (entry.ItemId.StartsWith(toFind))
//                    {
//                        entry.ItemId = toReplace + entry.ItemId.Substring(toFind.Length);
//                    }
//                }
//                else
//                    entry.ItemId = entry.ItemId.Replace(toFind, toReplace);

//                loadout.LoadoutEntryList[i] = entry;
//            }

//            for (int i = 0; i < loadout._inventoryItems.Count; ++i)
//            {
//                if (loadout._inventoryItems[i].ItemId?.Contains(toFind) ?? false)
//                    throw new NotImplementedException("Implement me");
//            }
//        }

//        public static void ReplaceItems(string toFind, string toReplace, bool fromStart = false)
//        {
//            var loadoutFiles = Directory.GetFiles(Consts.AGENTS_SAVE_PATH, "*.*", SearchOption.AllDirectories);

//            foreach (var file in loadoutFiles)
//            {
//                var loadout = (AgentLoadout)SaveLoadManager.Instance().Load(file);

//                ReplaceItem(loadout, toFind, toReplace, fromStart);

//                // Trim extension, it'll be reapplied by Save()
//                Debug.Assert(file.EndsWith(".save"));

//                SaveLoadManager.Instance().Save(file.Substring(0, file.Length - 5), loadout);
//            }

//        }
//    }
//}
