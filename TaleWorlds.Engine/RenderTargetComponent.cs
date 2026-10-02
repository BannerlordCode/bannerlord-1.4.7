using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200007D RID: 125
	public sealed class RenderTargetComponent : DotNetObject
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x0000B0E6 File Offset: 0x000092E6
		public Texture RenderTarget
		{
			get
			{
				return (Texture)this._renderTargetWeakReference.GetNativeObject();
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x0000B0F8 File Offset: 0x000092F8
		// (set) Token: 0x06000AB2 RID: 2738 RVA: 0x0000B100 File Offset: 0x00009300
		public object UserData { get; internal set; }

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0000B109 File Offset: 0x00009309
		internal RenderTargetComponent(Texture renderTarget)
		{
			this._renderTargetWeakReference = new WeakNativeObjectReference(renderTarget);
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0000B11D File Offset: 0x0000931D
		internal void OnTargetReleased()
		{
			this.PaintNeeded = null;
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0000B126 File Offset: 0x00009326
		[EngineCallback(null, false)]
		internal static RenderTargetComponent CreateRenderTargetComponent(Texture renderTarget)
		{
			return new RenderTargetComponent(renderTarget);
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000AB6 RID: 2742 RVA: 0x0000B130 File Offset: 0x00009330
		// (remove) Token: 0x06000AB7 RID: 2743 RVA: 0x0000B168 File Offset: 0x00009368
		internal event RenderTargetComponent.TextureUpdateEventHandler PaintNeeded;

		// Token: 0x06000AB8 RID: 2744 RVA: 0x0000B19D File Offset: 0x0000939D
		[EngineCallback(null, false)]
		internal void OnPaintNeeded()
		{
			RenderTargetComponent.TextureUpdateEventHandler paintNeeded = this.PaintNeeded;
			if (paintNeeded == null)
			{
				return;
			}
			paintNeeded(this.RenderTarget, EventArgs.Empty);
		}

		// Token: 0x04000195 RID: 405
		private readonly WeakNativeObjectReference _renderTargetWeakReference;

		// Token: 0x020000CE RID: 206
		// (Invoke) Token: 0x06001004 RID: 4100
		public delegate void TextureUpdateEventHandler(Texture sender, EventArgs e);
	}
}
