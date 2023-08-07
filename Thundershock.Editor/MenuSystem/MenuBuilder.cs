using Eto.Forms;
using Gtk;
using MenuBar = Eto.Forms.MenuBar;

namespace Thundershock.Editor.MenuSystem;

public class MenuBuilder
{
	private readonly Eto.Forms.MenuItemCollection itemCollection;

	private MenuBuilder(Eto.Forms.MenuItemCollection collection)
	{
		this.itemCollection = collection;
	}
	
	public MenuBuilder(MenuBar rootMenuBar)
		: this(rootMenuBar.Items)
	{
		
	}

	public void Divider()
	{
	}

	public void Command(string text, EventHandler<EventArgs> clickHandler)
	{
		var command = new Command(clickHandler);
		command.MenuText = text;

		this.itemCollection.Add(command);
	}
	
	public MenuBuilder CreateSubMenu(string text)
	{
		var subMenu = new SubMenuItem();
		subMenu.Text = text;

		this.itemCollection.Add(subMenu);

		return new MenuBuilder(subMenu.Items);
	}
}