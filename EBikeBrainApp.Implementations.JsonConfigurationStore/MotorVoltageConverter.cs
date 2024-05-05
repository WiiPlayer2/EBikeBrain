using System.Text.Json;
using System.Text.Json.Serialization;
using EBikeBrainApp.Domain;
using UnitsNet;

namespace EBikeBrainApp.Implementations.JsonConfigurationStore;

internal class MotorVoltageConverter : JsonConverter<MotorVoltage>
{
    public override MotorVoltage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        MotorVoltage.From(ElectricPotential.FromVolts(JsonSerializer.Deserialize<double>(ref reader, options)));

    public override void Write(Utf8JsonWriter writer, MotorVoltage value, JsonSerializerOptions options) =>
        JsonSerializer.Serialize(writer, value.Value.Volts, options);
}
