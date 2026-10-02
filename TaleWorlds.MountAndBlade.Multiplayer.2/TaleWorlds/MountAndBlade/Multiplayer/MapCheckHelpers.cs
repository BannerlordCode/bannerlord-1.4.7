using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000054 RID: 84
	public sealed class MapCheckHelpers
	{
		// Token: 0x060002AF RID: 687 RVA: 0x0000BC0A File Offset: 0x00009E0A
		private static string MapListEndpoint(GameServerEntry serverEntry)
		{
			return string.Format("http://{0}:{1}/maps/list", serverEntry.Address, serverEntry.Port);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000BC28 File Offset: 0x00009E28
		[return: TupleElementNames(new string[] { "isRefusedToJoin", "notExistingMap" })]
		public static async Task<ValueTuple<bool, string>> CheckMaps(GameServerEntry serverEntry)
		{
			ValueTuple<bool, string> valueTuple;
			if (MapCheckHelpers.CheckCurrentlyPlayedMap(serverEntry))
			{
				valueTuple = await MapCheckHelpers.CheckMapDownloaderMaps(serverEntry);
			}
			else
			{
				valueTuple = new ValueTuple<bool, string>(true, serverEntry.Map);
			}
			return valueTuple;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000BC70 File Offset: 0x00009E70
		private static bool CheckCurrentlyPlayedMap(GameServerEntry serverEntry)
		{
			UniqueSceneId uniqueSceneId;
			bool flag = UniqueSceneId.TryParse(serverEntry.UniqueMapId, out uniqueSceneId);
			return (flag && MapCheckHelpers.DoesSceneExist(serverEntry.Map, uniqueSceneId.UniqueToken, uniqueSceneId.Revision)) || !flag;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000BCB0 File Offset: 0x00009EB0
		[return: TupleElementNames(new string[] { "isRefusedToJoin", "notExistingMap" })]
		private static async Task<ValueTuple<bool, string>> CheckMapDownloaderMaps(GameServerEntry serverEntry)
		{
			ValueTuple<bool, string> valueTuple;
			try
			{
				Stopwatch watch = Stopwatch.StartNew();
				CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(MapCheckHelpers.TimeoutDuration));
				Task<string> downloadTask = HttpHelper.DownloadStringTaskAsync(MapCheckHelpers.MapListEndpoint(serverEntry));
				TaskAwaiter<Task> taskAwaiter = Task.WhenAny(new Task[]
				{
					downloadTask,
					Task.Delay(-1, cancellationTokenSource.Token)
				}).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<Task> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<Task>);
				}
				if (taskAwaiter.GetResult() == downloadTask)
				{
					foreach (MapListItemResponse mapListItemResponse in JsonConvert.DeserializeObject<MapListResponse>(await downloadTask).Maps)
					{
						if (!MapCheckHelpers.DoesSceneExist(mapListItemResponse.Name, mapListItemResponse.UniqueToken, mapListItemResponse.Revision))
						{
							return new ValueTuple<bool, string>(true, mapListItemResponse.Name);
						}
					}
					valueTuple = new ValueTuple<bool, string>(false, null);
				}
				else
				{
					watch.Stop();
					long elapsedMilliseconds = watch.ElapsedMilliseconds;
					Debug.Print(string.Format("Getting map list timeout to host: {0}:{1} in {2} milliseconds.", serverEntry.Address, serverEntry.Port, elapsedMilliseconds), 0, Debug.DebugColor.White, 17592186044416UL);
					valueTuple = new ValueTuple<bool, string>(false, null);
				}
			}
			catch (Exception ex)
			{
				Debug.Print("Exception getting map list from custom servers: " + ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
				valueTuple = new ValueTuple<bool, string>(false, null);
			}
			return valueTuple;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000BCF8 File Offset: 0x00009EF8
		private static bool DoesSceneExist(string mapId, string uniqueMapId, string revision)
		{
			string text;
			UniqueSceneId uniqueSceneId;
			return Utilities.TryGetFullFilePathOfScene(mapId, out text) && (uniqueMapId == null || (Utilities.TryGetUniqueIdentifiersForSceneFile(text, out uniqueSceneId) && uniqueSceneId.UniqueToken == uniqueMapId && uniqueSceneId.Revision == revision));
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000BD3E File Offset: 0x00009F3E
		private MapCheckHelpers()
		{
		}

		// Token: 0x040000E2 RID: 226
		private static readonly double TimeoutDuration = 1.8;
	}
}
