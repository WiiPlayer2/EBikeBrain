using FluentAssertions;

namespace BafangLib.Test;

[TestClass]
public class ChecksumTest
{
    [TestMethod]
    public void Calculate_WithEmptyBuffer_ThrowsError()
    {
        // Arrange
        byte[] buffer = [];
        var offset = 0;
        var length = 0;

        // Act
        var act = () => Checksum.Calculate(buffer, offset, length);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void Calculate_WithMultipleValues_ReturnsSum()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0x01, 0x02, 0x03, 0x04];
        var offset = 0;
        var length = 4;
        byte expectedValue = 0x01 + 0x02 + 0x03 + 0x04;

        // Act
        var result = Checksum.Calculate(buffer, offset, length);

        // Assert
        result.Should().Be(expectedValue);
    }

    [TestMethod]
    public void Calculate_WithMultipleValuesAndOverflow_ReturnsModSum()
    {
        // Arrange
        ReadOnlySpan<byte> buffer = [0xF1, 0xF2, 0xF3, 0xF4];
        var offset = 0;
        var length = 4;
        byte expectedValue = 0xCA;

        // Act
        var result = Checksum.Calculate(buffer, offset, length);

        // Assert
        result.Should().Be(expectedValue);
    }

    [TestMethod]
    public void Calculate_WithSingleValue_ReturnsValue()
    {
        // Arrange
        byte expectedValue = 0x01;
        ReadOnlySpan<byte> buffer = [expectedValue];
        var offset = 0;
        var length = 1;

        // Act
        var result = Checksum.Calculate(buffer, offset, length);

        // Assert
        result.Should().Be(expectedValue);
    }
}
