using System;
using System.Linq;
using JumperGame.components;
using JumperGame.src.components;
using JumperGame.src.manager;
using JumperGame.systems;
using SDL2;

namespace JumperGame
{
    public class JumperGame
    {
        public static entitySystem entitySystem;
        private RenderManager _rendering;
        private AudioManager _audio;
        private RescourceManager _rescource;
        
        private PhysicsSystem _physicsSystem;
        private InputSystem _inputSystem;
        private QuitSystem _quitSystem;
        private MovementSystem _movementSystem;
        private EnemyMovementSystem _enemyMovementSystem;
        
        public PlayerSteeringComponent PlayerSteeringComponent;
        
        public bool IsRunning;
        public bool IsReset;

        static int Main(string[] args)
        {
            JumperGame game = new JumperGame();

            //Initialize the game
            game.InitializeSystems();

            // Start the game loop
            game.Run();

            return 0;
        }
        
        public void InitializeSystems()
        {
            entitySystem = new entitySystem();
            
            _rendering = new RenderManager();
            _physicsSystem = new PhysicsSystem();
            _audio = new AudioManager();
            _rescource = new RescourceManager();

            _inputSystem = new InputSystem();
            _quitSystem = new QuitSystem(this);
            _movementSystem = new MovementSystem(entitySystem);
            _enemyMovementSystem = new EnemyMovementSystem(entitySystem);
            
            _inputSystem.KeyPressed += _movementSystem.Update;
            _inputSystem.KeyReleased += _movementSystem.OnKeyReleased;
            
            _inputSystem.GameQuitRequested += _quitSystem.QuitGame;
            _rescource.loadTiles();

            InitializeSdl();
        }
        
        public bool InitializeSdl()
        {
            // Initialize the managers and check if initialization was successful
            bool isPhysicsInitSuccessful = _physicsSystem.Initialize();
            bool isAudioInitSuccessful = _audio.Initialize();
            
            if (!isPhysicsInitSuccessful || !isAudioInitSuccessful)
            {
                Console.WriteLine("Failed to initialize the game!");
                Console.ReadLine();
                return false;
            }

            // If all initializations were successful, return true
            
            return true;
        }
        
        public void Run()
        {
            IsRunning = true;
          
            var timerStart = SDL.SDL_GetPerformanceCounter() / (double)SDL.SDL_GetPerformanceFrequency();
            var timerNew = timerStart;
          
            
                SDL.SDL_Event e;
                while (IsRunning)
                {
                    var timerCurrent = SDL.SDL_GetPerformanceCounter() / (double)SDL.SDL_GetPerformanceFrequency();
                    timerCurrent = timerCurrent- timerStart;

                    var deltaTime = (SDL.SDL_GetPerformanceCounter() - timerNew) / (double)SDL.SDL_GetPerformanceFrequency();
                    timerNew = SDL.SDL_GetPerformanceCounter();
                    
                    _movementSystem.UpdatePlayerState();

                    while (SDL.SDL_PollEvent(out e) != 0)
                    {
                        // Process the input events
                        _inputSystem.ProcessInput(e);
                    }
                    
                    // Update enemy movements
                    _enemyMovementSystem.Update(deltaTime);
                    
                    // Retrieve all entities
                    var entities = entitySystem.GetAllEntities();

                    // Update the managers
                    _physicsSystem.Update(entities, deltaTime);
                    _rendering.Update(deltaTime, timerCurrent);
                    
                    entitySystem.Update(deltaTime,timerCurrent);
                    
                    // _audio.Update();
                    // _input.Update();
                }
        }
    }
}