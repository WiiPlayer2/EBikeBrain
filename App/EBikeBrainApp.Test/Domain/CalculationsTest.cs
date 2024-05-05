using FluentAssertions;

namespace EBikeBrainApp.Test;

[TestClass]
public class CalculationsTest
{
    [DataTestMethod]
    [DataRow(0, 10, 0)]
    [DataRow(10, 0, 0)]
    [DataRow(10, 10, 0.479)]
    [DataRow(186.5, 28, 25)]
    public void ToLinearSpeed_WithInputValues_ReturnOutputValues(double rpm, double wheelDiameterInInches, double linearSpeedInKmh)
    {
        // Arrange
        var rotationalSpeed = RotationalSpeed.FromRevolutionsPerMinute(rpm);
        var wheelDiameter = Length.FromInches(wheelDiameterInInches);

        // Act
        var result = rotationalSpeed.ToLinearSpeed(wheelDiameter);

        // Assert
        result.KilometersPerHour.Should().BeApproximately(linearSpeedInKmh, 1e-2);
    }
}
