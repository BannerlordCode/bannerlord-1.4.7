using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual
{
	// Token: 0x0200002A RID: 42
	public abstract class VisualOrder
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600036C RID: 876 RVA: 0x0000D0E5 File Offset: 0x0000B2E5
		public string StringId { get; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600036D RID: 877 RVA: 0x0000D0ED File Offset: 0x0000B2ED
		public string IconId
		{
			get
			{
				return this.GetIconId();
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000D0F5 File Offset: 0x0000B2F5
		public VisualOrder(string stringId)
		{
			this.StringId = stringId;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000D104 File Offset: 0x0000B304
		protected virtual string GetIconId()
		{
			return this.StringId;
		}

		// Token: 0x06000370 RID: 880
		public abstract TextObject GetName(OrderController orderController);

		// Token: 0x06000371 RID: 881
		public abstract bool IsTargeted();

		// Token: 0x06000372 RID: 882
		public abstract void ExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters);

		// Token: 0x06000373 RID: 883 RVA: 0x0000D10C File Offset: 0x0000B30C
		public virtual void BeforeExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000D10E File Offset: 0x0000B30E
		public virtual void AfterExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
		}

		// Token: 0x06000375 RID: 885
		protected abstract bool? OnGetFormationHasOrder(Formation formation);

		// Token: 0x06000376 RID: 886 RVA: 0x0000D110 File Offset: 0x0000B310
		public bool GetFormationHasOrder(Formation formation)
		{
			bool? flag = this.OnGetFormationHasOrder(formation);
			bool flag2 = true;
			return (flag.GetValueOrDefault() == flag2) & (flag != null);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000D139 File Offset: 0x0000B339
		public OrderState GetActiveState(OrderController orderController)
		{
			this._lastActiveState = this.GetActiveStateAux(orderController);
			return this._lastActiveState;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000D150 File Offset: 0x0000B350
		private OrderState GetActiveStateAux(OrderController orderController)
		{
			if (orderController.SelectedFormations == null || orderController.SelectedFormations.Count == 0)
			{
				return OrderState.Default;
			}
			int num = orderController.SelectedFormations.Count;
			int num2 = 0;
			MBReadOnlyList<Formation> selectedFormations = orderController.SelectedFormations;
			for (int i = 0; i < selectedFormations.Count; i++)
			{
				Formation formation = selectedFormations[i];
				bool? flag = this.OnGetFormationHasOrder(formation);
				if (flag == null)
				{
					num--;
				}
				else
				{
					bool? flag2 = flag;
					bool flag3 = true;
					if ((flag2.GetValueOrDefault() == flag3) & (flag2 != null))
					{
						num2++;
					}
				}
			}
			if (num2 == 0)
			{
				return OrderState.Default;
			}
			if (num2 < num)
			{
				return OrderState.PartiallyActive;
			}
			if (num2 == num)
			{
				return OrderState.Active;
			}
			return OrderState.Default;
		}

		// Token: 0x04000189 RID: 393
		protected OrderState _lastActiveState;
	}
}
