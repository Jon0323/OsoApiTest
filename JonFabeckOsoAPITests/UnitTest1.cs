using NUnit.Framework;
using RestSharp;
using System.Net;
using Ubiety.Dns.Core;

namespace JonFabeckOsoAPITests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        //In this section we are testing the Post
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

        //In this section we are testing the post api
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
        public void CheckPutInvalidID()
        {
            RestClient client = new RestClient("https://jsonplaceholder.typicode.com/posts/agh");
            RestRequest request = new RestRequest(Method.PUT);

            IRestResponse response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError), "We expected the api to return 500 Internal Server Error");
        }

        //This we are going to test the delete api, important note, I will be running this call like it's a real api, however since the backend is fake
        //Noting ever gets created or deleted, I will instead like I'm creating then deleting afterwards.
        [Test]
        public void CheckDelete()
        {
            RestClient client = new RestClient("https://jsonplaceholder.typicode.com/posts/9");
            RestRequest request = new RestRequest(Method.PUT);

            IRestResponse response = client.Execute(request);

            //Check if the first call worked
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "We expected the api to return 200 OK");

            //Now make the call to delte
            RestClient client2 = new RestClient("https://jsonplaceholder.typicode.com/posts/9");
            RestRequest request2 = new RestRequest(Method.DELETE);

            IRestResponse response2 = client2.Execute(request2);

            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.OK), "We expected the api to return 200 OK");
        }

        /*/[Test]
        public void CheckPostWithComments()
        {
            string apiBody = "[ { \"postId\": 9, \"id\": 41, \"name\": \"voluptas deleniti ut\", \"email\": \"Lucio @gladys.tv\", \"body\": \"facere repudiandae vitae ea aut sed quo ut et\nfacere nihil ut voluptates in\nsaepe cupiditate accusantium numquam dolores\ninventore sint mollitia provident\" }, { \"postId\": 9, \"id\": 42, \"name\": \"nam qui et\", \"email\": \"Shemar @ewell.name\", \"body\": \"aut culpa quaerat veritatis eos debitis\naut repellat eius explicabo et\nofficiis quo sint at magni ratione et iure\nincidunt quo sequi quia dolorum beatae qui\" }, { \"postId\": 9, \"id\": 43, \"name\": \"molestias sint est voluptatem modi\", \"email\": \"Jackeline @eva.tv\", \"body\": \"voluptatem ut possimus laborum quae ut commodi delectus\nin et consequatur\nin voluptas beatae molestiae\nest rerum laborum et et velit sint ipsum dolorem\" }, { \"postId\": 9, \"id\": 44, \"name\": \"hic molestiae et fuga ea maxime quod\", \"email\": \"Marianna_Wilkinson @rupert.io\", \"body\": \"qui sunt commodi\nsint vel optio vitae quis qui non distinctio\nid quasi modi dicta\neos nihil sit inventore est numquam officiis\" }, { \"postId\": 9, \"id\": 45, \"name\": \"autem illo facilis\", \"email\": \"Marcia @name.biz\", \"body\": \"ipsum odio harum voluptatem sunt cumque et dolores\nnihil laboriosam neque commodi qui est\nquos numquam voluptatum\ncorporis quo in vitae similique cumque tempore\" } ]";
            var jsonBoby = request.JsonSerializer.Serialize(apiBody);

            RestClient client = new RestClient("https://jsonplaceholder.typicode.com/posts/9/comments");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Content-type", "application/json");
            request.AddJsonBody(apiBody);

            IRestResponse response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "We expected the api to return 201 Created");
        }/*/
    }
}