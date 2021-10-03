namespace Application.Common.Interfaces
{
    public interface IQuery<T>
    {
        T Get(int id);
        T Create(T model);
        void Update(T model);
        void Delete(T model);
    }
}