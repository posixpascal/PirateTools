using Microsoft.AspNetCore.Mvc;
using PirateTools.Harbor.Services;
using PirateTools.Models.AskTheChairs;
using System;
using System.Collections.Generic;

namespace PirateTools.Harbor.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AskYourChairsController : ControllerBase {
    private readonly SmtpService _smtpService;
    private readonly DBService _dbService;

    public AskYourChairsController(SmtpService smtpService, DBService dbService) {
        _smtpService = smtpService;
        _dbService = dbService;
    }

    [HttpPost]
    public IActionResult RequestToken([FromBody] string email) {
        var token = _dbService.GenerateToken();
        _smtpService.SendMail(email, "Ask your BuVo Token",
            $"Hier ist dein Ask your BuVo Token:\n{token}");

        return Ok();
    }

    [HttpPost]
    public IActionResult CheckToken([FromBody] string token) => Ok(_dbService.CheckToken(token));

    [HttpPost]
    public IActionResult AskQuestion([FromBody] AskQuestionRequest questionRequest) {
        if (!ValidateQuestion(questionRequest.Question))
            return BadRequest();

        if (_dbService.CheckToken(questionRequest.Token) <= 0)
            return Forbid();

        _dbService.AddQuestion(questionRequest.Token, questionRequest.Question);
        return Ok();
    }

    [HttpGet]
    public IActionResult GetAllQuestions() => Ok(_dbService.GetQuestions());

    //TODO: Move to general validator
    private bool ValidateQuestion(Question question) {
        if (question.Title.Length > 255)
            return false;
        if (question.Content.Length > 4096)
            return false;
        if (question.EMail?.Length > 1024)
            return false;

        return true;
    }
}