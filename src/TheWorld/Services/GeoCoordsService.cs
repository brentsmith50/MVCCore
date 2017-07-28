using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TheWorld.Services
{
    public class GeoCoordsService
    {
        #region Initialize
        private IConfigurationRoot configuration;
        private ILogger<GeoCoordsService> logger;

        public GeoCoordsService(IConfigurationRoot configuration, ILogger<GeoCoordsService> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }
        #endregion

        #region Methods
        public async Task<GeoCoordsResult> GetGeoCoordsAsync(string name)
        {
            var result = new GeoCoordsResult
            {
                Success = false,
                Message = "Unable to get coordinates"
            };

            var apiKey = this.configuration["Keys:BingKey"];
            var encodedName = WebUtility.UrlEncode(name);
            var url = $"http://dev.virtualearth.net/REST/v1/Locations?q={encodedName}&key={apiKey}";

            // I think this should be wrpped in a using
            HttpClient client = new HttpClient();
            string resultString = await client.GetStringAsync(url);
            var json = JObject.Parse(resultString);
            var resources = json["resourceSets"][0]["resources"];
            if (resources.HasValues)
            {
                string confidence = (string)resources[0]["confidence"];
                if (confidence != "High")
                {
                    result.Message = $"Could not find a confident match for '{name}' as a location";
                }
                else
                {
                    var coords = resources[0]["geocodePoints"][0]["coordinates"];
                    result.Latitude = (double)coords[0];
                    result.Longitude = (double)coords[1];
                    result.Success = true;
                    result.Message = "Sucessful match for location";
                }
            }
            else
            {
                result.Message = $"Could not find '{name}' as a location.";
            }

            return result;
        }
        #endregion
    }
}

