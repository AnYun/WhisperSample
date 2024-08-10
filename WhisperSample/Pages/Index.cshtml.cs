using Azure;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenAI.Audio;
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
            var deploymentName = "{your deploy}";

            var client = new AzureOpenAIClient(new Uri(apiUrl), new AzureKeyCredential(apikey)).GetAudioClient(deploymentName);
            try
            {
                var transcriptionOptions = new AudioTranscriptionOptions()
                {
                    ResponseFormat = AudioTranscriptionFormat.Verbose,
                };

                AudioTranscription transcription = await client.TranscribeAudioAsync(voicefile.OpenReadStream(), voicefile.FileName, transcriptionOptions);

                return Content(transcription.Text);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}