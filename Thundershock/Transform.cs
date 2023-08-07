#nullable enable

using System.Collections;
using System.Numerics;

namespace Thundershock;

public class Transform : ISceneObject
{
	private static readonly List<Transform> rootTransforms = new List<Transform>();

	private readonly TransformCollection children;
	private Transform? parent; 
	private Matrix4x4 transformMatrix = Matrix4x4.Identity;
	private Vector3 localPosition = Vector3.Zero;
	private Vector3 localScale = Vector3.One;
	private Quaternion localRotation = Quaternion.Identity;
	private Vector3 worldPosition;
	private Vector3 worldScale;
	private Quaternion worldRotation;
	private SceneObject owner;
	private Scene myScene;
	private string? name;

	public Matrix4x4 TransformMatrix => transformMatrix;

	public Vector3 LocalPosition
	{
		get => localPosition;
		set
		{
			if (localPosition == value)
				return;

			localPosition = value;
			RecalculateMatrix();
		}
	}

	public Vector3 LocalScale
	{
		get => localScale;
		set
		{
			if (localScale == value)
				return;

			localScale = value;
			RecalculateMatrix();
		}
	}

	public Quaternion LocalRotation
	{
		get => localRotation;
		set
		{
			if (localRotation == value)
				return;

			localRotation = value;
			RecalculateMatrix();
		}
	}

	public Vector3 Position
	{
		get => worldPosition;
		set
		{
			if (worldPosition == value)
				return;

			Vector3 parentPosition = parent?.Position ?? Vector3.Zero;

			localPosition = value - parentPosition;
			RecalculateMatrix();
		}
	}

	public Vector3 Scale
	{
		get => worldScale;
		set
		{
			if (worldScale == value)
				return;

			Vector3 parentScale = parent?.Scale ?? Vector3.One;

			localScale = value / parentScale;
			RecalculateMatrix();
		}
	}

	public Quaternion Rotation
	{
		get => worldRotation;
		set
		{
			if (worldRotation == value)
				return;

			Quaternion parentRotation = parent?.Rotation ?? Quaternion.Identity;

			localRotation = value - parentRotation;
			RecalculateMatrix();
		}
	}

	public Transform? Parent => parent;
	public int ChildCount => children.Count;

	public Transform this[int index]
	{
		get => children[index];
	}

	public SceneObject SceneObject => this.owner;
	
	internal Transform(SceneObject owner)
	{
		this.myScene = GetActiveOrFirstScene();
		
		this.children = new TransformCollection(this);

		worldPosition = localPosition;
		worldScale = localScale;
		worldRotation = localRotation;

		this.myScene = Scene.Active;
		this.owner = owner;

		rootTransforms.Add(this);
	}

	public void SetParent(Transform? newParent, bool keepWorldTransform = false)
	{
		if (newParent == this.parent)
			return;

		if (this.parent == null)
		{
			rootTransforms.Remove(this);
			myScene?.RebuildRootsInternal();
		}

		if (this.parent != null)
			this.parent.children.Remove(this);

		newParent?.children.Add(this);
		
		myScene = parent?.myScene ?? GetActiveOrFirstScene();

		if (this.parent == null)
		{
			rootTransforms.Add(this);
			myScene?.RebuildRootsInternal();
		}

		RecalculateMatrix();
	}

	private void RecalculateMatrix()
	{
		Matrix4x4 parentMatrix = parent?.transformMatrix ?? Matrix4x4.Identity;

		Vector3 parentWorldPosition = parent?.worldPosition ?? Vector3.Zero;
		Vector3 parentWorldScale = parent?.worldScale ?? Vector3.One;
		Quaternion parentWorldRotation = parent?.worldRotation ?? Quaternion.Identity;

		this.worldPosition = parentWorldPosition + localPosition;
		this.worldScale = parentWorldScale * localScale;
		this.worldRotation = parentWorldRotation + localRotation;
		
		this.transformMatrix = parentMatrix
		                       * Matrix4x4.CreateScale(localScale)
		                       * Matrix4x4.CreateFromQuaternion(localRotation)
		                       * Matrix4x4.CreateTranslation(localPosition);

		foreach (Transform child in children)
		{
			child.RecalculateMatrix();
		}
	}

	private Scene GetActiveOrFirstScene()
	{
		if (Scene.Active != null)
			return Scene.Active;

		if (SceneManager.LoadedScenesCount == 0)
			throw new InvalidOperationException("Cannot create the transform because there are no loaded scenes to attach it to.");

		return SceneManager.GetSceneByIndex(0);
	}

	public static int RootTransformCount => rootTransforms.Count;

	public static Transform GetRootTransform(int index)
	{
		return rootTransforms[index];
	}
	
	private class TransformCollection : ICollection<Transform>
	{
		private readonly Transform owner;
		private readonly List<Transform> transforms = new List<Transform>();

		public Transform this[int index]
		{
			get => transforms[index];
		}
		
		public TransformCollection(Transform owner)
		{
			this.owner = owner;
		}
		
		/// <inheritdoc />
		public IEnumerator<Transform> GetEnumerator()
		{
			return transforms.GetEnumerator();
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <inheritdoc />
		public void Add(Transform item)
		{
			if (item == owner)
				throw new InvalidOperationException("Cannot add a transform as a child of itself.");

			if (item.parent == owner)
				return;

			if (item.parent != null)
				throw new InvalidOperationException("Please remove the transform from its existing parent before adding it as a child of its new parent.");

			item.parent = owner;

			this.transforms.Add(item);
		}

		/// <inheritdoc />
		public void Clear()
		{
			while (this.transforms.Count > 0)
			{
				Remove(this.transforms[0]);
			}
		}

		/// <inheritdoc />
		public bool Contains(Transform item)
		{
			return transforms.Contains(item) && item.parent == owner;
		}

		/// <inheritdoc />
		public void CopyTo(Transform[] array, int arrayIndex)
		{
			transforms.CopyTo(array, arrayIndex);
		}

		/// <inheritdoc />
		public bool Remove(Transform item)
		{
			bool result = transforms.Remove(item);
			if (result)
			{
				item.parent = null;
			}

			return result;
		}

		/// <inheritdoc />
		public int Count => transforms.Count;

		/// <inheritdoc />
		public bool IsReadOnly => false;
	}

	/// <inheritdoc />
	public string Name
	{
		get => name;
		set => name = value;
	}

	/// <inheritdoc />
	public Scene Scene => myScene;
}