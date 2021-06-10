﻿using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.GameContent.Activities.Challenges
{
    class DuelChallengeScene : Scene
    {
        const int FADE_TIME = 340;
        private bool _levelHasEnded;
        private int _fadeOutTimer;
        private bool _hasWon;

        public DuelChallengeScene() : base(null, false)
        {

        }

        public override void InitScript(string parameters = null, ActivityParameters activityParameters = null)
        {
            float rad = 2000;
            string playerID = "PlayerClone";
            string enemyID = activityParameters.GetParam("enemy");
            this.GameEngine.Level = int.Parse(activityParameters.GetParam("level", "1"));
            AddGameObject(playerID, Vector2.UnitY * rad, 90, FactionType.Player, AgentControlType.Player);
            AddGameObject(enemyID, -Vector2.UnitY * rad, 90, FactionType.Pirates1);
        }

        public override void OnEnter(ActivityParameters parameters)
        {
            _fadeOutTimer = 0;
            _hasWon = false;
            UpdatePlayerShip = false;
        }
        public override void UpdateScript(InputState inpuState)
        {
            var survivingFactions = GameEngine.GetSurvivingFactions();
            if (survivingFactions.Count <= 1 && !_levelHasEnded && GameEngine.FrameCounter > 10)
            {
                _levelHasEnded = true;
                if (survivingFactions.Contains(FactionType.Player))
                {
                    Victory();
                }
                else
                {
                    Defeat();
                }
            }


            if (_levelHasEnded)
            {

                _fadeOutTimer++;
                fadeAlpha = _fadeOutTimer / (float)FADE_TIME;
                if (_fadeOutTimer >= FADE_TIME)
                {
                    ActivityManager.Inst.Back();
                }
            }

        }

        private void Victory()
        {
            DialogManager.AddDialog("victory");
            this.CameraManager.ZoomType = CameraZoomType.ToTargetZoom;
            CameraManager.TargetZoom = 0.5f;
            AddGameObject("VictoryImage", Vector2.Zero, 0);
            AddGameObject("FireworksSource", Camera.Position + new Vector2(0, ActivityManager.ScreenSize.Y), -90);
            _hasWon = true;
        }

        public void Defeat()
        {
            DialogManager.AddDialog("defeat");
        }

        public override ActivityParameters OnBack()
        {
            base.OnBack();
            ActivityParameters p = new ActivityParameters();
            if (_hasWon)
                p.ParamDictionary.Add("victory", _hasWon.ToString());
            return p;
        }

        public static Activity ActivityProvider(string parameters) //TODO: change
        {
            return new DuelChallengeScene();
        }
    }
}
