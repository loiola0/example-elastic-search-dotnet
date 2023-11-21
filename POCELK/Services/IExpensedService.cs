namespace POCELK.Services
{
    public interface IExpensedService
    {
        Task SyncDbInformationsWithElasticSearch();
    }
}
