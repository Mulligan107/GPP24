using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace JumperGame.src.manager
{
    class AudioManager
    {
        //The sound effects that will be used
        public static IntPtr[] Sounds = new IntPtr[10]; //TODO - Automatisieren

        //The music that will be played
        private static IntPtr _music = IntPtr.Zero;

        // Set the volume for sound effects and music
        public static int SoundVolume = 1; // 0 -> 128
        private static int _musicVolume = 4;

        public bool LoadMedia()
        {
            //Loading success flag
            bool success = true;

            //Load sound effects
            Sounds[0] = SDL_mixer.Mix_LoadWAV("sounds/coin.wav");
            if (Sounds[0] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load button.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                SDL_mixer.Mix_VolumeChunk(Sounds[0], SoundVolume);
            }

            Sounds[1] = SDL_mixer.Mix_LoadWAV("src\\sounds\\explosion.wav");
            if (Sounds[1] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load button_back.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                SDL_mixer.Mix_VolumeChunk(Sounds[1], SoundVolume);
            }

            Sounds[2] = SDL_mixer.Mix_LoadWAV("src\\sounds/hurt.wav");
            if (Sounds[2] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load button_next.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                SDL_mixer.Mix_VolumeChunk(Sounds[2], SoundVolume);
            }

            Sounds[3] = SDL_mixer.Mix_LoadWAV("src\\sounds/jump.wav");
            if (Sounds[3] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load enemy_explode.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                SDL_mixer.Mix_VolumeChunk(Sounds[3], SoundVolume);
            }

            Sounds[4] = SDL_mixer.Mix_LoadWAV("src\\sounds/power_up.wav");
            if (Sounds[4] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load player_hit.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                SDL_mixer.Mix_VolumeChunk(Sounds[4], SoundVolume);
            }

            Sounds[5] = SDL_mixer.Mix_LoadWAV("src\\sounds/tap.wav");
            if (Sounds[5] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load enemy_shoot_laser.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                SDL_mixer.Mix_VolumeChunk(Sounds[5], SoundVolume);
            }



            /*
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

            */

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

        public static void Close()
        {
            // Free the sound effects
            for (int i = 0; i < Sounds.Length; i++)
            {
                if (Sounds[i] != IntPtr.Zero)
                {
                    SDL_mixer.Mix_FreeChunk(Sounds[i]);
                    Sounds[i] = IntPtr.Zero;
                }
            }

            // Free the music
            if (_music != IntPtr.Zero)
            {
                SDL_mixer.Mix_FreeMusic(_music);
                _music = IntPtr.Zero;
            }
        }

        public bool Initialize()
        {
            //Initialize SDL
            if (SDL.SDL_Init(SDL.SDL_INIT_AUDIO) < 0)
            {
                Console.WriteLine("SDL could not initialize! SDL_Error: {0}", SDL.SDL_GetError());
                return false;
            }
            else
            {

                //Initialize SDL_mixer
                if (SDL_mixer.Mix_OpenAudio(44100, SDL_mixer.MIX_DEFAULT_FORMAT, 2, 2048) < 0)
                {
                    Console.WriteLine("SDL_mixer could not initialize! SDL_mixer Error: {0}", SDL.SDL_GetError());
                    return false;
                }
            }


            return true;
        }
    }
    
}
