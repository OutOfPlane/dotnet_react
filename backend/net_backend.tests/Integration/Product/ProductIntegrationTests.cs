using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using CustomAttributes;
using CustomExtensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;

public class ProductApiIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductApiIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsSuccessAndContent()
    {
        // Given
        var response = await _client.GetAsync("/products");
        // When
        response.EnsureSuccessStatusCode();
        // Then
        var val = await response.Content.ReadFromJsonAsync<PagedResult<Product>>();

        if (val == null)
        {
            throw new Exception("Error Deserializing Object");
        }

        if (val.TotalCount == 0)
        {
            throw new Exception("Endpoint did not return Data");
        }

        var html = await response.Content.ReadAsStringAsync();
        Console.WriteLine(html);
    }

    [Fact]
    public async Task Post_ReturnsSuccessAndContent()
    {
        // Given
        var filter = new FilterRule()
        {
            Field = "Price",
            Op = "<",
            Value = "200",
            Sub = []
        };
        var json = JsonSerializer.Serialize(filter);
        Console.WriteLine(json);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/products/filter", content);
        // When
        response.EnsureSuccessStatusCode();
        // Then
        var val = await response.Content.ReadFromJsonAsync<PagedResult<Product>>();

        if (val == null)
        {
            throw new Exception("Error Deserializing Object");
        }

        if (val.TotalCount == 0)
        {
            throw new Exception("Endpoint did not return Data");
        }

        var html = await response.Content.ReadAsStringAsync();
        Console.WriteLine(html);
    }
}