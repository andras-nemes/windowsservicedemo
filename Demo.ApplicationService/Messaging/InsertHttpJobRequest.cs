using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ApplicationService.Messaging
{
	public class InsertHttpJobRequest
	{
		private List<Uri> _urisToRun;

		public InsertHttpJobRequest(List<Uri> urisToRun)
		{
			if (urisToRun == null || !urisToRun.Any())
			{
				throw new ArgumentException("The URI list cannot be empty.");				
			}
			_urisToRun = urisToRun;
		}

		public List<Uri> UrisToRun
		{
			get
			{
				return _urisToRun;
			}
		}
	}
}
