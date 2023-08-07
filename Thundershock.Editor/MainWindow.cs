using Eto.Drawing;
using Eto.Forms;
using Thundershock.Editor.MenuSystem;
using Thundershock.Editor.Panels;
using Thundershock.Windowing;

namespace Thundershock.Editor;

public class MainWindow : Eto.Forms.Form
{
	private readonly MenuBar mainMenu;
	private readonly MenuController menuController;

	public MenuController MenuController => this.menuController;

	private readonly EditorDocumentManager documentManager;

	public EditorDocumentManager DocumentManager => documentManager;
	
	public MainWindow(WindowingModule windowingModule)
	{
		this.Title = ApplicationInfo.ProductName;
		this.ClientSize = new Size(1280, -1);

		this.mainMenu = new MenuBar();
		this.menuController = new MenuController(this.mainMenu);

		documentManager = windowingModule.CreateWindowManager<EditorDocumentManager, EditorDocument>();
		Content = documentManager;
	}

	/// <inheritdoc />
	protected override void OnShown(EventArgs e)
	{
		this.menuController.Rebuild();
		this.Menu = mainMenu;
		base.OnShown(e);
	}
}