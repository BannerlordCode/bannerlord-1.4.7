using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.ClassLoadout
{
	// Token: 0x020000D0 RID: 208
	public class MultiplayerClassLoadoutTroopTupleVisualWidget : Widget
	{
		// Token: 0x06000AC4 RID: 2756 RVA: 0x0001E1F9 File Offset: 0x0001C3F9
		public MultiplayerClassLoadoutTroopTupleVisualWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0001E204 File Offset: 0x0001C404
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				base.Sprite = base.Context.SpriteData.GetSprite("MPClassLoadout\\TroopTupleImages\\" + this.TroopTypeCode + "1");
				base.Sprite = base.Sprite;
				base.SuggestedWidth = (float)base.Sprite.Width;
				base.SuggestedHeight = (float)base.Sprite.Height;
				this._initialized = true;
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x0001E282 File Offset: 0x0001C482
		// (set) Token: 0x06000AC7 RID: 2759 RVA: 0x0001E28A File Offset: 0x0001C48A
		public string FactionCode
		{
			get
			{
				return this._factionCode;
			}
			set
			{
				if (value != this._factionCode)
				{
					this._factionCode = value;
					base.OnPropertyChanged<string>(value, "FactionCode");
				}
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x0001E2AD File Offset: 0x0001C4AD
		// (set) Token: 0x06000AC9 RID: 2761 RVA: 0x0001E2B5 File Offset: 0x0001C4B5
		public string TroopTypeCode
		{
			get
			{
				return this._troopTypeCode;
			}
			set
			{
				if (value != this._troopTypeCode)
				{
					this._troopTypeCode = value;
					base.OnPropertyChanged<string>(value, "TroopTypeCode");
				}
			}
		}

		// Token: 0x040004E5 RID: 1253
		private bool _initialized;

		// Token: 0x040004E6 RID: 1254
		private string _factionCode;

		// Token: 0x040004E7 RID: 1255
		private string _troopTypeCode;
	}
}
