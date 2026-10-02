using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Scoreboard
{
	// Token: 0x02000058 RID: 88
	public class ScoreboardShipsNavigatableGridWidget : NavigatableGridWidget
	{
		// Token: 0x060004E3 RID: 1251 RVA: 0x0000F352 File Offset: 0x0000D552
		public ScoreboardShipsNavigatableGridWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0000F35C File Offset: 0x0000D55C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			ScrollablePanel parentPanel = base.ParentPanel;
			bool flag;
			if (parentPanel == null)
			{
				flag = false;
			}
			else
			{
				ScrollbarWidget activeScrollbar = parentPanel.ActiveScrollbar;
				bool? flag2 = ((activeScrollbar != null) ? new bool?(activeScrollbar.IsVisible) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			if (flag)
			{
				base.HorizontalAlignment = this.OverflowHorizontalAlignment;
				return;
			}
			base.HorizontalAlignment = this.RegularHorizontalAlignment;
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x0000F3CB File Offset: 0x0000D5CB
		// (set) Token: 0x060004E6 RID: 1254 RVA: 0x0000F3D4 File Offset: 0x0000D5D4
		[Editor(false)]
		public HorizontalAlignment RegularHorizontalAlignment
		{
			get
			{
				return this._regularHorizontalAlignment;
			}
			set
			{
				if (this._regularHorizontalAlignment != value)
				{
					this._regularHorizontalAlignment = value;
					switch (value)
					{
					case HorizontalAlignment.Left:
						base.OnPropertyChanged<string>("Left", "RegularHorizontalAlignment");
						return;
					case HorizontalAlignment.Center:
						base.OnPropertyChanged<string>("Center", "RegularHorizontalAlignment");
						return;
					case HorizontalAlignment.Right:
						base.OnPropertyChanged<string>("Right", "RegularHorizontalAlignment");
						break;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x0000F436 File Offset: 0x0000D636
		// (set) Token: 0x060004E8 RID: 1256 RVA: 0x0000F440 File Offset: 0x0000D640
		[Editor(false)]
		public HorizontalAlignment OverflowHorizontalAlignment
		{
			get
			{
				return this._overflowHorizontalAlignment;
			}
			set
			{
				if (this._overflowHorizontalAlignment != value)
				{
					this._overflowHorizontalAlignment = value;
					switch (value)
					{
					case HorizontalAlignment.Left:
						base.OnPropertyChanged<string>("Left", "OverflowHorizontalAlignment");
						return;
					case HorizontalAlignment.Center:
						base.OnPropertyChanged<string>("Center", "OverflowHorizontalAlignment");
						return;
					case HorizontalAlignment.Right:
						base.OnPropertyChanged<string>("Right", "OverflowHorizontalAlignment");
						break;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x0400021A RID: 538
		private HorizontalAlignment _regularHorizontalAlignment;

		// Token: 0x0400021B RID: 539
		private HorizontalAlignment _overflowHorizontalAlignment;
	}
}
