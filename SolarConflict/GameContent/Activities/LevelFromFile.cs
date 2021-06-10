using SolarConflict.Framework;
using SolarConflict.Framework.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.GameContent.Activities {
    //public class LevelFromFile : SkirmishBattle
    // TODO: merge with or inherit from SkirmishBattle
    public class LevelFromFile : Scene {
        int _exitTimer;
        bool _challengeEnded;

        //public LevelFromFile(string parameters) : base(parameters)
        public LevelFromFile(string parameters) : base(parameters, false) {
            _exitTimer = 0;
            _challengeEnded = false;
        }

        public override void InitScript(string parameters = null, ActivityParameters activityParameters = null)
        {
            
        }

        public static Activity ActivityProvider(string parameters) {
            var name = parameters.Split('\\').Last();
            name = name.Substring(0, name.Length - 4);
            //if (Kludges.Skirmishes.ContainsKey(name)) {
            //    var skirmishDescriptor = Kludges.Skirmishes[name];

            //    var result = new SkirmishBattle();
            //    skirmishDescriptor.Item1.Do(id => result.AddLoadout(ContentBank.Inst.GetEmitter(id) as AgentLoadout, FactionType.Player));
            //    skirmishDescriptor.Item2.Do(id => result.AddLoadout(ContentBank.Inst.GetEmitter(id) as AgentLoadout, FactionType.Empire));

            //    result.Resources[FactionType.Player] *= skirmishDescriptor.Item3;
            //    result.Resources[FactionType.Empire] *= skirmishDescriptor.Item4;

            //    result.AddObjectRandomlyInCircle("BigAsteroid", skirmishDescriptor.Item5, 10000);

            //    return result;
            //}


            return new LevelFromFile(parameters);
        }

        public override void UpdateScript(InputState inpuState) {
            if (PlayerAgent != null) {
                var kills = PlayerAgent.GetMeterValue(MeterType.Kills);
                SetText("Kills: " + kills);
            }
            var faction = GameEngine.GetSoleFaction();
            if (_exitTimer >= 240) {
                ActivityManager.Inst.Back();
            }
            if (faction != Framework.FactionType.None) {
                if (faction == Framework.FactionType.Player) {
                    //won
                    //DialogManagerOld.AddDialogBox("Congratulations, you have completed the challenge\nReward: 1000 Spacebucks", boxID: "challengewon", isFixedSize: false);
                    var playerFaction = MetaWorld.Inst.GetFaction(Framework.FactionType.Player);
                    playerFaction.GetMeter(MeterType.Money).AddValue(1000);
                    if (!_challengeEnded)
                        this.AddObjectRandomlyInLocalCircle("CashDrop10", 100, 1000f, PlayerAgent.Position, 100);
                    _exitTimer++;
                } else {
                    //lost
                    //DialogManagerOld.AddDialogBox("You failed to complete the challenge", boxID: "challengelost");
                    _exitTimer++;
                }
                _challengeEnded = true;
            }
            
        }
    }
}
