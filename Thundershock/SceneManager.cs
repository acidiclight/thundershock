using System.Collections;
using System.Runtime.CompilerServices;

namespace Thundershock;

public static class SceneManager
{
	private static readonly SceneCollection loadedScenes = new SceneCollection();

	public static int LoadedScenesCount => loadedScenes.Count;

	internal static Scene CreateEmpty()
	{
		var emptyScene = Scene.CreateEmpty();
		loadedScenes.Add(emptyScene);
		return emptyScene;
	}
	
	public static Scene GetSceneByIndex(int index)
	{
		return loadedScenes[index];
	}
	
	
	
	private class SceneCollection : ICollection<Scene>
	{
		private readonly List<Scene> scenes = new List<Scene>();

		public Scene this[int index]
		{
			get => scenes[index];
		}
		
		/// <inheritdoc />
		public IEnumerator<Scene> GetEnumerator()
		{
			return scenes.GetEnumerator();
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <inheritdoc />
		public void Add(Scene item)
		{
			scenes.Add(item);
		}

		/// <inheritdoc />
		public void Clear()
		{
			scenes.Clear();
		}

		/// <inheritdoc />
		public bool Contains(Scene item)
		{
			return scenes.Contains(item);
		}

		/// <inheritdoc />
		public void CopyTo(Scene[] array, int arrayIndex)
		{
			scenes.CopyTo(array, arrayIndex);
		}

		/// <inheritdoc />
		public bool Remove(Scene item)
		{
			return scenes.Remove(item);
		}

		/// <inheritdoc />
		public int Count => scenes.Count;

		/// <inheritdoc />
		public bool IsReadOnly => false;
	}
}