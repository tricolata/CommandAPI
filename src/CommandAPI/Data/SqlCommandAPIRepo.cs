using System.Collections.Generic;
using System.Data;
using CommandAPI.Models;
using Dapper;

namespace CommandAPI.Data
{
    public class SqlCommandAPIRepo : ICommandAPIRepo
    {
        private readonly IDbConnection _dbConnection;
        public SqlCommandAPIRepo(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void CreateCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Command> GetAllCommands()
        {
            const string sql = @"SELECT * FROM CommandItems";
            return _dbConnection.Query<Command>(sql);
        }

        public Command GetCommandById(int id)
        {
            const string sql = @"SELECT * FROM CommandItems WHERE id = @id";
            return _dbConnection.QuerySingleOrDefault<Command>(sql, new {id = id});
        }

        public bool SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }
    }
}