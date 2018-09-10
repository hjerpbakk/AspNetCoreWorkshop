using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Workshop.Models;

namespace Workshop.Services
{
    public class HighScoreService : IHighScoreService
    {
        static readonly ReaderWriterLockSlim writeLock;

        readonly string highScorePath;
        
        static HighScoreService() 
        {
            writeLock = new ReaderWriterLockSlim();
        }

        public HighScoreService(IHostingEnvironment hostingEnvironment)
        {
            highScorePath = Path.Combine(hostingEnvironment.WebRootPath, "highScores.json");
            if (File.Exists(highScorePath)) 
            {
                HighScore = int.Parse(File.ReadAllText(highScorePath));
            }
        }

        public int HighScore { get; private set; }
        public async Task<bool> AddScore(int score) 
        {
            writeLock.EnterWriteLock();
            if (score < HighScore) {
                return false;
            }

            try
            {
                await File.WriteAllTextAsync(highScorePath, score.ToString());
            }
            finally 
            {
                writeLock.ExitWriteLock();
            }

            return true;
        }
    }
}