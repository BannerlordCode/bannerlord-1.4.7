using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000041 RID: 65
	public class SkillIconVisualWidget : Widget
	{
		// Token: 0x060003BD RID: 957 RVA: 0x0000BE6A File Offset: 0x0000A06A
		public SkillIconVisualWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000BE7C File Offset: 0x0000A07C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._requiresRefresh)
			{
				if (this.SkillId == null)
				{
					Debug.FailedAssert("SkillIconVisualWidget.OnLateUpdate called before SkillId has been set, or SkillId is set to null!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\SkillIconVisualWidget.cs", "OnLateUpdate", 21);
					this._requiresRefresh = false;
					return;
				}
				string text = "SPGeneral\\Skills\\gui_skills_icon_" + this.SkillId.ToLower();
				if (this.UseSmallestVariation && base.Context.SpriteData.GetSprite(text + "_tiny") != null)
				{
					base.Sprite = base.Context.SpriteData.GetSprite(text + "_tiny");
				}
				else if (this.UseSmallVariation && base.Context.SpriteData.GetSprite(text + "_small") != null)
				{
					base.Sprite = base.Context.SpriteData.GetSprite(text + "_small");
				}
				else if (base.Context.SpriteData.GetSprite(text) != null)
				{
					base.Sprite = base.Context.SpriteData.GetSprite(text);
				}
				this._requiresRefresh = false;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060003BF RID: 959 RVA: 0x0000BF98 File Offset: 0x0000A198
		// (set) Token: 0x060003C0 RID: 960 RVA: 0x0000BFA0 File Offset: 0x0000A1A0
		[Editor(false)]
		public string SkillId
		{
			get
			{
				return this._skillId;
			}
			set
			{
				if (this._skillId != value)
				{
					this._skillId = value;
					base.OnPropertyChanged<string>(value, "SkillId");
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x0000BFCA File Offset: 0x0000A1CA
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x0000BFD2 File Offset: 0x0000A1D2
		[Editor(false)]
		public bool UseSmallVariation
		{
			get
			{
				return this._useSmallVariation;
			}
			set
			{
				if (this._useSmallVariation != value)
				{
					this._useSmallVariation = value;
					base.OnPropertyChanged(value, "UseSmallVariation");
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x0000BFF7 File Offset: 0x0000A1F7
		// (set) Token: 0x060003C4 RID: 964 RVA: 0x0000BFFF File Offset: 0x0000A1FF
		[Editor(false)]
		public bool UseSmallestVariation
		{
			get
			{
				return this._useSmallestVariation;
			}
			set
			{
				if (this._useSmallestVariation != value)
				{
					this._useSmallestVariation = value;
					base.OnPropertyChanged(value, "UseSmallestVariation");
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x04000192 RID: 402
		private bool _requiresRefresh = true;

		// Token: 0x04000193 RID: 403
		private string _skillId;

		// Token: 0x04000194 RID: 404
		private bool _useSmallVariation;

		// Token: 0x04000195 RID: 405
		private bool _useSmallestVariation;
	}
}
