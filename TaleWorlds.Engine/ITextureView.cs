using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000032 RID: 50
	[ApplicationInterfaceBase]
	internal interface ITextureView
	{
		// Token: 0x0600053D RID: 1341
		[EngineMethod("create_texture_view", false, null, false)]
		TextureView CreateTextureView();

		// Token: 0x0600053E RID: 1342
		[EngineMethod("set_texture", false, null, true)]
		void SetTexture(UIntPtr pointer, UIntPtr texture_ptr);
	}
}
