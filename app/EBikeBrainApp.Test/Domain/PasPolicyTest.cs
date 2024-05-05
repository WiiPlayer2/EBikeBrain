using FluentAssertions;

namespace EBikeBrainApp.Test;

[TestClass]
public class PasPolicyTest
{
    [DataTestMethod]
    [DataRow(PasLevel.Level1, false)]
    [DataRow(PasLevel.Level2, true)]
    [DataRow(PasLevel.Level3, true)]
    [DataRow(PasLevel.Level4, true)]
    [DataRow(PasLevel.Level5, true)]
    [DataRow(PasLevel.Level6, true)]
    [DataRow(PasLevel.Level7, true)]
    [DataRow(PasLevel.Level8, true)]
    [DataRow(PasLevel.Level9, true)]
    public void CanDecrease(PasLevel level, bool expectedResult)
    {
        // Act
        var result = level.CanDecrease();

        // Assert
        result.Should().Be(expectedResult);
    }

    [DataTestMethod]
    [DataRow(PasLevel.Level1, true)]
    [DataRow(PasLevel.Level2, true)]
    [DataRow(PasLevel.Level3, true)]
    [DataRow(PasLevel.Level4, true)]
    [DataRow(PasLevel.Level5, true)]
    [DataRow(PasLevel.Level6, true)]
    [DataRow(PasLevel.Level7, true)]
    [DataRow(PasLevel.Level8, true)]
    [DataRow(PasLevel.Level9, false)]
    public void CanIncrease(PasLevel level, bool expectedResult)
    {
        // Act
        var result = level.CanIncrease();

        // Assert
        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public void TryDecrease_WithInvalidInput()
    {
        // Arrange
        var level = PasLevel.Level1;

        // Act
        var result = level.TryDecrease();

        // Assert
        result.IsNone.Should().BeTrue();
    }

    [DataTestMethod]
    [DataRow(PasLevel.Level2, PasLevel.Level1)]
    [DataRow(PasLevel.Level3, PasLevel.Level2)]
    [DataRow(PasLevel.Level4, PasLevel.Level3)]
    [DataRow(PasLevel.Level5, PasLevel.Level4)]
    [DataRow(PasLevel.Level6, PasLevel.Level5)]
    [DataRow(PasLevel.Level7, PasLevel.Level6)]
    [DataRow(PasLevel.Level8, PasLevel.Level7)]
    [DataRow(PasLevel.Level9, PasLevel.Level8)]
    public void TryDecrease_WithValidInput(PasLevel level, PasLevel expectedResult)
    {
        // Act
        var result = level.TryDecrease();

        // Assert
        result.Case.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public void TryIncrease_WithInvalidInput()
    {
        // Arrange
        var level = PasLevel.Level9;

        // Act
        var result = level.TryIncrease();

        // Assert
        result.IsNone.Should().BeTrue();
    }

    [DataTestMethod]
    [DataRow(PasLevel.Level1, PasLevel.Level2)]
    [DataRow(PasLevel.Level2, PasLevel.Level3)]
    [DataRow(PasLevel.Level3, PasLevel.Level4)]
    [DataRow(PasLevel.Level4, PasLevel.Level5)]
    [DataRow(PasLevel.Level5, PasLevel.Level6)]
    [DataRow(PasLevel.Level6, PasLevel.Level7)]
    [DataRow(PasLevel.Level7, PasLevel.Level8)]
    [DataRow(PasLevel.Level8, PasLevel.Level9)]
    public void TryIncrease_WithValidInput(PasLevel level, PasLevel expectedResult)
    {
        // Act
        var result = level.TryIncrease();

        // Assert
        result.Case.Should().BeEquivalentTo(expectedResult);
    }
}
