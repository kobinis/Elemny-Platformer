using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict.GameContent.Activities.Levels
{
    class MiningActivity:Scene
    {

        public override ActivityParameters OnLeave()
        {
            return base.OnLeave();
        }

        public override void InitScript(string parameters = null, ActivityParameters activityParameters = null)
        {
            SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.Inventory);
            AddGameObject("Player", Framework.FactionType.Player, Vector2.Zero, 0, AgentControlType.Player);
            this.AddObjectRandomlyInCircle("ResourceMine1", 4, 7000, 20);
            var shopGameObject = AddGameObject("SmallShop1", Framework.FactionType.TradingGuild, Vector2.One * 1000, 90) as Agent;
            var shop = new ShopSystem();            
            shop.AddItem("MediumRotationEngine3", 1);
            shop.AddItem("VacuumModulatorItem", 1);
            shop.AddItem("MiningLaser1", 1);
            this.AddObjectRandomlyInCircle("Asteroid0", 500, 20000, 20);
            this.AddObjectRandomlyInCircle("Asteroid1", 300, 20000, 20);
            this.AddObjectRandomlyInCircle("Asteroid2", 300, 20000, 1000);
            shopGameObject.AddSystem(shop);
        }

        public override void UpdateScript(InputState inpuState)
        {            
        }

        public override void DrawScript(SpriteBatch sb)
        {
            base.DrawScript(sb);
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new MiningActivity();
        }
    }
}
