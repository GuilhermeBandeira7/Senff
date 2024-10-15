namespace Senff.Api;

public static class ApiConfiguration
{
    public const string UserId = "user@senff.io";
    public static string ConnectionString { get; set; } = string.Empty;
    public static string CorsPolicyName = "wasm";
}