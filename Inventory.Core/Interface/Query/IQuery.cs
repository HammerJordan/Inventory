using System.Collections.Generic;

namespace Inventory.Core
{
    public interface IQuery<T>
    {
        public IEnumerable<T> LoadAll();
        public T Create();
        public void Update(T t);
        public void Delete(T t);

    }
}