using System;
using SDL2;

namespace JumperGame.src.manager
{
    class AudioManager
    {
        // The sound effects that will be used
        private static IntPtr[] Sounds = new IntPtr[100];

        // The music that will be played
        private static IntPtr _music = IntPtr.Zero;

        // Set the volume for sound effects and music
        public static int SoundVolume { get; set; } = 2; // 0 -> 128
        private static int _musicVolume = 4;

        public bool Initialize()
        {
            // Initialize SDL
            if (SDL.SDL_Init(SDL.SDL_INIT_AUDIO) < 0)
            {
                Console.WriteLine($"SDL could not initialize! SDL_Error: {SDL.SDL_GetError()}");
                return false;
            }

            // Initialize SDL_mixer
            if (SDL_mixer.Mix_OpenAudio(44100, SDL_mixer.MIX_DEFAULT_FORMAT, 2, 2048) < 0)
            {
                Console.WriteLine($"SDL_mixer could not initialize! SDL_mixer Error: {SDL.SDL_GetError()}");
                return false;
            }

            return true;
        }

        public bool LoadMedia()
        {
            bool success = true;

            // Load sound effects
            success &= LoadSound(0, "sounds/coin.wav");
            success &= LoadSound(1, "sounds/explosion.wav");
            success &= LoadSound(2, "sounds/hurt.wav");
            success &= LoadSound(3, "sounds/jump.wav");
            success &= LoadSound(4, "sounds/power_up.wav");
            success &= LoadSound(5, "sounds/tap.wav");
            success &= LoadSound(6, "sounds/button.wav");
            success &= LoadSound(7, "sounds/button_back.wav");
            success &= LoadSound(8, "sounds/button_next.wav");
            success &= LoadSound(9, "sounds/wilhelm.wav");

            // Load music
            _music = SDL_mixer.Mix_LoadMUS("sounds/music/Dorian-Concept_Hide.wav");
            if (_music == IntPtr.Zero)
            {
                Console.WriteLine($"Failed to load menu_music.wav! {SDL.SDL_GetError()}");
                success = false;
            }
            else
            {
                SDL_mixer.Mix_VolumeMusic(_musicVolume);
            }

            return success;
        }

        private bool LoadSound(int index, string path)
        {
            Sounds[index] = SDL_mixer.Mix_LoadWAV(path);
            if (Sounds[index] == IntPtr.Zero)
            {
                Console.WriteLine($"Failed to load {path}! {SDL.SDL_GetError()}");
                return false;
            }
            else
            {
                SDL_mixer.Mix_VolumeChunk(Sounds[index], SoundVolume);
                return true;
            }
        }

        public static void PlaySound(int soundIndex)
        {
            if (soundIndex >= 0 && soundIndex < Sounds.Length)
            {
                SDL_mixer.Mix_PlayChannel(-1, Sounds[soundIndex], 0);
            }
            else
            {
                Console.WriteLine($"Sound index out of range: {soundIndex}");
            }
        }

        public static void PlayOrPauseMusic()
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
        
        public static void SetMusicVolume(int volume)
        {
            _musicVolume = volume;
            SDL_mixer.Mix_VolumeMusic(_musicVolume);
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
    }
}