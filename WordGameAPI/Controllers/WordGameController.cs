using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WordGameAPI;
using WordGameAPI.Properties;
using static WordGameAPI.GameCashe;

namespace WordGameAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordGameController : ControllerBase
    {
        

        private readonly ILogger<WordGameController> _logger;
        private static HttpClient HttpClient = new HttpClient(new HttpClientHandler()) { Timeout = new TimeSpan(0, 0, 0, 120000) };
        public WordGameController(ILogger<WordGameController> logger)
        {
            _logger = logger;
        }
        public WordGameController()
        {
        }

        [HttpGet]
        public JsonResult StartGame(DateTime clientTime, int letterCount = 12, int seconds = 90)
        {
            try
            {
                Random random = new Random();
                char[] letters = new char[letterCount];
                for(int i =0; i < letterCount; i++)
                {
                    int a = random.Next(0, 26);
                    char ch = (char)('a' + a);
                    letters[i] = ch;
                }
                Game game = new Game { DateStart = clientTime, DateEnd = clientTime.AddSeconds(seconds), InitLetters = letters };
                AddGame(game);
                return new JsonResult(game);
            }
            catch (Exception e)
            {
                if (_logger != null)
                    _logger.LogError(e.ToString(), new object[] {clientTime, letterCount, seconds });
                return new JsonResult("An error occurred");
            }
        }

        [HttpGet]
        public JsonResult SubmitWord(int gameId, string word, DateTime clientTime)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(word))
                    word = word.ToLower();

                Game game = GetGame(gameId);
                string result = Resources.GameIsNotFound;
                if (game != null)
                {
                    if (clientTime > game.DateEnd)
                    {
                        result = $"{Resources.GameOver} {game.GetScoreText()}";
                        RemoveGame(gameId);
                    }
                    else
                    {
                        try
                        {
                            if (string.IsNullOrWhiteSpace(word))
                            {
                                result = Resources.EmptyWord;
                            }
                            else if(!game.CheckLetters(word))
                            {
                                result = Resources.ExtraLettersInSubmission;
                            }
                            else if (game.UsedWords.Contains(word))
                            {
                                result = Resources.AlreadySubmitted;
                            }
                            else
                            {
                                // Check word in dictionaryapi.dev
                                if (Exists(word)) 
                                { 
                                    game.Score += word.Length;
                                    game.UsedWords.Add(word);
                                    UpdateGame(game);
                                    result = $"{Resources.Success} {game.GetScoreText()}";
                                }
                                else
                                {
                                    result = Resources.NonexistentWord;
                                }                                
                            }                            
                        }
                        catch (Exception ex)
                        {
                            if (_logger != null)
                                _logger.LogError(ex.ToString(), new object[] { gameId, word, clientTime});
                            result = Resources.UnknownException;
                        }
                    }
                }
                return new JsonResult(result);
            }
            catch (Exception e)
            {
                if(_logger != null)
                    _logger.LogError(e.ToString(), new object[] { gameId, word, clientTime });
                return new JsonResult(Resources.UnknownException);
            }
        }

        

        [HttpGet]
        public JsonResult EndGame(int gameId)
        {
            try
            {
                string result;                
                Game game = GetGame(gameId);
                                
                if(game == null)
                {
                    result = Resources.GameIsNotFound;
                }
                else
                {
                    result = game.GetScoreText();
                }

                RemoveGame(gameId);
                return new JsonResult(result);
            }
            catch (Exception e)
            {
                if (_logger != null)
                    _logger.LogError(e.ToString(), new object[] { gameId});
                return new JsonResult(Resources.UnknownException);
            }
        }


        /// <summary>
        /// Checks if the word is real.
        /// </summary>
        /// <returns>True - the word exists; false - the word is non-existent.</returns>
        private static bool Exists(string word)
        {
            Encoding encode = Encoding.UTF8;
            UriBuilder builder = new UriBuilder($"https://api.dictionaryapi.dev/api/v2/entries/en/{word}");
            Uri uri = builder.Uri;
            var response = HttpClient.GetAsync(uri).Result;
            Task<string> read = response.Content.ReadAsStringAsync();
            read.Wait();

            try
            {
                var words = JArray.Parse(read.Result);
                if (words.Count > 0)
                    return true;
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}
