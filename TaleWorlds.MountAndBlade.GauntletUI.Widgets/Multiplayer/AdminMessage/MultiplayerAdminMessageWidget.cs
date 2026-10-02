using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.AdminMessage
{
	// Token: 0x020000D2 RID: 210
	public class MultiplayerAdminMessageWidget : Widget
	{
		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000ACC RID: 2764 RVA: 0x0001E2F3 File Offset: 0x0001C4F3
		// (set) Token: 0x06000ACD RID: 2765 RVA: 0x0001E2FB File Offset: 0x0001C4FB
		public TextWidget MessageTextWidget { get; set; }

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000ACE RID: 2766 RVA: 0x0001E304 File Offset: 0x0001C504
		public float MessageOnScreenStayTime
		{
			get
			{
				return 5f;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x0001E30B File Offset: 0x0001C50B
		public float MessageFadeInTime
		{
			get
			{
				return 0.4f;
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x0001E312 File Offset: 0x0001C512
		public float MessageFadeOutTime
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0001E319 File Offset: 0x0001C519
		public MultiplayerAdminMessageWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0001E324 File Offset: 0x0001C524
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.ChildCount <= 0)
			{
				this._currentTextOnScreenTime = 0f;
				return;
			}
			this._currentTextOnScreenTime += dt;
			if (this._currentTextOnScreenTime < this.MessageFadeInTime)
			{
				float num = MathF.Lerp(0f, 1f, this._currentTextOnScreenTime / this.MessageFadeInTime, 1E-05f);
				base.Children[0].SetGlobalAlphaRecursively(num);
				base.Children[0].IsVisible = true;
				return;
			}
			if (this._currentTextOnScreenTime > this.MessageFadeInTime && this._currentTextOnScreenTime < this.MessageOnScreenStayTime + this.MessageFadeInTime)
			{
				base.Children[0].SetGlobalAlphaRecursively(1f);
				return;
			}
			if (this._currentTextOnScreenTime < this.MessageFadeInTime + this.MessageOnScreenStayTime + this.MessageFadeOutTime)
			{
				float num2 = MathF.Lerp(1f, 0f, (this._currentTextOnScreenTime - (this.MessageFadeInTime + this.MessageOnScreenStayTime)) / this.MessageFadeOutTime, 1E-05f);
				base.Children[0].SetGlobalAlphaRecursively(num2);
				return;
			}
			MultiplayerAdminMessageItemWidget multiplayerAdminMessageItemWidget = base.Children[0] as MultiplayerAdminMessageItemWidget;
			if (multiplayerAdminMessageItemWidget != null)
			{
				multiplayerAdminMessageItemWidget.Remove();
			}
			this._currentTextOnScreenTime = 0f;
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0001E473 File Offset: 0x0001C673
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
		}

		// Token: 0x040004E9 RID: 1257
		private float _currentTextOnScreenTime;
	}
}
