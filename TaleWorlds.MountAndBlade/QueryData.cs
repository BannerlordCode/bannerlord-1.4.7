using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000179 RID: 377
	public class QueryData<T> : IQueryData
	{
		// Token: 0x060013E7 RID: 5095 RVA: 0x0004991E File Offset: 0x00047B1E
		public QueryData(Func<T> valueFunc, float lifetime)
		{
			this._cachedValue = default(T);
			this._expireTime = 0f;
			this._lifetime = lifetime;
			this._valueFunc = valueFunc;
			this._syncGroup = null;
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x00049952 File Offset: 0x00047B52
		public QueryData(Func<T> valueFunc, float lifetime, T defaultCachedValue)
		{
			this._cachedValue = defaultCachedValue;
			this._expireTime = 0f;
			this._lifetime = lifetime;
			this._valueFunc = valueFunc;
			this._syncGroup = null;
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x00049981 File Offset: 0x00047B81
		public void Evaluate(float currentTime)
		{
			this.SetValue(this._valueFunc(), currentTime);
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x00049995 File Offset: 0x00047B95
		public void SetValue(T value, float currentTime)
		{
			this._cachedValue = value;
			this._expireTime = currentTime + this._lifetime;
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x000499AC File Offset: 0x00047BAC
		public T GetCachedValue()
		{
			return this._cachedValue;
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x000499B4 File Offset: 0x00047BB4
		public T GetCachedValueUnlessTooOld()
		{
			return this._cachedValue;
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x000499BC File Offset: 0x00047BBC
		public T GetCachedValueWithMaxAge(float age)
		{
			if (Mission.Current.CurrentTime > this._expireTime - this._lifetime + MathF.Min(this._lifetime, age))
			{
				this.Expire();
				return this.Value;
			}
			return this._cachedValue;
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x060013EE RID: 5102 RVA: 0x000499F8 File Offset: 0x00047BF8
		public T Value
		{
			get
			{
				float currentTime = Mission.Current.CurrentTime;
				if (currentTime >= this._expireTime)
				{
					if (this._syncGroup != null)
					{
						IQueryData[] syncGroup = this._syncGroup;
						for (int i = 0; i < syncGroup.Length; i++)
						{
							syncGroup[i].Evaluate(currentTime);
						}
					}
					this.Evaluate(currentTime);
				}
				return this._cachedValue;
			}
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x00049A4C File Offset: 0x00047C4C
		public void Expire()
		{
			this._expireTime = 0f;
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x00049A5C File Offset: 0x00047C5C
		public static void SetupSyncGroup(params IQueryData[] groupItems)
		{
			for (int i = 0; i < groupItems.Length; i++)
			{
				groupItems[i].SetSyncGroup(groupItems);
			}
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x00049A82 File Offset: 0x00047C82
		public void SetSyncGroup(IQueryData[] syncGroup)
		{
			this._syncGroup = syncGroup;
		}

		// Token: 0x0400052C RID: 1324
		private T _cachedValue;

		// Token: 0x0400052D RID: 1325
		private float _expireTime;

		// Token: 0x0400052E RID: 1326
		private readonly float _lifetime;

		// Token: 0x0400052F RID: 1327
		private readonly Func<T> _valueFunc;

		// Token: 0x04000530 RID: 1328
		private IQueryData[] _syncGroup;
	}
}
