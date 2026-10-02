using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Conversation
{
	// Token: 0x02000172 RID: 370
	public class PersuasionResultChanceContainerListPanel : BrushListPanel
	{
		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06001359 RID: 4953 RVA: 0x0003496E File Offset: 0x00032B6E
		// (set) Token: 0x0600135A RID: 4954 RVA: 0x00034976 File Offset: 0x00032B76
		public float StayTime { get; set; } = 1f;

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x0600135B RID: 4955 RVA: 0x0003497F File Offset: 0x00032B7F
		// (set) Token: 0x0600135C RID: 4956 RVA: 0x00034987 File Offset: 0x00032B87
		public Widget CritFailWidget { get; set; }

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x0600135D RID: 4957 RVA: 0x00034990 File Offset: 0x00032B90
		// (set) Token: 0x0600135E RID: 4958 RVA: 0x00034998 File Offset: 0x00032B98
		public Widget FailWidget { get; set; }

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x0600135F RID: 4959 RVA: 0x000349A1 File Offset: 0x00032BA1
		// (set) Token: 0x06001360 RID: 4960 RVA: 0x000349A9 File Offset: 0x00032BA9
		public Widget SuccessWidget { get; set; }

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06001361 RID: 4961 RVA: 0x000349B2 File Offset: 0x00032BB2
		// (set) Token: 0x06001362 RID: 4962 RVA: 0x000349BA File Offset: 0x00032BBA
		public Widget CritSuccessWidget { get; set; }

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06001363 RID: 4963 RVA: 0x000349C3 File Offset: 0x00032BC3
		// (set) Token: 0x06001364 RID: 4964 RVA: 0x000349CB File Offset: 0x00032BCB
		public bool IsResultReady { get; set; }

		// Token: 0x06001365 RID: 4965 RVA: 0x000349D4 File Offset: 0x00032BD4
		public PersuasionResultChanceContainerListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x000349F4 File Offset: 0x00032BF4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.IsResultReady)
			{
				if (this._delayStartTime == -1f && base.AlphaFactor <= 0.001f)
				{
					this._delayStartTime = base.EventManager.Time;
				}
				float num = Mathf.Lerp(base.AlphaFactor, 0f, 0.35f);
				this.SetGlobalAlphaRecursively(num);
				Widget resultVisualWidget = this._resultVisualWidget;
				if (resultVisualWidget != null)
				{
					resultVisualWidget.SetGlobalAlphaRecursively(1f);
				}
				if (this._delayStartTime != -1f && base.EventManager.Time - this._delayStartTime > this.StayTime)
				{
					base.EventFired("OnReadyToContinue", Array.Empty<object>());
				}
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06001367 RID: 4967 RVA: 0x00034AA8 File Offset: 0x00032CA8
		// (set) Token: 0x06001368 RID: 4968 RVA: 0x00034AB0 File Offset: 0x00032CB0
		[Editor(false)]
		public int ResultIndex
		{
			get
			{
				return this._resultIndex;
			}
			set
			{
				if (value != this._resultIndex)
				{
					this._resultIndex = value;
					base.OnPropertyChanged(value, "ResultIndex");
					switch (value)
					{
					case 0:
						this._resultVisualWidget = this.CritFailWidget;
						this.SetState("CriticalFail");
						return;
					case 1:
						this._resultVisualWidget = this.FailWidget;
						this.SetState("Fail");
						return;
					case 2:
						this._resultVisualWidget = this.SuccessWidget;
						this.SetState("Success");
						return;
					case 3:
						this._resultVisualWidget = this.CritSuccessWidget;
						this.SetState("CriticalSuccess");
						break;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x040008C9 RID: 2249
		private Widget _resultVisualWidget;

		// Token: 0x040008CB RID: 2251
		private float _delayStartTime = -1f;

		// Token: 0x040008CC RID: 2252
		private int _resultIndex;
	}
}
