using Demo.Infrastructure.DatabaseConnectionSettingsService;
using Demo.Repository.MongoDb.DatabaseObjects;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Repository.MongoDb
{
	public abstract class MongoDbRepository
	{
		private readonly IDatabaseConnectionSettingsService _databaseConnectionSettingsService;
		private MongoClient _mongoClient;
		private MongoServer _mongoServer;
		private MongoDatabase _mongoDatabase;

		public MongoDbRepository(IDatabaseConnectionSettingsService databaseConnectionSettingsService)
		{
			if (databaseConnectionSettingsService == null) throw new ArgumentNullException();
			_databaseConnectionSettingsService = databaseConnectionSettingsService;
			_mongoClient = new MongoClient(_databaseConnectionSettingsService.GetDatabaseConnectionString());
			_mongoServer = _mongoClient.GetServer();
			_mongoDatabase = _mongoServer.GetDatabase(_databaseConnectionSettingsService.GetDatabaseName());
		}

		public MongoDatabase HttpJobsDatabase
		{
			get
			{
				return _mongoDatabase;
			}
		}

		public MongoCollection<DbHttpJob> HttpJobs
		{
			get
			{
				return HttpJobsDatabase.GetCollection<DbHttpJob>("httpjobs");
			}
		}
	}
}
