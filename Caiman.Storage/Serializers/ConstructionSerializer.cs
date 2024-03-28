using System.Text.Json;
using Caiman.Storage.Models;

namespace Caiman.Storage.Serializers;

public class ConstructionSerializer
{
    public string Serialize(ConstructionModel construction)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        string json = JsonSerializer.Serialize(construction, options);

        return json;
    }

    public ConstructionModel Deserialize(string construction)
    {
        var model = JsonSerializer.Deserialize<ConstructionModel>(construction);
        return model ?? throw new InvalidDataException("Invalid construction model");
    }
}
