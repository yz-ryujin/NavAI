using Microsoft.ML;
using NavAI.Models;
using NavAI.Hooks;

namespace NavAI.Service
{
    public static class ModelBuilder
    {
        public static void Treinar(string pastaModelos)
        {
            var ml = new MLContext(seed: 1);

            var caminhoCsv = Path.Combine(pastaModelos, "sentiment.csv");
            var appender = new AppendData(caminhoCsv);
            appender.DadosDeTeste();

            var data = ml.Data.LoadFromTextFile<SentimentData>(
                caminhoCsv,
                hasHeader: true, separatorChar: ',');

            var split = ml.Data.TrainTestSplit(data, testFraction: 0.2, seed: 1);

            var pipeline = ml.Transforms.Text.FeaturizeText("Features", nameof(SentimentData.Text))
                .Append(ml.BinaryClassification.Trainers.SdcaLogisticRegression(
                    labelColumnName: nameof(SentimentData.Label),
                    featureColumnName: "Features"));

            var model = pipeline.Fit(split.TrainSet);

            var caminhoModelo = Path.Combine(pastaModelos, "model.zip");
            ml.Model.Save(model, split.TrainSet.Schema, caminhoModelo);
        }
    }
}