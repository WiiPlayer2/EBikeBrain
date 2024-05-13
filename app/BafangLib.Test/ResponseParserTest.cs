using FluentAssertions;

namespace BafangLib.Test;

[TestClass]
public class ResponseParserTest
{
    [TestMethod]
    public void ParseGetBatteryResponse_WithBatteryResponse_ReturnsBatteryValue()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x66, 0x66];
        var expectedResult = new ParseResult<byte>(0x66, 0, 1, 0x66);

        // Act
        var result = ResponseParser.ParseGetBatteryResponse(buffer, 0, 2);

        // Assert
        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public void ParseGetCurrentResponse_WithCurrentResponse_ReturnsCurrentValue()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x66, 0x66];
        var expectedResult = new ParseResult<decimal>(0x66 / 2m, 0, 1, 0x66);

        // Act
        var result = ResponseParser.ParseGetCurrentResponse(buffer, 0, 2);

        // Assert
        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public void ParseGetRpmResponse_WithRpmResponse_ReturnsRpmValue()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x01, 0x66, 0x67];
        var expectedResult = new ParseResult<ushort>(0x0166, 0, 2, 0x67);

        // Act
        var result = ResponseParser.ParseGetRpmResponse(buffer, 0, 3);

        // Assert
        result.Should().Be(expectedResult);
    }
}
