//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework.Graphics;
//using XnaUtils;
//using SolarConflict.AI;
//using XnaUtils.SimpleGui;
//using System.Threading;
//using XnaUtils.SimpleGui.Controllers;
//using XnaUtils.Input;
//using SolarConflict.XnaUtils.SimpleGui;
//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.CameraControl.Zoom;

//namespace SolarConflict.GameContent.Activities.AI
//{
//    class LoadoutTrainerActivity : Activity
//    {
//        LoadoutTrainer trainer;
//        GuiManager gui;
//        private Thread thread;
//        RichTextControl startControl;
//        //ShipEvaluation eval;    
//        TextControl fitnessControl;       

//        public LoadoutTrainerActivity()
//        {
            
//            trainer = new LoadoutTrainer("MediumShip1A"); //Pirate1                        
//            gui = new GuiManager();
//            gui.Root = MakeGui();            
//        }

//        private GuiControl MakeGui()
//        {
//            ControlsGroup group = new ControlsGroup();

//            GuiControl layout = new VerticalLayout(ActivityManager.ScreenSize * 0.5f);
//            startControl = new RichTextControl("Train Generation");
//            startControl.IsShowFrame = true;
//            startControl.Action += StartTraining;
//            layout.AddChild(startControl);

//            RichTextControl showSimulationControl = new RichTextControl("Show Simulation", null, true);
//            showSimulationControl.Action += (GuiControl source, CursorInfo cursorLocation) => 
//            { ActivityManager.Inst.SwitchActivity(new AgentEvalActivity(trainer)); };
//            layout.AddChild(showSimulationControl);


//            fitnessControl = new TextControl("Fitness: ");
//            fitnessControl.Position = new Vector2(200, 20);
//            group.AddChild(fitnessControl);


//            group.AddChild(layout);           
//            return group;
//        }

//        public void StartTraining(GuiControl source, CursorInfo cursorLocation)
//        {
//            if (thread == null || !thread.IsAlive)
//            {
//                startControl.Text = "Training..";                
//                thread = new Thread(TrainingInvoke);
//                thread.Start();
//            }
//        }               

//        public void TrainingInvoke()
//        {
//            trainer.TrainOneGeneration();
//        }

//        public override void Update(InputState inputState)
//        {
//            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
//                ActivityManager.Inst.Back();
//            if (thread == null || !thread.IsAlive)
//            {
//                startControl.Text = "Train Generation";                
//            }

//            fitnessControl.Text = "Fitness: " + trainer.GetFitness().ToString();
//            gui.Update(inputState);                        
//        }

//        public override void Draw(SpriteBatch sb)
//        {
            
//            gui.Draw();
            
//        }

//        public static Activity ActivityProvider(string parameters)
//        {
//            return new LoadoutTrainerActivity();
//        }

//        public override ActivityParameters OnLeave()
//        {
//            if(thread != null)
//                thread.Abort();
//            return base.OnLeave();
//        }
//    }

    

//    public class AgentEvalActivity : Scene
//    {        
//        Agent trainedAgent;
//        Agent trainerAgent;
//        ManualZoom zoomLogic;

//        public AgentEvalActivity(LoadoutTrainer trainer)
//        {
//            zoomLogic = new ManualZoom();
            
//            GameEngine = trainer.MakeGameEngine(out trainedAgent, out trainerAgent);
//            GameEngine.Scene = this;
//            GameEngine.Camera = Camera;

//            trainerAgent.SetControlType(AgentControlType.Player);
//        }
     
//    }
//}
