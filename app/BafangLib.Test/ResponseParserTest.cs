using FluentAssertions;

namespace BafangLib.Test;

[TestClass]
public class ResponseParserTest
{
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
