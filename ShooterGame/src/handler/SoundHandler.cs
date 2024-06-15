using System;
using SDL2;

namespace ShooterGame
{
    public class SoundHandler
    {
        //The sound effects that will be used
        public static IntPtr[] sounds = new IntPtr[5]; //ToDo: Anzahl der Sounds anpassen

        //The music that will be played
        private static IntPtr _music = IntPtr.Zero;
        
        public static bool LoadMedia()
        {
            //Loading success flag
            bool success = true;

            //Load sound effects
            sounds[0] = SDL_mixer.Mix_LoadWAV("sounds/bloop.wav");
            if (sounds[0] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load bloop.wav! {0}", SDL.SDL_GetError());
                success = false;
            }

            sounds[1] = SDL_mixer.Mix_LoadWAV("sounds/wilhelm.wav");
            if (sounds[1] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load wilhelm.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            /*

            _medium = SDL_mixer.Mix_LoadWAV("medium.wav");
            if (_medium == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load medium.wav! {0}", SDL.SDL_GetError());
                success = false;
            }

            _low = SDL_mixer.Mix_LoadWAV("low.wav");
            if (_low == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load low.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            */
            
            //Load music
            _music = SDL_mixer.Mix_LoadMUS("sounds/lofi.wav");
            if (_music == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load lofi.wav! {0}", SDL.SDL_GetError());
                success = false;
            }

            return success;
        }

        public static void PlaySound(int soundIndex)
        {
            if (soundIndex >= 0 && soundIndex < sounds.Length)
            {
                SDL_mixer.Mix_PlayChannel(-1, sounds[soundIndex], 0);
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