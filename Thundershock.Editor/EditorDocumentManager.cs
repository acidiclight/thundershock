using Eto.Forms;
using Thundershock.Windowing;

namespace Thundershock.Editor;

public class EditorDocumentManager : WindowManager<EditorDocument>
{
	private readonly DocumentControl documentControl = new DocumentControl();
	
	/// <inheritdoc />
	protected override void OnInitialize()
	{
	}

	/// <inheritdoc />
	protected override void OnShutdown()
	{
	}

	/// <inheritdoc />
	protected override void OnUpdate()
	{
	}

	/// <inheritdoc />
	protected override EditorDocument GetNewWindow()
	{
		return new EditorDocument(this.documentControl);
	}
	
	public static implicit operator Control(EditorDocumentManager instance)
	{
		return instance.documentControl;
	}
}