using System;
using System.Linq;
using JumperGame.components;
using JumperGame.src.components;
using JumperGame.src.components.testComponents;
using JumperGame.src.manager;
using JumperGame.systems;
using SDL2;

namespace JumperGame
{
    public class JumperGame
    {
        public static entitySystem _entitySystem;
        private RenderManager _rendering;
        private AudioManager _audio;
        private RescourceManager _rescource;

        public PhysicsSystem _physicsSystem;
        public InputSystem _inputSystem;
        public ColorSystem _colorSystem;
        public QuitSystem _quitSystem;
        public MovementSystem _movementSystem;
        
        public ColorComponent _colorComponent;
        public PlayerSteeringComponent _playerSteeringComponent;
        
        public bool IsRunning;

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
            _colorComponent = new ColorComponent();

            _entitySystem = new entitySystem();
            
            _rendering = new RenderManager(_colorComponent);
            _physicsSystem = new PhysicsSystem();
            _audio = new AudioManager();
            _rescource = new RescourceManager();

            _inputSystem = new InputSystem();
            _colorSystem = new ColorSystem(_colorComponent);
            _quitSystem = new QuitSystem(this);
            _movementSystem = new MovementSystem(_entitySystem);

            _inputSystem.KeyPressed += _movementSystem.Update;
            
            _inputSystem.KeyPressed += _colorSystem.ChangeColor;
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


                    while (SDL.SDL_PollEvent(out e) != 0)
                    {
                        // Process the input events
                        _inputSystem.ProcessInput(e);
                    }
                    
                    // Retrieve all entities
                    var entities = _entitySystem.GetAllEntities();

                    // Update the managers
                    _rendering.Update(deltaTime, timerCurrent);
                    _physicsSystem.Update(entities, deltaTime);
                    
                    _entitySystem.Update(deltaTime);
                    
                    // _audio.Update();
                    // _input.Update();
                }
        }
    }
}