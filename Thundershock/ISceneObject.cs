namespace Thundershock;

public interface ISceneObject
{
	/// <summary>
	///		Gets or sets the name of the object.
	/// </summary>
	string Name { get; set; }
	
	/// <summary>
	///		Gets the <see cref="Scene"/> that owns this object.
	/// </summary>
	Scene Scene { get; }
}