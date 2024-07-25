using JumperGame.components;

namespace JumperGame.gameEntities;

public class MenuItemEntity
{
    public MenuComponent MenuComponent { get; set; }
    public MenuPositionComponent PositionComponent { get; set; }
    public RenderComponent RenderComponent { get; set; }

    public MenuItemEntity(MenuComponent menuComponent, MenuPositionComponent positionComponent, RenderComponent renderComponent)
    {
        MenuComponent = menuComponent;
        PositionComponent = positionComponent;
        RenderComponent = renderComponent;
    }
}