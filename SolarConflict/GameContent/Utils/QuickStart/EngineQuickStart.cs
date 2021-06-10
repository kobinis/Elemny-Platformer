using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Utils
{
    public struct EngineData
    {
        public ItemData ItemData;
        public float MaxSpeed { get; set; }
       // public float CommbatMaxSpeed { get; set; }
        public float Force { get; set; }
        public Color TrailColor { get; set; }
        public String TrailEmitterID;

        public EngineData(string name, float maxSpeed = 0, float force = 0)
        {            
            ItemData = new ItemData(name);
            ItemData.Category = ItemCategory.Engine;
            ItemData.SlotType = SlotType.Engine;
            MaxSpeed = maxSpeed;
            Force = force;
            TrailColor = Color.White;
            TrailEmitterID = null;             
        }


    }    

    public class EngineQuickStart
    {
        public static Item Make(EngineData data)
        {
            ItemProfile profile = ItemQuickStart.Profile(data.ItemData);
            profile.StatsText = ExtendDescription(data);                                   
            Item item = new Item(profile);
            AgentEngine engine = new AgentEngine(data.Force, data.MaxSpeed);
            if (data.TrailEmitterID != null)
                engine.trailEmitter = ContentBank.Inst.GetEmitter(data.TrailEmitterID);
            engine.color = data.TrailColor;
            item.System = engine;
            return item;
        }

        private static string ExtendDescription(EngineData data)
        {
            var result = "";     
            return result + $"Max Speed: #ctext{{255,255,0,\"{data.MaxSpeed}\"}}\nForce: #ctext{{255,255,0,\"{data.Force}\"}}";
        }
    }
}
