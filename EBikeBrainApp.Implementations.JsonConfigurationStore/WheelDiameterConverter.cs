using System.Text.Json;
using System.Text.Json.Serialization;
using EBikeBrainApp.Domain;
using UnitsNet;

namespace EBikeBrainApp.Implementations.JsonConfigurationStore;

internal class WheelDiameterConverter : JsonConverter<WheelDiameter>
{
    public override WheelDiameter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        WheelDiameter.From(Length.FromInches(JsonSerializer.Deserialize<double>(ref reader, options)));

    public override void Write(Utf8JsonWriter writer, WheelDiameter value, JsonSerializerOptions options) =>
        JsonSerializer.Serialize(writer, value.Value.Inches, options);
}
