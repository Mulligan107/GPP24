using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledCSPlus;
using JumperGame.gameEntities;
using JumperGame.components;
using System.Numerics;
using JumperGame.src.components;


namespace JumperGame.src.manager
{
    class RescourceManager
    {
        LTexture tileTexEnvi = new LTexture();
        LTexture tileTexCoin = new LTexture();
        LTexture tileTexKnight = new LTexture();
        LTexture tileTexSlime = new LTexture();
        LTexture tileTexMonster = new LTexture();

        LTexture addTexture = new LTexture();

        bool collision;
        int mass;

        public static IntPtr Font = IntPtr.Zero;

        TiledMap map;
        Dictionary<int, TiledTileset> tilesets;


        public RescourceManager(String worldselect)
        {
            map = new TiledMap("src\\worlds\\" + worldselect + ".tmx");
            tilesets = map.GetTiledTilesets("src/tilesets/"); // DO NOT forget the / at the 
            tileTexEnvi.loadFromFile("src\\tilesets/world_tileset.png");
            tileTexCoin.loadFromFile("src\\tilesets/coin.png");
            tileTexKnight.loadFromFile("src\\tilesets/knightOpti.png");
            tileTexSlime.loadFromFile("src\\tilesets/slime_green.png");
            tileTexMonster.loadFromFile("src\\tilesets/monsters.png");

        }

        public void loadTiles()
        {
            var tileLayers = map.Layers.Where(x => x.Type == TiledLayerType.TileLayer);

            foreach (var layer in tileLayers)
            {
                for (var y = 0; y < layer.Height; y++)
                {
                    for (var x = 0; x < layer.Width; x++)
                    {
                        var index = (y * layer.Width) + x; // Assuming the default render order is used which is from right to bottom
                        var gid = layer.Data[index]; // The tileset tile index
                        var tileX = (x * map.TileWidth);
                        var tileY = (y * map.TileHeight);

                        // Gid 0 is used to tell there is no tile set
                        if (gid == 0)
                        {
                            continue;
                        }

                        // Helper method to fetch the right TieldMapTileset instance. 
                        // This is a connection object Tiled uses for linking the correct tileset to the gid value using the firstgid property.
                        var mapTileset = map.GetTiledMapTileset(gid);

                        // Retrieve the actual tileset based on the firstgid property of the connection object we retrieved just now
                        var tileset = tilesets[mapTileset.FirstGid];

                        // Use the connection object as well as the tileset to figure out the source rectangle.
                        var rect = map.GetSourceRect(mapTileset, tileset, gid);

                        SDL.SDL_Rect destRect = new SDL.SDL_Rect { x = tileX, y = tileY, h = tileset.TileWidth, w = tileset.TileWidth };

                        SDL.SDL_Rect srcRect = new SDL.SDL_Rect { x = rect.X, y = rect.Y, h = rect.Height, w = rect.Width };


                        //Console.WriteLine(tileset.Name + ": X: " + rect.X + " Y: " + rect.Y+ " W: " + rect.Width + " H : " + rect.Height);
                        Entity entity = new Entity(gid);
                        collision = false;
                        mass = -1;

                        switch (tileset.Name)
                        {
                            case "Enviroment":
                                entity.Type = Entity.EntityType.Tile;
                                addTexture = tileTexEnvi;


                                if (Enumerable.Range(1, 44).Contains(entity.gid)) // if (entity.gid == 1 || entity.gid == 3 || entity.gid == 4 || entity.gid == 7 || entity.gid == 8 || entity.gid == 42|| entity.gid == 43|| entity.gid == 44) 
                                {
                                    collision = true;
                                }
                                
                                break;

                            case "coin": 
                                var coinCollisionComponent = new CollisionComponent(new Vector2(destRect.w, destRect.h));
                                entity.Type = Entity.EntityType.Coin;
                                addTexture = tileTexCoin;
                                collision = true;
                                mass = 0;

                                foreach (TiledTile till in tileset.Tiles)
                                {
                                    //Console.WriteLine("TILE ID " + till.Id);
                                    // Console.WriteLine(till.Animations.ToString());
                                    // Console.WriteLine("CoinCounter: " + coinCounter);
                                    //  Console.WriteLine("Anzahl: " + till.Animations.Length);
                                    var annie = till.Animations;
                                    entity.AddComponent(new AnimationComponent(annie, srcRect));
                                }

                                entity.AddComponent(coinCollisionComponent);

                                break;

                            case "knightOpti":

                               // destRect = changeRectSize(ref destRect, 2);
                                entity.Type = Entity.EntityType.Player;
                                addTexture = tileTexKnight;
                                collision = true;
                                mass = 40;
                                // Create components
                                var physicsComponent = new PhysicsComponent(40);
                                var playerSteeringComponent = new PlayerSteeringComponent(physicsComponent);
                                //var animationComponent = new AnimationComponent(tileset.)
                                

                                // Add components to the entity
                                entity.AddComponent(playerSteeringComponent);
                                
                                break;
                            case "slime_green":
                                entity.Type = Entity.EntityType.Enemy;
                                addTexture = tileTexSlime;
                                collision = true;
                                mass = 10;

                                var sSteeringComponent = new SlimeSteeringComponent();
                                
                                
                                entity.AddComponent(sSteeringComponent);
                                
                                break;
                            case "monsters":
                                entity.Type = Entity.EntityType.Enemy;
                                addTexture = tileTexMonster;
                                collision = true;
                                mass = 10;

                                var sSteeringComponent2 = new SlimeSteeringComponent();


                                entity.AddComponent(sSteeringComponent2);

                                break;
                        }


                        entity.AddComponent(new PositionComponent(new Vector3(destRect.x, destRect.y, layer.Id)));
                        entity.AddComponent(new RenderComponent(addTexture, srcRect, destRect));

                        if (collision)
                        {
                            entity.AddComponent(new CollisionComponent(new Vector2(destRect.w, destRect.h)));
                        }
                        if (mass > -1)
                        {
                            entity.AddComponent(new PhysicsComponent(mass));
                        }

                        JumperGame.entitySystem.AddEntity(entity);
                    }
                }

            }
        }

    }
}
