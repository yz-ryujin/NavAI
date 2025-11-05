using Microsoft.ML.Data;

namespace NavAI.Models
{
    public class SentimentPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool PredictedLabel { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }
    }
}
