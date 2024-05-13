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
    public void Parse_WithGetRpmCommand_ReturnsGetRpmRequestAndEndOffset()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x11, 0x20];
        var expectedResult = new ParseResult<Request>(new GetRpmRequest(), 2);

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
        var expectedResult = new ParseResult<Request>(new GetRpmRequest(), offset + 2);

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
        var expectedResult = new ParseResult<Request>(new GetRpmRequest(), 3);

        // Act
        var result = RequestParser.Parse(buffer);

        // Assert
        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public void Parse_WithSetPasCommand_ReturnsSetPasRequest()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x16, 0x0B, 0x17];
        var expectedResult = new ParseResult<Request>(new SetPasRequest(Pas.Level8), 3);

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