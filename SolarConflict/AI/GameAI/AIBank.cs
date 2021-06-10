using SolarConflict.AI.GameAI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using XnaUtils;

namespace SolarConflict.AI
{
    public class AIBank
    {
        //const int SMARTAI = 0; //Remove
        //const int ROUINDAI = 1;
        //const int MINERAI = 2;

        #region Singelton
        private static AIBank bank = null;
        public static AIBank Inst
        {
            get
            {
                if (bank == null)
                {
                    bank = new AIBank();
                }
                return bank;
            }
        }
        #endregion        
        private Dictionary<int, IAgentControl> controlBank;
        private Dictionary<string, int> _stringToID;

        private AIBank()
        {
            controlBank = new Dictionary<int, IAgentControl>();
            _stringToID = new Dictionary<string, int>();     
        }

        public void Clear()
        {
            controlBank.Clear();
            _stringToID.Clear();
        }

        public void AddControl(IAgentControl control, string nameID = null)
        {
            Debug.Assert(control.ID > 0, "IAgentControl ID must be bigger then zero");
            controlBank.Add(control.ID, control);
            if (nameID != null)
                _stringToID.Add(nameID, control.ID);
        }

        public IAgentControl GetControl(string id)
        {
            return GetControl(_stringToID[id]);
        }

        public IAgentControl GetControl(int id)
        {
            Debug.Assert(id > 0, "IAgentControl ID must be bigger then zero");
            if (controlBank.ContainsKey(id))
                return controlBank[id].GetWorkingCopy();
            else
                throw new Exception("IAgentControl: " + id + " not found!");
        }

    }
}
