namespace Thundershock.Windowing;

public abstract class WindowManager<T> : IWindowManager
	where T : class, IWindow
{
	private readonly List<T> windows = new List<T>();
	private WindowingModule? owningModule;
	private bool initialized = false;

	public T CreateWindow()
	{
		T win = GetNewWindow();
		this.windows.Add(win);

		if (initialized)
			win.Open();

		return win;
	}

	public T CreateWindow(string title)
	{
		T win = CreateWindow();
		win.Title = title;

		return win;
	}
	
	void IWindowManager.Initialize()
	{
		InitializeInternal();
	}

	private void InitializeInternal()
	{
		if (owningModule == null)
			return;

		if (initialized)
			return;

		if (!owningModule.IsInitialized)
			return;
			
		OnInitialize();

		initialized = true;

		foreach (T win in windows)
			win.Open();
	}

	void IWindowManager.Shutdown()
	{
		ShutdownInternal();
	}

	private void ShutdownInternal()
	{
		initialized = false;

		while (windows.Count > 0)
		{
			windows[0].Close();
			windows.RemoveAt(0);
		}
		
		OnShutdown();
	}

	protected abstract void OnInitialize();
	protected abstract void OnShutdown();
	protected abstract void OnUpdate();

	void IWindowManager.Update()
	{
		if (!initialized)
			return;

		OnUpdate();

		for (int i = windows.Count - 1; i >= 0; i--)
		{
			if (!windows[i].IsOpen)
				windows.RemoveAt(i);
		}
	}
	
	void IWindowManager.AssignToModuleInternal(WindowingModule? module)
	{
		if (owningModule != null && !owningModule.IsInitialized)
			ShutdownInternal();
		

		owningModule = module;

		if (owningModule != null)
			InitializeInternal();
	}
	
	protected abstract T GetNewWindow();
}