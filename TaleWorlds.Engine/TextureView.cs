using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x02000092 RID: 146
	[EngineClass("rglTexture_view")]
	public sealed class TextureView : View
	{
		// Token: 0x06000D10 RID: 3344 RVA: 0x0000E93F File Offset: 0x0000CB3F
		internal TextureView(UIntPtr meshPointer)
			: base(meshPointer)
		{
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0000E948 File Offset: 0x0000CB48
		public static TextureView CreateTextureView()
		{
			return EngineApplicationInterface.ITextureView.CreateTextureView();
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0000E954 File Offset: 0x0000CB54
		public void SetTexture(Texture texture)
		{
			EngineApplicationInterface.ITextureView.SetTexture(base.Pointer, texture.Pointer);
		}
	}
}
