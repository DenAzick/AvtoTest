using Autotest.Mvc.Models;
using Autotest.Mvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace Autotest.Mvc.Controllers;

public class UsersController : Controller
{
    public readonly UsersService _usersService;
    public QuestionService _questionService;

    public UsersController(UsersService usersService, QuestionService questionService)
    {
        _usersService = usersService;
        _questionService = questionService;
    }

    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SignUp(CreateUserModel createUser)
    {
        if (!ModelState.IsValid)
        {
            return View(createUser);
        }

        _usersService.Register(createUser, HttpContext);

        return RedirectToAction("Index", "Home");
    }

    //[HttpGet]
    //public IActionResult Edit()
    //{
    //    return View();
    //}

    //[HttpPost]
    //public IActionResult Edit(EditUserModel editUser)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return View(editUser);
    //    }

    //    UsersService.UpdateUser(editUser, HttpContext);


    //    return RedirectToAction("Profile");
    //}



    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SignIn(SignInUserModel signInUserModel)
    {
        if (!ModelState.IsValid)
        {
            return View(signInUserModel);
        }

        var isLogin = _usersService.LogIn(signInUserModel, HttpContext);

        if (!isLogin)
        {
            ModelState.AddModelError("Username", "Username or Password is incorrect");
            return View();
        }

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Profile()
    {
        var user = _usersService.GetCurrentUser(HttpContext);

        if (user == null)
        {
            return RedirectToAction("SignUp");
        }

        return View(user);
    }

    public IActionResult LogOut()
    {
        _usersService.LogOut(HttpContext);

        return RedirectToAction("SignIn");
    }

    public IActionResult ChangeLanguage(string language)
    {
        _questionService.LoadJson(language);

        return RedirectToAction("Index", "Home");
    }
}