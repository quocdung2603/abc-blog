using AbcBlog.Core.Domain.Royalty;
using AbcBlog.Core.Models;
using AbcBlog.Core.Models.Royalty;
using AbcBlog.Core.SeedWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcBlog.Core.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction, Guid>
    {
        Task<PageResult<TransactionDto>> GetTransactionPaging(string? userName, int fromMonth, int fromYear, int toMonth, int toYear, int pageIndex = 1, int pageSize = 10);
    }
}
