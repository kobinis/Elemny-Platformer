using Microsoft.Xna.Framework;
using SolarConflict.CameraControl;
using SolarConflict.Framework.CameraControl;
using SolarConflict.Framework.CameraControl.Movment;
using SolarConflict.Framework.CameraControl.Zoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict
{
    public enum CameraZoomType
    {
        ToTargetZoom,
        Manual,
        Auto,
        Custom, //?
    }

    public enum CameraMovmentType
    {
        ToTarget,
        OnPlayer,
        Manual,  
        TwoPlayers,      
        Custom, //??
    }

    [Serializable]
    public class CameraManager //KOBI: refactor class
    {
        public const float STARTING_ZOOM = 0.5f;

        private GameObject _mainTarget;
        private GameObject _secondaryTarget;
        private TargetType _targetType;

        //private ICameraZoom[] _cameraZoomLogic;        
        private ManualZoom _manualZoom;
        private AutoZoom _autoZoom;
        private CameraZoomBase _toTargetZoom; //TODO: change to custum
        private CameraOnPlayer _cameraPlayerMovment;
        private CameraMovementTwoPlayers _twoPlayersMove;

        private float _minZoom;
        private float _maxZoom;

        public ManualZoom ManualZoom { get { return _manualZoom; } }

        public CameraZoomType ZoomType { set; get; }

        private Vector2 _targetPosition;
        private CameraBaseMovment _liniarMovment;
        private ManualMovement _manualMovement;

        public CameraMovmentType MovmentType { set; get; }

        public float CameraMovmentSpeed
        {
            set { _liniarMovment.MovmentSpeed = value; }
            get { return _liniarMovment.MovmentSpeed; }
        }

        public float CameraMovmentFactor
        {
            set { _liniarMovment.MovmentFactor = value; }
            get { return _liniarMovment.MovmentFactor; }
        }

        public float CameraMovmentAcceleration
        {
            set { _liniarMovment.MovmentAcceleration = value; }
            get { return _liniarMovment.MovmentAcceleration; }
        }

        public Vector2 TargetPosition
        {
            get { return _targetPosition; }
            set { _targetPosition = value; }
        }

        public float TargetZoom
        {
            get { return _manualZoom.TargetZoom; }
            set { _manualZoom.TargetZoom = value; _toTargetZoom.TargetZoom = value; /*_autoZoom.TargetZoom = value;*/ } //TODO: change 
        }


        public CameraManager()
        {
            _targetType = TargetType.Enemy;
            //_cameraZoomLogic = new ICameraZoom[3];
            //_cameraZoomLogic[(int)ZoomType.Auto] = new AutoZoom();
            //_zoomType = ZoomType.Auto;
            ZoomType = CameraZoomType.Manual;
            _autoZoom = new AutoZoom(0.04f);
            _manualZoom = new ManualZoom(0.05f,0.01f);
            _toTargetZoom = new CameraZoomBase(0.05f, 0.01f); //maybe accelerating

            _liniarMovment = new CameraBaseMovment(0.1f, 0.1f);
            _manualMovement = new ManualMovement();
            _cameraPlayerMovment = new CameraOnPlayer(0.8f, 1f);            
            _twoPlayersMove = new CameraMovementTwoPlayers(0.05f, 1f);

            _minZoom = 0.15f;
            _maxZoom = 1.5f;

            MovmentType = CameraMovmentType.OnPlayer;
            ZoomType = CameraZoomType.Manual;
        }

        public void Update(Camera camera, GameEngine gameEngine, InputState inputState)
        {
            //Finds target
            GameObject player = gameEngine.Scene == null ? null : gameEngine.Scene.FindPlayer();


            if (player != null)
                _mainTarget = player;            

            //Secondary target logic
            
            if (_mainTarget != null)
            {
                float gainTargetRange = 8000;
                float loseTargetRange = gainTargetRange * 1.4f;
                if (_secondaryTarget == null || _secondaryTarget.IsNotActive)
                {
                    _secondaryTarget = gameEngine.CollisionManager.FindClosestTarget(_mainTarget.Position, gainTargetRange, GameObjectType.PotentialTarget, gameEngine.Scene?.GetPlayerFaction());
                }
                if (_secondaryTarget != null && (GameObject.DistanceFromEdge(_secondaryTarget, _mainTarget) > loseTargetRange || _secondaryTarget != _mainTarget.GetTarget(gameEngine, _targetType)))
                {
                    _secondaryTarget = null;
                }
            }

            if (gameEngine.Scene?.CoPlayerAgent!= null) //And is controlled by player
            {
                _secondaryTarget = gameEngine.Scene.CoPlayerAgent;
            }

            //if (_mainTarget != null)
            //{

            //Movement
            switch (MovmentType)
                {
                    case CameraMovmentType.ToTarget:
                        _liniarMovment.Update(camera, ref _targetPosition);
                        break;
                    case CameraMovmentType.OnPlayer:
                    _cameraPlayerMovment.Update(camera, ref _targetPosition, _mainTarget, _secondaryTarget, gameEngine, inputState);
                        break;
                    case CameraMovmentType.Manual:
                        _manualMovement.Update(camera, _mainTarget, _secondaryTarget, gameEngine, inputState);
                        break;
                    case CameraMovmentType.TwoPlayers:
                        _twoPlayersMove.Update(camera, ref _targetPosition, _mainTarget, _secondaryTarget, gameEngine, inputState);
                    break;
                    case CameraMovmentType.Custom:
                        break;
                    default:
                        break;
                }

                //Zoom
                switch (ZoomType)
                {
                    case CameraZoomType.ToTargetZoom:
                        _toTargetZoom.Update(camera);
                        break;
                    case CameraZoomType.Manual:
                         _manualZoom.TargetZoom = MathHelper.Clamp(_manualZoom.TargetZoom, _minZoom, _maxZoom);
                        _manualZoom.Update(camera, _mainTarget, _secondaryTarget, gameEngine, inputState);
                       camera.Zoom = MathHelper.Clamp(camera.Zoom, _minZoom, _maxZoom);
                        break;
                     case CameraZoomType.Auto:
                        _autoZoom.Update(camera, _mainTarget, _secondaryTarget, gameEngine, inputState);
                        break;
                default:
                        break;
                }

                
           // }

           
        }

        public bool IsCameraOnTarget(Camera camera)
        {
            return _targetPosition == camera.Position;
        }

     
    }
}
