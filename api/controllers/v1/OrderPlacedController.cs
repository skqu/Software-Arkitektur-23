using Microsoft.AspNetCore.Mvc;
using Service2;
using Service1;

[ApiController]
[Route(Routes.Placed1)]
public class OrderPlacedController : ControllerBase
{
    private OrderPlaced _order;
    private OrderShipment _ship;

    public OrderPlacedController()
    {
        _order = new OrderPlaced();
        _ship = new OrderShipment();
    }

    [HttpGet]
    public string PutOrder()
    {
        string invoice = _order.PlaceOrder();
        return _ship.Currier(invoice);
    }
}