namespace Thundershock;

public class SceneObject : ISceneObject
{
	private Transform transform;
	private string name = "Scene Object";

	public Transform Transform => transform;
	
	/// <inheritdoc />
	public Scene Scene => transform.Scene;

	/// <inheritdoc />
	public string Name
	{
		get => name;
		set => name = value;
	}
	
	public SceneObject()
	{
		transform = new Transform(this);
	}

	public T AddComponent<T>()
		where T : Component, new()
	{
		var component = new T();
		component.SceneObject = this;
		return component;
	}

	public void AddComponent(Component component)
	{
		component.SceneObject = this;
	}

	public IEnumerable<T> GetComponents<T>()
	{
		return Component.GetAllInObject(this)
			.OfType<T>();
	}

	public T? GetComponent<T>()
	{
		return GetComponents<T>()
			.FirstOrDefault();
	}

	public bool TryGetComponent<T>(out T? component)
	{
		component = GetComponent<T>();
		return component != null;
	}
}