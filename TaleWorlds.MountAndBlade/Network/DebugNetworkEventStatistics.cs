using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network
{
	// Token: 0x020003BC RID: 956
	public static class DebugNetworkEventStatistics
	{
		// Token: 0x140000A8 RID: 168
		// (add) Token: 0x0600356C RID: 13676 RVA: 0x000DBD0C File Offset: 0x000D9F0C
		// (remove) Token: 0x0600356D RID: 13677 RVA: 0x000DBD40 File Offset: 0x000D9F40
		public static event Action<IEnumerable<DebugNetworkEventStatistics.TotalEventData>> OnEventDataUpdated;

		// Token: 0x140000A9 RID: 169
		// (add) Token: 0x0600356E RID: 13678 RVA: 0x000DBD74 File Offset: 0x000D9F74
		// (remove) Token: 0x0600356F RID: 13679 RVA: 0x000DBDA8 File Offset: 0x000D9FA8
		public static event Action<DebugNetworkEventStatistics.PerSecondEventData> OnPerSecondEventDataUpdated;

		// Token: 0x140000AA RID: 170
		// (add) Token: 0x06003570 RID: 13680 RVA: 0x000DBDDC File Offset: 0x000D9FDC
		// (remove) Token: 0x06003571 RID: 13681 RVA: 0x000DBE10 File Offset: 0x000DA010
		public static event Action<IEnumerable<float>> OnFPSEventUpdated;

		// Token: 0x140000AB RID: 171
		// (add) Token: 0x06003572 RID: 13682 RVA: 0x000DBE44 File Offset: 0x000DA044
		// (remove) Token: 0x06003573 RID: 13683 RVA: 0x000DBE78 File Offset: 0x000DA078
		public static event Action OnOpenExternalMonitor;

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x06003574 RID: 13684 RVA: 0x000DBEAB File Offset: 0x000DA0AB
		// (set) Token: 0x06003575 RID: 13685 RVA: 0x000DBEB2 File Offset: 0x000DA0B2
		public static int SamplesPerSecond
		{
			get
			{
				return DebugNetworkEventStatistics._samplesPerSecond;
			}
			set
			{
				DebugNetworkEventStatistics._samplesPerSecond = value;
				DebugNetworkEventStatistics.MaxGraphPointCount = value * 5;
			}
		} = 10;

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x06003576 RID: 13686 RVA: 0x000DBEC2 File Offset: 0x000DA0C2
		// (set) Token: 0x06003577 RID: 13687 RVA: 0x000DBEC9 File Offset: 0x000DA0C9
		public static bool IsActive { get; private set; }

		// Token: 0x06003579 RID: 13689 RVA: 0x000DBF9C File Offset: 0x000DA19C
		internal static void StartEvent(string eventName, int eventType)
		{
			if (!DebugNetworkEventStatistics.IsActive)
			{
				return;
			}
			DebugNetworkEventStatistics._curEventType = eventType;
			if (!DebugNetworkEventStatistics._statistics.ContainsKey(DebugNetworkEventStatistics._curEventType))
			{
				DebugNetworkEventStatistics._statistics.Add(DebugNetworkEventStatistics._curEventType, new DebugNetworkEventStatistics.PerEventData
				{
					Name = eventName
				});
			}
			DebugNetworkEventStatistics._statistics[DebugNetworkEventStatistics._curEventType].Count++;
			DebugNetworkEventStatistics._totalData.TotalCount++;
		}

		// Token: 0x0600357A RID: 13690 RVA: 0x000DC014 File Offset: 0x000DA214
		internal static void EndEvent()
		{
			if (!DebugNetworkEventStatistics.IsActive)
			{
				return;
			}
			DebugNetworkEventStatistics.PerEventData perEventData = DebugNetworkEventStatistics._statistics[DebugNetworkEventStatistics._curEventType];
			perEventData.DataSize = perEventData.TotalDataSize / perEventData.Count;
			DebugNetworkEventStatistics._curEventType = -1;
		}

		// Token: 0x0600357B RID: 13691 RVA: 0x000DC052 File Offset: 0x000DA252
		internal static void AddDataToStatistic(int bitCount)
		{
			if (!DebugNetworkEventStatistics.IsActive)
			{
				return;
			}
			DebugNetworkEventStatistics._statistics[DebugNetworkEventStatistics._curEventType].TotalDataSize += bitCount;
			DebugNetworkEventStatistics._totalData.TotalDataSize += bitCount;
		}

		// Token: 0x0600357C RID: 13692 RVA: 0x000DC08A File Offset: 0x000DA28A
		public static void OpenExternalMonitor()
		{
			if (DebugNetworkEventStatistics.OnOpenExternalMonitor != null)
			{
				DebugNetworkEventStatistics.OnOpenExternalMonitor();
			}
		}

		// Token: 0x0600357D RID: 13693 RVA: 0x000DC09D File Offset: 0x000DA29D
		public static void ControlActivate()
		{
			DebugNetworkEventStatistics.IsActive = true;
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x000DC0A5 File Offset: 0x000DA2A5
		public static void ControlDeactivate()
		{
			DebugNetworkEventStatistics.IsActive = false;
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x000DC0AD File Offset: 0x000DA2AD
		public static void ControlJustDump()
		{
			DebugNetworkEventStatistics.DumpData();
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x000DC0B4 File Offset: 0x000DA2B4
		public static void ControlDumpAll()
		{
			DebugNetworkEventStatistics.DumpData();
			DebugNetworkEventStatistics.DumpReplicationData();
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x000DC0C0 File Offset: 0x000DA2C0
		public static void ControlClear()
		{
			DebugNetworkEventStatistics.Clear();
		}

		// Token: 0x06003582 RID: 13698 RVA: 0x000DC0C7 File Offset: 0x000DA2C7
		public static void ClearNetGraphs()
		{
			DebugNetworkEventStatistics._eventSamples.Clear();
			DebugNetworkEventStatistics._lossSamples.Clear();
			DebugNetworkEventStatistics._prevEventData = new DebugNetworkEventStatistics.TotalEventData();
			DebugNetworkEventStatistics._currEventData = new DebugNetworkEventStatistics.TotalEventData();
			DebugNetworkEventStatistics._collectSampleCheck = 0f;
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x000DC0FB File Offset: 0x000DA2FB
		public static void ClearFpsGraph()
		{
			DebugNetworkEventStatistics._fpsSamplesUntilNextSampling.Clear();
			DebugNetworkEventStatistics._fpsSamples.Clear();
			DebugNetworkEventStatistics._collectFpsSampleCheck = 0f;
		}

		// Token: 0x06003584 RID: 13700 RVA: 0x000DC11B File Offset: 0x000DA31B
		public static void ControlClearAll()
		{
			DebugNetworkEventStatistics.Clear();
			DebugNetworkEventStatistics.ClearFpsGraph();
			DebugNetworkEventStatistics.ClearNetGraphs();
			DebugNetworkEventStatistics.ClearReplicationData();
		}

		// Token: 0x06003585 RID: 13701 RVA: 0x000DC131 File Offset: 0x000DA331
		public static void ControlDumpReplicationData()
		{
			DebugNetworkEventStatistics.DumpReplicationData();
			DebugNetworkEventStatistics.ClearReplicationData();
		}

		// Token: 0x06003586 RID: 13702 RVA: 0x000DC140 File Offset: 0x000DA340
		public static void EndTick(float dt)
		{
			if (DebugNetworkEventStatistics._useImgui && Input.DebugInput.IsHotKeyPressed("DebugNetworkEventStatisticsHotkeyToggleActive"))
			{
				DebugNetworkEventStatistics.ToggleActive();
				if (DebugNetworkEventStatistics.IsActive)
				{
					Imgui.NewFrame();
				}
			}
			if (!DebugNetworkEventStatistics.IsActive)
			{
				return;
			}
			DebugNetworkEventStatistics._totalData.TotalTime += dt;
			DebugNetworkEventStatistics._totalData.TotalFrameCount++;
			if (DebugNetworkEventStatistics._useImgui)
			{
				Imgui.BeginMainThreadScope();
				Imgui.Begin("Network panel");
				if (Imgui.Button("Disable Network Panel"))
				{
					DebugNetworkEventStatistics.ToggleActive();
				}
				Imgui.Separator();
				if (Imgui.Button("Show Upload Data (screen)"))
				{
					DebugNetworkEventStatistics._showUploadDataText = !DebugNetworkEventStatistics._showUploadDataText;
				}
				Imgui.Separator();
				if (Imgui.Button("Clear Data"))
				{
					DebugNetworkEventStatistics.Clear();
				}
				if (Imgui.Button("Dump Data (console)"))
				{
					DebugNetworkEventStatistics.DumpData();
				}
				Imgui.Separator();
				if (Imgui.Button("Clear Replication Data"))
				{
					DebugNetworkEventStatistics.ClearReplicationData();
				}
				if (Imgui.Button("Dump Replication Data (console)"))
				{
					DebugNetworkEventStatistics.DumpReplicationData();
				}
				if (Imgui.Button("Dump & Clear Replication Data (console)"))
				{
					DebugNetworkEventStatistics.DumpReplicationData();
					DebugNetworkEventStatistics.ClearReplicationData();
				}
				if (DebugNetworkEventStatistics._showUploadDataText)
				{
					Imgui.Separator();
					DebugNetworkEventStatistics.ShowUploadData();
				}
				Imgui.End();
			}
			if (!DebugNetworkEventStatistics.IsActive)
			{
				return;
			}
			DebugNetworkEventStatistics.CollectFpsSample(dt);
			DebugNetworkEventStatistics._collectSampleCheck += dt;
			if (DebugNetworkEventStatistics._collectSampleCheck >= 1f / (float)DebugNetworkEventStatistics.SamplesPerSecond)
			{
				DebugNetworkEventStatistics._currEventData = DebugNetworkEventStatistics.GetCurrentEventData();
				if (DebugNetworkEventStatistics._currEventData.HasData && DebugNetworkEventStatistics._prevEventData.HasData && DebugNetworkEventStatistics._currEventData != DebugNetworkEventStatistics._prevEventData)
				{
					DebugNetworkEventStatistics._lossSamples.Enqueue(GameNetwork.GetAveragePacketLossRatio());
					DebugNetworkEventStatistics._eventSamples.Enqueue(DebugNetworkEventStatistics._currEventData - DebugNetworkEventStatistics._prevEventData);
					DebugNetworkEventStatistics._prevEventData = DebugNetworkEventStatistics._currEventData;
					if (DebugNetworkEventStatistics._eventSamples.Count > DebugNetworkEventStatistics.MaxGraphPointCount)
					{
						DebugNetworkEventStatistics._eventSamples.Dequeue();
						DebugNetworkEventStatistics._lossSamples.Dequeue();
					}
					if (DebugNetworkEventStatistics._eventSamples.Count >= DebugNetworkEventStatistics.SamplesPerSecond)
					{
						List<DebugNetworkEventStatistics.TotalEventData> range = DebugNetworkEventStatistics._eventSamples.ToList<DebugNetworkEventStatistics.TotalEventData>().GetRange(DebugNetworkEventStatistics._eventSamples.Count - DebugNetworkEventStatistics.SamplesPerSecond, DebugNetworkEventStatistics.SamplesPerSecond);
						DebugNetworkEventStatistics.UploadPerSecondEventData = new DebugNetworkEventStatistics.PerSecondEventData(range.Sum<DebugNetworkEventStatistics.TotalEventData>((DebugNetworkEventStatistics.TotalEventData x) => x.TotalUpload), range.Sum<DebugNetworkEventStatistics.TotalEventData>((DebugNetworkEventStatistics.TotalEventData x) => x.TotalConstantsUpload), range.Sum<DebugNetworkEventStatistics.TotalEventData>((DebugNetworkEventStatistics.TotalEventData x) => x.TotalReliableUpload), range.Sum<DebugNetworkEventStatistics.TotalEventData>((DebugNetworkEventStatistics.TotalEventData x) => x.TotalReplicationUpload), range.Sum<DebugNetworkEventStatistics.TotalEventData>((DebugNetworkEventStatistics.TotalEventData x) => x.TotalUnreliableUpload), range.Sum<DebugNetworkEventStatistics.TotalEventData>((DebugNetworkEventStatistics.TotalEventData x) => x.TotalOtherUpload));
						if (DebugNetworkEventStatistics.OnPerSecondEventDataUpdated != null)
						{
							DebugNetworkEventStatistics.OnPerSecondEventDataUpdated(DebugNetworkEventStatistics.UploadPerSecondEventData);
						}
					}
					if (DebugNetworkEventStatistics.OnEventDataUpdated != null)
					{
						DebugNetworkEventStatistics.OnEventDataUpdated(DebugNetworkEventStatistics._eventSamples.ToList<DebugNetworkEventStatistics.TotalEventData>());
					}
					DebugNetworkEventStatistics._collectSampleCheck -= 1f / (float)DebugNetworkEventStatistics.SamplesPerSecond;
				}
			}
			if (DebugNetworkEventStatistics._useImgui)
			{
				Imgui.Begin("Network Graph panel");
				float[] array = DebugNetworkEventStatistics._eventSamples.Select<DebugNetworkEventStatistics.TotalEventData, float>((DebugNetworkEventStatistics.TotalEventData x) => (float)x.TotalUpload / 8192f).ToArray<float>();
				float num = ((array.Length != 0) ? array.Max() : 0f);
				DebugNetworkEventStatistics._targetMaxGraphHeight = (DebugNetworkEventStatistics._useAbsoluteMaximum ? MathF.Max(num, DebugNetworkEventStatistics._targetMaxGraphHeight) : num);
				float num2 = MBMath.ClampFloat(3f * dt, 0f, 1f);
				DebugNetworkEventStatistics._curMaxGraphHeight = MBMath.Lerp(DebugNetworkEventStatistics._curMaxGraphHeight, DebugNetworkEventStatistics._targetMaxGraphHeight, num2, 1E-05f);
				if (DebugNetworkEventStatistics.UploadPerSecondEventData != null)
				{
					Imgui.Text(string.Concat(new object[]
					{
						"Taking ",
						DebugNetworkEventStatistics.SamplesPerSecond,
						" samples per second. Total KiB per second:",
						(float)DebugNetworkEventStatistics.UploadPerSecondEventData.TotalUploadPerSecond / 8192f
					}));
				}
				float[] array2 = DebugNetworkEventStatistics._eventSamples.Select<DebugNetworkEventStatistics.TotalEventData, float>((DebugNetworkEventStatistics.TotalEventData x) => (float)x.TotalConstantsUpload / 8192f).ToArray<float>();
				Imgui.PlotLines("", array2, DebugNetworkEventStatistics._eventSamples.Count, 0, "Constants upload (in KiB)", 0f, DebugNetworkEventStatistics._curMaxGraphHeight, 400f, 45f, 4);
				Imgui.SameLine(0f, 0f);
				Imgui.Text("Y-range: " + DebugNetworkEventStatistics._curMaxGraphHeight);
				float[] array3 = DebugNetworkEventStatistics._eventSamples.Select<DebugNetworkEventStatistics.TotalEventData, float>((DebugNetworkEventStatistics.TotalEventData x) => (float)x.TotalReliableUpload / 8192f).ToArray<float>();
				Imgui.PlotLines("", array3, DebugNetworkEventStatistics._eventSamples.Count, 0, "Reliable upload (in KiB)", 0f, DebugNetworkEventStatistics._curMaxGraphHeight, 400f, 45f, 4);
				Imgui.SameLine(0f, 0f);
				Imgui.Text("Y-range: " + DebugNetworkEventStatistics._curMaxGraphHeight);
				float[] array4 = DebugNetworkEventStatistics._eventSamples.Select<DebugNetworkEventStatistics.TotalEventData, float>((DebugNetworkEventStatistics.TotalEventData x) => (float)x.TotalReplicationUpload / 8192f).ToArray<float>();
				Imgui.PlotLines("", array4, DebugNetworkEventStatistics._eventSamples.Count, 0, "Replication upload (in KiB)", 0f, DebugNetworkEventStatistics._curMaxGraphHeight, 400f, 45f, 4);
				Imgui.SameLine(0f, 0f);
				Imgui.Text("Y-range: " + DebugNetworkEventStatistics._curMaxGraphHeight);
				float[] array5 = DebugNetworkEventStatistics._eventSamples.Select<DebugNetworkEventStatistics.TotalEventData, float>((DebugNetworkEventStatistics.TotalEventData x) => (float)x.TotalUnreliableUpload / 8192f).ToArray<float>();
				Imgui.PlotLines("", array5, DebugNetworkEventStatistics._eventSamples.Count, 0, "Unreliable upload (in KiB)", 0f, DebugNetworkEventStatistics._curMaxGraphHeight, 400f, 45f, 4);
				Imgui.SameLine(0f, 0f);
				Imgui.Text("Y-range: " + DebugNetworkEventStatistics._curMaxGraphHeight);
				float[] array6 = DebugNetworkEventStatistics._eventSamples.Select<DebugNetworkEventStatistics.TotalEventData, float>((DebugNetworkEventStatistics.TotalEventData x) => (float)x.TotalOtherUpload / 8192f).ToArray<float>();
				Imgui.PlotLines("", array6, DebugNetworkEventStatistics._eventSamples.Count, 0, "Other upload (in KiB)", 0f, DebugNetworkEventStatistics._curMaxGraphHeight, 400f, 45f, 4);
				Imgui.SameLine(0f, 0f);
				Imgui.Text("Y-range: " + DebugNetworkEventStatistics._curMaxGraphHeight);
				Imgui.Separator();
				float[] array7 = DebugNetworkEventStatistics._eventSamples.Select<DebugNetworkEventStatistics.TotalEventData, float>((DebugNetworkEventStatistics.TotalEventData x) => (float)x.TotalUpload / (float)x.TotalPackets / 8f).ToArray<float>();
				Imgui.PlotLines("", array7, DebugNetworkEventStatistics._eventSamples.Count, 0, "Data per package (in B)", 0f, 1400f, 400f, 45f, 4);
				Imgui.SameLine(0f, 0f);
				Imgui.Text("Y-range: " + 1400);
				Imgui.Separator();
				float num3 = ((DebugNetworkEventStatistics._lossSamples.Count > 0) ? DebugNetworkEventStatistics._lossSamples.Max() : 0f);
				DebugNetworkEventStatistics._targetMaxLossGraphHeight = (DebugNetworkEventStatistics._useAbsoluteMaximum ? MathF.Max(num3, DebugNetworkEventStatistics._targetMaxLossGraphHeight) : num3);
				float num4 = MBMath.ClampFloat(3f * dt, 0f, 1f);
				DebugNetworkEventStatistics._currMaxLossGraphHeight = MBMath.Lerp(DebugNetworkEventStatistics._currMaxLossGraphHeight, DebugNetworkEventStatistics._targetMaxLossGraphHeight, num4, 1E-05f);
				Imgui.PlotLines("", DebugNetworkEventStatistics._lossSamples.ToArray(), DebugNetworkEventStatistics._lossSamples.Count, 0, "Averaged loss ratio", 0f, DebugNetworkEventStatistics._currMaxLossGraphHeight, 400f, 45f, 4);
				Imgui.SameLine(0f, 0f);
				Imgui.Text("Y-range: " + DebugNetworkEventStatistics._currMaxLossGraphHeight);
				Imgui.Checkbox("Use absolute Maximum", ref DebugNetworkEventStatistics._useAbsoluteMaximum);
				Imgui.End();
			}
			Imgui.EndMainThreadScope();
		}

		// Token: 0x06003587 RID: 13703 RVA: 0x000DC9AC File Offset: 0x000DABAC
		private static void CollectFpsSample(float dt)
		{
			if (DebugNetworkEventStatistics.TrackFps)
			{
				float fps = Utilities.GetFps();
				if (!float.IsInfinity(fps) && !float.IsNegativeInfinity(fps) && !float.IsNaN(fps))
				{
					DebugNetworkEventStatistics._fpsSamplesUntilNextSampling.Add(fps);
				}
				DebugNetworkEventStatistics._collectFpsSampleCheck += dt;
				if (DebugNetworkEventStatistics._collectFpsSampleCheck >= 1f / (float)DebugNetworkEventStatistics.SamplesPerSecond)
				{
					if (DebugNetworkEventStatistics._fpsSamplesUntilNextSampling.Count > 0)
					{
						DebugNetworkEventStatistics._fpsSamples.Enqueue(DebugNetworkEventStatistics._fpsSamplesUntilNextSampling.Min());
						DebugNetworkEventStatistics._fpsSamplesUntilNextSampling.Clear();
						if (DebugNetworkEventStatistics._fpsSamples.Count > DebugNetworkEventStatistics.MaxGraphPointCount)
						{
							DebugNetworkEventStatistics._fpsSamples.Dequeue();
						}
						Action<IEnumerable<float>> onFPSEventUpdated = DebugNetworkEventStatistics.OnFPSEventUpdated;
						if (onFPSEventUpdated != null)
						{
							onFPSEventUpdated(DebugNetworkEventStatistics._fpsSamples.ToList<float>());
						}
					}
					DebugNetworkEventStatistics._collectFpsSampleCheck -= 1f / (float)DebugNetworkEventStatistics.SamplesPerSecond;
				}
			}
		}

		// Token: 0x06003588 RID: 13704 RVA: 0x000DCA83 File Offset: 0x000DAC83
		private static void ToggleActive()
		{
			DebugNetworkEventStatistics.IsActive = !DebugNetworkEventStatistics.IsActive;
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x000DCA92 File Offset: 0x000DAC92
		private static void Clear()
		{
			DebugNetworkEventStatistics._totalData = new DebugNetworkEventStatistics.TotalData();
			DebugNetworkEventStatistics._statistics = new Dictionary<int, DebugNetworkEventStatistics.PerEventData>();
			GameNetwork.ResetDebugUploads();
			DebugNetworkEventStatistics._curEventType = -1;
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x000DCAB4 File Offset: 0x000DACB4
		private static void DumpData()
		{
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "DumpData");
			mbstringBuilder.AppendLine();
			mbstringBuilder.AppendLine<string>("///GENERAL DATA///");
			mbstringBuilder.AppendLine<string>("Total elapsed time: " + DebugNetworkEventStatistics._totalData.TotalTime + " seconds.");
			mbstringBuilder.AppendLine<string>("Total frame count: " + DebugNetworkEventStatistics._totalData.TotalFrameCount);
			mbstringBuilder.AppendLine<string>("Total avg packet count: " + (int)(DebugNetworkEventStatistics._totalData.TotalTime / 60f));
			mbstringBuilder.AppendLine<string>("Total event data size: " + DebugNetworkEventStatistics._totalData.TotalDataSize + " bits.");
			mbstringBuilder.AppendLine<string>("Total event count: " + DebugNetworkEventStatistics._totalData.TotalCount);
			mbstringBuilder.AppendLine();
			mbstringBuilder.AppendLine<string>("///ALL DATA///");
			List<DebugNetworkEventStatistics.PerEventData> list = new List<DebugNetworkEventStatistics.PerEventData>();
			list.AddRange(DebugNetworkEventStatistics._statistics.Values);
			list.Sort();
			foreach (DebugNetworkEventStatistics.PerEventData perEventData in list)
			{
				mbstringBuilder.AppendLine<string>("Event name: " + perEventData.Name);
				mbstringBuilder.AppendLine<string>("\tEvent size (for one event): " + perEventData.DataSize + " bits.");
				mbstringBuilder.AppendLine<string>("\tTotal count: " + perEventData.Count);
				mbstringBuilder.AppendLine<string>(string.Concat(new object[]
				{
					"\tTotal size: ",
					perEventData.TotalDataSize,
					"bits | ~",
					perEventData.TotalDataSize / 8 + ((perEventData.TotalDataSize % 8 == 0) ? 0 : 1),
					" bytes."
				}));
				mbstringBuilder.AppendLine<string>("\tTotal count per frame: " + (float)perEventData.Count / (float)DebugNetworkEventStatistics._totalData.TotalFrameCount);
				mbstringBuilder.AppendLine<string>("\tTotal size per frame: " + (float)perEventData.TotalDataSize / (float)DebugNetworkEventStatistics._totalData.TotalFrameCount + " bits per frame.");
				mbstringBuilder.AppendLine();
			}
			DebugNetworkEventStatistics.GetFormattedDebugUploadDataOutput(ref mbstringBuilder);
			mbstringBuilder.AppendLine<string>("NetworkEventStaticticsLogLength: " + mbstringBuilder.Length + "\n");
			MBDebug.Print(mbstringBuilder.ToStringAndRelease(), 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x0600358B RID: 13707 RVA: 0x000DCD70 File Offset: 0x000DAF70
		private static void GetFormattedDebugUploadDataOutput(ref MBStringBuilder outStr)
		{
			GameNetwork.DebugNetworkPacketStatisticsStruct debugNetworkPacketStatisticsStruct = default(GameNetwork.DebugNetworkPacketStatisticsStruct);
			GameNetwork.DebugNetworkPositionCompressionStatisticsStruct debugNetworkPositionCompressionStatisticsStruct = default(GameNetwork.DebugNetworkPositionCompressionStatisticsStruct);
			GameNetwork.GetDebugUploadsInBits(ref debugNetworkPacketStatisticsStruct, ref debugNetworkPositionCompressionStatisticsStruct);
			outStr.AppendLine<string>("REAL NETWORK UPLOAD PERCENTS");
			if (debugNetworkPacketStatisticsStruct.TotalUpload == 0)
			{
				outStr.AppendLine<string>("Total Upload is ZERO");
				return;
			}
			int num = debugNetworkPacketStatisticsStruct.TotalUpload - (debugNetworkPacketStatisticsStruct.TotalConstantsUpload + debugNetworkPacketStatisticsStruct.TotalReliableEventUpload + debugNetworkPacketStatisticsStruct.TotalReplicationUpload + debugNetworkPacketStatisticsStruct.TotalUnreliableEventUpload);
			if (num == debugNetworkPacketStatisticsStruct.TotalUpload)
			{
				outStr.AppendLine<string>("USE_DEBUG_NETWORK_PACKET_PERCENTS not defined!");
			}
			else
			{
				outStr.AppendLine<string>("\tAverage Ping: " + debugNetworkPacketStatisticsStruct.AveragePingTime);
				outStr.AppendLine<string>("\tTime out period: " + debugNetworkPacketStatisticsStruct.TimeOutPeriod);
				outStr.AppendLine<string>("\tLost Percent: " + debugNetworkPacketStatisticsStruct.LostPercent);
				outStr.AppendLine<string>("\tlost_count: " + debugNetworkPacketStatisticsStruct.LostCount);
				outStr.AppendLine<string>("\ttotal_count_on_lost_check: " + debugNetworkPacketStatisticsStruct.TotalCountOnLostCheck);
				outStr.AppendLine<string>("\tround_trip_time: " + debugNetworkPacketStatisticsStruct.RoundTripTime);
				float num2 = (float)debugNetworkPacketStatisticsStruct.TotalUpload;
				float num3 = 1f / (float)debugNetworkPacketStatisticsStruct.TotalPackets;
				outStr.AppendLine<string>(string.Concat(new object[]
				{
					"\tConstants Upload: percent: ",
					(float)debugNetworkPacketStatisticsStruct.TotalConstantsUpload / num2 * 100f,
					"; size in bits: ",
					(float)debugNetworkPacketStatisticsStruct.TotalConstantsUpload * num3,
					";"
				}));
				outStr.AppendLine<string>(string.Concat(new object[]
				{
					"\tReliable Upload: percent: ",
					(float)debugNetworkPacketStatisticsStruct.TotalReliableEventUpload / num2 * 100f,
					"; size in bits: ",
					(float)debugNetworkPacketStatisticsStruct.TotalReliableEventUpload * num3,
					";"
				}));
				outStr.AppendLine<string>(string.Concat(new object[]
				{
					"\tReplication Upload: percent: ",
					(float)debugNetworkPacketStatisticsStruct.TotalReplicationUpload / num2 * 100f,
					"; size in bits: ",
					(float)debugNetworkPacketStatisticsStruct.TotalReplicationUpload * num3,
					";"
				}));
				outStr.AppendLine<string>(string.Concat(new object[]
				{
					"\tUnreliable Upload: percent: ",
					(float)debugNetworkPacketStatisticsStruct.TotalUnreliableEventUpload / num2 * 100f,
					"; size in bits: ",
					(float)debugNetworkPacketStatisticsStruct.TotalUnreliableEventUpload * num3,
					";"
				}));
				outStr.AppendLine<string>(string.Concat(new object[]
				{
					"\tOthers (headers, ack etc.) Upload: percent: ",
					(float)num / num2 * 100f,
					"; size in bits: ",
					(float)num * num3,
					";"
				}));
				int num4 = debugNetworkPositionCompressionStatisticsStruct.totalPositionCoarseBitCountX + debugNetworkPositionCompressionStatisticsStruct.totalPositionCoarseBitCountY + debugNetworkPositionCompressionStatisticsStruct.totalPositionCoarseBitCountZ;
				float num5 = 1f / (float)debugNetworkPacketStatisticsStruct.TotalCellPriorityChecks;
				outStr.AppendLine<string>(string.Concat(new object[]
				{
					"\n\tTotal PPS: ",
					(float)debugNetworkPacketStatisticsStruct.TotalPackets / DebugNetworkEventStatistics._totalData.TotalTime,
					"; bps: ",
					(float)debugNetworkPacketStatisticsStruct.TotalUpload / DebugNetworkEventStatistics._totalData.TotalTime,
					";"
				}));
			}
			outStr.AppendLine<string>(string.Concat(new object[]
			{
				"\n\tTotal packets: ",
				debugNetworkPacketStatisticsStruct.TotalPackets,
				"; bits per packet: ",
				(float)debugNetworkPacketStatisticsStruct.TotalUpload / (float)debugNetworkPacketStatisticsStruct.TotalPackets,
				";"
			}));
			outStr.AppendLine<string>("Total Upload: " + debugNetworkPacketStatisticsStruct.TotalUpload + " in bits");
		}

		// Token: 0x0600358C RID: 13708 RVA: 0x000DD13C File Offset: 0x000DB33C
		private static void ShowUploadData()
		{
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "ShowUploadData");
			DebugNetworkEventStatistics.GetFormattedDebugUploadDataOutput(ref mbstringBuilder);
			string[] array = mbstringBuilder.ToStringAndRelease().Split(new char[] { '\n' });
			for (int i = 0; i < array.Length; i++)
			{
				Imgui.Text(array[i]);
			}
		}

		// Token: 0x0600358D RID: 13709 RVA: 0x000DD194 File Offset: 0x000DB394
		private static DebugNetworkEventStatistics.TotalEventData GetCurrentEventData()
		{
			GameNetwork.DebugNetworkPacketStatisticsStruct debugNetworkPacketStatisticsStruct = default(GameNetwork.DebugNetworkPacketStatisticsStruct);
			GameNetwork.DebugNetworkPositionCompressionStatisticsStruct debugNetworkPositionCompressionStatisticsStruct = default(GameNetwork.DebugNetworkPositionCompressionStatisticsStruct);
			GameNetwork.GetDebugUploadsInBits(ref debugNetworkPacketStatisticsStruct, ref debugNetworkPositionCompressionStatisticsStruct);
			return new DebugNetworkEventStatistics.TotalEventData(debugNetworkPacketStatisticsStruct.TotalPackets, debugNetworkPacketStatisticsStruct.TotalUpload, debugNetworkPacketStatisticsStruct.TotalConstantsUpload, debugNetworkPacketStatisticsStruct.TotalReliableEventUpload, debugNetworkPacketStatisticsStruct.TotalReplicationUpload, debugNetworkPacketStatisticsStruct.TotalUnreliableEventUpload);
		}

		// Token: 0x0600358E RID: 13710 RVA: 0x000DD1E3 File Offset: 0x000DB3E3
		private static void DumpReplicationData()
		{
			GameNetwork.PrintReplicationTableStatistics();
		}

		// Token: 0x0600358F RID: 13711 RVA: 0x000DD1EA File Offset: 0x000DB3EA
		private static void ClearReplicationData()
		{
			GameNetwork.ClearReplicationTableStatistics();
		}

		// Token: 0x040016EC RID: 5868
		private static DebugNetworkEventStatistics.TotalData _totalData = new DebugNetworkEventStatistics.TotalData();

		// Token: 0x040016ED RID: 5869
		private static int _curEventType = -1;

		// Token: 0x040016EE RID: 5870
		private static Dictionary<int, DebugNetworkEventStatistics.PerEventData> _statistics = new Dictionary<int, DebugNetworkEventStatistics.PerEventData>();

		// Token: 0x040016EF RID: 5871
		private static int _samplesPerSecond;

		// Token: 0x040016F0 RID: 5872
		public static int MaxGraphPointCount;

		// Token: 0x040016F1 RID: 5873
		private static bool _showUploadDataText = false;

		// Token: 0x040016F2 RID: 5874
		private static bool _useAbsoluteMaximum = false;

		// Token: 0x040016F3 RID: 5875
		private static float _collectSampleCheck = 0f;

		// Token: 0x040016F4 RID: 5876
		private static float _collectFpsSampleCheck = 0f;

		// Token: 0x040016F5 RID: 5877
		private static float _curMaxGraphHeight = 0f;

		// Token: 0x040016F6 RID: 5878
		private static float _targetMaxGraphHeight = 0f;

		// Token: 0x040016F7 RID: 5879
		private static float _currMaxLossGraphHeight = 0f;

		// Token: 0x040016F8 RID: 5880
		private static float _targetMaxLossGraphHeight = 0f;

		// Token: 0x040016F9 RID: 5881
		private static DebugNetworkEventStatistics.PerSecondEventData UploadPerSecondEventData;

		// Token: 0x040016FA RID: 5882
		private static readonly Queue<DebugNetworkEventStatistics.TotalEventData> _eventSamples = new Queue<DebugNetworkEventStatistics.TotalEventData>();

		// Token: 0x040016FB RID: 5883
		private static readonly Queue<float> _lossSamples = new Queue<float>();

		// Token: 0x040016FC RID: 5884
		private static DebugNetworkEventStatistics.TotalEventData _prevEventData = new DebugNetworkEventStatistics.TotalEventData();

		// Token: 0x040016FD RID: 5885
		private static DebugNetworkEventStatistics.TotalEventData _currEventData = new DebugNetworkEventStatistics.TotalEventData();

		// Token: 0x040016FE RID: 5886
		private static readonly List<float> _fpsSamplesUntilNextSampling = new List<float>();

		// Token: 0x040016FF RID: 5887
		private static readonly Queue<float> _fpsSamples = new Queue<float>();

		// Token: 0x04001700 RID: 5888
		private static bool _useImgui = !GameNetwork.IsDedicatedServer;

		// Token: 0x04001701 RID: 5889
		public static bool TrackFps = false;

		// Token: 0x02000681 RID: 1665
		public class TotalEventData
		{
			// Token: 0x06004141 RID: 16705 RVA: 0x000FB580 File Offset: 0x000F9780
			protected bool Equals(DebugNetworkEventStatistics.TotalEventData other)
			{
				return this.TotalPackets == other.TotalPackets && this.TotalUpload == other.TotalUpload && this.TotalConstantsUpload == other.TotalConstantsUpload && this.TotalReliableUpload == other.TotalReliableUpload && this.TotalReplicationUpload == other.TotalReplicationUpload && this.TotalUnreliableUpload == other.TotalUnreliableUpload && this.TotalOtherUpload == other.TotalOtherUpload;
			}

			// Token: 0x06004142 RID: 16706 RVA: 0x000FB5F1 File Offset: 0x000F97F1
			public override bool Equals(object obj)
			{
				return obj != null && (this == obj || (obj.GetType() == base.GetType() && this.Equals((DebugNetworkEventStatistics.TotalEventData)obj)));
			}

			// Token: 0x06004143 RID: 16707 RVA: 0x000FB620 File Offset: 0x000F9820
			public override int GetHashCode()
			{
				return (((((((((((this.TotalPackets * 397) ^ this.TotalUpload) * 397) ^ this.TotalConstantsUpload) * 397) ^ this.TotalReliableUpload) * 397) ^ this.TotalReplicationUpload) * 397) ^ this.TotalUnreliableUpload) * 397) ^ this.TotalOtherUpload;
			}

			// Token: 0x06004144 RID: 16708 RVA: 0x000FB681 File Offset: 0x000F9881
			public TotalEventData()
			{
			}

			// Token: 0x06004145 RID: 16709 RVA: 0x000FB68C File Offset: 0x000F988C
			public TotalEventData(int totalPackets, int totalUpload, int totalConstants, int totalReliable, int totalReplication, int totalUnreliable)
			{
				this.TotalPackets = totalPackets;
				this.TotalUpload = totalUpload;
				this.TotalConstantsUpload = totalConstants;
				this.TotalReliableUpload = totalReliable;
				this.TotalReplicationUpload = totalReplication;
				this.TotalUnreliableUpload = totalUnreliable;
				this.TotalOtherUpload = totalUpload - (totalConstants + totalReliable + totalReplication + totalUnreliable);
			}

			// Token: 0x17000AF5 RID: 2805
			// (get) Token: 0x06004146 RID: 16710 RVA: 0x000FB6DE File Offset: 0x000F98DE
			public bool HasData
			{
				get
				{
					return this.TotalUpload > 0;
				}
			}

			// Token: 0x06004147 RID: 16711 RVA: 0x000FB6EC File Offset: 0x000F98EC
			public static DebugNetworkEventStatistics.TotalEventData operator -(DebugNetworkEventStatistics.TotalEventData d1, DebugNetworkEventStatistics.TotalEventData d2)
			{
				return new DebugNetworkEventStatistics.TotalEventData(d1.TotalPackets - d2.TotalPackets, d1.TotalUpload - d2.TotalUpload, d1.TotalConstantsUpload - d2.TotalConstantsUpload, d1.TotalReliableUpload - d2.TotalReliableUpload, d1.TotalReplicationUpload - d2.TotalReplicationUpload, d1.TotalUnreliableUpload - d2.TotalUnreliableUpload);
			}

			// Token: 0x06004148 RID: 16712 RVA: 0x000FB74C File Offset: 0x000F994C
			public static bool operator ==(DebugNetworkEventStatistics.TotalEventData d1, DebugNetworkEventStatistics.TotalEventData d2)
			{
				return d1.TotalPackets == d2.TotalPackets && d1.TotalUpload == d2.TotalUpload && d1.TotalConstantsUpload == d2.TotalConstantsUpload && d1.TotalReliableUpload == d2.TotalReliableUpload && d1.TotalReplicationUpload == d2.TotalReplicationUpload && d1.TotalUnreliableUpload == d2.TotalUnreliableUpload;
			}

			// Token: 0x06004149 RID: 16713 RVA: 0x000FB7AF File Offset: 0x000F99AF
			public static bool operator !=(DebugNetworkEventStatistics.TotalEventData d1, DebugNetworkEventStatistics.TotalEventData d2)
			{
				return !(d1 == d2);
			}

			// Token: 0x0400227E RID: 8830
			public readonly int TotalPackets;

			// Token: 0x0400227F RID: 8831
			public readonly int TotalUpload;

			// Token: 0x04002280 RID: 8832
			public readonly int TotalConstantsUpload;

			// Token: 0x04002281 RID: 8833
			public readonly int TotalReliableUpload;

			// Token: 0x04002282 RID: 8834
			public readonly int TotalReplicationUpload;

			// Token: 0x04002283 RID: 8835
			public readonly int TotalUnreliableUpload;

			// Token: 0x04002284 RID: 8836
			public readonly int TotalOtherUpload;
		}

		// Token: 0x02000682 RID: 1666
		private class PerEventData : IComparable<DebugNetworkEventStatistics.PerEventData>
		{
			// Token: 0x0600414A RID: 16714 RVA: 0x000FB7BB File Offset: 0x000F99BB
			public int CompareTo(DebugNetworkEventStatistics.PerEventData other)
			{
				return other.TotalDataSize - this.TotalDataSize;
			}

			// Token: 0x04002285 RID: 8837
			public string Name;

			// Token: 0x04002286 RID: 8838
			public int DataSize;

			// Token: 0x04002287 RID: 8839
			public int TotalDataSize;

			// Token: 0x04002288 RID: 8840
			public int Count;
		}

		// Token: 0x02000683 RID: 1667
		public class PerSecondEventData
		{
			// Token: 0x0600414C RID: 16716 RVA: 0x000FB7D2 File Offset: 0x000F99D2
			public PerSecondEventData(int totalUploadPerSecond, int constantsUploadPerSecond, int reliableUploadPerSecond, int replicationUploadPerSecond, int unreliableUploadPerSecond, int otherUploadPerSecond)
			{
				this.TotalUploadPerSecond = totalUploadPerSecond;
				this.ConstantsUploadPerSecond = constantsUploadPerSecond;
				this.ReliableUploadPerSecond = reliableUploadPerSecond;
				this.ReplicationUploadPerSecond = replicationUploadPerSecond;
				this.UnreliableUploadPerSecond = unreliableUploadPerSecond;
				this.OtherUploadPerSecond = otherUploadPerSecond;
			}

			// Token: 0x04002289 RID: 8841
			public readonly int TotalUploadPerSecond;

			// Token: 0x0400228A RID: 8842
			public readonly int ConstantsUploadPerSecond;

			// Token: 0x0400228B RID: 8843
			public readonly int ReliableUploadPerSecond;

			// Token: 0x0400228C RID: 8844
			public readonly int ReplicationUploadPerSecond;

			// Token: 0x0400228D RID: 8845
			public readonly int UnreliableUploadPerSecond;

			// Token: 0x0400228E RID: 8846
			public readonly int OtherUploadPerSecond;
		}

		// Token: 0x02000684 RID: 1668
		private class TotalData
		{
			// Token: 0x0400228F RID: 8847
			public float TotalTime;

			// Token: 0x04002290 RID: 8848
			public int TotalFrameCount;

			// Token: 0x04002291 RID: 8849
			public int TotalCount;

			// Token: 0x04002292 RID: 8850
			public int TotalDataSize;
		}
	}
}
