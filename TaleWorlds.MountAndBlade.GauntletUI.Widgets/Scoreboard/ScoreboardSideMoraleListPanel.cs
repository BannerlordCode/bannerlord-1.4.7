using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Scoreboard
{
	// Token: 0x02000059 RID: 89
	public class ScoreboardSideMoraleListPanel : ListPanel
	{
		// Token: 0x060004E9 RID: 1257 RVA: 0x0000F4A2 File Offset: 0x0000D6A2
		public ScoreboardSideMoraleListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0000F4B8 File Offset: 0x0000D6B8
		private void OnMoraleUpdated()
		{
			if (base.ChildCount > 0)
			{
				float num = ((this.MaxMorale != 0f) ? (this.Morale / this.MaxMorale * 100f) : 0f);
				num = MathF.Clamp(num, 0f, 100f);
				num = (float)MathF.Round(num);
				float num2 = (float)(100 / base.ChildCount);
				for (int i = 0; i < base.ChildCount; i++)
				{
					float num3 = (num - (float)i * num2) / num2;
					num3 = MathF.Clamp(num3, 0f, 1f);
					Widget child = base.GetChild(i);
					if (num3 > 0f)
					{
						child.SetState("Default");
					}
					else
					{
						child.SetState("Disabled");
					}
					(child as FillBarWidget).InitialAmountAsFloat = num3;
				}
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x0000F580 File Offset: 0x0000D780
		// (set) Token: 0x060004EC RID: 1260 RVA: 0x0000F588 File Offset: 0x0000D788
		[Editor(false)]
		public float Morale
		{
			get
			{
				return this._morale;
			}
			set
			{
				if (this._morale != value)
				{
					this._morale = value;
					base.OnPropertyChanged(value, "Morale");
					this.OnMoraleUpdated();
				}
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0000F5AC File Offset: 0x0000D7AC
		// (set) Token: 0x060004EE RID: 1262 RVA: 0x0000F5B4 File Offset: 0x0000D7B4
		[Editor(false)]
		public float MaxMorale
		{
			get
			{
				return this._maxMorale;
			}
			set
			{
				if (this._maxMorale != value)
				{
					this._maxMorale = value;
					base.OnPropertyChanged(value, "MaxMorale");
					this.OnMoraleUpdated();
				}
			}
		}

		// Token: 0x0400021C RID: 540
		private float _morale;

		// Token: 0x0400021D RID: 541
		private float _maxMorale = 100f;
	}
}
