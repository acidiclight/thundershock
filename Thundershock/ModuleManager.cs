namespace Thundershock;

public class ModuleManager : IEngineModule
{
	private bool isInitialized;
	private readonly List<IEngineModule> activeModules = new List<IEngineModule>();

	public void AddModule<T>() where T : IEngineModule, new()
	{
		ThrowIfInitialized();

		var module = new T();
		this.activeModules.Add(module);
	}

	public void AddModule(IEngineModule module)
	{
		ThrowIfInitialized();
		activeModules.Add(module);
	}
	
	/// <inheritdoc />
	public void Initialize()
	{
		ThrowIfInitialized();

		isInitialized = true;
		
		foreach (IEngineModule module in activeModules)
			module.Initialize();
	}

	/// <inheritdoc />
	public void RunOneUpdate()
	{
		foreach (IEngineModule module in activeModules)
			module.RunOneUpdate();
	}

	/// <inheritdoc />
	public void Shutdown()
	{
		foreach (IEngineModule module in activeModules)
			module.Shutdown();

		activeModules.Clear();
	}

	private void ThrowIfInitialized()
	{
		if (isInitialized)
			throw new InvalidOperationException("Cannot perform this operation when the ModuleManager has already been initialized.");
	}
}