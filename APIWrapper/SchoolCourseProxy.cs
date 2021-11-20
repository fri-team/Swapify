using FRITeam.Swapify.APIWrapper.Objects;
using FRITeam.Swapify.SwapifyBase.Settings.ProxySettings;
using Microsoft.Extensions.Options;
using NLog;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Threading.Tasks;

namespace FRITeam.Swapify.APIWrapper
{
    public class SchoolCourseProxy : ISchoolCourseProxy
    {        
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ProxySettings _proxySettings;
        private readonly RestClient _client;

        public SchoolCourseProxy(IOptions<ProxySettings> proxySettings)
        {
            _proxySettings = proxySettings.Value;
            _client = new RestClient(_proxySettings.Base_URL);
            _client.AddHandler("text/html", () => { return new JsonNetSerializer(); });
        }

        public async Task<UnizaCourseContentResult> GetByCourseName(string courseName)
        {
            var request = new RestRequest(_proxySettings.CourseContentURL);
            request.AddParameter("q", courseName);
            try
            {
                return await _client.GetAsync<UnizaCourseContentResult>(request);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);                
                throw;
            }                        
        }  
    }
}
