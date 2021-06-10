using Microsoft.Xna.Framework;
using SolarConflict.Framework.InGameEvent.GenericProcess;
using SolarConflict.Framework.Scenes.HudEngine;
using SolarConflict.Framework.Scenes.HudEngine.Components;
using SolarConflict.Session.World.MissionManagment.GlobalObjectives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaUtils;

namespace SolarConflict.GameContent.Activities.Challenges
{
    class LowBudgetShooter: Scene
    {
        PlaceHolderObjective objective;
        SpwanEnemysProcess process;
        int counter;

        public override void InitScript(string parameters = null, ActivityParameters activityParameters = null)
        {
            HudManager.AddComponent(new TargetIndicator());
            HudManager.AddComponent(new AnnouncerEffects());

            AddGameObject("Sun", -Vector2.One * 10000000);
            AddGameObject("PlayerWave", Vector2.Zero, faction: Framework.FactionType.Player, controlType: AgentControlType.Player);           
        }

        public override void UpdateScript(InputState inputState)
        {
            if(process == null || process.Finished)
            {
                GameEngine.Level = counter/2;
                counter++;
                process = new SpwanEnemysProcess("MediumShip9_sg", new ReferencePositionProvider(FMath.ToCartesian(4000, FMath.Rand.NextAngle())));
                GameEngine.AddGameProcces(process);
                PlayerAgent?.SetMeterValue(MeterType.Hitpoints, 100000);
                PlayerAgent?.SetMeterValue(MeterType.Shield, 1000000);
            }
        }

        public static Activity ActivityProvider(string parameters) //TODO: change
        {
            return new LowBudgetShooter();
        }
    }
}
