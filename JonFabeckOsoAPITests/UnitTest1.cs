using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Net;

namespace JonFabeckOsoAPITests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        //This test checks the happy path for the GET
        [Test]
        public void CheckIfOk()
        {
            RestClient client = new RestClient("https://jsonplaceholder.typicode.com/posts");
            RestRequest request = new RestRequest(Method.GET);

            IRestResponse response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),"We expected the api to return 200 OK");
        }

        [Test]
        public void CheckIfNotFound()
        {
            RestClient client = new RestClient("https://jsonplaceholder.typicode.com/post");
            RestRequest request = new RestRequest(Method.GET);

            IRestResponse response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound),"We expected a 404 not found after mispelling the api call");
        }

        [Test]
        public void CheckWrongMethod()
        {
            RestClient client = new RestClient("https://jsonplaceholder.typicode.com/posts");
            RestRequest request = new RestRequest(Method.PATCH);

            IRestResponse response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), "We expected a 404 not found after using the wrong rquest type");
        }

        //In this section we are testing POST
        [Test]
        public void CheckIfPOstOk()
        {
            RestClient client = new RestClient("https://jsonplaceholder.typicode.com/posts");
            RestRequest request = new RestRequest(Method.POST);

            IRestResponse response = client.Execute(request);

            //Here we check the status code and verify the content returned is correct
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "We expected the api to return 201 Created");
            Assert.That(response.Content, Is.EqualTo("{\n  \"id\": 101\n}"), "The content returned was incorrect");
        }

        //In this section we are testing the put call api
        [Test]
        public void CheckIfPutOk()
        {
            RestClient client = new RestClient("https://jsonplaceholder.typicode.com/posts/7");
            RestRequest request = new RestRequest(Method.PUT);

            IRestResponse response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "We expected the api to return 200 OK");
            Assert.That(response.Content, Is.EqualTo("{\n  \"id\": 7\n}"), "The content returned was incorrect");
        }

        [Test]
        public void CheckPutInvalidId()
        {
            RestClient client = new RestClient("https://jsonplaceholder.typicode.com/posts/agh");
            RestRequest request = new RestRequest(Method.PUT);

            IRestResponse response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError), "We expected the api to return 500 Internal Server Error");
        }

        //This we are going to test the delete api, important note, I will be running this call like it's a real api, however since the backend is fake
        //Nothing ever gets created or deleted, this will act like I'm creating and then deleting afterwards.
        [Test]
        public void CheckDelete()
        {
            RestClient client = new RestClient("https://jsonplaceholder.typicode.com/posts/9");
            RestRequest request = new RestRequest(Method.PUT);

            IRestResponse response = client.Execute(request);

            //Check if the first call worked
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "We expected the api to return 200 OK");

            //Now make the call to delete
            RestClient client2 = new RestClient("https://jsonplaceholder.typicode.com/posts/9");
            RestRequest request2 = new RestRequest(Method.DELETE);

            IRestResponse response2 = client2.Execute(request2);

            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.OK), "We expected the api to return 200 OK");
        }

        //Check the post with comments and make sure the json body works
        [Test]
        public void CheckPostWithComments()
        {
            RestClient client = new RestClient("https://jsonplaceholder.typicode.com/posts/9/comments");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Content-type", "application/json");
            request.AddJsonBody(new
            {
                id = 1,
                title = "foo",
                body = "bar",
                userId = 1
            });

            IRestResponse response = client.Execute(request);

            //parse the response to test it is correct
            string jsonResponse = response.Content;
            JObject jObject = JObject.Parse(jsonResponse);
            JToken jId = jObject["id"];
            JToken jTitle = jObject["title"];
            JToken jBody = jObject["body"];
            JToken jUserId = jObject["userId"];
            JToken jPostId = jObject["postId"];

            //Check the Status code
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "We expected the api to return 201 Created");
            //Check the return code
            Assert.That(jId.ToString(), Is.EqualTo("501"), "We expected the ID to be 501");
            Assert.That(jTitle.ToString(), Is.EqualTo("foo"), "We expected the Title to be foo");
            Assert.That(jBody.ToString(), Is.EqualTo("bar"), "We expected the Body to be bar");
            Assert.That(jUserId.ToString(), Is.EqualTo("1"), "We expected the UserId to be 1");
            Assert.That(jPostId.ToString(), Is.EqualTo("9"), "We expected the PostId to be 9");
        }

        //Test the get call with comments
        [Test]
        public void CheckGetPostComment()
        {
            RestClient client = new RestClient("https://jsonplaceholder.typicode.com/comments?postId=12");
            RestRequest request = new RestRequest(Method.GET);

            IRestResponse response = client.Execute(request);

            //parse the response to test it is correct
            string jsonResponse = response.Content;
            //I just want to check one response, so I'm trimming off the rest 
            jsonResponse = jsonResponse.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' });
            jsonResponse = jsonResponse.Substring(0, jsonResponse.IndexOf("}") + 1);
            Console.WriteLine(jsonResponse);
            JObject jObject = JObject.Parse(jsonResponse);
            JToken jPostId = jObject["postId"];

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "We expected a 200 ok");
            //We just want to make sure one of the post ID is equal to 12, the one we called
            Assert.That(jPostId.ToString(), Is.EqualTo("12"), "We expected the PostId to be 12");
        }
    }
}