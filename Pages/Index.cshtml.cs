using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.ML;
using NavAI.Models;

public class IndexModel : PageModel
{
    private readonly PredictionEngine<SentimentData, SentimentPrediction> _predictionEngine;

    public IndexModel(PredictionEngine<SentimentData, SentimentPrediction> predictionEngine)
    {
        _predictionEngine = predictionEngine;
    }

    [BindProperty]
    public string InputText { get; set; }

    public string UserQuestion { get; set; }
    public string BotResponse { get; set; }
    public string ResponseConfidence { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(InputText))
        {
            return Page();
        }

        UserQuestion = InputText;

        var inputData = new SentimentData { Text = InputText };
        var predictionResult = _predictionEngine.Predict(inputData);

        if (predictionResult.PredictedLabel == true)
        {
            BotResponse = "Isso descreve um lugar com boas condições de vida!";
        }
        else
        {
            BotResponse = "Isso descreve um lugar com condições desafiadoras.";
        }

        ResponseConfidence = predictionResult.Probability.ToString("P1");

        return Page();
    }
}