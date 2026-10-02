using System;
using TaleWorlds.Engine.Options;

namespace TaleWorlds.MountAndBlade.Options.ManagedOptions
{
	// Token: 0x0200039B RID: 923
	public abstract class ManagedOptionData : IOptionData
	{
		// Token: 0x060034BA RID: 13498 RVA: 0x000D9368 File Offset: 0x000D7568
		protected ManagedOptionData(ManagedOptions.ManagedOptionsType type)
		{
			this.Type = type;
			this._value = ManagedOptions.GetConfig(type);
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x000D9383 File Offset: 0x000D7583
		public virtual float GetDefaultValue()
		{
			return ManagedOptions.GetDefaultConfig(this.Type);
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x000D9390 File Offset: 0x000D7590
		public void Commit()
		{
			if (this._value != ManagedOptions.GetConfig(this.Type))
			{
				ManagedOptions.SetConfig(this.Type, this._value);
			}
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x000D93B6 File Offset: 0x000D75B6
		public float GetValue(bool forceRefresh)
		{
			if (forceRefresh)
			{
				this._value = ManagedOptions.GetConfig(this.Type);
			}
			return this._value;
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x000D93D2 File Offset: 0x000D75D2
		public void SetValue(float value)
		{
			this._value = value;
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x000D93DB File Offset: 0x000D75DB
		public object GetOptionType()
		{
			return this.Type;
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x000D93E8 File Offset: 0x000D75E8
		public bool IsNative()
		{
			return false;
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x000D93EB File Offset: 0x000D75EB
		public bool IsAction()
		{
			return false;
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x000D93F0 File Offset: 0x000D75F0
		public ValueTuple<string, bool> GetIsDisabledAndReasonID()
		{
			ManagedOptions.ManagedOptionsType type = this.Type;
			if (type - ManagedOptions.ManagedOptionsType.ControlBlockDirection <= 1 && BannerlordConfig.GyroOverrideForAttackDefend)
			{
				return new ValueTuple<string, bool>("str_gyro_overrides_attack_block_direction", true);
			}
			return new ValueTuple<string, bool>(string.Empty, false);
		}

		// Token: 0x04001663 RID: 5731
		public readonly ManagedOptions.ManagedOptionsType Type;

		// Token: 0x04001664 RID: 5732
		private float _value;
	}
}
