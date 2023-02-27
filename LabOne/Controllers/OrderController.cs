using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LabOne.Models;

namespace LabOne.Controllers;

public class OrderController : Controller
{
    private readonly ILogger<OrderController> _logger;

    public OrderController(ILogger<OrderController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new OrderModel());
    }

    [HttpPost]
    public IActionResult Index(OrderModel model)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogInformation("User Order contains {ErrorCount} errors.", ModelState.ErrorCount);
            return View(model);
        }

        model.IsReadOnly = true;
        return View(model);
    }

    [HttpPost]
    public IActionResult ConfirmOrder()
    {
        return View(model: Guid.NewGuid().ToString());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}