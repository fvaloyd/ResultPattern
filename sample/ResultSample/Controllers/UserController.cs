using Francisvac.Result;
using Microsoft.AspNetCore.Mvc;
using ResultSample.Services;

namespace ResultSample.Controllers;

[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _service;

    public UserController(UserService service) => _service = service;

    [HttpGet("{userId}")]
    public IActionResult GetUser(int userId) => _service.GetUser(userId).ToActionResult();

    [HttpGet]
    public ActionResult<IEnumerable<User>> GetUsers() => _service.GetUsers().ToActionResult();

    [HttpDelete("{userId}")]
    public ActionResult DeleteUser(int userId) => _service.DeleteUser(userId).ToActionResult();

    [HttpPost]
    public ActionResult AddUser(User user) => _service.AddUser(user).ToActionResult();
}
