using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

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
            var scoresFolder = Path.Combine(binPath, "scores");
            Directory.CreateDirectory(scoresFolder);
            highScorePath = Path.Combine(scoresFolder, "highScore.json");
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