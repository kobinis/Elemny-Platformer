using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework;
using SolarConflict.GameContent.Activities.SceneActivitys;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.GameContent.Activities.Challenges
{
    /// <summary>
    /// Desgin a ship on a budget to win a challange
    /// </summary>
    class DesignChallengeScene:Scene
    {
        private class ChallengeData
        {
            public List<string> Ships;
            public List<string> EnemyShips;
            public List<string> Items;
            public float Money;            

            public string MainShip { get { return Ships[0]; } }

      
            public ChallengeData()
            {
                
            }

            public ChallengeData(ActivityParameters parameters)
            {
                Ships = parameters.ParamDictionary["Ships"].Split(',').ToList();
                EnemyShips = parameters.ParamDictionary["EnemyShips"].Split(',').ToList();
                Items = parameters.ParamDictionary["Items"].Split(',').ToList();
                Money = float.Parse(parameters.ParamDictionary["Money"], new CultureInfo("en-US"));
            }
        }
        

        private ShopActivity _shopActivity;
        
        public DesignChallengeScene(string parameters):base(parameters, false)
        {

        }

        public override void InitScript(string parameters = null, ActivityParameters activityParameters = null)
        {                        
           
        }
        
        public override void OnEnter(ActivityParameters parameters)
        {
            base.OnEnter(parameters);
            ChallengeData data = new ChallengeData(parameters);
            _shopActivity = new ShopActivity();
            foreach (var item in data.Items)
            {
                _shopActivity.AddItem(item);
            }
            float distance = 3000;
            Vector2 initPosition = -Vector2.UnitX * distance;
            var objects = GameEngine.AddObjectsInFormation(data.Ships, FactionType.Player, initPosition, 0);
            if (objects.Count > 0)
                objects[0].SetControlType(AgentControlType.Player);
            GameEngine.AddObjectsInFormation(data.Ships, FactionType.Pirates1, -initPosition, 180);            
            GameEngine.GetFaction(Framework.FactionType.Player).GetMeter(MeterType.Money).SetValue(data.Money);
            GameEngine.Update(InputState.EmptyState);      
        }

        public override void UpdateScript(InputState inpuState)
        {
            if(SceneFrameCounter == 0)
              ActivityManager.Inst.SwitchActivity(_shopActivity, true);            
        }

        public override void DrawScript(SpriteBatch sb)
        {            
        }

        public static Activity ActivityProvider(string parameters) //TODO: change
        {
            return new DesignChallengeScene(parameters);
        }
    }
}
