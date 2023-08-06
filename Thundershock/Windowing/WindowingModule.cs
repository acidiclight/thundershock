#nullable enable

using System.Collections;

namespace Thundershock.Windowing;

public class WindowingModule : IEngineModule
{
	private readonly WindowManagerCollection windowManagers;
	
	internal bool IsInitialized { get; private set; }

	public T CreateWindowManager<T, TWindow>()
		where TWindow : class, IWindow
		where T : WindowManager<TWindow>, new()
	{
		T wm = new T();
		windowManagers.Add(wm);
		return wm;
	}

	internal WindowingModule()
	{
		windowManagers = new WindowManagerCollection(this);
	}
	
	/// <inheritdoc />
	public void Initialize()
	{
		IsInitialized = true;
		foreach (IWindowManager wm in windowManagers)
			wm.Initialize();
	}

	/// <inheritdoc />
	public void RunOneUpdate()
	{
		for (var i = 0; i < windowManagers.Count; i++)
		{
			windowManagers[i].Update();
		}
	}
	

	/// <inheritdoc />
	public void Shutdown()
	{
		IsInitialized = false;
		windowManagers.Clear();
	}

	private class WindowManagerCollection : ICollection<IWindowManager>
	{
		private readonly WindowingModule windowingModule;
		private readonly List<IWindowManager> collection = new List<IWindowManager>();

		public IWindowManager this[int index]
		{
			get => collection[index];
		}
		
		public WindowManagerCollection(WindowingModule module)
		{
			this.windowingModule = module;
		}

		/// <inheritdoc />
		public IEnumerator<IWindowManager> GetEnumerator()
		{
			return collection.GetEnumerator();
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <inheritdoc />
		public void Add(IWindowManager item)
		{
			item.AssignToModuleInternal(this.windowingModule);
			
			collection.Add(item);
		}

		/// <inheritdoc />
		public void Clear()
		{
			while (collection.Count > 0)
				Remove(collection[0]);
		}

		/// <inheritdoc />
		public bool Contains(IWindowManager item)
		{
			return collection.Contains(item);
		}

		/// <inheritdoc />
		public void CopyTo(IWindowManager[] array, int arrayIndex)
		{
			collection.CopyTo(array, arrayIndex);
		}

		/// <inheritdoc />
		public bool Remove(IWindowManager item)
		{
			bool result = collection.Remove(item);

			if (result)
				item.AssignToModuleInternal(null);

			return result;
		}

		/// <inheritdoc />
		public int Count => collection.Count;

		/// <inheritdoc />
		public bool IsReadOnly => false;
	}
}