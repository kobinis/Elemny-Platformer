//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using SolarConflict.XnaUtils.Input;
//using XnaUtils;

//namespace SolarConflict
//{
    
//    public class CameraLogicNew
//    {
//        private static Dictionary<Mode, CameraProfile> profiles;

//        static CameraLogicNew()
//        {
//            profiles = new Dictionary<Mode, CameraProfile>();
//            CameraProfile auto = new CameraProfile();            
//            auto.LinearMovementSpeed = 0;
//            auto.MultMovmentFactor = 0.7f;
//            profiles.Add(Mode.Auto, auto);
//        }

//        public enum Movment
//        {
            
//        }

//        public struct CameraProfile
//        {
//            //Camera Movment
//            public float LinearMovementSpeed;
//            public float MultMovmentFactor;
//            public float zoomFactor;
//            public TargetType CameraTargetType;
//        }


//        public CameraLogicNew()
//        {
//            Profile = profiles[Mode.Auto];
//        }

//        public enum Mode
//        {
//            Auto,
//            AutoNoZoom,
//            Manual,
//            None,
//        }

//        public enum TargetType
//        {
//            Player, //Sets the target of the camera to the player
//            Manual, //Sets the target of the camera to a spesific position
//           // PlayerCoPlayer, //
//        }
        
        

//        public CameraProfile Profile;        

//        public GameObject mainFocus;

//        float targetZoom = 1;

//        //Vector2 targetPos;
        
        

//        public void CameraUpdate(GameEngine gameEngine, Camera camera)
//        {
//            Profile.zoomFactor = 0.1f;
//            mainFocus = gameEngine.Scene.FindPlayer(true);
//            if(mainFocus == null)
//            {
//                mainFocus = gameEngine.Scene.GetPlayerFaction().Mothership;
//            }
//            //if ((mainFocus == null || mainFocus.IsNotActive) ) //Change
//            //{            
//            //        float mainFocusCameraPriority = -1;
//            //        foreach (var gameObject in gameEngine.PotentialTargets) //change
//            //        {
//            //            if (gameObject.IsActive && gameObject.GetCameraPriority() > mainFocusCameraPriority)
//            //            {
//            //                mainFocus = gameObject;
//            //                mainFocusCameraPriority = mainFocus.GetCameraPriority();
//            //            }
//            //        }             
//            //}

//            Vector2 targetPos = Vector2.Zero;
//            if(mainFocus != null)
//                targetPos = mainFocus.Position;

//          //  camera.position = targetPos;
                                    
            
//            float zoomValue = targetZoom * MathHelper.Clamp(((50f + MouseUtils.Inst.GetDScroolWheel()) / 50f), 0.9f, 1.1f);
//            targetZoom = MathHelper.Clamp(zoomValue, 0.05f, 1.5f);

//            camera.Zoom = camera.Zoom * (1 - Profile.zoomFactor) + targetZoom * Profile.zoomFactor;
//            //camera.zoom = targetZoom;
//            camera.Position = UpdateMovemant(camera.Position, targetPos);
//            //camera.position = (camera.position * (1 - movmentFactor) + cameraTargetPos * movmentFactor);
//             //camera.zoom = 0.4f;
//        }

//        //private Vector2 GetTarget()
//        //{
//        //    switch (Profile.CameraTargetType)
//        //    {
//        //        case TargetType.Player:
//        //            break;
//        //        case TargetType.Manual:
//        //            break;
//        //        case TargetType.PlayerCoPlayer:
//        //            break;
//        //        default:
//        //            break;
//        //    }
//        //    return targetPos;
//        //}


//        // can be moved to my math
//        private Vector2 UpdateMovemant(Vector2 currentPos, Vector2 targetPos)
//        {
//            Vector2 resPos = currentPos;
//            currentPos = currentPos * (1 - Profile.MultMovmentFactor) + targetPos * Profile.MultMovmentFactor;
//            Vector2 dif = targetPos - currentPos;

//            if (Profile.LinearMovementSpeed > 0)
//            {
//                float lengath = dif.Length();
//                if (lengath > Profile.LinearMovementSpeed)
//                {
//                    resPos = currentPos + dif / lengath * Profile.LinearMovementSpeed;
//                }
//                else
//                {
//                    resPos = currentPos;
//                }
//            }
//            return resPos;
//        }

//    }
//}
