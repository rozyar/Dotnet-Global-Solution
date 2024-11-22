using System.Collections.Generic;
using System.Text.Json;
using SolarPanelCalculatorApi.Application.DTO;

public static class JsonHelper
{
    public static string SerializeAppliances(List<ApplianceDto> appliances)
    {
        return JsonSerializer.Serialize(appliances);
    }

    public static List<ApplianceDto> DeserializeAppliances(string appliancesJson)
    {
        if (string.IsNullOrEmpty(appliancesJson))
            return new List<ApplianceDto>();

        var appliances = JsonSerializer.Deserialize<List<ApplianceDto>>(appliancesJson);

        // Log dos IDs
        if (appliances != null)
        {
            foreach (var appliance in appliances)
            {
                Console.WriteLine($"Deserialized Appliance ID: {appliance.Id}, Name: {appliance.ApplianceName}");
            }
        }

        return appliances;
    }

}
