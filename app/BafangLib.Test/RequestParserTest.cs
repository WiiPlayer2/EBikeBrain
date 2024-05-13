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
    public void Parse_WithGetRpmCommand_ReturnsGetRpmRequestAndConsumeAmount()
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
