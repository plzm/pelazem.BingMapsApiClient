using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BingMapsRESTToolkit;

namespace pelazem.BingMapsApiClient
{
	public class ApiClient
	{
		private string ApiKey { get; set; }

		private ApiClient() { }

		public ApiClient(string apiKey)
		{
			this.ApiKey = apiKey;
		}

		public async Task<IEnumerable<BingMapsApiClient.Location>> GetLocationsAsync(string addressText, string culture = "en-US")
		{
			List<BingMapsApiClient.Location> result = new List<BingMapsApiClient.Location>();

			var request = new GeocodeRequest();

			request.BingMapsKey = this.ApiKey;
			request.Culture = culture;
			request.IncludeIso2 = true;
			request.IncludeNeighborhood = true;
			request.UserIp = "127.0.0.1"; // Hardcode this to prevent attempts by API to do stuff near requester instead of address string

			request.Query = addressText;

			Response response = null;

			try
			{
				response = await request.Execute();
			}
			catch (Exception ex)
			{
				// TODO log exception

				response = null;
			}

			if
			(
				response != null &&
				response.ResourceSets != null &&
				response.ResourceSets.Length > 0 &&
				response.ResourceSets[0].Resources != null &&
				response.ResourceSets[0].Resources.Length > 0
			)
			{
				foreach (var resourceSet in response.ResourceSets)
				{
					foreach (var resource in resourceSet.Resources)
					{
						var bingResult = resource as BingMapsRESTToolkit.Location;

						if (bingResult != null)
							result.Add(GetLocation(culture, addressText, bingResult));
					}
				}
			}

			return result;
		}

		private BingMapsApiClient.Location GetLocation(string culture, string rawAddress, BingMapsRESTToolkit.Location bingResult)
		{
			BingMapsApiClient.Location result = new BingMapsApiClient.Location();

			result.Culture = culture;
			result.RawAddress = rawAddress;

			result.BoundingBox = bingResult.BoundingBox;
			result.Confidence = bingResult.Confidence;

			Coordinate coordinate = bingResult.Point.GetCoordinate();
			result.Latitude = coordinate.Latitude;
			result.Longitude = coordinate.Longitude;

			result.Address = new BingMapsApiClient.Address();
			result.Address.AddressLine = bingResult.Address.AddressLine;
			result.Address.AdminDistrict = bingResult.Address.AdminDistrict;
			result.Address.AdminDistrict2 = bingResult.Address.AdminDistrict2;
			result.Address.CountryRegion = bingResult.Address.CountryRegion;
			result.Address.CountryRegionIso2 = bingResult.Address.CountryRegionIso2;
			result.Address.FormattedAddress = bingResult.Address.FormattedAddress;
			result.Address.Landmark = bingResult.Address.Landmark;
			result.Address.Locality = bingResult.Address.Locality;
			result.Address.Neighborhood = bingResult.Address.Neighborhood;
			result.Address.PostalCode = bingResult.Address.PostalCode;

			return result;
		}
	}
}
