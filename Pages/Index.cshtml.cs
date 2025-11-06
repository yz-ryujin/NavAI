using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.ML; 
using NavAI.Models;
using Microsoft.AspNetCore.Hosting; 
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;

    public class IndexModel : PageModel
    {

        private readonly PredictionEngine<SentimentData, SentimentPrediction> _engine;
        private readonly IWebHostEnvironment _env;

        [BindProperty]
        public string InputText { get; set; }
        public string UserQuestion { get; set; }
        public string BotResponse { get; set; }
        public string ResponseConfidence { get; set; }

        public IndexModel(PredictionEngine<SentimentData, SentimentPrediction> engine, IWebHostEnvironment env)
        {
            _engine = engine;
            _env = env;
        }

        public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(InputText))
            {
                return Page();
            }

            var prediction = _engine.Predict(new SentimentData { Text = InputText });

            UserQuestion = InputText;
            BotResponse = prediction.PredictedLabel
                ? "A análise deste local é predominantemente POSITIVA."
                : "A análise deste local é predominantemente NEGATIVA.";

            ResponseConfidence = $"{prediction.Probability:P2}";

            InputText = string.Empty;

            return Page();
        }

        public class FeedbackData
        {
            public string Prompt { get; set; }
            public string Response { get; set; }
            public bool IsUseful { get; set; }
        }

        public async Task<IActionResult> OnPostFeedbackAsync([FromBody] FeedbackData data)
        {
            try
            {
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var logEntry = $"[{timestamp}] (Útil: {data.IsUseful})" + Environment.NewLine +
                               $"PROMPT: {data.Prompt}" + Environment.NewLine +
                               $"RESPONSE: {data.Response}" + Environment.NewLine +
                               "-----------------------------------------------" + Environment.NewLine + Environment.NewLine;

                var rootPath = _env.ContentRootPath;
                var logGeralPath = Path.Combine(rootPath, "todas_as_requisicoes.txt");
                var logUteisPath = Path.Combine(rootPath, "requisicoes_uteis.txt");

                await System.IO.File.AppendAllTextAsync(logGeralPath, logEntry);

                if (data.IsUseful)
                {
                    await System.IO.File.AppendAllTextAsync(logUteisPath, logEntry);
                }

                return new OkResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar feedback: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
    }