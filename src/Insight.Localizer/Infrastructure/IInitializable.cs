using System.Threading.Tasks;

namespace Insight.Localizer.Infrastructure
{
    /// <summary>
    /// Marks that implementation requires initialization
    /// </summary>
    public interface IInitializable
    {
        Task Initialize();
    }
}