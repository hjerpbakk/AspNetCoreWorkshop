using System.Threading.Tasks;

namespace api.Services
{
    public interface IHighScoreService
    {
        int HighScore { get; }
        Task<bool> AddScore(int score);
    }
}