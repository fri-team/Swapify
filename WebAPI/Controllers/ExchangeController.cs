using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FRITeam.Swapify.Backend.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebAPI.Models.Exchanges;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ExchangeController : BaseController
    {
        private readonly IBlockChangesService _blockChangesService;
        private readonly ILogger<UserController> _logger;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly Uri _baseUrl;

        public ExchangeController(ILogger<UserController> logger, IBlockChangesService blockChangeService, IEmailService emailService,
            IUserService userService, IOptions<EnvironmentSettings> environmentSettings) : base()
        {
            _logger = logger;
            _blockChangesService = blockChangeService;
            _emailService = emailService;
            _userService = userService;
            _baseUrl = new Uri(environmentSettings.Value.BaseUrl);
        }

        [HttpPost]
        [Route("exchangeConfirm")]
        public async Task<IActionResult> ExchangeConfirm([FromBody]ExchangeRequestModel request)
        {
            var blockChangeRequest = new BlockChangeRequest();
            blockChangeRequest.BlockFrom = BlockForExchangeModel.ConvertToBlock(request.BlockFrom);
            blockChangeRequest.BlockTo = BlockForExchangeModel.ConvertToBlock(request.BlockTo);
            blockChangeRequest.Status = ExchangeStatus.WaitingForExchange;
            blockChangeRequest.DateOfCreation = DateTime.Now;
            blockChangeRequest.StudentId = Guid.Parse(request.StudentId);

            var res = await _blockChangesService.AddAndFindMatch(blockChangeRequest);
            if (res != null)
            {
                var user1 = await _userService.GetUserByIdAsync(res.FirstID);
                if (user1 == null)
                {
                    _logger.LogError($"Cannot find user with ID {res.FirstID}.");
                    return BadRequest();
                }

                var user2 = await _userService.GetUserByIdAsync(res.SecondID);
                if (user2 == null)
                {
                    _logger.LogError($"Cannot find user with ID {res.SecondID}.");
                    return BadRequest();
                }

                string callbackUrl1 = new Uri(_baseUrl, $@"getStudentTimetable/{user1.Email}").ToString();
                string callbackUrl2 = new Uri(_baseUrl, $@"getStudentTimetable/{user2.Email}").ToString();

                if (!_emailService.SendConfirmationEmail(user1.Email, callbackUrl1, "ExchangeEmail"))
                {
                    _logger.LogError($"Error when sending confirmation email to user {user1.Email}.");
                    return BadRequest();
                }

                if (!_emailService.SendConfirmationEmail(user2.Email, callbackUrl2, "ExchangeEmail"))
                {
                    _logger.LogError($"Error when sending confirmation email to user {user2.Email}.");
                    return BadRequest();
                }
            }
            return Ok(res);
        }
        
        [HttpPost("userWaitingExchanges")]
        public async Task<IActionResult> GetUserWaitingExchanges([FromBody] string studentId)
        {
            bool isValidGUID = Guid.TryParse(studentId, out Guid guid);
            if (!isValidGUID)
            {
                return ErrorResponse($"Student id: {studentId} is not valid GUID.");
            }

            var response = await _blockChangesService.FindWaitingStudentRequests(guid);
            return Ok(response);
        }
    }
}
