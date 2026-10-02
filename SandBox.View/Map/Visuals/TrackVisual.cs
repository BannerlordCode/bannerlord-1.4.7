using System;
using SandBox.View.Map.Managers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.View.Map.Visuals
{
	// Token: 0x02000065 RID: 101
	public class TrackVisual : MapEntityVisual<Track>
	{
		// Token: 0x06000453 RID: 1107 RVA: 0x000240D4 File Offset: 0x000222D4
		public TrackVisual(Track track)
			: base(track)
		{
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x000240DD File Offset: 0x000222DD
		public override CampaignVec2 InteractionPositionForPlayer
		{
			get
			{
				return base.MapEntity.Position;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x000240EA File Offset: 0x000222EA
		public override MapEntityVisual AttachedTo
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x000240ED File Offset: 0x000222ED
		public override Vec3 GetVisualPosition()
		{
			return base.MapEntity.Position.AsVec3();
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x000240FF File Offset: 0x000222FF
		public override bool IsVisibleOrFadingOut()
		{
			return base.MapEntity.IsDetected;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0002410C File Offset: 0x0002230C
		public override void OnHover()
		{
			InformationManager.ShowTooltip(typeof(Track), new object[] { base.MapEntity });
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0002412C File Offset: 0x0002232C
		public override bool OnMapClick(bool followModifierUsed)
		{
			return false;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0002412F File Offset: 0x0002232F
		public override void OnOpenEncyclopedia()
		{
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00024131 File Offset: 0x00022331
		public override void ReleaseResources()
		{
			MapTracksVisualManager.Current.ReleaseResources(base.MapEntity);
		}

		// Token: 0x0400021B RID: 539
		private static TextObject _defaultTrackTitle = new TextObject("{=maptrack}Track", null);
	}
}
