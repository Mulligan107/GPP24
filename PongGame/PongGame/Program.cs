using SDL2;
using System;
using static SDL2.SDL;

namespace PongGame
{

    internal class Program
    {
        static void Main(string[] args)
        {
            SDL.SDL_Event passiert;

            if (SDL_Init(SDL_INIT_VIDEO) < 0) 
            {
                Error("Init failed");
                return;
            }

            var window = SDL_CreateWindow("Hello SDL2",
                SDL_WINDOWPOS_UNDEFINED,
                SDL_WINDOWPOS_UNDEFINED,
                800,
                600,
                SDL_WindowFlags.SDL_WINDOW_SHOWN);

         //   SDL_SetWindowFullscreen(window, (uint)SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);

            if(window == null)
            {
                Error("Window creation failed");
                return;
            }

            var renderer = SDL_CreateRenderer(window, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
            SDL_SetRenderDrawColor(renderer, 0, 0, 255, 255);
            SDL_RenderClear(renderer);

            SDL_RenderPresent(renderer);

            SDL_Delay(3000);

            //while (SDL.SDL_PollEvent(out var @event) != 0)
            while (true)
            {
                SDL.SDL_PollEvent(out var @event);
                    // We are only worried about SDL.SDL_EventType.SDL_KEYDOWN and SDL.SDL_EventType.SDL_KEYUP events
                    switch (@event.type)
                {
                    case SDL.SDL_EventType.SDL_KEYDOWN:
                        Console.WriteLine("Key press detected");
                        byte r = (byte)gibZahl();
                        byte g = (byte)gibZahl();
                        byte b = (byte)gibZahl();
                        SDL_SetRenderDrawColor(renderer, r, g, b, 255);
                        SDL_RenderClear(renderer);
                        SDL_RenderPresent(renderer);
                        break;

                    case SDL.SDL_EventType.SDL_KEYUP:
                        Console.WriteLine("Key release detected");
                        break;

                    default:
                        break;
                }
            }
            //SDL_Quit();

            return;

            
        }



        private static void Error(string v)
        {
            Console.WriteLine($"Error: {v} SDL_Error: {SDL_GetError()}");
        }

        static int gibZahl()
        {
            var rand = new Random();
            return rand.Next(0, 255);
        }
    }
}
