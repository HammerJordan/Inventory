using System;
using System.Threading.Tasks;

namespace Application.WPF.WebScraping.Common
{
    public interface IProductUpdateRunner
    {
        public Task RunProductUpdateAsync(Action callBack = null);

        public string LastItemScraped { get; }
        public int NumberOfItemsScraped { get; }
        public float PercentDone { get; }
        public bool IsDone { get; }
    }
}