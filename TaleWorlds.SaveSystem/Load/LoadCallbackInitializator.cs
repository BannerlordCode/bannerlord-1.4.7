using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x02000039 RID: 57
	internal class LoadCallbackInitializator
	{
		// Token: 0x0600023D RID: 573 RVA: 0x0000B30F File Offset: 0x0000950F
		public LoadCallbackInitializator(LoadData loadData, ObjectHeaderLoadData[] objectHeaderLoadDatas, int objectCount)
		{
			this._loadData = loadData;
			this._objectHeaderLoadDatas = objectHeaderLoadDatas;
			this._objectCount = objectCount;
			this._objectLoadDatas = new Dictionary<int, ObjectLoadData>();
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000B338 File Offset: 0x00009538
		public void InitializeObjects()
		{
			using (new PerformanceTestBlock("LoadContext::Callbacks"))
			{
				for (int i = 0; i < this._objectCount; i++)
				{
					ObjectHeaderLoadData objectHeaderLoadData = this._objectHeaderLoadDatas[i];
					if (objectHeaderLoadData.Target != null)
					{
						TypeDefinition typeDefinition = objectHeaderLoadData.TypeDefinition;
						IEnumerable<MethodInfo> enumerable = ((typeDefinition != null) ? typeDefinition.InitializationCallbacks : null);
						if (enumerable != null)
						{
							foreach (MethodInfo methodInfo in enumerable)
							{
								ParameterInfo[] parameters = methodInfo.GetParameters();
								if (parameters.Length > 1 && parameters[1].ParameterType == typeof(ObjectLoadData))
								{
									ObjectLoadData objectLoadData = this.GetObjectLoadData(objectHeaderLoadData, i);
									methodInfo.Invoke(objectHeaderLoadData.Target, new object[]
									{
										this._loadData.MetaData,
										objectLoadData
									});
								}
								else if (parameters.Length == 1)
								{
									methodInfo.Invoke(objectHeaderLoadData.Target, new object[] { this._loadData.MetaData });
								}
								else
								{
									methodInfo.Invoke(objectHeaderLoadData.Target, null);
								}
							}
						}
					}
				}
			}
			GC.Collect();
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000B4A0 File Offset: 0x000096A0
		public void AfterInitializeObjects()
		{
			using (new PerformanceTestBlock("LoadContext::AfterCallbacks"))
			{
				for (int i = 0; i < this._objectCount; i++)
				{
					ObjectHeaderLoadData objectHeaderLoadData = this._objectHeaderLoadDatas[i];
					if (objectHeaderLoadData.Target != null)
					{
						TypeDefinition typeDefinition = objectHeaderLoadData.TypeDefinition;
						IEnumerable<MethodInfo> enumerable = ((typeDefinition != null) ? typeDefinition.LateInitializationCallbacks : null);
						if (enumerable != null)
						{
							foreach (MethodInfo methodInfo in enumerable)
							{
								ParameterInfo[] parameters = methodInfo.GetParameters();
								if (parameters.Length > 1 && parameters[1].ParameterType == typeof(ObjectLoadData))
								{
									ObjectLoadData objectLoadData = this.GetObjectLoadData(objectHeaderLoadData, i);
									methodInfo.Invoke(objectHeaderLoadData.Target, new object[]
									{
										this._loadData.MetaData,
										objectLoadData
									});
								}
								else if (parameters.Length == 1)
								{
									methodInfo.Invoke(objectHeaderLoadData.Target, new object[] { this._loadData.MetaData });
								}
								else
								{
									methodInfo.Invoke(objectHeaderLoadData.Target, null);
								}
							}
						}
					}
				}
			}
			this._objectLoadDatas.Clear();
			GC.Collect();
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000B614 File Offset: 0x00009814
		private ObjectLoadData GetObjectLoadData(ObjectHeaderLoadData objectHeaderLoadData, int i)
		{
			ObjectLoadData objectLoadData;
			if (!this._objectLoadDatas.TryGetValue(i, out objectLoadData))
			{
				objectLoadData = LoadContext.CreateLoadData(this._loadData, i, objectHeaderLoadData);
				this._objectLoadDatas[i] = objectLoadData;
			}
			return objectLoadData;
		}

		// Token: 0x040000A7 RID: 167
		private ObjectHeaderLoadData[] _objectHeaderLoadDatas;

		// Token: 0x040000A8 RID: 168
		private int _objectCount;

		// Token: 0x040000A9 RID: 169
		private LoadData _loadData;

		// Token: 0x040000AA RID: 170
		private Dictionary<int, ObjectLoadData> _objectLoadDatas;
	}
}
