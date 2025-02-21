﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPath.Application.DTO;
using PowerPath.Application.Interfaces.Services;

namespace PowerPath.WebApplication.Controllers
{
    [Authorize]
    public class LogController(ILogApplicationService logAppService) : Controller
    {
        private readonly ILogApplicationService _logAppService = logAppService;

        public IActionResult Index()
        {
            DateTime now = DateTime.Now;
            Resposta<List<LogDTO>> rLogs = _logAppService.ListarPorData(now.Year, now.Month, now.Day);

            if (rLogs.IsSucesso)
            {
                return View(rLogs.Resultado);
            }
            else
            {
                TempData["MensagemErro"] = rLogs.Mensagem;
                return View(new List<LogDTO>());
            }
        }

        [HttpGet]
        public IActionResult Listar(int ano, int mes, int dia)
        {
            return Json(_logAppService.ListarPorData(ano, mes, dia));
        }
    }
}
