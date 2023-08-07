namespace Thundershock;

public abstract class Component
{
	private static readonly Dictionary<SceneObject, List<Component>> componentLists = new Dictionary<SceneObject, List<Component>>();

	private bool enabled = false;
	
	private SceneObject? sceneObject;

	public Transform? Transform => SceneObject?.Transform;
	
	public SceneObject? SceneObject
	{
		get => sceneObject;
		set
		{
			if (this.sceneObject == value)
				return;

			AssignToSceneObjectInternal(value);
		}
	}

	public bool Enabled
	{
		get => enabled;
		set
		{
			if (enabled == value)
				return;

			enabled = value;

			if (sceneObject == null)
				return;

			if (enabled)
				OnEnable();
			else
				OnDisable();
		}
	}
	
	protected virtual void OnEnable() {}
	protected virtual void OnDisable() {}
	
	private void AssignToSceneObjectInternal(SceneObject? newObject)
	{
		if (this.sceneObject != null)
		{
			if (componentLists.TryGetValue(this.sceneObject, out List<Component>? list))
			{
				list.Remove(this);
				if (list.Count == 0)
					componentLists.Remove(this.sceneObject);
			}

			if (enabled)
				OnDisable();
		}

		this.sceneObject = newObject;

		if (this.sceneObject != null)
		{
			if (!componentLists.TryGetValue(this.sceneObject, out List<Component>? list))
			{
				list = new List<Component>();
				componentLists.Add(sceneObject, list);
			}
			
			list.Add(this);

			if (enabled)
				OnEnable();
		}
	}

	public static IEnumerable<Component> GetAllInObject(SceneObject sceneObject)
	{
		if (componentLists.TryGetValue(sceneObject, out List<Component>? list))
			return list;
		
		return Enumerable.Empty<Component>();
	}
}