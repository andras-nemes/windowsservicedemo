using Demo.Domain;
using Demo.Repository.MongoDb.DatabaseObjects;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Repository.MongoDb
{
	public static class ModelExtensions
	{
		public static DbHttpJob ConvertToInsertDbObject(this HttpJob domain)
		{
			return new DbHttpJob()
			{
				CorrelationId = domain.CorrelationId
				, Finished = domain.Finished
				, Started = domain.Started
				, StatusMessage = domain.StatusMessage
				, TotalJobDuration = domain.TotalJobDuration
				, UrlsToRun = domain.UrlsToRun
				, Id = ObjectId.GenerateNewId()
			};
		}		
	}
}
