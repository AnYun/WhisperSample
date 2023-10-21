using Azure;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;

namespace WhisperSample.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnPost(IFormFile voicefile)
        {
            var apikey = "{your api key}";
            var apiUrl = "https://{your openai endpoint}.openai.azure.com";

            var client = new OpenAIClient(new Uri(apiUrl), new AzureKeyCredential(apikey));
            try
            {
                var transcriptionOptions = new AudioTranscriptionOptions()
                {
                    AudioData = BinaryData.FromStream(voicefile.OpenReadStream()),
                    ResponseFormat = AudioTranscriptionFormat.Verbose,
                };

                Response<AudioTranscription> transcriptionResponse = await client.GetAudioTranscriptionAsync(
                    deploymentId: "{your deploy}",
                    transcriptionOptions);
                AudioTranscription transcription = transcriptionResponse.Value;

                return Content(transcription.Text);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}