using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;
using XnaUtils;

namespace SolarConflict.GameContent.Activities.Challenges
{    
    class Minesweeper:Scene
    {
        string baseName = "ball";

        public override void InitScript(string parameters = null, ActivityParameters activityParameters = null)
        {
            //ActionOnPlayerDeath = ActionOnPlayerDeathType.Back; Add process
            int mineFieldSize = 10;
            float mineSize = 135;
            var board = GenerateBoard(mineFieldSize, mineFieldSize, 10);
            for (int y = 0; y < board.GetLength(1); y++)
            {
                for (int x = 0; x < board.GetLength(0); x++)
                {
                    var mine = GenerateMine(board[x, y]);
                    mine.Position = new Vector2(x, y) * mineSize;
                    GameEngine.AddGameObject(mine);
                }
            }

            AddGameObject("MineSweeperShip", Framework.FactionType.Player, new Vector2(0,2000), 0, AgentControlType.Player);
        }

        public void GenerateMineFiled(GameEngine gameEngine, Vector2 postion)
        {

        }

        public Agent GenerateMine(int level)
        {
            
            Agent agent = new Agent();
            agent.Mass = 1000000;
            agent.impactSpec.Force = 20;
            agent.VelocityInertia = 0;
            agent.Size = 128 / 2f;
            agent.MaxHitpoints = 10;
            agent.CurrentHitpoints = agent.MaxHitpoints;
            agent.Sprite = Sprite.Get(baseName);// + level.ToString());
            if (level >= 0)
            {
                agent.AddSystem(new BasicEmitterCallerSystem(ControlSignals.OnDestroyed, "FullExplosionFx1"));
                if (level > 0)
                {
                    var emitter = new BasicEmitterCallerSystem();
                    emitter.Activation = ControlSignals.OnDestroyed;
                    emitter.Emitter = GenerateRevaledMine(level);
                    agent.AddSystem(emitter);
                }
                else
                {                 
                    var expEmitter = new ParamEmitter();
                    expEmitter.EmitterID = "Shot1";
                    expEmitter.MinNumberOfGameObjects = 8;
                    expEmitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Range;
                    expEmitter.VelocityAngleRange = 360;
                    expEmitter.VelocityMagMin = 10;
                    expEmitter.RotationType = ParamEmitter.EmitterRotation.VelocityAngle;
                    expEmitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
                    expEmitter.LifetimeMin = 14;
                    agent.AddSystem(new BasicEmitterCallerSystem(ControlSignals.OnDestroyed, expEmitter));
                }
            }
            if(level == -1)
            {
                var emitter = new ParamEmitter();
                emitter.EmitterID = "NovaRingExplosion";
                emitter.SizeBase = 15;
                emitter.SizeType = ParamEmitter.InitSizeType.Const;
                agent.AddSystem(new BasicEmitterCallerSystem(ControlSignals.OnDestroyed, emitter));
            }
            return agent;
        }

        private Agent GenerateRevaledMine(int level)
        {
            Agent agent = new Agent();
            agent.Size = 128 / 2f;
            agent.Mass = 1000000;
            agent.VelocityInertia = 0;
            agent.MaxHitpoints = 5000;
            agent.CurrentHitpoints = agent.MaxHitpoints;
            agent.Sprite = Sprite.Get(baseName + level.ToString());
         //   agent.AddSystem(new BasicEmitterCallerSystem(ControlSignals.OnTakingDamage, "EmitterDebris1"));
            return agent;
        }

        public static int[,] GenerateBoard(int sizeX, int sizeY, int mineNum)
        {
            Random rand = new Random();
            int[,] board = new int[sizeX, sizeY];
            int mineCount = 0;
            while (mineCount < mineNum)
            {
                int x = rand.Next(sizeX);
                int y = rand.Next(sizeY);
                if(board[x,y] == 0)
                {
                    board[x, y] = -1;
                    mineCount++;
                }
            }

            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    if (board[x, y] != -1)
                    {
                        int count = 0;
                        for (int py = Math.Max(0, y - 1); py <= Math.Min(sizeY - 1, y + 1); py++)
                        {
                            for (int px = Math.Max(0, x - 1); px <= Math.Min(sizeX - 1, x + 1); px++)
                            {
                                if (board[px, py] == -1)
                                {
                                    count++;
                                }

                            }
                        }
                        board[x, y] = count;
                    }
                }
            }

            return board;
        }

        public static Activity ActivityProvider(string parameters) 
        {
            return new Minesweeper();
        }

    }
}
