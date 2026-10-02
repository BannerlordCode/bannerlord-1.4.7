using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Diamond.Lobby
{
	// Token: 0x0200016F RID: 367
	public abstract class MultiplayerLocalDataContainer<T> where T : MultiplayerLocalData
	{
		// Token: 0x06000A2A RID: 2602 RVA: 0x000103B3 File Offset: 0x0000E5B3
		public MultiplayerLocalDataContainer()
		{
			this._operationQueue = new List<MultiplayerLocalDataContainer<T>.ContainerOperation>();
			this._dataList = new List<T>();
			this._saveDirectoryName = this.GetSaveDirectoryName();
			this._saveFileName = this.GetSaveFileName();
			this._isCacheDirty = true;
		}

		// Token: 0x06000A2B RID: 2603
		protected abstract string GetSaveDirectoryName();

		// Token: 0x06000A2C RID: 2604
		protected abstract string GetSaveFileName();

		// Token: 0x06000A2D RID: 2605 RVA: 0x000103F0 File Offset: 0x0000E5F0
		public void AddEntry(T item)
		{
			List<MultiplayerLocalDataContainer<T>.ContainerOperation> operationQueue = this._operationQueue;
			lock (operationQueue)
			{
				MultiplayerLocalDataContainer<T>.ContainerOperation containerOperation = MultiplayerLocalDataContainer<T>.ContainerOperation.CreateAsAdd(item);
				this._operationQueue.Add(containerOperation);
			}
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x00010440 File Offset: 0x0000E640
		public void InsertEntry(T item, int index)
		{
			List<MultiplayerLocalDataContainer<T>.ContainerOperation> operationQueue = this._operationQueue;
			lock (operationQueue)
			{
				MultiplayerLocalDataContainer<T>.ContainerOperation containerOperation = MultiplayerLocalDataContainer<T>.ContainerOperation.CreateAsInsert(item, index);
				this._operationQueue.Add(containerOperation);
			}
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x00010490 File Offset: 0x0000E690
		public void RemoveEntry(T item)
		{
			List<MultiplayerLocalDataContainer<T>.ContainerOperation> operationQueue = this._operationQueue;
			lock (operationQueue)
			{
				MultiplayerLocalDataContainer<T>.ContainerOperation containerOperation = MultiplayerLocalDataContainer<T>.ContainerOperation.CreateAsRemove(item);
				this._operationQueue.Add(containerOperation);
			}
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x000104E0 File Offset: 0x0000E6E0
		public MBReadOnlyList<T> GetEntries()
		{
			return new MBReadOnlyList<T>(this._dataList);
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x000104F0 File Offset: 0x0000E6F0
		internal async Task Tick(float dt)
		{
			if (this._isCacheDirty)
			{
				await this.LoadFileAux();
				this._isCacheDirty = false;
			}
			while (this._operationQueue.Count > 0)
			{
				this.HandleOperation(this._operationQueue[0]);
				this._operationQueue.RemoveAt(0);
			}
			if (this._isFileDirty)
			{
				await this.SaveFileAux();
				this._isFileDirty = false;
			}
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x00010538 File Offset: 0x0000E738
		private void HandleOperation(MultiplayerLocalDataContainer<T>.ContainerOperation operation)
		{
			switch (operation.OperationType)
			{
			case MultiplayerLocalDataContainer<T>.OperationType.Add:
				this.AddEntryAux(operation.Item);
				return;
			case MultiplayerLocalDataContainer<T>.OperationType.Insert:
				this.InsertEntryAux(operation.Item, operation.Index);
				return;
			case MultiplayerLocalDataContainer<T>.OperationType.Remove:
				this.RemoveEntryAux(operation.Item);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0001058C File Offset: 0x0000E78C
		private void AddEntryAux(T item)
		{
			bool flag;
			this.OnBeforeAddEntry(item, out flag);
			if (!flag)
			{
				return;
			}
			bool flag2 = false;
			using (List<T>.Enumerator enumerator = this._dataList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.HasSameContentWith(item))
					{
						flag2 = true;
						break;
					}
				}
			}
			if (!flag2)
			{
				this._dataList.Add(item);
			}
			else
			{
				Debug.FailedAssert("Item is already in container: " + item, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\MultiplayerLocalDataManager.cs", "AddEntryAux", 234);
			}
			this._isFileDirty = true;
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x00010638 File Offset: 0x0000E838
		private void InsertEntryAux(T item, int index)
		{
			bool flag;
			this.OnBeforeAddEntry(item, out flag);
			if (!flag)
			{
				return;
			}
			bool flag2 = false;
			using (List<T>.Enumerator enumerator = this._dataList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.HasSameContentWith(item))
					{
						flag2 = true;
						break;
					}
				}
			}
			if (!flag2)
			{
				if (index >= 0 && index < this._dataList.Count)
				{
					this._dataList.Insert(index, item);
				}
				else
				{
					this._dataList.Add(item);
				}
			}
			else
			{
				Debug.FailedAssert("Item is already in container: " + item, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\MultiplayerLocalDataManager.cs", "InsertEntryAux", 272);
			}
			this._isFileDirty = true;
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x00010708 File Offset: 0x0000E908
		protected virtual void OnBeforeAddEntry(T item, out bool canAddEntry)
		{
			canAddEntry = true;
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x00010710 File Offset: 0x0000E910
		private void RemoveEntryAux(T item)
		{
			bool flag;
			this.OnBeforeRemoveEntry(item, out flag);
			if (!flag)
			{
				return;
			}
			int count = this._dataList.Count;
			for (int i = this._dataList.Count - 1; i >= 0; i--)
			{
				if (this._dataList[i].HasSameContentWith(item))
				{
					this._dataList.Remove(item);
				}
			}
			if (count == this._dataList.Count)
			{
				Debug.FailedAssert("Item is not in container: " + item, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\MultiplayerLocalDataManager.cs", "RemoveEntryAux", 304);
			}
			this._isFileDirty = true;
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x000107B2 File Offset: 0x0000E9B2
		protected virtual void OnBeforeRemoveEntry(T item, out bool canRemoveEntry)
		{
			canRemoveEntry = true;
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x000107B7 File Offset: 0x0000E9B7
		private PlatformFilePath GetDataFilePath()
		{
			return new PlatformFilePath(new PlatformDirectoryPath(PlatformFileType.User, this._saveDirectoryName), this._saveFileName);
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x000107D0 File Offset: 0x0000E9D0
		private async Task SaveFileAux()
		{
			try
			{
				await FileHelper.SaveFileAsync(this.GetDataFilePath(), Common.SerializeObjectAsJson(this._dataList));
			}
			catch (Exception ex)
			{
				Debug.FailedAssert("An exception occured while trying to save " + base.GetType().Name + " data: " + ex.Message, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\MultiplayerLocalDataManager.cs", "SaveFileAux", 331);
			}
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x00010818 File Offset: 0x0000EA18
		private async Task LoadFileAux()
		{
			PlatformFilePath oldFilePath = this.GetCompatibilityFilePath();
			if (FileHelper.FileExists(oldFilePath))
			{
				string text = await FileHelper.GetFileContentStringAsync(oldFilePath);
				FileHelper.DeleteFile(oldFilePath);
				if (!string.IsNullOrEmpty(text))
				{
					this._dataList.Clear();
					List<T> list = null;
					try
					{
						list = JsonConvert.DeserializeObject<List<T>>(text);
					}
					catch
					{
						try
						{
							list = this.DeserializeInCompatibilityMode(text);
						}
						catch
						{
							Debug.FailedAssert("Failed to load old data in compatibility mode", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\MultiplayerLocalDataManager.cs", "LoadFileAux", 362);
						}
					}
					if (list != null)
					{
						foreach (T t in list)
						{
							this._dataList.Add(t);
						}
					}
					this._isFileDirty = true;
					return;
				}
			}
			PlatformFilePath dataFilePath = this.GetDataFilePath();
			if (FileHelper.FileExists(dataFilePath))
			{
				string text2 = await FileHelper.GetFileContentStringAsync(dataFilePath);
				if (!string.IsNullOrEmpty(text2))
				{
					this._dataList.Clear();
					List<T> list2 = null;
					try
					{
						list2 = JsonConvert.DeserializeObject<List<T>>(text2);
					}
					catch
					{
						try
						{
							list2 = this.DeserializeInCompatibilityMode(text2);
							this._isFileDirty = true;
						}
						catch
						{
							Debug.FailedAssert("Failed to load file in compatibility mode", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\MultiplayerLocalDataManager.cs", "LoadFileAux", 403);
						}
					}
					if (list2 != null)
					{
						foreach (T t2 in list2)
						{
							this._dataList.Add(t2);
						}
					}
				}
			}
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0001085D File Offset: 0x0000EA5D
		protected virtual PlatformFilePath GetCompatibilityFilePath()
		{
			return new PlatformFilePath(new PlatformDirectoryPath(PlatformFileType.User, "DataOld"), "TmpData");
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x00010874 File Offset: 0x0000EA74
		protected virtual List<T> DeserializeInCompatibilityMode(string serializedJson)
		{
			return null;
		}

		// Token: 0x040004FF RID: 1279
		private readonly string _saveDirectoryName;

		// Token: 0x04000500 RID: 1280
		private readonly string _saveFileName;

		// Token: 0x04000501 RID: 1281
		private readonly List<MultiplayerLocalDataContainer<T>.ContainerOperation> _operationQueue;

		// Token: 0x04000502 RID: 1282
		private readonly List<T> _dataList;

		// Token: 0x04000503 RID: 1283
		private bool _isFileDirty;

		// Token: 0x04000504 RID: 1284
		private bool _isCacheDirty;

		// Token: 0x020001D3 RID: 467
		private enum OperationType
		{
			// Token: 0x040006C8 RID: 1736
			Add,
			// Token: 0x040006C9 RID: 1737
			Insert,
			// Token: 0x040006CA RID: 1738
			Remove
		}

		// Token: 0x020001D4 RID: 468
		private struct ContainerOperation
		{
			// Token: 0x06000B5F RID: 2911 RVA: 0x00016926 File Offset: 0x00014B26
			private ContainerOperation(MultiplayerLocalDataContainer<T>.OperationType type, T item, int index)
			{
				this.OperationType = type;
				this.Item = item;
				this.Index = index;
			}

			// Token: 0x06000B60 RID: 2912 RVA: 0x0001693D File Offset: 0x00014B3D
			public static MultiplayerLocalDataContainer<T>.ContainerOperation CreateAsAdd(T item)
			{
				return new MultiplayerLocalDataContainer<T>.ContainerOperation(MultiplayerLocalDataContainer<T>.OperationType.Add, item, -1);
			}

			// Token: 0x06000B61 RID: 2913 RVA: 0x00016947 File Offset: 0x00014B47
			public static MultiplayerLocalDataContainer<T>.ContainerOperation CreateAsRemove(T item)
			{
				return new MultiplayerLocalDataContainer<T>.ContainerOperation(MultiplayerLocalDataContainer<T>.OperationType.Remove, item, -1);
			}

			// Token: 0x06000B62 RID: 2914 RVA: 0x00016951 File Offset: 0x00014B51
			public static MultiplayerLocalDataContainer<T>.ContainerOperation CreateAsInsert(T item, int index)
			{
				return new MultiplayerLocalDataContainer<T>.ContainerOperation(MultiplayerLocalDataContainer<T>.OperationType.Insert, item, index);
			}

			// Token: 0x040006CB RID: 1739
			public readonly MultiplayerLocalDataContainer<T>.OperationType OperationType;

			// Token: 0x040006CC RID: 1740
			public readonly T Item;

			// Token: 0x040006CD RID: 1741
			public readonly int Index;
		}

		// Token: 0x020001D5 RID: 469
		private class ContainerOperationComparer : IComparer<MultiplayerLocalDataContainer<T>.ContainerOperation>
		{
			// Token: 0x06000B63 RID: 2915 RVA: 0x0001695B File Offset: 0x00014B5B
			public int Compare(MultiplayerLocalDataContainer<T>.ContainerOperation x, MultiplayerLocalDataContainer<T>.ContainerOperation y)
			{
				return x.OperationType.CompareTo(y.OperationType);
			}
		}
	}
}
