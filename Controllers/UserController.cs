using Microsoft.AspNetCore.Mvc;
using Mocassini.Models;

namespace Mocassini.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly MocassiniDbContext _dbContext;

    public UserController(ILogger<UserController> logger, MocassiniDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet(Name = "GetUsers")]
    public IEnumerable<UserEntity> Get()
    {
        return _dbContext.Users;
    }
    
    [HttpGet("{id}", Name = "GetUser")]
    public UserEntity Get(int id)
    {
        return _dbContext.Users.Find(id);
    }

    public class CreateUserRequest
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    [HttpPost(Name = "CreateUser")]
    public UserEntity Create([FromBody] CreateUserRequest req)
    {
        var user = _dbContext.Users.Add(
            new UserEntity
            {
                Name = req.Name,
                Surname = req.Surname,
                Username = req.Username,
                PasswordHash = req.Password,
            }
        );

        _dbContext.SaveChanges();
        return user.Entity;
    }

    [HttpDelete("{id}", Name = "DeleteUser")]
    public void Delete(int id)
    {
        var user = _dbContext.Users.Find(id);
        _dbContext.Users.Remove(user);
        _dbContext.SaveChanges();
    }
}