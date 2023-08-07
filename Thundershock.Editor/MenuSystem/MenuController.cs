#nullable enable

using Eto.Forms;

namespace Thundershock.Editor.MenuSystem;

public class MenuController : MenuItem
{
	private readonly MenuBar menuBar;

	public MenuController(MenuBar menuBar)
	{
		this.menuBar = menuBar;
	}

	public void Rebuild()
	{
		this.menuBar.Items.Clear();

		var builder = new MenuBuilder(this.menuBar);

		this.Build(builder);
	}
}