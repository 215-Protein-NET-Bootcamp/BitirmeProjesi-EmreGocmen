using System;
using System.Threading.Tasks;

namespace BuyWithOffer
{
    public interface IUnitOfWork : IDisposable
    {
        Task CompleteAsync();
    }
}
