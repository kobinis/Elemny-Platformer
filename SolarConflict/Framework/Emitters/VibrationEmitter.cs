/*

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Xml.Serialization;

namespace SolarConflict
{
    //ToDo: add volume, pitch, add save and load
    public class VibrationEmitter : IEmitter
    {
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }


        public float vibration = 5;



        public VibrationEmitter()
        {
        }


        public void Emit(GameObject owner, List<GameObject> spriteList, Vector2 refPosition, Vector2 refSpeed, float refRotation, Color? color, int faction, float refRotationSpeed = 0)
        {
           // ControlXbox.vibrationTimer = vibration; //posibliy reletive to camera
        }
    

        public IEmitter Load(string path)
        {
            return null;
        }
    }
}

*/