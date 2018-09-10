using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Workshop.Models;

namespace Workshop.Services
{
    public class HighScoreService : IHighScoreService
    {
        static readonly SemaphoreSlim semaphoreSlim;

        readonly string highScorePath;

        static HighScoreService() => semaphoreSlim = new SemaphoreSlim(1, 1);

        public HighScoreService()
        {
            var binPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            highScorePath = Path.Combine(binPath, "highScores.json");
            if (File.Exists(highScorePath)) 
            {
                HighScore = int.Parse(File.ReadAllText(highScorePath));
            }
        }

        public int HighScore { get; private set; }
        public async Task<bool> AddScore(int score) 
        {
            await semaphoreSlim.WaitAsync();
            try {
                if (score <= HighScore) {
                    return false;
                }

                HighScore = score;
                await File.WriteAllTextAsync(highScorePath, score.ToString());
                return true;
            } finally {
                semaphoreSlim.Release();
            }           
        }
    }
}