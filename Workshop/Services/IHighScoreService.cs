using System.Threading.Tasks;

namespace Workshop.Services
{
    public interface IHighScoreService
    {
        int HighScore { get; }
        Task<bool> AddScore(int score);
    }
}