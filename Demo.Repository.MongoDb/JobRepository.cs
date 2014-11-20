using Demo.Domain;
using Demo.Infrastructure.DatabaseConnectionSettingsService;
using Demo.Repository.MongoDb.DatabaseObjects;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Repository.MongoDb
{
	public class JobRepository : MongoDbRepository, IJobRepository
	{
		public JobRepository(IDatabaseConnectionSettingsService databaseConnectionSettingsService)
			: base(databaseConnectionSettingsService)
		{}

		public Guid InsertNewHttpJob(HttpJob httpJob)
		{
			Guid correlationId = Guid.NewGuid();
			httpJob.CorrelationId = correlationId;
			DbHttpJob dbHttpJob = httpJob.ConvertToInsertDbObject();
			HttpJobs.Insert(dbHttpJob);
			return correlationId;
		}

		public HttpJob FindBy(Guid correlationId)
		{
			return FindInDb(correlationId);
		}

		public void Update(HttpJob httpJob)
		{
			DbHttpJob existing = FindInDb(httpJob.CorrelationId);
			existing.Finished = httpJob.Finished;
			existing.Started = httpJob.Started;
			existing.StatusMessage = httpJob.StatusMessage;
			existing.TotalJobDuration = httpJob.TotalJobDuration;
			existing.UrlsToRun = httpJob.UrlsToRun;
			HttpJobs.Save(existing);
		}

		public IEnumerable<HttpJob> GetUnhandledJobs()
		{
			IMongoQuery query = Query<DbHttpJob>.EQ(j => j.Started, false);
			return HttpJobs.Find(query);
		}

		private DbHttpJob FindInDb(Guid correlationId)
		{
			IMongoQuery query = Query<DbHttpJob>.EQ(j => j.CorrelationId, correlationId);
			DbHttpJob firstJob = HttpJobs.FindOne(query);
			return firstJob;
		}		
	}
}
