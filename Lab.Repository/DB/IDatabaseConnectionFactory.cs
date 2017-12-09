using System;
using System.Data;

namespace Lab.Repository.DB
{
    public interface IDatabaseConnectionFactory
    {
        IDbConnection Create();
    }
}