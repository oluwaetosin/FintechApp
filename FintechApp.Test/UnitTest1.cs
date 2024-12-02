using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FintechApp.Test;

public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]

    public async Task  ProcessResult_ReturnOkResult(){

        HttpClient client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/transaction/process", new TransactionRequest("376100000000004", "02/03", 200, "1234"));

        response.EnsureSuccessStatusCode();

    }

        [Fact]

    public async Task  ProcessResult_ReturnBadRequestResult(){

        HttpClient client = _factory.CreateClient();

    

        var response = await client.PostAsJsonAsync("/transaction/process", new TransactionRequest("123412341234", "02/01/2024", 12, "123423"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        
        Assert.NotNull(problemDetails);
        Assert.Contains("CardPAN", problemDetails.Errors);
        Assert.Contains("ExpiryDate", problemDetails.Errors);
        Assert.Contains("PIN", problemDetails.Errors);
      
    }
}