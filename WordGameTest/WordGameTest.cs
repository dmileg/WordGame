using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using WordGameAPI;
using WordGameAPI.Controllers;
using WordGameAPI.Properties;

namespace WordGameTest
{
    public class WordGameTest
    {
        WordGameController _controller;

        public WordGameTest()
        {
            _controller = new WordGameController();
        }

        [Fact]
        public void TestDictionaryAPI()
        {
            string word = "fff";

            Encoding encode = Encoding.UTF8;
            UriBuilder builder = new UriBuilder($"https://api.dictionaryapi.dev/api/v2/entries/en/{word}");
            Uri uri = builder.Uri;
            HttpClient _httpClient = new HttpClient(new HttpClientHandler()) { Timeout = new TimeSpan(0, 0, 0, 120000) };
            var response = _httpClient.GetAsync(uri).Result;
            Task<string> read = response.Content.ReadAsStringAsync();
            read.Wait();
            JObject jo = JObject.Parse(read.Result);
            string title = jo.GetOrDefault("title", "");
            Assert.Equal("No Definitions Found", title);
        }

        [Fact]
        public void StartGameDefaultParams()
        {
            Game game = (Game)_controller.StartGame(DateTime.Now).Value;
            Assert.Equal(12, game.InitLetters.Length);
            Assert.Equal(90, (game.DateEnd - game.DateStart).TotalSeconds);
            Assert.True(game.Id > 0);
            Assert.Equal(0, game.Score);
            string result = (string)_controller.EndGame(game.Id).Value;
            Assert.Equal("Your score: 0", result);
        }

        [Fact]
        public void StartGameCustomParams()
        {
            int letters = 20;
            int seconds = 60 * 60;
            Game game = (Game)_controller.StartGame(DateTime.Now, letters, seconds).Value;
            Assert.Equal(letters, game.InitLetters.Length);
            Assert.Equal(seconds, (game.DateEnd - game.DateStart).TotalSeconds);
            Assert.True(game.Id > 0);
            Assert.Equal(0, game.Score);
            string result = (string)_controller.EndGame(game.Id).Value;
            Assert.Equal("Your score: 0", result);
        }

        [Fact]
        public void EndNonExistentGame()
        {
            string result = (string)_controller.EndGame(0).Value;
            Assert.Equal("Game is already over. Start a new one.", result);
        }

        [Fact]
        public void EnterNonExistentWord()
        {
            Game game = (Game)_controller.StartGame(DateTime.Now).Value;
            Assert.Equal(12, game.InitLetters.Length);
            Assert.Equal(90, (game.DateEnd - game.DateStart).TotalSeconds);
            Assert.True(game.Id > 0);
            Assert.Equal(0, game.Score);

            game.InitLetters = new char[] { 'f', 'f', 'f', 'f' };
            string submittedResult = (string)_controller.SubmitWord(game.Id, "ffff", DateTime.Now).Value;
            Assert.Equal(Resources.NonexistentWord, submittedResult);

            string endGameResult = (string)_controller.EndGame(game.Id).Value;
            Assert.Equal("Your score: 0", endGameResult);
        }

        [Fact]
        public void EnterExtraLettersWord()
        {
            Game game = (Game)_controller.StartGame(DateTime.Now).Value;
            Assert.Equal(0, game.Score);

            // Replacing randomly generated letters
            game.InitLetters = new char[] { 'w', 'o', 'r', 'd' };

            // 'a' is not given
            string submittedResult = (string)_controller.SubmitWord(game.Id, "road", DateTime.Now).Value;
            Assert.Equal(Resources.ExtraLettersInSubmission, submittedResult);

            Assert.Equal(0, game.Score);
        }

        [Fact]
        public void EnterEmptyWord()
        {
            Game game = (Game)_controller.StartGame(DateTime.Now).Value;
            Assert.Equal(0, game.Score);

            // Replacing randomly generated letters
            game.InitLetters = new char[] { 'w', 'o', 'r', 'd' };

            string submittedResult = (string)_controller.SubmitWord(game.Id, "", DateTime.Now).Value;
            Assert.Equal(Resources.EmptyWord, submittedResult);

            Assert.Equal(0, game.Score);
        }

        [Fact]
        public void EnterAlreadySubmittedWord()
        {
            Game game = (Game)_controller.StartGame(DateTime.Now).Value;
            Assert.Equal(0, game.Score);

            // Replacing randomly generated letters
            game.InitLetters = new char[] { 'w', 'o', 'r', 'd' };

            string submittedResult = (string)_controller.SubmitWord(game.Id, "word", DateTime.Now).Value;
            Assert.Equal(4, game.Score);

            submittedResult = (string)_controller.SubmitWord(game.Id, "word", DateTime.Now).Value;
            Assert.Equal(Resources.AlreadySubmitted, submittedResult);

            string endGameResult = (string)_controller.EndGame(game.Id).Value;
            Assert.Equal("Your score: 4", endGameResult);
        }

        [Fact]
        public void EnterWords()
        {
            Game game = (Game)_controller.StartGame(DateTime.Now).Value;
            Assert.Equal(0, game.Score);

            // Replacing randomly generated letters
            game.InitLetters = new char[] { 'w', 'o', 'r', 'd' };

            string submittedResult = (string)_controller.SubmitWord(game.Id, "word" , DateTime.Now).Value;
            Assert.Equal(4, game.Score);

            submittedResult = (string)_controller.SubmitWord(game.Id, "rod", DateTime.Now).Value;
            Assert.Equal(7, game.Score);

            string endGameResult = (string)_controller.EndGame(game.Id).Value;
            Assert.Equal("Your score: 7", endGameResult);
        }

        [Fact]
        public void TwoGamesAtATime()
        {
            Game game1 = (Game)_controller.StartGame(DateTime.Now).Value;
            Assert.Equal(0, game1.Score);

            Game game2 = (Game)_controller.StartGame(DateTime.Now).Value;
            Assert.Equal(0, game2.Score);

            // Replacing randomly generated letters
            game1.InitLetters = new char[] { 'w', 'o', 'r', 'd' };
            game2.InitLetters = new char[] { 'w', 'o', 'r', 'd' };

            string submittedResult = (string)_controller.SubmitWord(game1.Id, "word", DateTime.Now).Value;
            Assert.Equal(4, game1.Score);

            submittedResult = (string)_controller.SubmitWord(game2.Id, "rod", DateTime.Now).Value;
            Assert.Equal(3, game2.Score);

            string endGameResult = (string)_controller.EndGame(game1.Id).Value;
            Assert.Equal("Your score: 4", endGameResult);

            endGameResult = (string)_controller.EndGame(game2.Id).Value;
            Assert.Equal("Your score: 3", endGameResult);
        }
    }
}
