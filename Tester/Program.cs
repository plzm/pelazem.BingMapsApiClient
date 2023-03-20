using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pelazem.BingMapsApiClient;

namespace Tester
{
	class Program
	{
		static void Main(string[] args)
		{
			DoIt().Wait();
		}

		static async Task DoIt()
		{
			string apiKey = ""; // Get at https://www.bingmapsportal.com/
			string addressText = "";

			ApiClient apiClient = new ApiClient(apiKey);

			List<Location> locations = (await apiClient.GetLocationsAsync(addressText)).ToList();
		}
	}
}
