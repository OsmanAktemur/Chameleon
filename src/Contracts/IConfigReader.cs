using Chameleon.Entities;

namespace Chameleon.Contracts
{
    public interface IConfigReader<T>
    {
        public T GetConfig();
    }
}