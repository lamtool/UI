using System.Text.Json.Serialization;

namespace Sunny.Subdy.Server
{
    [JsonSerializable(typeof(ApiResponse<List<DeviceRespone>>))]
    [JsonSerializable(typeof(ApiResponse<object>))]
    [JsonSerializable(typeof(DeviceRespone))]
    [JsonSourceGenerationOptions(WriteIndented = false)]
    public partial class MyJsonContext : JsonSerializerContext
    {
    }
}
