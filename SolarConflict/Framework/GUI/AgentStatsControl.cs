using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.Framework.Scenes.GuiControls
{
    public class StatEntryControl : RichTextControl//HorizontalLayout
    {
        public Stats Stat { get; private set; }
        private AgentStatExtractor _extractor; //TODO: maybe change 
        private Agent _agent;
        private string _textureID;

        public StatEntryControl(AgentStatExtractor extractor, Stats stat, Agent agent, string textureID) : base("Hello and goodbye")
        {
            Stat = stat;
            _agent = agent;
            _extractor = extractor;
            _textureID = "{" + textureID + "}";
            IsShowFrame = false;
        }

        public override void UpdateLogic(InputState inputState)
        {
            string stat = string.Empty;

            switch (Stat)
            {
                case Stats.Hitpoints:
                    stat = "HP";
                    break;
                case Stats.ShieldCapacity:
                    stat = "Shield";
                    break;
                case Stats.EnergyCapacity:
                    stat = "Energy";
                    break;
                case Stats.MaxSpeed:
                    stat = "Speed";
                    break;
                case Stats.DPS:
                    stat = "DPS";
                    break;
            }

            Text = string.Format("#image{2}{0}:{1}", stat, _extractor.GetStatValue(Stat), _textureID);
        }
    }

    public class AgentStatsControl: VerticalLayout
    {
        private AgentStatExtractor _statsExtractor;
        private Agent _agent;
        private RichTextControl _title;
        //private Dictionary<Stats, TextControl> _statsControls; //TODO: replace with StatEnteryControl        
        public AgentStatsControl(Agent agent, GuiManager gui):base(Vector2.Zero)
        {
            _agent = agent;
            _statsExtractor = new AgentStatExtractor();
            //_statsExtractor.CalculateStats(agent);
            _title = new RichTextControl("#image{helpicon} Ship Stats:");
            _title.CursorOn += gui.ToolTipHandler;
            _title.IsShowFrame = true;
            AddChild(_title);

            AddChild(new RichTextControl("Cost: " + agent.GetCost() + Sprite.Get("coin").ToTag()));
            AddChild(new RichTextControl("Size Class: " + agent.SizeType));
            AddChild(new RichTextControl("Hitpoints: " + agent.CurrentHitpoints + "/" + agent.MaxHitpoints));
     
            //AddChild(new StatEntryControl(_statsExtractor, Stats.ShieldCapacity, agent, "Shield"));          
            //AddChild(new StatEntryControl(_statsExtractor, Stats.EnergyCapacity, agent, "Energy"));
            //AddChild(new StatEntryControl(_statsExtractor, Stats.Hitpoints, agent, "HP"));

            //AddChild(new StatEntryControl(_statsExtractor, Stats.DPS, agent, "DPS"));
            //AddChild(new StatEntryControl(_statsExtractor, Stats.MaxSpeed, agent, "Speed"));

            //AddChild(new StatEntryControl(statsExtarcor, Stats.AngularMass, agent));
            //AddChild(new StatEntryControl(statsExtarcor, Stats.Size, agent));            
            // _statsControls.Add(Stats.Hitpoints, control);
        }

        

        public override void UpdateLogic(InputState inputState)
        {
            CalculateStats(_agent);
            if (_title.IsCursorOn)
            {
                _title.TooltipText = AgentUtils.DescribeStatsAndAbilities(_agent);
            }
        }

        public void CalculateStats(Agent agent)
        {
            //_statsExtractor.CalculateStats(agent);
        }


    }
}
