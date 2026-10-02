using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000361 RID: 865
	public class SoundPlayer : ScriptComponentBehavior
	{
		// Token: 0x0600318B RID: 12683 RVA: 0x000CA2C8 File Offset: 0x000C84C8
		private void ValidateSoundEvent()
		{
			if ((this.SoundEvent == null || !this.SoundEvent.IsValid) && this.SoundName.Length > 0)
			{
				if (this.SoundCode == -1)
				{
					this.SoundCode = SoundManager.GetEventGlobalIndex(this.SoundName);
				}
				this.SoundEvent = SoundEvent.CreateEvent(this.SoundCode, base.GameEntity.Scene);
			}
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x000CA331 File Offset: 0x000C8531
		public void UpdatePlaying()
		{
			this.Playing = this.SoundEvent != null && this.SoundEvent.IsValid && this.SoundEvent.IsPlaying();
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x000CA35C File Offset: 0x000C855C
		public void PlaySound()
		{
			if (this.Playing)
			{
				return;
			}
			if (this.SoundEvent != null && this.SoundEvent.IsValid)
			{
				this.SoundEvent.SetPosition(base.GameEntity.GlobalPosition);
				this.SoundEvent.Play();
				this.Playing = true;
			}
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x000CA3B3 File Offset: 0x000C85B3
		public void ResumeSound()
		{
			if (this.Playing)
			{
				return;
			}
			if (this.SoundEvent != null && this.SoundEvent.IsValid && this.SoundEvent.IsPaused())
			{
				this.SoundEvent.Resume();
				this.Playing = true;
			}
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x000CA3F2 File Offset: 0x000C85F2
		public void PauseSound()
		{
			if (!this.Playing)
			{
				return;
			}
			if (this.SoundEvent != null && this.SoundEvent.IsValid)
			{
				this.SoundEvent.Pause();
				this.Playing = false;
			}
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x000CA424 File Offset: 0x000C8624
		public void StopSound()
		{
			if (!this.Playing)
			{
				return;
			}
			if (this.SoundEvent != null && this.SoundEvent.IsValid)
			{
				this.SoundEvent.Stop();
				this.Playing = false;
			}
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x000CA456 File Offset: 0x000C8656
		protected internal override void OnInit()
		{
			base.OnInit();
			MBDebug.Print("SoundPlayer : OnInit called.", 0, Debug.DebugColor.Yellow, 17592186044416UL);
			this.ValidateSoundEvent();
			if (this.AutoStart)
			{
				this.PlaySound();
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x000CA494 File Offset: 0x000C8694
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x000CA49E File Offset: 0x000C869E
		protected internal override void OnTick(float dt)
		{
			this.UpdatePlaying();
			if (!this.Playing && this.AutoLoop)
			{
				this.ValidateSoundEvent();
				this.PlaySound();
			}
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x000CA4C2 File Offset: 0x000C86C2
		protected internal override bool MovesEntity()
		{
			return false;
		}

		// Token: 0x040014F1 RID: 5361
		private bool Playing;

		// Token: 0x040014F2 RID: 5362
		private int SoundCode = -1;

		// Token: 0x040014F3 RID: 5363
		private SoundEvent SoundEvent;

		// Token: 0x040014F4 RID: 5364
		public bool AutoLoop;

		// Token: 0x040014F5 RID: 5365
		public bool AutoStart;

		// Token: 0x040014F6 RID: 5366
		public string SoundName;
	}
}
