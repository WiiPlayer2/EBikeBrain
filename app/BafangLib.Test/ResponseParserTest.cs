using BafangLib.Messages;
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
        var expectedResult = new ParseResult<GetBatteryResponse>(new GetBatteryResponse(0x66), 0, 1, 0x66);

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
        var expectedResult = new ParseResult<GetCurrentResponse>(new GetCurrentResponse(0x66 / 2m), 0, 1, 0x66);

        // Act
        var result = ResponseParser.ParseGetCurrentResponse(buffer, 0, 2);

        // Assert
        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public void ParseGetErrorResponse_WithErrorResponse_ReturnsErrorCode()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x01];
        var expectedResult = new ParseResult<GetErrorResponse>(new GetErrorResponse(ErrorCode.Ok), 0, 1);

        // Act
        var result = ResponseParser.ParseGetErrorResponse(buffer, 0, 1);

        // Assert
        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public void ParseGetRpmResponse_WithRpmResponse_ReturnsRpmValue()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x01, 0x66, 0x67];
        var expectedResult = new ParseResult<GetRpmResponse>(new GetRpmResponse(0x0166), 0, 2, 0x47);

        // Act
        var result = ResponseParser.ParseGetRpmResponse(buffer, 0, 3);

        // Assert
        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public void ParseUnknownResponse_WithUnknownResponseAndValidChecksum_ReturnsUnknownValue()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x01, 0x03, 0x03, 0x07];
        var expectedResult = new ParseResult<ReadOnlyMemory<byte>>(new byte[] {0x01, 0x03, 0x03}, 0, 3, 0x07);

        // Act
        var result = ResponseParser.ParseUnknownResponse(buffer, 0, 4);

        // Assert
        result.Should().BeEquivalentTo(
            expectedResult,
            options => options.Excluding(x => x.Value));
        result!.Value.ToArray().Should().Equal(
            expectedResult.Value.ToArray());
    }
}
