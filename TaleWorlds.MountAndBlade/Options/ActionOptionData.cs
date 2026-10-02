using System;
using TaleWorlds.Engine.Options;

namespace TaleWorlds.MountAndBlade.Options
{
	// Token: 0x02000395 RID: 917
	public class ActionOptionData : IOptionData
	{
		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x06003486 RID: 13446 RVA: 0x000D8E85 File Offset: 0x000D7085
		// (set) Token: 0x06003487 RID: 13447 RVA: 0x000D8E8D File Offset: 0x000D708D
		public Action OnAction { get; private set; }

		// Token: 0x06003488 RID: 13448 RVA: 0x000D8E96 File Offset: 0x000D7096
		public ActionOptionData(ManagedOptions.ManagedOptionsType managedType, Action onAction)
		{
			this._managedType = managedType;
			this.OnAction = onAction;
		}

		// Token: 0x06003489 RID: 13449 RVA: 0x000D8EAC File Offset: 0x000D70AC
		public ActionOptionData(NativeOptions.NativeOptionsType nativeType, Action onAction)
		{
			this._nativeType = nativeType;
			this.OnAction = onAction;
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x000D8EC2 File Offset: 0x000D70C2
		public ActionOptionData(string optionTypeId, Action onAction)
		{
			this._actionOptionTypeId = optionTypeId;
			this._nativeType = NativeOptions.NativeOptionsType.None;
			this.OnAction = onAction;
		}

		// Token: 0x0600348B RID: 13451 RVA: 0x000D8EDF File Offset: 0x000D70DF
		public void Commit()
		{
		}

		// Token: 0x0600348C RID: 13452 RVA: 0x000D8EE1 File Offset: 0x000D70E1
		public float GetDefaultValue()
		{
			return 0f;
		}

		// Token: 0x0600348D RID: 13453 RVA: 0x000D8EE8 File Offset: 0x000D70E8
		public object GetOptionType()
		{
			if (this._nativeType != NativeOptions.NativeOptionsType.None)
			{
				return this._nativeType;
			}
			if (this._managedType != ManagedOptions.ManagedOptionsType.Language)
			{
				return this._managedType;
			}
			return this._actionOptionTypeId;
		}

		// Token: 0x0600348E RID: 13454 RVA: 0x000D8F19 File Offset: 0x000D7119
		public float GetValue(bool forceRefresh)
		{
			return 0f;
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x000D8F20 File Offset: 0x000D7120
		public bool IsNative()
		{
			return this._nativeType != NativeOptions.NativeOptionsType.None;
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x000D8F2E File Offset: 0x000D712E
		public void SetValue(float value)
		{
		}

		// Token: 0x06003491 RID: 13457 RVA: 0x000D8F30 File Offset: 0x000D7130
		public bool IsAction()
		{
			return this._nativeType == NativeOptions.NativeOptionsType.None && this._managedType == ManagedOptions.ManagedOptionsType.Language;
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x000D8F46 File Offset: 0x000D7146
		public ValueTuple<string, bool> GetIsDisabledAndReasonID()
		{
			return new ValueTuple<string, bool>(string.Empty, false);
		}

		// Token: 0x04001656 RID: 5718
		private ManagedOptions.ManagedOptionsType _managedType;

		// Token: 0x04001657 RID: 5719
		private NativeOptions.NativeOptionsType _nativeType;

		// Token: 0x04001658 RID: 5720
		private string _actionOptionTypeId;
	}
}
