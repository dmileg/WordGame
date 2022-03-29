using System;
using System.Collections.Generic;

namespace WordGameAPI
{
    /// <summary>
    /// Game state stored in memory cashe. 
    /// </summary>
    public class Game
    {
        // Unique game id. Set on start.
        public int Id { get; set; }

        // Client time of game start.
        public DateTime DateStart { get; set; }
        
        // Start time plus time limit.
        public DateTime DateEnd { get; set; }
        
        // Randomly generated letters at start.
        public char[] InitLetters { get; set; }
        
        // All submitted words during the game.
        public HashSet<string> UsedWords = new HashSet<string>();
        
        // Current score. Calculated based on length of the word.
        public int Score { get; set; }

        // Gets text with current score.
        internal string GetScoreText()
        {
            return $"Your score: {this.Score}";
        }

        /// <summary>
        /// Checks if only initial letters are being used. Each letter can be used only once.
        /// </summary>
        /// <param name="word"></param>
        /// <returns>True - the word consist of only initial letters; false - there are extra letters</returns>
        public bool CheckLetters(string word)
        {
            List<char> availableLetters = new();
            availableLetters.AddRange(InitLetters);
            for (int i = 0; i < word.Length; i++)
            {
                if (availableLetters.Contains(word[i]))
                {
                    availableLetters.Remove(word[i]);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
