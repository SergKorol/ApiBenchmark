using ApiBenchmark.Application.Common.Interfaces;
using ApiBenchmark.Application.Rates;
using Bogus;
using FluentAssertions;
using Moq;
using Xunit;

namespace ApiBenchmark.UnitTests;

public class RefitClientHandler
{
    private const decimal Amount = 10;
    private const string? SourceCurrency = "EUR";
    private const string? TargetCurrency = "GBP";

    [Fact]
    public async Task HandleRefitClientRequest_Success()
    {
        var refitClientAPIMock = new Mock<IForexApiRefit>();
        refitClientAPIMock.Setup(x => x.GetRates(SourceCurrency,TargetCurrency))
            .ReturnsAsync(0.8M);
        var handler = new AddRateRefitCommandHandler(refitClientAPIMock.Object);

        var command = new Faker<AddRateRefitCommand>()
            .RuleFor(x => x.Amount, Amount)
            .RuleFor(x => x.SourceCurrency, SourceCurrency)
            .RuleFor(x => x.TargetCurrency, TargetCurrency)
            .Generate();

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        refitClientAPIMock.Verify(x => x.GetRates(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        result.Amount.Should().Be(command.Amount);
        result.SourceCurrency.Should().Be(command.SourceCurrency);
        result.TargetCurrency.Should().Be(command.TargetCurrency);
        result.RateAmount.Should().Be(0.8M*command.Amount);
    }


    [Fact]
    public async Task HandleRefitClientRequest_IfRateNotExists()
    {
        var refitClientAPIMock = new Mock<IForexApiRefit>();
        var handler = new AddRateRefitCommandHandler(refitClientAPIMock.Object);

        var command = new Faker<AddRateRefitCommand>()
            .RuleFor(x => x.Amount, Amount)
            .RuleFor(x => x.SourceCurrency, SourceCurrency)
            .RuleFor(x => x.TargetCurrency, TargetCurrency)
            .Generate();

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        refitClientAPIMock.Verify(x => x.GetRates(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        result.Amount.Should().Be(command.Amount);
        result.SourceCurrency.Should().Be(command.SourceCurrency);
        result.TargetCurrency.Should().Be(command.TargetCurrency);
        result.RateAmount.Should().Be(default);
    }


    [Fact]
    public async Task HandleRefitClientRequest_IfCommandEmpty()
    {
        var refitClientAPIMock = new Mock<IForexApiRefit>();
        var handler = new AddRateRefitCommandHandler(refitClientAPIMock.Object);

        var command = new Faker<AddRateRefitCommand>()
            .Generate();

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        refitClientAPIMock.Verify(x => x.GetRates(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        result.Amount.Should().Be(default);
        result.SourceCurrency.Should().Be(null);
        result.TargetCurrency.Should().Be(null);
        result.RateAmount.Should().Be(default);
    }
}