using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Information
{
	// Token: 0x02000147 RID: 327
	public class PropertyBasedTooltipWidget : TooltipWidget
	{
		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x0600113E RID: 4414 RVA: 0x0002F6A8 File Offset: 0x0002D8A8
		// (set) Token: 0x0600113F RID: 4415 RVA: 0x0002F6B0 File Offset: 0x0002D8B0
		public Color AllyColor { get; set; }

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001140 RID: 4416 RVA: 0x0002F6B9 File Offset: 0x0002D8B9
		// (set) Token: 0x06001141 RID: 4417 RVA: 0x0002F6C1 File Offset: 0x0002D8C1
		public Color EnemyColor { get; set; }

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001142 RID: 4418 RVA: 0x0002F6CA File Offset: 0x0002D8CA
		// (set) Token: 0x06001143 RID: 4419 RVA: 0x0002F6D2 File Offset: 0x0002D8D2
		public Color NeutralColor { get; set; }

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x0002F6DB File Offset: 0x0002D8DB
		// (set) Token: 0x06001145 RID: 4421 RVA: 0x0002F6E3 File Offset: 0x0002D8E3
		public Widget PropertyListBackground { get; set; }

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06001146 RID: 4422 RVA: 0x0002F6EC File Offset: 0x0002D8EC
		// (set) Token: 0x06001147 RID: 4423 RVA: 0x0002F6F4 File Offset: 0x0002D8F4
		public ListPanel PropertyList { get; set; }

		// Token: 0x06001148 RID: 4424 RVA: 0x0002F6FD File Offset: 0x0002D8FD
		public PropertyBasedTooltipWidget(UIContext context)
			: base(context)
		{
			this._animationDelayInFrames = 2;
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x0002F714 File Offset: 0x0002D914
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.UpdateBattleScopes();
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x0002F724 File Offset: 0x0002D924
		private void UpdateBattleScopes()
		{
			bool flag = false;
			foreach (TooltipPropertyWidget tooltipPropertyWidget in this.PropertyWidgets)
			{
				if (tooltipPropertyWidget.IsBattleMode)
				{
					flag = true;
				}
				else if (tooltipPropertyWidget.IsBattleModeOver)
				{
					flag = false;
				}
				tooltipPropertyWidget.SetBattleScope(flag);
			}
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x0002F78C File Offset: 0x0002D98C
		private float GetBattleScopeSize()
		{
			bool flag = false;
			float num = 0f;
			if (this.PropertyList != null)
			{
				for (int i = 0; i < this.PropertyList.ChildCount; i++)
				{
					TooltipPropertyWidget tooltipPropertyWidget;
					if ((tooltipPropertyWidget = this.PropertyList.GetChild(i) as TooltipPropertyWidget) != null)
					{
						if (tooltipPropertyWidget.IsBattleMode)
						{
							flag = true;
						}
						else if (tooltipPropertyWidget.IsBattleModeOver)
						{
							flag = false;
						}
						if (flag)
						{
							float num2 = ((tooltipPropertyWidget.ValueLabel.Size.X > tooltipPropertyWidget.DefinitionLabel.Size.X) ? tooltipPropertyWidget.ValueLabel.Size.X : tooltipPropertyWidget.DefinitionLabel.Size.X);
							if (num2 > num)
							{
								num = num2;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x0002F848 File Offset: 0x0002DA48
		private void UpdateRelationBrushes()
		{
			TooltipPropertyWidget tooltipPropertyWidget = this.PropertyWidgets.SingleOrDefault<TooltipPropertyWidget>((TooltipPropertyWidget p) => p.IsRelation);
			if (tooltipPropertyWidget != null)
			{
				if ((tooltipPropertyWidget.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarFirstAlly) == TooltipPropertyWidget.TooltipPropertyFlags.WarFirstAlly)
				{
					this._definitionRelationBrush = this.AllyTroopsTextBrush;
				}
				else if ((tooltipPropertyWidget.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarFirstEnemy) == TooltipPropertyWidget.TooltipPropertyFlags.WarFirstEnemy)
				{
					this._definitionRelationBrush = this.EnemyTroopsTextBrush;
				}
				else
				{
					this._definitionRelationBrush = this.NeutralTroopsTextBrush;
				}
				if ((tooltipPropertyWidget.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarSecondAlly) == TooltipPropertyWidget.TooltipPropertyFlags.WarSecondAlly)
				{
					this._valueRelationBrush = this.AllyTroopsTextBrush;
					return;
				}
				if ((tooltipPropertyWidget.PropertyModifierAsFlag & TooltipPropertyWidget.TooltipPropertyFlags.WarSecondEnemy) == TooltipPropertyWidget.TooltipPropertyFlags.WarSecondEnemy)
				{
					this._valueRelationBrush = this.EnemyTroopsTextBrush;
					return;
				}
				this._valueRelationBrush = this.NeutralTroopsTextBrush;
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x0600114D RID: 4429 RVA: 0x0002F90C File Offset: 0x0002DB0C
		private IEnumerable<TooltipPropertyWidget> PropertyWidgets
		{
			get
			{
				if (this.PropertyList != null)
				{
					int num;
					for (int i = 0; i < this.PropertyList.ChildCount; i = num + 1)
					{
						TooltipPropertyWidget tooltipPropertyWidget;
						if ((tooltipPropertyWidget = this.PropertyList.GetChild(i) as TooltipPropertyWidget) != null)
						{
							yield return tooltipPropertyWidget;
						}
						num = i;
					}
				}
				yield break;
			}
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x0002F91C File Offset: 0x0002DB1C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			bool flag = false;
			float battleScopeSize = this.GetBattleScopeSize();
			float num = -1f;
			float num2 = -1f;
			this._definitionRelationBrush = null;
			this._valueRelationBrush = null;
			if (this.PropertyList != null)
			{
				if (this._firstFrame)
				{
					this._firstFrame = false;
					return;
				}
				for (int i = 0; i < this.PropertyList.ChildCount; i++)
				{
					TooltipPropertyWidget tooltipPropertyWidget;
					if ((tooltipPropertyWidget = this.PropertyList.GetChild(i) as TooltipPropertyWidget) != null && tooltipPropertyWidget.IsTwoColumn && !tooltipPropertyWidget.IsMultiLine)
					{
						if (num < tooltipPropertyWidget.ValueLabelContainer.Size.X)
						{
							num = tooltipPropertyWidget.ValueLabelContainer.Size.X;
						}
						if (num2 < tooltipPropertyWidget.DefinitionLabelContainer.Size.X)
						{
							num2 = tooltipPropertyWidget.DefinitionLabelContainer.Size.X;
						}
					}
				}
				for (int j = 0; j < this.PropertyList.ChildCount; j++)
				{
					TooltipPropertyWidget tooltipPropertyWidget2;
					if ((tooltipPropertyWidget2 = this.PropertyList.GetChild(j) as TooltipPropertyWidget) != null)
					{
						if (tooltipPropertyWidget2.IsBattleMode)
						{
							flag = true;
						}
						else if (tooltipPropertyWidget2.IsBattleModeOver)
						{
							flag = false;
						}
						if (flag && (this._definitionRelationBrush == null || this._valueRelationBrush == null))
						{
							this.UpdateRelationBrushes();
						}
						tooltipPropertyWidget2.RefreshSize(flag, battleScopeSize, num, num2, this._definitionRelationBrush, this._valueRelationBrush);
					}
				}
			}
			else
			{
				this._firstFrame = true;
			}
			if (this.PropertyListBackground != null)
			{
				if (this.Mode == 2)
				{
					this.PropertyListBackground.Color = this.AllyColor;
					return;
				}
				if (this.Mode == 3)
				{
					this.PropertyListBackground.Color = this.EnemyColor;
					return;
				}
				this.PropertyListBackground.Color = this.NeutralColor;
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x0600114F RID: 4431 RVA: 0x0002FAD1 File Offset: 0x0002DCD1
		// (set) Token: 0x06001150 RID: 4432 RVA: 0x0002FAD9 File Offset: 0x0002DCD9
		[Editor(false)]
		public int Mode
		{
			get
			{
				return this._mode;
			}
			set
			{
				if (this._mode != value)
				{
					this._mode = value;
				}
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06001151 RID: 4433 RVA: 0x0002FAEB File Offset: 0x0002DCEB
		// (set) Token: 0x06001152 RID: 4434 RVA: 0x0002FAF3 File Offset: 0x0002DCF3
		[Editor(false)]
		public Brush NeutralTroopsTextBrush
		{
			get
			{
				return this._neutralTroopsTextBrush;
			}
			set
			{
				if (this._neutralTroopsTextBrush != value)
				{
					this._neutralTroopsTextBrush = value;
					base.OnPropertyChanged<Brush>(value, "NeutralTroopsTextBrush");
				}
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06001153 RID: 4435 RVA: 0x0002FB11 File Offset: 0x0002DD11
		// (set) Token: 0x06001154 RID: 4436 RVA: 0x0002FB19 File Offset: 0x0002DD19
		[Editor(false)]
		public Brush EnemyTroopsTextBrush
		{
			get
			{
				return this._enemyTroopsTextBrush;
			}
			set
			{
				if (this._enemyTroopsTextBrush != value)
				{
					this._enemyTroopsTextBrush = value;
					base.OnPropertyChanged<Brush>(value, "EnemyTroopsTextBrush");
				}
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06001155 RID: 4437 RVA: 0x0002FB37 File Offset: 0x0002DD37
		// (set) Token: 0x06001156 RID: 4438 RVA: 0x0002FB3F File Offset: 0x0002DD3F
		[Editor(false)]
		public Brush AllyTroopsTextBrush
		{
			get
			{
				return this._allyTroopsTextBrush;
			}
			set
			{
				if (this._allyTroopsTextBrush != value)
				{
					this._allyTroopsTextBrush = value;
					base.OnPropertyChanged<Brush>(value, "AllyTroopsTextBrush");
				}
			}
		}

		// Token: 0x040007D6 RID: 2006
		private Brush _definitionRelationBrush;

		// Token: 0x040007D7 RID: 2007
		private Brush _valueRelationBrush;

		// Token: 0x040007D8 RID: 2008
		private bool _firstFrame = true;

		// Token: 0x040007D9 RID: 2009
		private int _mode;

		// Token: 0x040007DA RID: 2010
		private Brush _neutralTroopsTextBrush;

		// Token: 0x040007DB RID: 2011
		private Brush _allyTroopsTextBrush;

		// Token: 0x040007DC RID: 2012
		private Brush _enemyTroopsTextBrush;
	}
}
