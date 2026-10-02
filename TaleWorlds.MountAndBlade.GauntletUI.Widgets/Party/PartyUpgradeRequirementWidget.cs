using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x0200006E RID: 110
	public class PartyUpgradeRequirementWidget : Widget
	{
		// Token: 0x060005F9 RID: 1529 RVA: 0x00011BCC File Offset: 0x0000FDCC
		public PartyUpgradeRequirementWidget(UIContext context)
			: base(context)
		{
			this.NormalColor = new Color(1f, 1f, 1f, 1f);
			this.InsufficientColor = new Color(0.753f, 0.071f, 0.098f, 1f);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00011C28 File Offset: 0x0000FE28
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._requiresRefresh)
			{
				if (this.RequirementId != null)
				{
					string text = (this.IsPerkRequirement ? "SPGeneral\\Skills\\gui_skills_icon_" : "StdAssets\\ItemIcons\\");
					string text2 = (this.IsPerkRequirement ? "_tiny" : "");
					base.Sprite = base.Context.SpriteData.GetSprite(text + this.RequirementId + text2);
				}
				base.Color = (this.IsSufficient ? this.NormalColor : this.InsufficientColor);
				this._requiresRefresh = false;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060005FB RID: 1531 RVA: 0x00011CBC File Offset: 0x0000FEBC
		// (set) Token: 0x060005FC RID: 1532 RVA: 0x00011CC4 File Offset: 0x0000FEC4
		[Editor(false)]
		public string RequirementId
		{
			get
			{
				return this._requirementId;
			}
			set
			{
				if (value != this._requirementId)
				{
					this._requirementId = value;
					base.OnPropertyChanged<string>(value, "RequirementId");
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060005FD RID: 1533 RVA: 0x00011CEE File Offset: 0x0000FEEE
		// (set) Token: 0x060005FE RID: 1534 RVA: 0x00011CF6 File Offset: 0x0000FEF6
		[Editor(false)]
		public bool IsSufficient
		{
			get
			{
				return this._isSufficient;
			}
			set
			{
				if (value != this._isSufficient)
				{
					this._isSufficient = value;
					base.OnPropertyChanged(value, "IsSufficient");
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x00011D1B File Offset: 0x0000FF1B
		// (set) Token: 0x06000600 RID: 1536 RVA: 0x00011D23 File Offset: 0x0000FF23
		[Editor(false)]
		public bool IsPerkRequirement
		{
			get
			{
				return this._isPerkRequirement;
			}
			set
			{
				if (value != this._isPerkRequirement)
				{
					this._isPerkRequirement = value;
					base.OnPropertyChanged(value, "IsPerkRequirement");
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x00011D48 File Offset: 0x0000FF48
		// (set) Token: 0x06000602 RID: 1538 RVA: 0x00011D50 File Offset: 0x0000FF50
		public Color NormalColor
		{
			get
			{
				return this._normalColor;
			}
			set
			{
				if (value != this._normalColor)
				{
					this._normalColor = value;
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x00011D6E File Offset: 0x0000FF6E
		// (set) Token: 0x06000604 RID: 1540 RVA: 0x00011D76 File Offset: 0x0000FF76
		public Color InsufficientColor
		{
			get
			{
				return this._insufficientColor;
			}
			set
			{
				if (value != this._insufficientColor)
				{
					this._insufficientColor = value;
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x04000291 RID: 657
		private bool _requiresRefresh = true;

		// Token: 0x04000292 RID: 658
		private string _requirementId;

		// Token: 0x04000293 RID: 659
		private bool _isSufficient;

		// Token: 0x04000294 RID: 660
		private bool _isPerkRequirement;

		// Token: 0x04000295 RID: 661
		private Color _normalColor;

		// Token: 0x04000296 RID: 662
		private Color _insufficientColor;
	}
}
