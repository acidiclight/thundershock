#nullable enable

using Cairo;

namespace Thundershock.Editor.Panels;

public class EditorPanel
{
	private readonly EditorPanelInternal panel;

	internal EditorPanel(EditorPanelInternal internalPanel)
	{
		this.panel = internalPanel;
	}
}