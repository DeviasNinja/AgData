using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace AgData {
    public class TestBase {
        protected readonly HttpClient _httpClient;

        public TestBase() {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://jsonplaceholder.typicode.com/") };
        }

    }

    public class GetPostsTests : TestBase {
        [Fact]
        public async Task GetPosts_ShouldReturnSuccess() {
            //Act
            var response = await _httpClient.GetAsync("posts");

            // Assert
            response.EnsureSuccessStatusCode(); //Happy path: 200 OK
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("GET /posts Response: " + content);
            Assert.NotEmpty(content); //Ensure content is returned
        }

        [Theory]
        [InlineData(101)] // Non-existent postId for negative test case
        public async Task GetPost_WithInvalidId_ShouldReturnNotFound(int postId) {
            //Act
            var response = await _httpClient.GetAsync($"posts/{postId}");

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode); //Negative path: 404 Not Found
            Console.WriteLine($"GET /posts/{postId} returned 404 as expected.");
        }
    }

    public class CreatePostTests : TestBase {
        [Fact]
        public async Task CreatePost_ShouldReturnCreated() {
            //References
            var newPost = new { title = "foo", body = "bar", userId = 1 };
            var content = new StringContent(JsonConvert.SerializeObject(newPost), System.Text.Encoding.UTF8, "application/json");

            //Act
            var response = await _httpClient.PostAsync("posts", content);

            //Assert
            response.EnsureSuccessStatusCode(); //Happy path: 201 Created
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("POST /posts Response: " + responseBody);
            Assert.Contains("foo", responseBody);
        }

        [Fact]
        public async Task CreatePost_MissingTitle_ShouldReturnBadRequest() {
            //References
            var newPost = new { body = "bar", userId = 1 };
            var content = new StringContent(JsonConvert.SerializeObject(newPost), System.Text.Encoding.UTF8, "application/json");

            //Act
            var response = await _httpClient.PostAsync("posts", content);

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode); //Negative path: Missing title
            Console.WriteLine("POST /posts returned BadRequest as expected.");
        }
    }

    public class UpdatePostTests : TestBase {
        [Fact]
        public async Task UpdatePost_ShouldReturnSuccess() {
            //References
            var updatedPost = new { id = 1, title = "updated title", body = "updated body", userId = 1 };
            var content = new StringContent(JsonConvert.SerializeObject(updatedPost), System.Text.Encoding.UTF8, "application/json");

            //Act
            var response = await _httpClient.PutAsync("posts/1", content);

            //Assert
            response.EnsureSuccessStatusCode(); //Happy path: 200 OK
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("PUT /posts/1 Response: " + responseBody);
            Assert.Contains("updated title", responseBody);
        }

        [Fact]
        public async Task UpdatePost_InvalidId_ShouldReturnNotFound() {
            //Arrange
            var updatedPost = new { id = 102, title = "updated title", body = "updated body", userId = 1 };
            var content = new StringContent(JsonConvert.SerializeObject(updatedPost), System.Text.Encoding.UTF8, "application/json");

            //Act
            var response = await _httpClient.PutAsync("posts/101", content);

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode); //Negative path: Invalid postId
            Console.WriteLine("PUT /posts/101 returned NotFound as expected.");
        }
    }

    public class DeletePostTests : TestBase {
        [Fact]
        public async Task DeletePost_ShouldReturnSuccess() {
            //References
            var response = await _httpClient.DeleteAsync("posts/1");

            // Assert
            response.EnsureSuccessStatusCode(); //Happy path: 200 OK
            Console.WriteLine("DELETE /posts/1 returned 200 OK.");
        }

        [Fact]
        public async Task DeletePost_InvalidId_ShouldReturnNotFound() {
            //Act
            var response = await _httpClient.DeleteAsync("posts/102");

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode); //Negative path: Invalid postId
            Console.WriteLine("DELETE /posts/101 returned NotFound as expected.");
        }
    }

    public class CreateCommentTests : TestBase {
        [Fact]
        public async Task CreateComment_ShouldReturnCreated() {
            //References
            var newComment = new { name = "foo", body = "bar", email = "foo@bar.com", postId = 1 };
            var content = new StringContent(JsonConvert.SerializeObject(newComment), System.Text.Encoding.UTF8, "application/json");

            //Act
            var response = await _httpClient.PostAsync("posts/1/comments", content);

            //Assert
            response.EnsureSuccessStatusCode(); //Happy path: 201 Created
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("POST /posts/1/comments Response: " + responseBody);
            Assert.Contains("foo", responseBody);
        }

        [Fact]
        public async Task CreateComment_MissingEmail_ShouldReturnBadRequest() {
            //References
            var newComment = new { name = "foo", body = "bar", postId = 1 };
            var content = new StringContent(JsonConvert.SerializeObject(newComment), System.Text.Encoding.UTF8, "application/json");

            //Act
            var response = await _httpClient.PostAsync("posts/1/comments", content);

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode); //Negative path: Missing email
            Console.WriteLine("POST /posts/1/comments returned BadRequest as expected.");
        }
    }

    public class GetCommentsTests : TestBase {
        [Theory]
        [InlineData(1)]
        public async Task GetComments_ShouldReturnSuccess(int postId) {
            //Act
            var response = await _httpClient.GetAsync($"comments?postId={postId}");

            //Assert
            response.EnsureSuccessStatusCode(); //Happy path: 200 OK
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"GET /comments?postId={postId} Response: " + content);
            Assert.NotEmpty(content);
        }

        [Theory]
        [InlineData(102)] //Invalid postId
        public async Task GetComments_InvalidPostId_ShouldReturnEmpty(int postId) {
            //Act
            var response = await _httpClient.GetAsync($"comments?postId={postId}");

            //Assert
            response.EnsureSuccessStatusCode(); //Happy path but no content
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"GET /comments?postId={postId} Response: " + content);
            Assert.Equal("[]", content); //Expecting empty result
        }
    }
}
