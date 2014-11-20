using Demo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Demo.Repository.MongoDb.DatabaseObjects
{
	public class DbHttpJob : HttpJob
	{
		public ObjectId Id { get; set; }
	}
}
