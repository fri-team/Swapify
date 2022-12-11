using FRITeam.Swapify.Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebAPI.Models.Exchanges;
using FRITeam.Swapify.SwapifyBase.Settings;
using FRITeam.Swapify.SwapifyBase.Entities;
using FRITeam.Swapify.SwapifyBase.Entities.Enums;

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
        private readonly ITimetableDataService _timetableDataService;
        private readonly Uri _baseUrl;

        public ExchangeController(ILogger<UserController> logger, IBlockChangesService blockChangeService, IEmailService emailService,
            IUserService userService, ITimetableDataService timetableDataService, IOptions<EnvironmentSettings> environmentSettings) : base()
        {
            _logger = logger;
            _blockChangesService = blockChangeService;
            _emailService = emailService;
            _userService = userService;
            _timetableDataService = timetableDataService;
            _baseUrl = new Uri(environmentSettings.Value.BaseUrl);
        }

        [HttpPost]
        [Route("exchangeConfirm")]
        public async Task<IActionResult> ExchangeConfirm([FromBody]ExchangeRequestModel request)
        {
            try
            {
                var blockChangeRequest = new BlockChangeRequest();
                blockChangeRequest.BlockFrom = BlockForExchangeModel.ConvertToBlock(request.BlockFrom);
                blockChangeRequest.BlockTo = BlockForExchangeModel.ConvertToBlock(request.BlockTo);
                blockChangeRequest.Status = ExchangeStatus.WaitingForExchange;
                blockChangeRequest.DateOfCreation = DateTime.Now;
                blockChangeRequest.StudentId = Guid.Parse(request.StudentId);

                var res = await _blockChangesService.AddAndFindMatch(blockChangeRequest);
                if (res != (null, null))
                {
                    var timetableData1 = await _timetableDataService.FindByIdAsync(res.Item1.StudentId);
                    timetableData1.Timetable.RemoveBlock(res.Item1.BlockFrom.BlockId);
                    timetableData1.Timetable.AddNewBlock(res.Item1.BlockTo.Clone());
                    await _timetableDataService.UpdateTimetableDataAsync(timetableData1);
                    var user1 = await _userService.GetUserByIdAsync(timetableData1.UserId.ToString());
                    if (user1 == null)
                    {
                        string message = $"Cannot find user with ID {res.Item1.StudentId}.";
                        _logger.LogError(message);
                        return NotFound(message);
                    }

                    var timetableData2 = await _timetableDataService.FindByIdAsync(res.Item2.StudentId);
                    timetableData2.Timetable.RemoveBlock(res.Item2.BlockFrom.BlockId);
                    timetableData2.Timetable.AddNewBlock(res.Item2.BlockTo.Clone());
                    await _timetableDataService.UpdateTimetableDataAsync(timetableData2);
                    var user2 = await _userService.GetUserByIdAsync(timetableData2.UserId.ToString());
                    if (user2 == null)
                    {
                        string message = $"Cannot find user with ID {res.Item2.StudentId}.";
                        _logger.LogError(message);
                        return NotFound(message);
                    }

                    string callbackUrl1 = new Uri(_baseUrl, $@"getUserTimetable/{user1.Email}").ToString();
                    string callbackUrl2 = new Uri(_baseUrl, $@"getUserTimetable/{user2.Email}").ToString();

                    if (!_emailService.SendConfirmationEmail(user1.Email, callbackUrl1, "ConfirmExchangeEmail"))
                    {
                        string message = $"Error when sending confirmation email to user {user1.Email}.";
                        _logger.LogError(message);
                    }

                    if (!_emailService.SendConfirmationEmail(user2.Email, callbackUrl2, "ConfirmExchangeEmail"))
                    {
                        string message = $"Error when sending confirmation email to user {user2.Email}.";
                        _logger.LogError(message);
                    }
                    return Ok(res);
                }
                return Ok(null);
            }
            catch
            {
                string message = $"Error when creating block change request";
                _logger.LogError(message);
                return ErrorResponse(message);
            }
        }

        [AllowAnonymous]
        [HttpPost("userWaitingExchanges")]
        public async Task<IActionResult> GetUserWaitingExchanges([FromBody] string studentId)
        {
            bool isValidGUID = Guid.TryParse(studentId, out Guid guid);
            if (!isValidGUID)
            {
                return ErrorResponse($"StudentTimetable id: {studentId} is not valid GUID.");
            }

            var response = await _blockChangesService.FindWaitingStudentRequests(guid);
            return Ok(response);
        }

        [HttpPost("cancelExchangeRequest")]
        public async Task<IActionResult> CancelExchangeRequest([FromBody] BlockChangeRequest request)
        {
            var response = await _blockChangesService.CancelExchangeRequest(request);
            if (response)
            {
                return Ok(response);
            }
            return ErrorResponse($"Cannot cancel request from student {request.StudentId} because it was changed");
        }
    }
}
