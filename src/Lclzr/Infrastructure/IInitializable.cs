using System.Threading.Tasks;

namespace Lclzr.Infrastructure
{
    /// <summary>
    /// Marks that implementation requires initialization
    /// </summary>
    public interface IInitializable
    {
        Task Initialize();
    }
}