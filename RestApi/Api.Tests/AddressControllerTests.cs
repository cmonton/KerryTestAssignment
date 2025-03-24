using Api.AddressApiClient;
using Api.AddressApiClient.Models;
using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using NSubstitute;

namespace Api.Tests;

public class AddressControllerTests
{
    private readonly IRequestAdapter _adapter;
    private readonly AddressController _controller;

    public AddressControllerTests()
    {
        _adapter = Substitute.For<IRequestAdapter>();
        _adapter.SerializationWriterFactory.GetSerializationWriter("application/json")
            .Returns(sw => new JsonSerializationWriter());
        _controller = new AddressController(new AddressClient(_adapter));
    }

    [Fact]
    public async Task GetAddresses_ReturnsOkResult_WithAddresses()
    {
        // Arrange
        var addresses = new List<Address>
        {
            new Address { Id = 1, Name = "Test1", City = "City1", Country = "Country1", Email = "test1@test.com" },
            new Address { Id = 2, Name = "Test2", City = "City2", Country = "Country2", Email = "test2@test.com" }
        };

        // Return the mocked list of addresses when the client is called
        _adapter.SendCollectionAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Address>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(addresses);

        // Act
        var result = await _controller.GetAddresses();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedAddresses = Assert.IsAssignableFrom<IEnumerable<Address>>(okResult.Value);
        Assert.Equal(2, returnedAddresses.Count());
    }

    [Fact]
    public async Task GetAddress_ReturnsOkResult_WhenAddressExists()
    {
        // Arrange
        var address = new Address { Id = 1, Name = "Test", City = "City", Country = "Country", Email = "test@test.com" };
        
        // Return the mocked address #1 object if the path
        // contains the ID of address #1
        _adapter.SendAsync(
            Arg.Is<RequestInformation>(req => req.PathParameters.Values.Contains(1)),
            Arg.Any<ParsableFactory<Address>>(),
            Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
            Arg.Any<CancellationToken>())
            .Returns(address);

        // Act
        var result = await _controller.GetAddress(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedAddress = Assert.IsType<Address>(okResult.Value);
        Assert.Equal(1, returnedAddress.Id);
    }

    [Fact]
    public async Task UpdateAddress_ReturnsNoContent_WhenSuccessful()
    {
        // Arrange
        var requestAddress = new RequestAddress { Name = "Test", City = "City", Country = "Country", Email = "test@test.com" };

        // Act
        var result = await _controller.UpdateAddress(1, requestAddress);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteAddress_ReturnsNoContent_WhenAddressExists()
    { 
        // Act
        var result = await _controller.DeleteAddress(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
