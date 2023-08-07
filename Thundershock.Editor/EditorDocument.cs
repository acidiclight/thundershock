using Eto.Forms;
using Thundershock.Windowing;

namespace Thundershock.Editor;

public class EditorDocument : Panel, Thundershock.Windowing.IWindow
{
	private readonly DocumentControl owningDocumentControl;
	private readonly DocumentPage page;
	private readonly EtoNativeWindow nativeWindow;

	/// <inheritdoc />
	public bool IsOpen => owningDocumentControl.Pages.Contains(page);

	/// <inheritdoc />
	public string? Title
	{
		get => page.Text;
		set => page.Text = value;
	}

	/// <inheritdoc />
	public INativeWindow? NativeWindow => nativeWindow;

	public EditorDocument(DocumentControl documentControl)
	{
		this.owningDocumentControl = documentControl;
		this.page = new DocumentPage(this);
		this.nativeWindow = new EtoNativeWindow(this);
	}
	
	/// <inheritdoc />
	public void Open()
	{
		if (IsOpen)
			return;

		owningDocumentControl.Pages.Add(page);
	}

	/// <inheritdoc />
	public void Close()
	{
		this.owningDocumentControl.Pages.Remove(page);
	}
}