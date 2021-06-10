//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SolarConflict.Session.World.MissionManagment;

//namespace SolarConflict.Framework.Scenes.DialogEngine
//{
//    [Serializable]
//    class DialogHolder : IDialogNode
//    {
//        public string ID { get; set; }
//        public string TextAssetID { get; set; }
//        public bool? IsBlocking { get; set; }
//        public bool? IsSkippable { get; set; }
//        public bool IsInterupting { get; set; }
//        private IDialogNode dialog;

//        public DialogHolder(string textAssetID, string id = null)
//        {
//            TextAssetID = textAssetID;
//            ID = id;
//        }

//        public string ImageID
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }

//            set
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public bool IsFinished
//        {
//            get
//            {
//                //CreateDialogNode();
//                throw new NotImplementedException();
//            }

//            set
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public string Text
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }

//            set
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public string SoundID
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }

//            set
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public IDialogNode GetNextNode(DialogManager dialogManager, Scene scene)
//        {
//            throw new NotImplementedException();
//        }

//        public IDialogNode InitCreate(DialogManager dialogManager, Scene scene, IDialogNode parent)
//        {
//            CreateDialogNode();
//            return dialog.InitCreate(dialogManager, scene, parent);
//        }

//        public void Update(DialogManager dialogManager, Scene scene)
//        {
//            throw new NotImplementedException();
//        }

//        private void CreateDialogNode()
//        {
//            if (dialog == null)
//                dialog = TextBank.Inst.MakeDialogFromAsset(TextBank.Inst.GetTextAsset(TextAssetID), ID);
//        }

//        public List<DialogOption> GetDialogOptions()
//        {
//            throw new NotImplementedException();
//        }

//        public void SelectDialogOption(int index, Scene scene)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
