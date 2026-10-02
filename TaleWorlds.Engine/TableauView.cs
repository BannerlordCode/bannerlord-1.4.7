using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x02000090 RID: 144
	[EngineClass("rglTableau_view")]
	public sealed class TableauView : SceneView
	{
		// Token: 0x06000CE6 RID: 3302 RVA: 0x0000E5E9 File Offset: 0x0000C7E9
		internal TableauView(UIntPtr meshPointer)
			: base(meshPointer)
		{
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0000E5F2 File Offset: 0x0000C7F2
		public static TableauView CreateTableauView(string viewName)
		{
			return EngineApplicationInterface.ITableauView.CreateTableauView(viewName);
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0000E5FF File Offset: 0x0000C7FF
		public void SetSortingEnabled(bool value)
		{
			EngineApplicationInterface.ITableauView.SetSortingEnabled(base.Pointer, value);
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0000E612 File Offset: 0x0000C812
		public void SetContinuousRendering(bool value)
		{
			EngineApplicationInterface.ITableauView.SetContinousRendering(base.Pointer, value);
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0000E625 File Offset: 0x0000C825
		public void SetDoNotRenderThisFrame(bool value)
		{
			EngineApplicationInterface.ITableauView.SetDoNotRenderThisFrame(base.Pointer, value);
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0000E638 File Offset: 0x0000C838
		public void SetDeleteAfterRendering(bool value)
		{
			EngineApplicationInterface.ITableauView.SetDeleteAfterRendering(base.Pointer, value);
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x0000E64B File Offset: 0x0000C84B
		public static Texture AddTableau(string name, RenderTargetComponent.TextureUpdateEventHandler eventHandler, object objectRef, int tableauSizeX, int tableauSizeY)
		{
			Texture texture = Texture.CreateTableauTexture(name, eventHandler, objectRef, tableauSizeX, tableauSizeY);
			texture.TableauView.SetRenderOnDemand(false);
			return texture;
		}
	}
}
