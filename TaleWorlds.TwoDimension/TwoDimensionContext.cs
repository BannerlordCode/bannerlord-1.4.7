using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000036 RID: 54
	public class TwoDimensionContext
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000963E File Offset: 0x0000783E
		public float Width
		{
			get
			{
				return this.Platform.Width;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0000964B File Offset: 0x0000784B
		public float Height
		{
			get
			{
				return this.Platform.Height;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600026F RID: 623 RVA: 0x00009658 File Offset: 0x00007858
		// (set) Token: 0x06000270 RID: 624 RVA: 0x00009660 File Offset: 0x00007860
		public ITwoDimensionPlatform Platform { get; private set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000271 RID: 625 RVA: 0x00009669 File Offset: 0x00007869
		// (set) Token: 0x06000272 RID: 626 RVA: 0x00009671 File Offset: 0x00007871
		public ITwoDimensionResourceContext ResourceContext { get; private set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000967A File Offset: 0x0000787A
		// (set) Token: 0x06000274 RID: 628 RVA: 0x00009682 File Offset: 0x00007882
		public ResourceDepot ResourceDepot { get; private set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000968B File Offset: 0x0000788B
		public bool IsDebugModeEnabled
		{
			get
			{
				return this.Platform.IsDebugModeEnabled();
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00009698 File Offset: 0x00007898
		public TwoDimensionContext(ITwoDimensionPlatform platform, ITwoDimensionResourceContext resourceContext, ResourceDepot resourceDepot)
		{
			this.ResourceDepot = resourceDepot;
			this.Platform = platform;
			this.ResourceContext = resourceContext;
		}

		// Token: 0x06000277 RID: 631 RVA: 0x000096B5 File Offset: 0x000078B5
		public void PlaySound(string soundName)
		{
			this.Platform.PlaySound(soundName);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x000096C3 File Offset: 0x000078C3
		public void CreateSoundEvent(string soundName)
		{
			this.Platform.CreateSoundEvent(soundName);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x000096D1 File Offset: 0x000078D1
		public void StopAndRemoveSoundEvent(string soundName)
		{
			this.Platform.StopAndRemoveSoundEvent(soundName);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x000096DF File Offset: 0x000078DF
		public void PlaySoundEvent(string soundName)
		{
			this.Platform.PlaySoundEvent(soundName);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x000096ED File Offset: 0x000078ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawImage(SimpleMaterial material, in ImageDrawObject drawObject2D, int layer = 0)
		{
			this.Platform.DrawImage(material, in drawObject2D, layer);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x000096FD File Offset: 0x000078FD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawText(TextMaterial material, in TextDrawObject drawObject2D, int layer = 0)
		{
			this.Platform.DrawText(material, in drawObject2D, layer);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000970D File Offset: 0x0000790D
		public void BeginDebugPanel(string panelTitle)
		{
			this.Platform.BeginDebugPanel(panelTitle);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000971B File Offset: 0x0000791B
		public void EndDebugPanel()
		{
			this.Platform.EndDebugPanel();
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00009728 File Offset: 0x00007928
		public void DrawDebugText(string text)
		{
			this.Platform.DrawDebugText(text);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00009736 File Offset: 0x00007936
		public bool DrawDebugTreeNode(string text)
		{
			return this.Platform.DrawDebugTreeNode(text);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00009744 File Offset: 0x00007944
		public void PopDebugTreeNode()
		{
			this.Platform.PopDebugTreeNode();
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00009751 File Offset: 0x00007951
		public void DrawCheckbox(string label, ref bool isChecked)
		{
			this.Platform.DrawCheckbox(label, ref isChecked);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00009760 File Offset: 0x00007960
		public bool IsDebugItemHovered()
		{
			return this.Platform.IsDebugItemHovered();
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000976D File Offset: 0x0000796D
		public Texture LoadTexture(string name)
		{
			return this.ResourceContext.LoadTexture(this.ResourceDepot, name);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00009781 File Offset: 0x00007981
		public void SetScissor(ScissorTestInfo scissor)
		{
			this.Platform.SetScissor(scissor);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000978F File Offset: 0x0000798F
		public void ResetScissor()
		{
			this.Platform.ResetScissors();
		}
	}
}
