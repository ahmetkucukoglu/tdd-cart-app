namespace CartTdd.Infrastructure.Database;

public record DbSettings
{
    public string ConnectionString { get; init; }
    public string DatabaseName { get; init; }
}