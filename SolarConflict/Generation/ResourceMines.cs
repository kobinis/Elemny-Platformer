using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.ContentGeneration {
    class ResourceMineGeneration {
        static string[] MINE_SPRITE = new string[] { "RubyMine", "EmeraldMine",
            "SapphireMine", "TopazMine", "AmetystMine" };

        public static void MakeMines() {
            Utility.Range(1, 6).Do(i => ContentBank.Inst.AddContent(MakeMine(i)));
        }

        static GameObject MakeMine(int level) {
            Agent mine = new Agent();
            ActivitySwitcherSystem interaction = new ActivitySwitcherSystem("CraftingActivity");
            interaction.Persistent = false;
            interaction.InteractionText = "Crafting";
            mine.InteractionSystem = interaction;
            mine.ID = $"ResourceMine{level}";
            mine.control = null; 
           
            mine.Sprite = Sprite.Get(MINE_SPRITE[level-1]);
            mine._drawType = DrawType.Lit;
            mine.VelocityInertia = 0.99f;
            
            mine.Size = Math.Min( mine.Sprite.Width, mine.Sprite.Height) * 0.3f;
            
            mine.RotateOnImpact = true;
            mine.RotationInertia = 0.98f;
            mine.Mass = 20000;
            mine.RotationMass = 200000;
            mine.SetMeter(MeterType.Hitpoints, new Meter(float.MaxValue*0.5f));           
            mine.Inventory = null;
            mine.impactSpec = new CollisionSpec(0, 6);
            mine.CraftingStationType |= CraftingStationType.Mining;
            //GroupEmitter oreEmitter = new GroupEmitter(); 
            //oreEmitter.EmitType = GroupEmitter.EmitterType.RandomOne;
            //for (int i = 1; i <= level; i++)
            //{
            //    oreEmitter.AddEmitter($"MatC{i}", 2f);
            //}       
            //oreEmitter.AddEmitter("MatA1", 1f);
            //oreEmitter.AddEmitter("MatB1", 1f);
            
            var item = ContentBank.Inst.GetItem($"MatC{level}", false);
            mine.Name = ItemProfile.GetLevelColor(level).ToTag(item.Tag +" resource mine");            

            ParamEmitter emitter = new ParamEmitter(); // decides where to drop it
            //emitter.EmitterID = //"MineLoot" + level.ToString(); ///oreEmitter;
            LootEmitter loot = new LootEmitter();
            loot.AddEmitter("MatC" + level.ToString(), 1, 1, 0);
            if(level < 5)
                loot.AddEmitter("MatC" + (1).ToString(), 0.1f, 1, 0);
            //if (level > 1)
            //    loot.AddEmitter("MatC" + (level - 1).ToString(), 0.1f, 1, 0);

            emitter.Emitter = loot;
            //emmiter.PosRadType = ParamEmitter.EmitterPosRad.ParentSizeTransformed;
            //emmiter.PosRadRange = 0.8f;
            emitter.PosRadMin = 0;
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Random;
            emitter.PosAngleRange = 360;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagRange = 10;
            emitter.VelocityMagMin = 10;
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.LastObjectToColide;
            emitter.VelocityAngleRange = 90;
            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = 10f;

            var miningDamageHandler = new EmitterCallerSystem(ControlSignals.OnColision, Utility.Frames(0.5f), Vector2.Zero, emitter); // detects mining damage, drops loot
            miningDamageHandler.ActivationCheck.AddCost(MeterType.MiningLevel, level + 1);
            miningDamageHandler.SelfImpactSpec = new CollisionSpec(); 
            miningDamageHandler.SelfImpactSpec.AddEntry(MeterType.MiningLevel, 0, ImpactType.Min); // here's where we prevent mining damage from accumulating

            
            mine.AddSystem(miningDamageHandler);
            mine.gameObjectType |= GameObjectType.Mine | GameObjectType.CraftingStation;
            //mine.CraftingStationType = CraftingStationType.Mining | CraftingStationType.ResourceMine;
            return mine;
        }
    }
}
