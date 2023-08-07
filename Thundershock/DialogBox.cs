#nullable enable

using Eto.Forms;

namespace Thundershock;

/// <summary>
///		Provides a platform-agnostic API for displaying system dialog boxes.
/// </summary>
public static class DialogBox
{
	public static void Message(string title, string message)
	{
		using var app = new Eto.Forms.Application();
		MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxType.Information);
	}
}