using System;
using System.Linq;
using JumperGame.components;
using JumperGame.gameEntities;
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

        private MenuSystem _menuSystem;
        private PhysicsSystem _physicsSystem;
        private InputSystem _inputSystem;
        private QuitSystem _quitSystem;
        private MovementSystem _movementSystem;
        private EnemyMovementSystem _enemyMovementSystem;

        public bool IsRunning;
        public bool IsReset;
        
        public static JumperGame Instance { get; private set; }
        public bool IsMenuOpen { get; set; } = true;

        public JumperGame()
        {
            Instance = this;
        }

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
            _menuSystem = new MenuSystem();
            _rendering.InitializeMenu(_menuSystem);
    
            _physicsSystem = new PhysicsSystem();
            _audio = new AudioManager();
            _rescource = new RescourceManager("Level1");

            _inputSystem = new InputSystem();
            _quitSystem = new QuitSystem(this);
            _movementSystem = new MovementSystem(entitySystem);
            _enemyMovementSystem = new EnemyMovementSystem(entitySystem);
    
            _inputSystem.KeyPressed += _movementSystem.Update;
            _inputSystem.KeyReleased += _movementSystem.OnKeyReleased;
    
            _inputSystem.InitializeMenuRequested += () => _rendering.InitializeMenu(_menuSystem);
            _inputSystem.SelectNextMenuItem += _menuSystem.SelectNextItem;
            _inputSystem.SelectPreviousMenuItem += _menuSystem.SelectPreviousItem;
            _inputSystem.ExecuteMenuItem += _menuSystem.ExecuteSelectedItem;
    
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
        
        public void LoadLevel(string levelName)
        {
            entitySystem = new entitySystem();
            _rescource = new RescourceManager(levelName);
            _inputSystem = new InputSystem();
            _quitSystem = new QuitSystem(this);
            _movementSystem = new MovementSystem(entitySystem);
            _enemyMovementSystem = new EnemyMovementSystem(entitySystem);

            _inputSystem.KeyPressed += _movementSystem.Update;
            _inputSystem.KeyReleased += _movementSystem.OnKeyReleased;
            
            _inputSystem.InitializeMenuRequested += () => _rendering.InitializeMenu(_menuSystem);
            _inputSystem.SelectNextMenuItem += _menuSystem.SelectNextItem;
            _inputSystem.SelectPreviousMenuItem += _menuSystem.SelectPreviousItem;
            _inputSystem.ExecuteMenuItem += _menuSystem.ExecuteSelectedItem;
            
            _rescource.loadTiles();
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
                timerCurrent = timerCurrent - timerStart;

                var deltaTime = (SDL.SDL_GetPerformanceCounter() - timerNew) / (double)SDL.SDL_GetPerformanceFrequency();
                timerNew = SDL.SDL_GetPerformanceCounter();

                if (IsMenuOpen)
                {
                    deltaTime = 0;
                }
                
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
                _rendering.Update(deltaTime, timerCurrent, _menuSystem);

                entitySystem.Update(deltaTime, timerCurrent);

                // _audio.Update();
                // _input.Update();
            }
        }
    }
}