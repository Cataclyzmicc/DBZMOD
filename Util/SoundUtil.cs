﻿using DBZMOD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace Util
{
    public static class SoundUtil
    {
        public static uint InvalidSlot = (uint)ReLogic.Utilities.SlotId.Invalid.ToFloat();
        public static DBZMOD.DBZMOD _mod;
        public static DBZMOD.DBZMOD mod
        {
            get
            {
                if (_mod == null)
                {
                    _mod = DBZMOD.DBZMOD.instance;
                }

                return _mod;
            }
        }

        public static void PlayVanillaSound(int soundId, Player player = null, float volume = 1f, float pitchVariance = 0f)
        {
            Vector2 location = player != null ? player.Center : Vector2.Zero;
            PlayVanillaSound(soundId, location, volume, pitchVariance);
        }

        public static void PlayVanillaSound(int soundId, Vector2 location, float volume = 1f, float pitchVariance = 0f)
        {
            if (Main.dedServ)
                return;
            if (location == Vector2.Zero)
            {
                Main.PlaySound(soundId, (int)location.X, (int)location.Y, 1, volume, pitchVariance);
            }
            else
            {
                // this method doesn't return a sound effect instance, it just plays a sound.
                Main.PlaySound(soundId, -1, -1, 1, volume, pitchVariance);
            }
        }

        public static SoundEffectInstance PlayVanillaSound(int soundId, Vector2 location, int style)
        {
            if (Main.dedServ)
                return null;

            // this method doesn't return a sound effect instance, it just plays a sound.
            return Main.PlaySound(soundId, (int)location.X, (int)location.Y, style);
        }        

        public static ReLogic.Utilities.SlotId PlayVanillaSound(Terraria.Audio.LegacySoundStyle soundId, Player player = null, float volume = 1f, float pitchVariance = 0f)
        {
            Vector2 location = player != null ? player.Center : Vector2.Zero;
            return PlayVanillaSound(soundId, location, volume, pitchVariance);
        }

        public static ReLogic.Utilities.SlotId PlayVanillaSound(Terraria.Audio.LegacySoundStyle soundId, Vector2 location, float volume = 1f, float pitchVariance = 0f)
        {
            if (Main.dedServ)
                return ReLogic.Utilities.SlotId.Invalid;
            if (location == Vector2.Zero)
            {
                return Main.PlayTrackedSound(soundId.WithVolume(volume).WithPitchVariance(pitchVariance), location);
            } else
            {
                // this method doesn't return a sound effect instance, it just plays a sound.
                return Main.PlayTrackedSound(soundId.WithVolume(volume).WithPitchVariance(pitchVariance), location);
            }            
        }

        public static KeyValuePair<uint, SoundEffectInstance> PlayCustomSound(string soundId, Player player = null, float volume = 1f, float pitchVariance = 0f)
        {
            Vector2 location = player != null ? player.Center : Vector2.Zero;
            return PlayCustomSound(soundId, location, volume, pitchVariance);
        }

        public static KeyValuePair<uint, SoundEffectInstance> PlayCustomSound(string soundId, Vector2 location, float volume = 1f, float pitchVariance = 0f)
        {
            if (Main.dedServ)
                return new KeyValuePair<uint, SoundEffectInstance>(InvalidSlot, null);

            var slotId = InvalidSlot;
            var style = GetCustomStyle(soundId, volume, pitchVariance);
            SoundEffectInstance sound = null;
            if (location == Vector2.Zero)
            {
                sound = Main.PlaySound(style);                
            } else
            {
                sound = Main.PlaySound(style, location);
            }
            slotId = (uint)mod.GetSoundSlot(SoundType.Custom, soundId);
            return new KeyValuePair<uint, SoundEffectInstance>(slotId, sound);
        }

        public static void PlayCustomSound(ReLogic.Utilities.SlotId slotId)
        {            
            var sound = Main.GetActiveSound(slotId);
            if (!sound.IsPlaying)
                sound.Resume();
        }

        public static Terraria.Audio.LegacySoundStyle GetCustomStyle(string soundId, float volume = 1f, float pitchVariance = 0f)
        {
            return mod.GetLegacySoundSlot(SoundType.Custom, soundId).WithVolume(volume).WithPitchVariance(pitchVariance);
        }

        public static KeyValuePair<uint, SoundEffectInstance> KillTrackedSound(KeyValuePair<uint, SoundEffectInstance> soundInfo)
        {
            var sound = Main.GetActiveSound(new ReLogic.Utilities.SlotId(soundInfo.Key));
            if (sound != null)
            {
                sound.Stop();
            } else
            {
                var soundInstance = soundInfo.Value;
                if (soundInstance != null)
                    soundInstance.Stop();
            }

            return new KeyValuePair<uint, SoundEffectInstance>(InvalidSlot, null);
        }

        public static void UpdateTrackedSound(KeyValuePair<uint, SoundEffectInstance> soundInfo, Vector2 position)
        {
            var sound = Main.GetActiveSound(new ReLogic.Utilities.SlotId(soundInfo.Key));
            if (sound != null)
            {
                sound.Position = position;
                sound.Update();
            }
        }

        public static bool CanPlayOtherPlayerAudio(MyPlayer myPlayer, Player otherPlayer)
        {
            return myPlayer.PlayerIndexWithLocalAudio == otherPlayer.whoAmI || myPlayer.PlayerIndexWithLocalAudio == -1;
        }

        // tries to settle ties when trying to play aura and charge effects - the local player always wins, otherwise it's first come first serve. Only one at a time.
        public static bool ShouldPlayPlayerAudio(Player player, bool isTransformation)
        {
            bool shouldPlayAudio;
            var modPlayer = player.GetModPlayer<MyPlayer>();            
            if (player.whoAmI == Main.myPlayer)
            {
                shouldPlayAudio = modPlayer.TransformationSoundInfo.Value == null || isTransformation;
                if (modPlayer.ChargeSoundInfo.Value != null && isTransformation)
                {
                    modPlayer.ChargeSoundInfo = KillTrackedSound(modPlayer.ChargeSoundInfo);
                }
                
                if (modPlayer.PlayerIndexWithLocalAudio != -1)
                {
                    KillOtherPlayerAudio(player);
                }
            }            
            else
            {
                var myPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
                shouldPlayAudio = myPlayer.ChargeSoundInfo.Value == null && myPlayer.TransformationSoundInfo.Value == null && CanPlayOtherPlayerAudio(myPlayer, player);                
                if (shouldPlayAudio)
                {
                    myPlayer.PlayerIndexWithLocalAudio = player.whoAmI;
                }
            }
            return shouldPlayAudio;
        }

        // handles terminating other player's audio loops when my player does something that needs unrestricted audio usage.
        public static void KillOtherPlayerAudio(Player myPlayer)
        {
            for(var i = 0; i < Main.player.Length; i++)
            {
                var player = Main.player[i];
                if (player.whoAmI != i)
                    continue;
                if (player.whoAmI == Main.myPlayer)
                    continue;
                var modPlayer = player.GetModPlayer<MyPlayer>();
                modPlayer.ChargeSoundInfo = KillTrackedSound(modPlayer.ChargeSoundInfo);
                modPlayer.TransformationSoundInfo = KillTrackedSound(modPlayer.TransformationSoundInfo);
            }
            var myModPlayer = myPlayer.GetModPlayer<MyPlayer>();
            myModPlayer.PlayerIndexWithLocalAudio = -1;
        }
    }
}
