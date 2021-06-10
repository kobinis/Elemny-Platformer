using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SolarConflict.Framework.Menu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.Framework
{

    [Serializable]
    public struct MenuData
    {
        public bool PlayMainMenuMusic { get; set; }
        public bool ShowLogo { get; set; }
        public List<MenuEntry> MenuEntryList { get; set; }
        public string Title { get; set; }
        //public 

        public MenuData(string title)
        {
            Title = title;
            ShowLogo = false;
            PlayMainMenuMusic = false;
            MenuEntryList = new List<MenuEntry>();
        }

        public void AddEntry(string text, string activity, string parameters = "", bool rememberPreviousActivity = true, Keys key = Keys.None)
        {
            MenuEntry entry = new MenuEntry(text);
            entry.ActivityName = activity;
            entry.ActivityParams = parameters;
            entry.RememberPreviousActivity = rememberPreviousActivity;
            entry.ActivationKey = key;
            MenuEntryList.Add(entry);
        }

        public void SaveSettings(string filename)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
            StreamWriter file = new StreamWriter(filename);
            xmlSerializer.Serialize(file, this);
            file.Close();
        }

        public static MenuData LoadSettings(string filename)
        {
            filename = Path.Combine(Consts.MENUS_PATH, filename);
            if (File.Exists(filename))
            {
                // This path is a file
                // ProcessFile(filename);
                return LoadMenuFromFile(filename);
            }
            else if (Directory.Exists(filename))
            {
                return LoadMenuFromDirectory(filename);
            }
            else
            {
                throw new Exception(filename + " is not a valid file or directory.");
                //Console.WriteLine("{0} is not a valid file or directory.", filename);
            }
        }

        private static MenuData LoadMenuFromDirectory(string path)
        {

            Dictionary<string, string> extensionMap = new Dictionary<string, string>();
            extensionMap.Add(".xml", "Menu");
            extensionMap.Add(".txt", "LevelFromFile");


            string title = Path.GetDirectoryName(path);
            MenuData menuData = new MenuData(title);


            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(path);
            foreach (string subdirectory in subdirectoryEntries)
            {
                // ProcessDirectory(subdirectory);
                MenuEntry entry = new MenuEntry();
                entry.DisplayText = Path.GetDirectoryName(subdirectory);
                entry.ActivityName = "Menu"; //to read from params
                entry.ActivityParams = subdirectory;
                menuData.MenuEntryList.Add(entry);
            }

            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(path);
            foreach (string fileName in fileEntries)
            {
                MenuEntry entry = new MenuEntry();
                entry.DisplayText = Path.GetFileNameWithoutExtension(fileName);
                string fileExtension = Path.GetExtension(fileName);
                entry.ActivityName = extensionMap[fileExtension];
                entry.ActivityParams = fileName;
                menuData.MenuEntryList.Add(entry);
                // ProcessFile(fileName);
            }
            MenuEntry back = new MenuEntry("Back");
            back.ActivityName = "back";
            menuData.MenuEntryList.Add(back);

            return menuData;
        }

        private static MenuData LoadMenuFromFile(string filename)
        {
            MenuData menuData;
            if (File.Exists(filename))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(MenuData));
                StreamReader file = new StreamReader(filename);
                menuData = (MenuData)xmlSerializer.Deserialize(file);
                file.Close();

                return menuData;
            }
            else
            {
                throw new Exception("Menu file was not found!");
            }
        }

        public GuiControl MakeGui(GuiManager gui, bool isHorizontal = false, Sprite sprite = null, Color? color = null)
        {
            GuiControl layout;
            if (isHorizontal)
            {
                layout = new HorizontalLayout(new Vector2(ActivityManager.ScreenCenter.X, ActivityManager.ScreenSize.Y - 40), 10, true);

            }
            else
            {
                layout = new VerticalLayout(ActivityManager.ScreenCenter, 10, true, true);
            }
            //layout.is = true;
            if(sprite != null)
                layout.Sprite = sprite;
            if (color.HasValue)
            {
                layout.ControlColor = color.Value;
                layout.CursorOverColor = layout.ControlColor;
                layout.PressedControlColor = layout.ControlColor;
            }

            if (!string.IsNullOrEmpty(Title))
            {
                RichTextControl titleControl = new RichTextControl(Title, Game1.menuFont);
                layout.AddChild(titleControl);
            }
            for (int i = 0; i < MenuEntryList.Count; i++)
            {
                string text = MenuEntryList[i].DisplayText;
                RichTextControl textControl = new RichTextControl(text);
                textControl.Shadow = new TextShadow(2, Color.Black);
                textControl.TextHighlightColor = Color.Yellow;
                textControl.ActivationKey = MenuEntryList[i].ActivationKey;
                textControl.Data = MenuEntryList[i];
                textControl.TextColor = Color.White;
                textControl.CursorOverColor = Color.Yellow;
                textControl.IsShowFrame = false;
                //textControl.ControlColor = Color.White;

                if (MenuEntryList[i].TooltipText != null)
                {
                    textControl.TooltipText = MenuEntryList[i].TooltipText;
                    textControl.CursorOn += gui.ToolTipHandler;
                }

                textControl.Action += (source, cursorLocation) =>
                {
                    MenuEntry entry = source.Data as MenuEntry;
                    if (entry.ActivityName != null)
                        ActivityManager.Inst.SwitchActivity(entry.ActivityName, entry.ActivityParams, entry.RememberPreviousActivity);
                };

                if (MenuEntryList[i].Action != null)
                {
                    textControl.Action += MenuEntryList[i].Action;
                }

                textControl.UserData = (i).ToString();
                layout.AddChild(textControl);
            }
            layout.Update(InputState.EmptyState);
            return layout;
        }
    }
}