using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestGithubApi
{
    public class Github_Tests
    {
        private RestClient client;
        private RestRequest request;

        public object DataTime { get; private set; }

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient("https://api.github.com");
            client.Authenticator = new HttpBasicAuthenticator("{YOUR USER NAME}", "{YOUR TOKEN}");

            string url = "/repos/BorislavaGrigorova/Postman/issues";
            this.request = new RestRequest(url);
        }

    
        [Test]
        public void GET_Issue_GitHub()
        {
            var response = client.Execute(request);
            var issues = JsonSerializer.Deserialize<List<Issues>>(response.Content);

            foreach( var issue in issues)
            {
                Assert.IsNotNull(issue.html_url);
                Assert.IsNotNull(issue.id, "Issue id must not be null");
                Assert.IsNotEmpty(issue.title);
            }
            Assert.IsNotNull(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
        [Test]
        public void POST_Issue_GitHub()
        {
            var issue = new
            {
                title = "Missing form" + DateTime.Now.Ticks,
                id = 123 + DateTime.Now.Ticks,
                number = 123 + DateTime.Now.Ticks,
            };
            request.AddJsonBody(issue);

            var response = client.Execute(request, Method.Post);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            
                Assert.Greater(issue.number, 0);
                Assert.Greater(issue.id, 0);
                Assert.IsNotEmpty(issue.title);
            



        }
    }
}
