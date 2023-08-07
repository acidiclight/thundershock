using Eto.Forms;

namespace Thundershock.Editor.Panels;

internal sealed class EditorPanelInternal : Panel
{
	private readonly TabControl tabControl = new();
	
	public EditorPanelInternal()
	{
		Content = tabControl;
	}
}