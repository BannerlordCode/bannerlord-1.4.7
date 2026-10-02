using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x0200008D RID: 141
	public class MultiplayerPlayerBadgeVisualWidget : Widget
	{
		// Token: 0x060007BC RID: 1980 RVA: 0x00016726 File Offset: 0x00014926
		public MultiplayerPlayerBadgeVisualWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0001672F File Offset: 0x0001492F
		private void UpdateVisual(string badgeId)
		{
			if (badgeId == "badge_official_server_admin")
			{
				badgeId = "badge_taleworlds_dev";
			}
			base.Sprite = base.Context.SpriteData.GetSprite("MPPlayerBadges\\" + badgeId);
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00016766 File Offset: 0x00014966
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._hasForcedSize)
			{
				base.SuggestedWidth = this._forcedWidth;
				base.SuggestedHeight = this._forcedHeight;
			}
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0001678F File Offset: 0x0001498F
		public void SetForcedSize(float width, float height)
		{
			this._forcedWidth = width;
			this._forcedHeight = height;
			this._hasForcedSize = true;
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x000167A6 File Offset: 0x000149A6
		// (set) Token: 0x060007C1 RID: 1985 RVA: 0x000167AE File Offset: 0x000149AE
		public string BadgeId
		{
			get
			{
				return this._badgeId;
			}
			set
			{
				if (value != this._badgeId)
				{
					this._badgeId = value;
					base.OnPropertyChanged<string>(value, "BadgeId");
					this.UpdateVisual(value);
				}
			}
		}

		// Token: 0x04000364 RID: 868
		private float _forcedWidth;

		// Token: 0x04000365 RID: 869
		private float _forcedHeight;

		// Token: 0x04000366 RID: 870
		private bool _hasForcedSize;

		// Token: 0x04000367 RID: 871
		private string _badgeId;
	}
}
