using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework.Scenes;
using SolarConflict.Framework.World.MetaGame;

using System;
using System.Collections.Generic;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.Framework.MetaGame.World
{
    public enum NodeVisibility
    {
        /// <summary>
        /// Node shows partial information
        /// </summary>
        Hidden,
        Visible,
        Visited
    }
    /// <summary>
    /// Represents an interesting point in space, could be a star system, a nebula, an asteroid, a comet, etc. to be shown on the galaxy map
    /// </summary>
    [Serializable]
    public class NodeInfo //TODO: change name
    {
        private readonly int NODE_RADIUS = 20;
        private static Sprite nodeSprite = Sprite.Get("node1");

        public String Name { get; set; }
        /// <summary>
        /// Addtional description, generated after visting a node
        /// </summary>
        public String Description { get; set; }
        /// <summary>For procedural generation</summary>
        public int RandomSeed { get; set; }
        public NodeType Type { get; set; }
        public FactionType ControllingFaction { get; set; }
        public List<FactionType> FactionsByStrength;
        public int Level { get; set; } // TODO: decompose into population and tech level, maybe into militarization as well

        [NonSerialized]
        public GuiControl _tooltipVerbose;
        [NonSerialized]
        public GuiControl _tooltipTerse;

        [NonSerialized]
        public RichTextControl _textControl;

        public List<int> Neighbors { get; private set; }

        NodeVisibility _visibility;
        public NodeVisibility Visibility {
            get {
                return _visibility;
            }
            set {
                if (value != _visibility) {
                    // Refresh tooltipw
                    _tooltipTerse = null;
                    _tooltipVerbose = null;
                }
                _visibility = value;
            }
        }

        // Meta Data
        // REFACTOR: find a way to stop copying the IDs code all the time.
        public string ToolTipPlanetTextureID
        {
            get { return ToolTipPlanetTexture.ID; }
            set { ToolTipPlanetTexture = Sprite.Get(value); }
        }
        
        public Sprite MapTexture;        
        public Sprite ToolTipPlanetTexture;
        public Vector2 Position { get; set; }
        public String ActivityName { get; set; }
        public String ActivityParams { get; set; }
        public float Rotation { get; set; }

        public NodeInfo() {
            Neighbors = new List<int>();
            FactionsByStrength = new List<FactionType>();
            FactionsByStrength.Add(FactionType.Pirates1); //NOW: change
            Visibility = NodeVisibility.Hidden;
        }

        public void Draw(Camera camera, float time, bool inRange)
        {
            //Visibility = NodeVisibility.Visible;
            float twinkle = (float)(Math.Cos(Rotation + Game1.time * 0.0051f) + 1) * 0.5f * 0.5f + 0.5f;
            Color nodeColor = new Color(twinkle,twinkle,twinkle);
            switch (Visibility)
            {
                case NodeVisibility.Hidden:
                    break;
                case NodeVisibility.Visible:
                    nodeColor = FactionColorIndicator.FactionToColor(ControllingFaction);
                    break;
                case NodeVisibility.Visited:
                    nodeColor = FactionColorIndicator.FactionToColor(ControllingFaction);
                    break;
                default:
                    break;
            }
            //nodeSprite.Draw(camera.SpriteBatch, Position, Rotation + Game1.time * 0.005f, (0.7f + 8 * 0.1f) * 0.1f, nodeColor);
            //nodeSprite.Draw(camera.SpriteBatch, Position, Rotation - Game1.time * 0.006f, (0.7f + 8 * 0.1f) * 0.1f, nodeColor);
            camera.CameraDraw(nodeSprite, Position, Rotation + Game1.time * 0.005f, (0.7f +8* 0.1f)* 0.1f, nodeColor);
            camera.CameraDraw(nodeSprite, Position, Rotation - Game1.time * 0.006f, (0.7f + 8 * 0.1f) * 0.1f, nodeColor);
        }



        public void DrawTooltip(Camera camera, Vector2 position, int index, bool hideDetails = false)
        {
            
            if (hideDetails )
            {
                // Details hidden
                var text = new RichTextControl(Name);
                text.Shadow = new TextShadow(2);
                _tooltipTerse = text;
            }

            if (!hideDetails)
            {
                // Details visible
                var layout = new VerticalLayout(Vector2.Zero);
                layout.ShowFrame = true;
                _tooltipVerbose = layout;
                Faction faction = MetaWorld.Inst.GetFaction(ControllingFaction);
                //ToolTipPlanetTexture

                var imageControl = new ImageControl(GetNodeSprite(), Vector2.Zero, Vector2.One * 200);
                _tooltipVerbose.AddChild(imageControl);
                Color nodeColor = FactionColorIndicator.FactionToColor(ControllingFaction);
                //var faction = MetaWorld.Inst.GetFaction(ControllingFaction);
                string text = "Name: " + Name + "\n"
                + "Faction:" + faction.Color.ToTag(faction.Name) +"\n#s{96}" + faction.LogoSprite.ToTag() +"\n #line{}" 
                // + faction.Motto + "\n"
                + "Type: #hcolor{} " + Type.GetUserName() + "#dcolor{}\n"
                + "Level: " + Level.ToString() + "\n"
              //  + "Num: " + index + "\n"
                + Description;
                if (DebugUtils.Mode != ModeType.Release)
                    text += "\n" + index.ToString();
                RichTextControl richTextControl = new RichTextControl(text, Game1.font);
                _tooltipVerbose.AddChild(richTextControl);
                _tooltipVerbose.Update(InputState.EmptyState);
            }

            var tooltip = hideDetails ? _tooltipTerse : _tooltipVerbose;

            tooltip.Position = position + new Vector2(tooltip.HalfSize.X, -tooltip.HalfSize.Y);
            tooltip.FitToScreen();
            tooltip.Draw(camera.SpriteBatch);
        }        

        public bool IsPositionOverNode(Vector2 searchPosition)
        {
            return (Position - searchPosition).LengthSquared() < NODE_RADIUS * NODE_RADIUS;
        }

        public Sprite GetNodeSprite()
        {
            return Sprite.Get(Type.GetSpriteID());
           // return nodeSprite;
        }

        public void Reveal() {
            Visibility = (NodeVisibility)Math.Max((int)Visibility, (int)NodeVisibility.Visible);
        }

        public void PopulateDescription(Scene scene)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Color.Yellow.ToTag("Visited"));
            //sb.AppendLine("Faction Rep:");
            //FactionType[] factions = { FactionType.Federation, FactionType.Empire, FactionType.Pirates1, FactionType.TradingGuild };
            //foreach (var factionType in factions)
            //{
            //    var faction = scene.GameEngine.GetFaction(factionType);
            //    if (faction == null)
            //        continue;
            //    sb.Append("   " + faction.ToTag() + " : ");
            //    float rel = faction.GetRelationToFaction(FactionType.Player);
            //    if (rel < 0)
            //        sb.Append("#color{255,0,0}");
            //    sb.Append(rel.ToString());
            //    sb.AppendLine("#dcolor{}");
            //}
            if (scene.GetMissions().Count > 0)
            {
                sb.AppendLine("Missions:");
                foreach (var mission in scene.GetMissions())
                {
                    if (!mission.IsGlobal && !mission.IsHidden)
                        sb.AppendLine("   " + mission.Title);
                }
            }
             
            Description = sb.ToString();
        }
    }
}
