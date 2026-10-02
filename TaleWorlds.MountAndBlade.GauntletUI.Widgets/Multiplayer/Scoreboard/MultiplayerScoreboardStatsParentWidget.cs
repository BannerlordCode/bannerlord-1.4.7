using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Scoreboard
{
	// Token: 0x02000096 RID: 150
	public class MultiplayerScoreboardStatsParentWidget : Widget
	{
		// Token: 0x0600081E RID: 2078 RVA: 0x0001768E File Offset: 0x0001588E
		public MultiplayerScoreboardStatsParentWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00017698 File Offset: 0x00015898
		private void RefreshActiveState()
		{
			float num = (this.IsActive ? this.ActiveAlpha : this.InactiveAlpha);
			List<Widget> allChildrenRecursive = base.GetAllChildrenRecursive(null);
			for (int i = 0; i < allChildrenRecursive.Count; i++)
			{
				RichTextWidget richTextWidget;
				TextWidget textWidget;
				if ((richTextWidget = allChildrenRecursive[i] as RichTextWidget) != null)
				{
					richTextWidget.SetAlpha(num);
				}
				else if ((textWidget = allChildrenRecursive[i] as TextWidget) != null)
				{
					textWidget.SetAlpha(num);
				}
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x00017707 File Offset: 0x00015907
		// (set) Token: 0x06000821 RID: 2081 RVA: 0x0001770F File Offset: 0x0001590F
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChanged(value, "IsActive");
					this.RefreshActiveState();
				}
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x00017733 File Offset: 0x00015933
		// (set) Token: 0x06000823 RID: 2083 RVA: 0x0001773E File Offset: 0x0001593E
		public bool IsInactive
		{
			get
			{
				return !this.IsActive;
			}
			set
			{
				if (value == this.IsActive)
				{
					this.IsActive = !value;
					base.OnPropertyChanged(value, "IsInactive");
				}
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x0001775F File Offset: 0x0001595F
		// (set) Token: 0x06000825 RID: 2085 RVA: 0x00017767 File Offset: 0x00015967
		public float ActiveAlpha
		{
			get
			{
				return this._activeAlpha;
			}
			set
			{
				if (value != this._activeAlpha)
				{
					this._activeAlpha = value;
					base.OnPropertyChanged(value, "ActiveAlpha");
				}
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x00017785 File Offset: 0x00015985
		// (set) Token: 0x06000827 RID: 2087 RVA: 0x0001778D File Offset: 0x0001598D
		public float InactiveAlpha
		{
			get
			{
				return this._inactiveAlpha;
			}
			set
			{
				if (value != this._inactiveAlpha)
				{
					this._inactiveAlpha = value;
					base.OnPropertyChanged(value, "InactiveAlpha");
				}
			}
		}

		// Token: 0x040003A3 RID: 931
		private bool _isActive;

		// Token: 0x040003A4 RID: 932
		private float _activeAlpha;

		// Token: 0x040003A5 RID: 933
		private float _inactiveAlpha;
	}
}
