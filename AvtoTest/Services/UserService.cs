using Autotest.Mvc.Models;
using AvtoTest.Repositories;

namespace Autotest.Mvc.Services;

public class UsersService
{

    private const string UserIdCookieKey = "user_id";
    public TicketRepository _ticketRepository;
    public  UserRepository _userRepository;
    public  QuestionService _questionService;
    
    public UsersService(TicketRepository ticketRepository, UserRepository userRepository, QuestionService questionService)
    {
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
        _questionService = questionService;
    }


    public void Register(CreateUserModel createUser, HttpContext httpContext)
    {
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = createUser.Name!,
            Password = createUser.Password!,
            Username = createUser.Username!,
            PhotoPath = SavePhoto(createUser.Photo!),
        };

        CreateUserTickets(user);

        _userRepository.AddUser(user);

        httpContext.Response.Cookies.Append(UserIdCookieKey, user.Id);
    }
    public void UpdateUser(CreateUserModel updateUser, HttpContext httpContext)
    {
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = updateUser.Name!,
            Password = updateUser.Password!,
            Username = updateUser.Username!,
            PhotoPath = SavePhoto(updateUser.Photo!),
        };

        CreateUserTickets(user);

        _userRepository.UpdateUser(user);

        httpContext.Response.Cookies.Append(UserIdCookieKey, user.Id);
    }
    public void EditUser(EditUserModel editUser, HttpContext httpContext)
    {
        var user = new User
        {
            Name = editUser.Name!,
            Password = editUser.Password!,
            Username = editUser.Username!,
            PhotoPath = SavePhoto(editUser.Photo!),
        };

        CreateUserTickets(user);

        _userRepository.UpdateUser(user);

        httpContext.Response.Cookies.Append(UserIdCookieKey, user.Id);
    }

    public bool LogIn(SignInUserModel signInUserModel, HttpContext httpContext)
    {
        var user = _userRepository.GetByUsername(signInUserModel.Username);

        if (user == null || user.Password != signInUserModel.Password)
            return false;

        httpContext.Response.Cookies.Append(UserIdCookieKey, user.Id);

        return true;
    }

    public User? GetCurrentUser(HttpContext context)
    {
        if (context.Request.Cookies.ContainsKey(UserIdCookieKey))
        {
            var userId = context.Request.Cookies[UserIdCookieKey];
            var user = _userRepository.GetById(userId);

            return user;
        }

        return null;
    }

    public bool IsLoggedIn(HttpContext context)
    {
        if (!context.Request.Cookies.ContainsKey(UserIdCookieKey)) return false;

        var userId = context.Request.Cookies[UserIdCookieKey];
        var user = _userRepository.GetById(userId);

        return user != null;
    }


    public void LogOut(HttpContext httpContext)
    {
        httpContext.Response.Cookies.Delete(UserIdCookieKey);
    }

    private void CreateUserTickets(User user)
    {
        for (var i = 0; i < _questionService.TicketsCount; i++)
        {
            var startIndex = i * _questionService.TicketQuestionsCount + 1;

            _ticketRepository.AddTicket(new Ticket()
            {
                UserId = user.Id,
                TicketId = i,
                CurrentQuestionIndex = startIndex,
                StartIndex = startIndex,
                QuestionsCount = _questionService.TicketQuestionsCount
            });
        }
    }

    private string SavePhoto(IFormFile file)
    {
        if (!Directory.Exists("wwwroot/UserImages"))
            Directory.CreateDirectory("wwwroot/UserImages");

        var fileName = Guid.NewGuid() + ".jpg";
        var ms = new MemoryStream();
        file.CopyTo(ms);
        System.IO.File.WriteAllBytes(Path.Combine("wwwroot", "UserImages", fileName), ms.ToArray());

        return "/UserImages/" + fileName;
    }
}