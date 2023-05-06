using ApiBenchmark.Application.Common.Interfaces;
using ApiBenchmark.Application.Rates;
using Bogus;
using FluentAssertions;
using Moq;
using Xunit;

namespace ApiBenchmark.UnitTests;

public class RestsharpClientHandlerTests
{
    private const decimal Amount = 10;
    private const string? SourceCurrency = "EUR";
    private const string? TargetCurrency = "GBP";

    [Fact]
    public async void HandleRestsharpClientRequest_Success()
    {
        var restsharpClientApiMock = new Mock<IForexApiRestsharp>();
        restsharpClientApiMock.Setup(x => x.GetRates(SourceCurrency,TargetCurrency))
            .ReturnsAsync(0.8M);
        var handler = new AddRateRestsharpCommandHandler(restsharpClientApiMock.Object);

        var command = new Faker<AddRateRestsharpCommand>()
            .RuleFor(x => x.Amount, Amount)
            .RuleFor(x => x.SourceCurrency, SourceCurrency)
            .RuleFor(x => x.TargetCurrency, TargetCurrency)
            .Generate();

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        restsharpClientApiMock.Verify(x => x.GetRates(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        result.Amount.Should().Be(command.Amount);
        result.SourceCurrency.Should().Be(command.SourceCurrency);
        result.TargetCurrency.Should().Be(command.TargetCurrency);
        result.RateAmount.Should().Be(0.8M*command.Amount);
    }

    [Fact]
    public async Task HandleRestsharpClientRequest_IfRateNotExists()
    {
        var restsharpClientApiMock = new Mock<IForexApiRestsharp>();
        var handler = new AddRateRestsharpCommandHandler(restsharpClientApiMock.Object);

        var command = new Faker<AddRateRestsharpCommand>()
            .RuleFor(x => x.Amount, Amount)
            .RuleFor(x => x.SourceCurrency, SourceCurrency)
            .RuleFor(x => x.TargetCurrency, TargetCurrency)
            .Generate();

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        restsharpClientApiMock.Verify(x => x.GetRates(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        result.Amount.Should().Be(command.Amount);
        result.SourceCurrency.Should().Be(command.SourceCurrency);
        result.TargetCurrency.Should().Be(command.TargetCurrency);
        result.RateAmount.Should().Be(default);
    }

    [Fact]
    public async Task HandleRestsharptClientRequest_IfCommandEmpty()
    {
        var restsharpClientApiMock = new Mock<IForexApiRestsharp>();
        var handler = new AddRateRestsharpCommandHandler(restsharpClientApiMock.Object);

        var command = new Faker<AddRateRestsharpCommand>()
            .Generate();

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        restsharpClientApiMock.Verify(x => x.GetRates(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        result.Amount.Should().Be(default);
        result.SourceCurrency.Should().Be(null);
        result.TargetCurrency.Should().Be(null);
        result.RateAmount.Should().Be(default);
    }
}