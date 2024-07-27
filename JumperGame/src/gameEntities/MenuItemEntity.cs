using JumperGame.components;

namespace JumperGame.gameEntities;

public class MenuItemEntity
{
    public MenuComponent MenuComponent { get; set; }
    public MenuPositionComponent PositionComponent { get; set; }

    public MenuItemEntity(MenuComponent menuComponent, MenuPositionComponent positionComponent)
    {
        MenuComponent = menuComponent;
        PositionComponent = positionComponent;
    }
}