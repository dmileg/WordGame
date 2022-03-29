using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WordGameAPI
{
    /// <summary>
    /// In-memory cashe for started games. ToDo: free memory using background worker e.g. delete games started an hour ago.
    /// </summary>
    public class GameCashe
    {
        // Main lock object for lockers collection
        private static object _gamesLocker = new object();

        // Subsequent id for a new game
        private static int nextId = 0;

        // Locker object. Different for each game.
        public class GameLocker
        {
            public object Locker = new object();
        }
        
        /// <summary>
        /// Lockers for each game. Key - game id, value - locker for the game.
        /// </summary>
        private static Dictionary<int, GameLocker> gameLockers = new Dictionary<int, GameLocker>();

        /// <summary>
        /// Cashe for games.
        /// </summary>
        public static Dictionary<int, Game> GamesCashe = new Dictionary<int, Game>();

        /// <summary>
        /// Get the lock object for the particular game.
        /// </summary>
        public static GameLocker GetGameLocker(int id)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException("id", $"GetGameLocker incorrect id={id}");

            lock (gameLockers)
            {
                if (!gameLockers.ContainsKey(id))
                    gameLockers.Add(id, new GameLocker());
                return gameLockers[id];
            }
        }

        /// <summary>
        /// Adds the game to cashe.
        /// </summary>
        public static void AddGame(Game game)
        {
            lock (_gamesLocker)
            {
                nextId++;
                game.Id = nextId;
                GamesCashe.Add(game.Id, game);
            }
        }

        /// <summary>
        /// Gets game from cashe by id.
        /// </summary>
        /// <param name="game"></param>
        /// <returns>True: success, false: game has not been found in cashe.</returns>
        public static Game GetGame(int gameId)
        {
            GameLocker gameLocker = GetGameLocker(gameId);
            lock (gameLocker.Locker)
            {
                if (GamesCashe.ContainsKey(gameId))
                {
                    return GamesCashe[gameId];
                }
            }
            return null;
        }

        /// <summary>
        /// Updates game in cashe.
        /// </summary>
        /// <param name="game"></param>
        /// <returns>True: success, false: game has not been found in cashe.</returns>
        public static bool UpdateGame(Game game)
        {
            GameLocker gameLocker = GetGameLocker(game.Id);
            lock (gameLocker.Locker)
            {
                if (GamesCashe.ContainsKey(game.Id))
                {
                    GamesCashe[game.Id] = game;
                    return true;
                }              
            }
            return false;
        }

        /// <summary>
        /// Removes game from cashe.
        /// </summary>
        /// <param name="game"></param>
        /// <returns>True: success, false: game has not been found in cashe.</returns>
        public static bool RemoveGame(int id)
        {
            GameLocker gameLocker = GetGameLocker(id);
            bool success = false;
            lock (gameLocker.Locker)
            {
                if (GamesCashe.ContainsKey(id))
                {
                    GamesCashe.Remove(id);
                    success = true;
                }
            }
            gameLockers.Remove(id);

            return success;
        }
    }
}
