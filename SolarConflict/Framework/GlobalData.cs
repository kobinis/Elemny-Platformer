using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework
{
    public class GlobalData
    {
        #region Singelton
        private static GlobalData instance;
        public static GlobalData Inst
        {
            get
            {
                if (instance == null)
                    instance = new GlobalData();
                return instance;
            }
        }
        #endregion
        private Dictionary<string, string> data;

        private GlobalData()
        {
            data = new Dictionary<string, string>();
            Init();
        }

        public string GetData(string id)
        {
            string ans;
            data.TryGetValue(id, out ans);
            return ans;
        }

        public void SetData(string id, string value)
        {
            data[id] = value;
        }

        //TODO: read data from settings file, use template
        private void Init()
        {
            data.Add("prolog_ships", "PrologShip2,PrologShip1");
            data.Add("starting_ships", "StartingShip2,StartingShip1");
        }


    }
}
