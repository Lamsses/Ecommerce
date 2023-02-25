﻿namespace EcommerceLibrary.DataAccess
{
    public interface ISqlDataAccess
    {
        Task<List<T>> Loaddata<T, U>(string storedProcedure, U parameters, string connectionStringName);
        Task<List<T>> Loaddata<T>(string storedProcedure, string connectionStringName);
        Task SaveData<T>(string storedProcedure, T parameters, string connectionStringName);
    }
}