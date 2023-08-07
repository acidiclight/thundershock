using System.Collections;

namespace Thundershock.Editor.MenuSystem;

public abstract class MenuItem
{
	private readonly MenuItemCollection subItems; 
	private MenuItem? parent;

	public string? Text { get; set; }

	protected MenuItemCollection Children => this.subItems;
	
	public MenuItem()
	{
		this.subItems = new MenuItemCollection(this);
	}

	protected virtual void Build(MenuBuilder builder)
	{
		BuildChildren(builder);
	}

	private void BuildChildren(MenuBuilder builder)
	{
		foreach (MenuItem item in this.subItems)
			item.Build(builder);
	}
	
	protected class MenuItemCollection : ICollection<MenuItem>
	{
		private readonly MenuItem owner;
		private readonly List<MenuItem> items = new List<MenuItem>();

		public MenuItemCollection(MenuItem owner)
		{
			this.owner = owner;
		}

		/// <inheritdoc />
		public IEnumerator<MenuItem> GetEnumerator()
		{
			return items.GetEnumerator();
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <inheritdoc />
		public void Add(MenuItem item)
		{
			if (item.parent == owner)
				return;

			if (item.parent == null)
				throw new InvalidOperationException("Item is already parented to another menu item.");

			item.parent = owner;
			items.Add(item);
		}

		/// <inheritdoc />
		public void Clear()
		{
			while (items.Count > 0)
				Remove(items[0]);
		}

		/// <inheritdoc />
		public bool Contains(MenuItem item)
		{
			return item.parent == owner;
		}

		/// <inheritdoc />
		public void CopyTo(MenuItem[] array, int arrayIndex)
		{
			items.CopyTo(array, arrayIndex);
		}

		/// <inheritdoc />
		public bool Remove(MenuItem item)
		{
			if (item.parent != owner)
				return false;

			item.parent = null;
			return items.Remove(item);
		}

		/// <inheritdoc />
		public int Count => items.Count;

		/// <inheritdoc />
		public bool IsReadOnly => false;
	}
}