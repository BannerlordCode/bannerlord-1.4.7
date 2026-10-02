using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x02000063 RID: 99
	public class PartyFormationDropdownWidget : DropdownWidget
	{
		// Token: 0x0600054B RID: 1355 RVA: 0x00010053 File Offset: 0x0000E253
		public PartyFormationDropdownWidget(UIContext context)
			: base(context)
		{
			base.DoNotHandleDropdownListPanel = true;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00010063 File Offset: 0x0000E263
		private void ListStateChangerUpdated()
		{
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00010065 File Offset: 0x0000E265
		private void SeperatorStateChangerUpdated()
		{
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00010068 File Offset: 0x0000E268
		protected override void OpenPanel()
		{
			base.ListPanel.IsVisible = true;
			this.SeperatorStateChanger.IsVisible = true;
			this.ListStateChanger.Delay = this.SeperatorStateChanger.VisualDefinition.TransitionDuration;
			this.ListStateChanger.State = "Opened";
			this.ListStateChanger.Start();
			this.SeperatorStateChanger.Delay = 0f;
			this.SeperatorStateChanger.State = "Opened";
			this.SeperatorStateChanger.Start();
			base.Context.TwoDimensionContext.PlaySound("dropdown");
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00010104 File Offset: 0x0000E304
		protected override void ClosePanel()
		{
			this.ListStateChanger.Delay = 0f;
			this.ListStateChanger.State = "Closed";
			this.ListStateChanger.Start();
			this.SeperatorStateChanger.Delay = this.ListStateChanger.TargetWidget.VisualDefinition.TransitionDuration;
			this.SeperatorStateChanger.State = "Closed";
			this.SeperatorStateChanger.Start();
			base.Context.TwoDimensionContext.PlaySound("dropdown");
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x0001018C File Offset: 0x0000E38C
		// (set) Token: 0x06000551 RID: 1361 RVA: 0x00010194 File Offset: 0x0000E394
		[Editor(false)]
		public DelayedStateChanger SeperatorStateChanger
		{
			get
			{
				return this._seperatorStateChanger;
			}
			set
			{
				if (this._seperatorStateChanger != value)
				{
					this._seperatorStateChanger = value;
					base.OnPropertyChanged<DelayedStateChanger>(value, "SeperatorStateChanger");
					this.SeperatorStateChangerUpdated();
				}
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x000101B8 File Offset: 0x0000E3B8
		// (set) Token: 0x06000553 RID: 1363 RVA: 0x000101C0 File Offset: 0x0000E3C0
		[Editor(false)]
		public DelayedStateChanger ListStateChanger
		{
			get
			{
				return this._listStateChanger;
			}
			set
			{
				if (this._listStateChanger != value)
				{
					this._listStateChanger = value;
					base.OnPropertyChanged<DelayedStateChanger>(value, "ListStateChanger");
					this.ListStateChangerUpdated();
				}
			}
		}

		// Token: 0x04000247 RID: 583
		private DelayedStateChanger _seperatorStateChanger;

		// Token: 0x04000248 RID: 584
		private DelayedStateChanger _listStateChanger;
	}
}
