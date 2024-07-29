using System;
using System.Linq;
using JumperGame.components;
using System.Collections.Generic;
using JumperGame.gameEntities;
using JumperGame.src.components;
using JumperGame.src.manager;
using JumperGame.systems;
using SDL2;
using System.IO;

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
        public string CurrentLevel { get;  set; }
        
        public LevelProgressionSystem LevelProgressionSystem { get; private set; }
        public LifeSystem LifeSystem { get; private set; }

        public IEnumerable<Entity> entities;



        public JumperGame()
        {
            Instance = this;
            LevelProgressionSystem = new LevelProgressionSystem();
            LifeSystem = new LifeSystem();
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
            _audio = new AudioManager();
            InitializeSdl();
            entitySystem = new entitySystem();
    
            _rendering = new RenderManager();
            _menuSystem = new MenuSystem();
            _rendering.InitializeMenu(_menuSystem);
    
            _physicsSystem = new PhysicsSystem();
            
            
            _rescource = new RescourceManager("Level1");

            _inputSystem = new InputSystem();
            _quitSystem = new QuitSystem(this);
            _movementSystem = new MovementSystem(entitySystem);
            _enemyMovementSystem = new EnemyMovementSystem(entitySystem);
    
            _inputSystem.KeyPressed += _movementSystem.Update;
            _inputSystem.KeyReleased += _movementSystem.OnKeyReleased;
    
            _inputSystem.InitializePauseRequested += () => _rendering.InitializePauseMenu(_menuSystem);
            _inputSystem.SelectNextMenuItem += _menuSystem.SelectNextItem;
            _inputSystem.SelectPreviousMenuItem += _menuSystem.SelectPreviousItem;
            _inputSystem.ExecuteMenuItem += _menuSystem.ExecuteSelectedItem;

            

            _rescource.loadTiles();


            _audio.LoadMedia();

        }
        
        public bool InitializeSdl()
        {
            // Initialize the managers and check if initialization was successful
            bool isAudioInitSuccessful = _audio.Initialize();
            
            if (!isAudioInitSuccessful)
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
            _rendering.levelname = levelName;
            entitySystem = new entitySystem();
            _rescource = new RescourceManager(levelName);
            _inputSystem = new InputSystem();
            _quitSystem = new QuitSystem(this);
            _movementSystem = new MovementSystem(entitySystem);
            _enemyMovementSystem = new EnemyMovementSystem(entitySystem);
            _rendering.levelStart = true;
            _rendering.resetSystem();

            if (levelName == "Level3")
            {
                _rendering.levelWidth = 9450; //TODO das gehört hier nicht hin
            }

            _inputSystem.KeyPressed += _movementSystem.Update;
            _inputSystem.KeyReleased += _movementSystem.OnKeyReleased;
            
            _inputSystem.InitializePauseRequested += () => _rendering.InitializePauseMenu(_menuSystem);
            _inputSystem.SelectNextMenuItem += _menuSystem.SelectNextItem;
            _inputSystem.SelectPreviousMenuItem += _menuSystem.SelectPreviousItem;
            _inputSystem.ExecuteMenuItem += _menuSystem.ExecuteSelectedItem;
            
            _rescource.loadTiles();
        }
        
        public void LoadNextLevel()
        {
            _menuSystem.LoadNextLevel();
        }
        
        public void Run()
        {
            IsRunning = true;

            double timerStart = SDL.SDL_GetPerformanceCounter() / (double)SDL.SDL_GetPerformanceFrequency();
            double timerNew = timerStart;
            double timerCurrent;
            double deltaTime;

            SDL.SDL_Event e;
            while (IsRunning)
            {
                timerCurrent = SDL.SDL_GetPerformanceCounter() / (double)SDL.SDL_GetPerformanceFrequency();
                timerCurrent = timerCurrent - timerStart;

                deltaTime = (SDL.SDL_GetPerformanceCounter() - timerNew) / (double)SDL.SDL_GetPerformanceFrequency();
                timerNew = SDL.SDL_GetPerformanceCounter();

                if (IsMenuOpen)
                {
                    deltaTime = 0;
                }
                
                if (LifeSystem.IsGameOver())
                {
                    _rendering.resetSystem();
                    _rendering.deathEvent(_menuSystem);
                    _rendering.InitializeDeathMenu(_menuSystem);
                    IsMenuOpen = true; // Open the death menu
                }
                
                _movementSystem.UpdatePlayerState();

                while (SDL.SDL_PollEvent(out e) != 0)
                {
                    // Process the input events
                    _inputSystem.ProcessInput(e, IsMenuOpen);
                }

                // Update enemy movements
                _enemyMovementSystem.Update(deltaTime);

                // Retrieve all entities
                entities = entitySystem.GetAllEntities();

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