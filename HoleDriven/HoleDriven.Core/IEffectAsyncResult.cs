using System.Threading.Tasks;

namespace HoleDriven.Core
{
    public interface IEffectAsyncResult
    {
        Task Task { get; }
    }
}
