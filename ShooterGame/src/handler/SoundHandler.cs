using System;
using SDL2;

namespace ShooterGame
{
    public class SoundHandler
    {
        //The sound effects that will be used
        public static IntPtr[] Sounds = new IntPtr[10]; //TODO - Automatisieren

        //The music that will be played
        private static IntPtr _music = IntPtr.Zero;
        
        // Set the volume for sound effects and music
        public static int SoundVolume = 0; // 0 -> 128
        private static int _musicVolume = 4; 
        
        public static bool LoadMedia()
        {
            //Loading success flag
            bool success = true;

            //Load sound effects
            Sounds[0] = SDL_mixer.Mix_LoadWAV("sounds/buttons/button.wav");
            if (Sounds[0] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load button.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                SDL_mixer.Mix_VolumeChunk(Sounds[0], SoundVolume);
            }

            Sounds[1] = SDL_mixer.Mix_LoadWAV("sounds/buttons/button_back.wav");
            if (Sounds[1] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load button_back.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                SDL_mixer.Mix_VolumeChunk(Sounds[1], SoundVolume);
            }
            
            Sounds[2] = SDL_mixer.Mix_LoadWAV("sounds/buttons/button_next.wav");
            if (Sounds[2] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load button_next.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                SDL_mixer.Mix_VolumeChunk(Sounds[2], SoundVolume);
            }
            
            Sounds[3] = SDL_mixer.Mix_LoadWAV("sounds/enemy/enemy_explode.wav");
            if (Sounds[3] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load enemy_explode.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                SDL_mixer.Mix_VolumeChunk(Sounds[3], SoundVolume);
            }
            
            Sounds[4] = SDL_mixer.Mix_LoadWAV("sounds/enemy/enemy_hit.wav");
            if (Sounds[4] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load player_hit.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                SDL_mixer.Mix_VolumeChunk(Sounds[4], SoundVolume);
            }
            
            Sounds[5] = SDL_mixer.Mix_LoadWAV("sounds/enemy/enemy_shoot_laser.wav");
            if (Sounds[5] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load enemy_shoot_laser.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                SDL_mixer.Mix_VolumeChunk(Sounds[5], SoundVolume);
            }
            
            Sounds[6] = SDL_mixer.Mix_LoadWAV("sounds/player/player_hit.wav");
            if (Sounds[6] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load player_explode.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                SDL_mixer.Mix_VolumeChunk(Sounds[6], SoundVolume);
            }
            
            Sounds[7] = SDL_mixer.Mix_LoadWAV("sounds/player/player_shoot1.wav");
            if (Sounds[7] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load player_shoot1.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                SDL_mixer.Mix_VolumeChunk(Sounds[7], SoundVolume);
            }
            
            Sounds[8] = SDL_mixer.Mix_LoadWAV("sounds/enemy/enemy_shoot_laser_2.wav");
            if (Sounds[8] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load enemy_shoot_laser_2.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                SDL_mixer.Mix_VolumeChunk(Sounds[8], SoundVolume);
            }
            
            Sounds[9] = SDL_mixer.Mix_LoadWAV("sounds/enemy/enemy_shoot_laser_3.wav");
            if (Sounds[9] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load enemy_shoot_laser_3.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                SDL_mixer.Mix_VolumeChunk(Sounds[9], SoundVolume);
            }
            
            
            //Load music
            _music = SDL_mixer.Mix_LoadMUS("sounds/music/menu_music.wav");
            if (_music == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load lofi.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                SDL_mixer.Mix_VolumeMusic(_musicVolume);
            }

            return success;
        }

        public static void PlaySound(int soundIndex)
        {
            if (soundIndex >= 0 && soundIndex < Sounds.Length)
            {
                SDL_mixer.Mix_PlayChannel(-1, Sounds[soundIndex], 0);
            }
            else
            {
                Console.WriteLine("Sound index out of range: {0}", soundIndex);
            }
        }
        
        public static void PlayMusic()
        {
            if (SDL_mixer.Mix_PlayingMusic() == 0)
            {
                SDL_mixer.Mix_PlayMusic(_music, -1);
            }
            else
            {
                if (SDL_mixer.Mix_PausedMusic() == 1)
                {
                    SDL_mixer.Mix_ResumeMusic();
                }
                else
                {
                    SDL_mixer.Mix_PauseMusic();
                }
            }
        }
        
        public static void StopMusic()
        {
            SDL_mixer.Mix_HaltMusic();
        }
    }
}