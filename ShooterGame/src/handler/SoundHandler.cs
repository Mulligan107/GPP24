using System;
using SDL2;

namespace ShooterGame
{
    public class SoundHandler
    {
        //The sound effects that will be used
        public static IntPtr[] sounds = new IntPtr[8];

        //The music that will be played
        private static IntPtr _music = IntPtr.Zero;
        
        public static bool LoadMedia()
        {
            //Loading success flag
            bool success = true;

            //Load sound effects
            sounds[0] = SDL_mixer.Mix_LoadWAV("sounds/buttons/button.wav");
            if (sounds[0] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load button.wav! {0}", SDL.SDL_GetError());
                success = false;
            }

            sounds[1] = SDL_mixer.Mix_LoadWAV("sounds/buttons/button_back.wav");
            if (sounds[1] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load button_back.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            
            sounds[2] = SDL_mixer.Mix_LoadWAV("sounds/buttons/button_next.wav");
            if (sounds[2] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load button_next.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            
            sounds[3] = SDL_mixer.Mix_LoadWAV("sounds/enemy/enemy_explode.wav");
            if (sounds[3] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load enemy_explode.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            
            sounds[4] = SDL_mixer.Mix_LoadWAV("sounds/enemy/enemy_hit.wav");
            if (sounds[4] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load player_hit.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            
            sounds[5] = SDL_mixer.Mix_LoadWAV("sounds/enemy/enemy_shoot_laser.wav");
            if (sounds[5] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load enemy_shoot_laser.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            
            sounds[6] = SDL_mixer.Mix_LoadWAV("sounds/player/player_hit.wav");
            if (sounds[6] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load player_explode.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            
            sounds[7] = SDL_mixer.Mix_LoadWAV("sounds/player/player_shoot1.wav");
            if (sounds[7] == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load player_shoot1.wav! {0}", SDL.SDL_GetError());
                success = false;
            }
            
            //Load music
            _music = SDL_mixer.Mix_LoadMUS("sounds/music/menu_music.wav");
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