using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace SandBox.View.Map.Navigation
{
	// Token: 0x02000066 RID: 102
	public abstract class MapNavigationElementBase : INavigationElement
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x00024155 File Offset: 0x00022355
		public NavigationPermissionItem Permission
		{
			get
			{
				return this.GetPermission();
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x0002415D File Offset: 0x0002235D
		public TextObject Tooltip
		{
			get
			{
				return this.GetTooltip();
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x00024165 File Offset: 0x00022365
		public TextObject AlertTooltip
		{
			get
			{
				return this.GetAlertTooltip();
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000460 RID: 1120
		public abstract bool IsActive { get; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000461 RID: 1121
		public abstract bool IsLockingNavigation { get; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000462 RID: 1122
		public abstract bool HasAlert { get; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000463 RID: 1123
		public abstract string StringId { get; }

		// Token: 0x06000464 RID: 1124
		public abstract void OpenView();

		// Token: 0x06000465 RID: 1125
		public abstract void OpenView(params object[] parameters);

		// Token: 0x06000466 RID: 1126
		public abstract void GoToLink();

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x0002416D File Offset: 0x0002236D
		protected Game _game
		{
			get
			{
				return Game.Current;
			}
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00024174 File Offset: 0x00022374
		public MapNavigationElementBase(MapNavigationHandler handler)
		{
			this._handler = handler;
			this._viewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
		}

		// Token: 0x06000469 RID: 1129
		protected abstract NavigationPermissionItem GetPermission();

		// Token: 0x0600046A RID: 1130
		protected abstract TextObject GetTooltip();

		// Token: 0x0600046B RID: 1131
		protected abstract TextObject GetAlertTooltip();

		// Token: 0x0400021C RID: 540
		protected readonly MapNavigationHandler _handler;

		// Token: 0x0400021D RID: 541
		protected readonly IViewDataTracker _viewDataTracker;
	}
}
