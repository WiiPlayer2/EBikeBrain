using BafangLib.Messages;
using FluentAssertions;

namespace BafangLib.Test;

[TestClass]
public class RequestParserTest
{
    [TestMethod]
    public void Parse_WithEmptyBuffer_ReturnsNull()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [];

        // Act
        var result = RequestParser.Parse(buffer);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void Parse_WithExplicitBufferOffsetTooSmall_ReturnsNull()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x00, 0x00, 0x00, 0x00];
        var offset = buffer.Length;
        var length = buffer.Length - offset;

        // Act
        var result = RequestParser.Parse(buffer, offset, length);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void Parse_WithGetBatteryCommand_ReturnsGetRpmRequestAndEndOffset()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x11, 0x11];
        var expectedResult = new ParseResult<Request>(new GetBatteryRequest(), 0, 2);

        // Act
        var result = RequestParser.Parse(buffer);

        // Assert
        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public void Parse_WithGetCurrentCommand_ReturnsSetPasRequest()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x11, 0x0A];
        var expectedResult = new ParseResult<Request>(new GetCurrentRequest(), 0, 2);

        // Act
        var result = RequestParser.Parse(buffer);

        // Assert
        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public void Parse_WithGetErrorCommand_ReturnsGetErrorRequest()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x11, 0x08];
        var expectedResult = new ParseResult<Request>(new GetErrorRequest(), 0, 2);

        // Act
        var result = RequestParser.Parse(buffer);

        // Assert
        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public void Parse_WithGetRpmCommand_ReturnsGetRpmRequestAndEndOffset()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x11, 0x20];
        var expectedResult = new ParseResult<Request>(new GetRpmRequest(), 0, 2);

        // Act
        var result = RequestParser.Parse(buffer);

        // Assert
        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public void Parse_WithGetRpmCommandAndExplicitOffset_ReturnsGetRpmRequestAndEndOffset()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x00, 0x11, 0x20];
        var offset = 1;
        var length = buffer.Length - offset;
        var expectedResult = new ParseResult<Request>(new GetRpmRequest(), offset, 2);

        // Act
        var result = RequestParser.Parse(buffer, offset, length);

        // Assert
        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public void Parse_WithGetRpmCommandAndImplicitOffset_ReturnsGetRpmRequestAndEndOffset()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x00, 0x11, 0x20];
        var expectedResult = new ParseResult<Request>(new GetRpmRequest(), 1, 2);

        // Act
        var result = RequestParser.Parse(buffer);

        // Assert
        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public void Parse_WithSetLightsCommand_ReturnsSetLightsRequest()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x16, 0x1A, 0xF1];
        var expectedResult = new ParseResult<Request>(new SetLightsRequest(true), 0, 3);

        // Act
        var result = RequestParser.Parse(buffer);

        // Assert
        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public void Parse_WithSetMaxRpmCommand_ReturnsSetMaxRpmRequest()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x16, 0x1F, 0x00, 0x20, 0x55];
        var expectedResult = new ParseResult<Request>(new SetMaxRpmRequest(0x20), 0, 4, 0x55);

        // Act
        var result = RequestParser.Parse(buffer);

        // Assert
        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public void Parse_WithSetPasCommand_ReturnsSetPasRequest()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x16, 0x0B, 0x17, 0x38];
        var expectedResult = new ParseResult<Request>(new SetPasRequest(Pas.Level8), 0, 3, 0x38);

        // Act
        var result = RequestParser.Parse(buffer);

        // Assert
        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public void Parse_WithTooSmallBuffer_ReturnsNull()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x00];

        // Act
        var result = RequestParser.Parse(buffer);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void Parse_WithUnknownCommand_ReturnsNull()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x00, 0x00, 0x00, 0x00];

        // Act
        var result = RequestParser.Parse(buffer);

        // Assert
        result.Should().BeNull();
    }
}
