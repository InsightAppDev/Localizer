using System.Threading.Tasks;

namespace Lclzr.Infrastructure
{
    internal interface IInitializable
    {
        Task Initialize();
    }
}