//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using XnaUtils;
//using XnaUtils.Graphics;

//namespace SolarConflict
//{
//    class DrawOneFromList : AgentDraw
//    {
//        List<Sprite> Sprites;
//        //List<float> scaleList;

//        int selectedSprite;

//        public DrawOneFromList(List<Sprite> textureProxies)
//        {
//            selectedSprite = -1;
//            this.Sprites = textureProxies;
//        }

//        public DrawOneFromList()
//            : this(new List<Sprite>())
//        {
//        }

//        public void AddTexture(Sprite sprite)
//        {
//            Sprites.Add(sprite);
//        }

//        public override void Draw(Agent agent, Camera camera)
//        {
//            if (selectedSprite == -1)
//            {
//                selectedSprite = FMath.Rand.Next(Sprites.Count);
//            }
//            camera.CameraDraw(Sprites[selectedSprite], agent.Position, agent.Rotation, 1f, Color.White);//mov    
//        }


//    }
//}
