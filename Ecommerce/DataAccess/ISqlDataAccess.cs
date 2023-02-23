namespace Ecommerce.DataAccess
{
    public interface ISqlDataAccess
    {
        Task<List<T>> Loaddata<T>(string storedProceduere, string connectionStringName);
        Task<List<T>> Loaddata<T, U>(string storedProceduere, U parmters, string connectionStringName);
        Task SaveData<T>(string storedProcedure, T parmters, string connectionStringName);
    }
}