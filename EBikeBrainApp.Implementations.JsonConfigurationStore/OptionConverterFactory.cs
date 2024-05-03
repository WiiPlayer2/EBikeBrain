using System.Text.Json;
using System.Text.Json.Serialization;
using LanguageExt;

namespace EBikeBrainApp.Implementations.JsonConfigurationStore;

internal class OptionConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Option<>);

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
        (JsonConverter) Activator.CreateInstance(typeof(OptionConverter<>).MakeGenericType(typeToConvert.GenericTypeArguments[0]))!;
}
