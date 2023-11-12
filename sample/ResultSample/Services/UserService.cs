using Francisvac.Result;

namespace ResultSample.Services;
public record User(int Id, string Name, uint Age);

public class UserService
{
    private readonly List<User> _users;

    public UserService()
    {
        _users = new()
        {
            new User(1, "User1", 21),
            new User(2, "User2", 22),
            new User(3, "User3", 23),
            new User(4, "User4", 24),
            new User(5, "User5", 25),
        };
    }

    public Result<IEnumerable<User>> GetUsers()
        => _users;

    public Result AnyUser(int userId)
    {
        if (userId < 0) return Result.Error("Invalid user Id");
        Result result = Result.Error("");
        if (_users.Any(u => u.Id == userId)) return Result.NotFound($"Not found any user with the id {userId}");
        return Result.Success("User exist");
    }

    public Result<User> GetUser(int userId)
    {
        if (userId < 0) return Result.Error("Invalid user Id");
        User? user = _users.Find(u => u.Id == userId);
        if (user is null) return Result.NotFound($"Not found any user with the id {userId}");
        return user;
    }

    public Result AddUser(User? user)
    {
        if (user is null || user.Id <= 0) return Result.Error("Invalid user");
        if (_users.Any(u => u.Name == user.Name) || _users.Any(u => u.Id == user.Id)) return Result.NotFound("User already exist");
        if (user.Name.Length < 4) return Result.Error("Invalid username");
        if (user.Age < 18) return Result.Error("Could not add a minor");
        _users.Add(user);
        return Result.Success("User added successfully");
    }

    public Result DeleteUser(int userId)
    {
        User? userToDelete = _users.Find(u => u.Id == userId);
        if (userToDelete is null) return Result.Error("User not found");
        _users.Remove(userToDelete);
        return Result.Success("User delete successfully");
    }
}
