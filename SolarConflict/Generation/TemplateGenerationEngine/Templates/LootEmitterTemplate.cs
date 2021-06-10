using SolarConflict.Framework.Emitters;
using SolarConflict.GameContent.ContentGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Generation.TemplateGenerationEngine.Templates
{
    class LootEmitterTemplate : GenerationTemplate
    {
        const string BASE_EMITTER = "Base*";
        const string LEVEL = "Level";
        int maxLevel = 10;
        public LootEmitterTemplate()
        {
            _directoryName = "LootEmitters";
            AddParametereName(ID);
            AddParametereName(BASE_EMITTER);
            for (int i = 1; i <= maxLevel; i++)
            {
                AddParametereName(LEVEL + i.ToString());
            }
        }

        protected override void ParseAndAddEmitter(string[] parameters)
        {
            string id = csvUtils.GetString(ID);
            IEmitter baseEmitter = ContentParsingUtil.ParseEmitter(csvUtils.GetString(BASE_EMITTER, null));
            IEmitter[] emitters = new IEmitter[maxLevel + 1]; 
            int currentSize = 0;
            for (int i = 1; i <= maxLevel; i++)
            {
                IEmitter emitter = ContentParsingUtil.ParseEmitter(csvUtils.GetString(LEVEL + i.ToString(), null));
                emitters[i] = emitter;
                if (emitter != null)
                    currentSize = i + 1;
            }
            LevelEmitter levelEmitter = null;
            if (currentSize > 0)
                levelEmitter = new LevelEmitter(emitters);
            if(baseEmitter != null || levelEmitter != null)
            {
                if (levelEmitter == null)
                {
                    if (base.ID != null)
                    {
                        var emitter = new EmitterIdHolder(id, baseEmitter);
                        ContentBank.Inst.AddContent(emitter);
                    }
                    else
                    {
                        baseEmitter.ID = id;
                        ContentBank.Inst.AddContent(baseEmitter);
                    } 
                }
                else
                {
                    if(baseEmitter == null)
                    {
                        levelEmitter.ID = id;
                        ContentBank.Inst.AddContent(levelEmitter);
                    }
                    else
                    {
                        GroupEmitter groupEmitter = new GroupEmitter();
                        groupEmitter.MakeRandomVelocitiy();
                        groupEmitter.ID = id;
                        groupEmitter.AddEmitter(baseEmitter);
                        groupEmitter.AddEmitter(levelEmitter);
                        ContentBank.Inst.AddContent(groupEmitter);
                    }
                }
            }
        }
    }
}
