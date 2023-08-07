#nullable enable

using GLib;

namespace Thundershock;

public class Scene
{
	private static Scene? activeScene;
	private readonly List<Transform> rootTransforms = new List<Transform>();

	public static Scene? Active => activeScene;

	private Scene()
	{
		
	}

	internal void RebuildRootsInternal()
	{
		rootTransforms.Clear();

		for (var i = 0; i < Transform.RootTransformCount; i++)
		{
			var transform = Transform.GetRootTransform(i);
			if (transform.Scene==this)
				rootTransforms.Add(transform);
		}
	}
	
	internal void Activate()
	{
		activeScene = this;
	}

	internal static Scene CreateEmpty()
	{
		return new Scene();
	}
}