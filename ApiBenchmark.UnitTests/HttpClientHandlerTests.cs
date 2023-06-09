using ApiBenchmark.Application.Common.Interfaces;
using ApiBenchmark.Application.Rates;
using Bogus;
using FluentAssertions;
using Moq;
using Xunit;

namespace ApiBenchmark.UnitTests;

public class HttpClientHandlerTests
{
    private const decimal Amount = 10;
    private const string? SourceCurrency = "EUR";
    private const string? TargetCurrency = "GBP";
    
    [Fact]
    public async void HandleHttpClientRequest_Success()
    {
        var httpClientApiMock = new Mock<IForexApiHttpClient>();
        httpClientApiMock.Setup(x => x.GetRates(SourceCurrency,TargetCurrency))
            .ReturnsAsync(0.8M);
        var handler = new AddRateHttpClientCommandHandler(httpClientApiMock.Object);

        var command = new Faker<AddRateHttpClientCommand>()
            .RuleFor(x => x.Amount, Amount)
            .RuleFor(x => x.SourceCurrency, SourceCurrency)
            .RuleFor(x => x.TargetCurrency, TargetCurrency)
            .Generate();

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        httpClientApiMock.Verify(x => x.GetRates(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        result.Amount.Should().Be(command.Amount);
        result.SourceCurrency.Should().Be(command.SourceCurrency);
        result.TargetCurrency.Should().Be(command.TargetCurrency);
        result.RateAmount.Should().Be(0.8M*command.Amount);
    }
    
    [Fact]
    public async void HandleHttpClientRequest_IfRateNotExists()
    {
        var httpClientApiMock = new Mock<IForexApiHttpClient>();
        var handler = new AddRateHttpClientCommandHandler(httpClientApiMock.Object);

        var command = new Faker<AddRateHttpClientCommand>()
            .RuleFor(x => x.Amount, Amount)
            .RuleFor(x => x.SourceCurrency, SourceCurrency)
            .RuleFor(x => x.TargetCurrency, TargetCurrency)
            .Generate();

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        httpClientApiMock.Verify(x => x.GetRates(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        result.Amount.Should().Be(command.Amount);
        result.SourceCurrency.Should().Be(command.SourceCurrency);
        result.TargetCurrency.Should().Be(command.TargetCurrency);
        result.RateAmount.Should().Be(default);
    }
    
    [Fact]
    public async void HandleHttpClientRequest_IfCommandEmpty()
    {
        var httpClientApiMock = new Mock<IForexApiHttpClient>();
        var handler = new AddRateHttpClientCommandHandler(httpClientApiMock.Object);

        var command = new Faker<AddRateHttpClientCommand>()
            .Generate();

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        httpClientApiMock.Verify(x => x.GetRates(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        result.Amount.Should().Be(default);
        result.SourceCurrency.Should().Be(null);
        result.TargetCurrency.Should().Be(null);
        result.RateAmount.Should().Be(default);
    }
}