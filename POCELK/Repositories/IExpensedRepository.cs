using POCELK.Entities;

namespace POCELK.Repositories
{
    public interface IExpensedRepository
    {
        Task<List<Expense>> GetAllExpense();
    }
}
