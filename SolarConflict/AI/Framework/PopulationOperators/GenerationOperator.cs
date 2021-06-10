using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.AI.Framework.PopulationOperators
{
    class GenerationOperator : OperatorGroup
    {
        public GenerationOperator(EvaluationOperator evaluationOperator, FeedbackEventOperator feedbackEventOperator, RepopulateOperator repopulate)
        {
            AddOperator(evaluationOperator);
            AddOperator(new SortOperator());
            AddOperator(feedbackEventOperator);
            //TODO: Add memory operator //??
            AddOperator(new RankSelectionOperator());
            AddOperator(repopulate);                        
        }
    }
}
