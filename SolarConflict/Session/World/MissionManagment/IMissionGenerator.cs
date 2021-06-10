using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.Session.World.MissionManagment
{    
    public interface IMissionGenerator
    {
        string ID { get; set; }
        string Title { get; set; }        
        TextAsset Description { get; set; }        
        FactionType Faction { get; set; }
        StarDate StartMissionDate { get; set; }
        bool IsTaken { get; set; }
        bool IsOverrideID { get; set; }
        bool IsGlobal { get; set; }        
        Color Color { get; set; }
        bool IsDismissable { get; set; }
        bool IsHidden { get; set; }        
        int? DestenationNode { get; set; }
        Sprite Icon { get; set; }
        int Level { get; set; }

        bool CheckIfValid(Scene scene);
        Mission GenerateMission();
        string GetTooltipText();
    }
}
