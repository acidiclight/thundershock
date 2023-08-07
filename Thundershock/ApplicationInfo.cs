namespace Thundershock;

public static class ApplicationInfo
{
	public static string EngineVersionString { get; internal set; } = string.Empty;

	public static string CompanyName { get; internal set; } = "Your Company";
	public static string ProductName { get; internal set; } = "Your Product";

	public static string PersistentDataPath => Path.Combine(
		Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
		CompanyName,
		ProductName
	);

	public static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;
}