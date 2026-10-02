using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000031 RID: 49
	public abstract class TextureProvider
	{
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600035D RID: 861 RVA: 0x0000F009 File Offset: 0x0000D209
		// (set) Token: 0x0600035E RID: 862 RVA: 0x0000F011 File Offset: 0x0000D211
		public string SourceInfo { get; set; }

		// Token: 0x0600035F RID: 863 RVA: 0x0000F01A File Offset: 0x0000D21A
		public virtual void SetTargetSize(int width, int height)
		{
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000F01C File Offset: 0x0000D21C
		public Texture GetTextureForRender(TwoDimensionContext context, string name = null)
		{
			return this.OnGetTextureForRender(context, name);
		}

		// Token: 0x06000361 RID: 865
		protected abstract Texture OnGetTextureForRender(TwoDimensionContext twoDimensionContext, string name);

		// Token: 0x06000362 RID: 866 RVA: 0x0000F026 File Offset: 0x0000D226
		public virtual void Tick(float dt)
		{
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000F028 File Offset: 0x0000D228
		public virtual void Clear(bool clearNextFrame)
		{
			this._getGetMethodCache.Clear();
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000F038 File Offset: 0x0000D238
		public void SetProperty(string name, object value)
		{
			PropertyInfo property = base.GetType().GetProperty(name);
			if (property != null)
			{
				property.GetSetMethod().Invoke(this, new object[] { value });
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000F074 File Offset: 0x0000D274
		public object GetProperty(string name)
		{
			MethodInfo methodInfo;
			if (this._getGetMethodCache.TryGetValue(name, out methodInfo))
			{
				return methodInfo.Invoke(this, null);
			}
			PropertyInfo property = base.GetType().GetProperty(name);
			if (property != null)
			{
				MethodInfo getMethod = property.GetGetMethod();
				this._getGetMethodCache.Add(name, getMethod);
				return getMethod.Invoke(this, null);
			}
			return null;
		}

		// Token: 0x040001A9 RID: 425
		private Dictionary<string, MethodInfo> _getGetMethodCache = new Dictionary<string, MethodInfo>();
	}
}
