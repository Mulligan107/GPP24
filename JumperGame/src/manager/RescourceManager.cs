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


namespace JumperGame.src.manager
{
    class RescourceManager
    {
        LTexture tileTexEnvi = new LTexture();
        LTexture tileTexCoin = new LTexture();
        LTexture tileTexKnight = new LTexture();
        LTexture tileTexSlime = new LTexture();

        public static IntPtr Font = IntPtr.Zero;

        TiledMap map = new TiledMap("src\\worlds\\testWorld.tmx");
        Dictionary<int, TiledTileset> tilesets;


        public RescourceManager()
        {
            tilesets = map.GetTiledTilesets("src/tilesets/"); // DO NOT forget the / at the 
            tileTexEnvi.loadFromFile("src\\tilesets/world_tileset.png");
            tileTexCoin.loadFromFile("src\\tilesets/coin.png");
            tileTexKnight.loadFromFile("src\\tilesets/knight.png");
            tileTexSlime.loadFromFile("src\\tilesets/slime_green.png");
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

                        SDL.SDL_Rect destRect = new SDL.SDL_Rect { x = tileX, y = tileY, h = map.TileWidth, w = map.TileWidth };

                        SDL.SDL_Rect srcRect = new SDL.SDL_Rect { x = rect.X, y = rect.Y, h = rect.Height, w = rect.Width };


                        //Console.WriteLine(tileset.Name + ": X: " + rect.X + " Y: " + rect.Y+ " W: " + rect.Width + " H : " + rect.Height);
                        Entity entity = new Entity(gid);

                        switch (tileset.Name)
                        {
                            case "Enviroment":
                                var envPositionComponent = new PositionComponent(new Vector3(destRect.x, destRect.y, layer.Id));
                                var envRenderComponent = (new RenderComponent(tileTexEnvi, srcRect, destRect));
                                var envCollisionComponent = new CollisionComponent(new Vector2(destRect.w, destRect.h));
                                
                                entity.AddComponent(envPositionComponent); 
                                entity.AddComponent(envRenderComponent);
                                if (entity.gid == 1) { entity.AddComponent(envCollisionComponent); }
                                
                                JumperGame._entitySystem.AddEntity(entity);
                                
                                break;
                            case "coin":
                                var coinCollisionComponent = new CollisionComponent(new Vector2(destRect.w, destRect.h));
                                var coinPhysicsComponent = new PhysicsComponent(1);
                                var coinPositionComponent = new PositionComponent(new Vector3(destRect.x, destRect.y, 0));
                                var coinRenderComponent = new RenderComponent(tileTexCoin, srcRect, destRect);
                                
                                entity.AddComponent(coinPhysicsComponent);
                                entity.AddComponent(coinPositionComponent); 
                                entity.AddComponent(coinRenderComponent);
                                entity.AddComponent(coinCollisionComponent);
                                
                                break;
                            case "knight":
                                destRect = changeRectSize(ref destRect, 2);
                                entity.Type = "knight";

                                // Create components
                                var knightCollisionComponent = new CollisionComponent(new Vector2(destRect.w, destRect.h));
                                var physicsComponent = new PhysicsComponent(1);
                                var playerSteeringComponent = new PlayerSteeringComponent(physicsComponent);
                                var positionComponent = new PositionComponent(new Vector3(destRect.x, destRect.y, 0));
                                var renderComponent = new RenderComponent(tileTexKnight, srcRect, destRect);

                                // Add components to the entity
                                entity.AddComponent(physicsComponent);
                                entity.AddComponent(playerSteeringComponent);
                                entity.AddComponent(positionComponent);
                                entity.AddComponent(renderComponent);
                                entity.AddComponent(knightCollisionComponent);
                                
                                break;
                            case "slime_green":
                                var sPhysicsComponent = new PhysicsComponent(10);
                                var sPositionComponent = new PositionComponent(new Vector3(destRect.x, destRect.y, layer.Id));
                                var sRenderComponent = new RenderComponent(tileTexSlime, srcRect, destRect);
                                var sCollisionComponent = new CollisionComponent(new Vector2(destRect.w, destRect.h));
                                
                                entity.AddComponent(sPhysicsComponent);
                                entity.AddComponent(sPositionComponent);
                                entity.AddComponent(sRenderComponent);
                                entity.AddComponent(sCollisionComponent);
                                
                                break;
                        }
                        JumperGame._entitySystem.AddEntity(entity);

                        //SDL.SDL_RenderCopy(gRenderer, tileTexCoin.getTexture(), ref srcRect, ref destRect);

                        // Render sprite at position tileX, tileY using the rect
                    }
                }

            }
        }
        static SDL.SDL_Rect changeRectSize(ref SDL.SDL_Rect rect, int num)
        {
            SDL.SDL_Rect shrunkRect = new SDL.SDL_Rect { x = rect.x - rect.w / 2, y = rect.y - rect.h, w = rect.w * num, h = rect.h * num };
            return shrunkRect;
        }
    }
}
