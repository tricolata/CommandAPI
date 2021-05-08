using System;
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
        public int CreateCommand(Command cmd)
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));
            string sql = @"INSERT INTO CommandItems (Howto, Platform, commandLine)
            VALUES (@howto, @platform, @commandLine); SELECT LAST_INSERT_ID();";
            int id = _dbConnection.ExecuteScalar<int>(sql, new {howto = cmd.HowTo, platform=cmd.Platform, commandLine=cmd.CommandLine});
            return id;
        }
        public void UpdateCommand(Command cmd)
        {
            string sql = @"UPDATE CommandItems SET Howto=@howto, Platform=@platform, CommandLine=@commandLine WHERE id=@id";
            _dbConnection.Execute(sql, new {id = cmd.Id, howto = cmd.HowTo, platform = cmd.Platform, commandLine = cmd.CommandLine});
        }
        public void DeleteCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }
        public bool SaveChanges()
        {
            throw new System.NotImplementedException();
        }
    }
}