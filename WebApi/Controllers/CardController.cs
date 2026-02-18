using Application.Interfaces;
using Application.Services;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/card")]
    [CorrelationLoggingFilter]
    [Authorize(Roles = "Administrator")]
    public class CardController(
        ICardRepository cardRepository,
        IActionService actionService,
        IPermissionService permissionService,
        ILogger<CardController> logger) : ControllerBase
    {
        [HttpGet("{userId}/{cardNumber}/actions")]
        public async Task<IActionResult> GetActions(string userId, string cardNumber)
        {
            try
            {
                var isNotAllowed =
                    await permissionService.IsUserNotAllowed(userId, HttpContext, CancellationToken.None);
                if (isNotAllowed)
                {
                    return Unauthorized(new { message = "User is not allowed" });
                }

                var card = await cardRepository.GetCardAsync(userId, cardNumber);
                var actions = actionService.GetAllowedActions(card);

                return Ok(actions);
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception {ex.Message} occured during the `userId/cardNumber/actions` execution");
                return NotFound(new { error = ex.Message });
            }
        }
    }
}