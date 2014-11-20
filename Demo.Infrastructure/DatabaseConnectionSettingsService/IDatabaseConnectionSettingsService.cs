using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.DatabaseConnectionSettingsService
{
	public interface IDatabaseConnectionSettingsService
	{
		string GetDatabaseConnectionString();
		string GetDatabaseName();
	}
}
