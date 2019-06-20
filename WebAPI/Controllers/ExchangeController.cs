using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FRITeam.Swapify.Entities.Notifications;
using WebAPI.Models.Exchanges;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ExchangeController : BaseController
    {        
        private readonly IBlockChangesService _blockChangesService;
        private readonly INotificationService _notificationService;

        public ExchangeController(IBlockChangesService blockChangeService, INotificationService notificationService) : base()
        {
            _blockChangesService = blockChangeService;
            _notificationService = notificationService;
        }

        [HttpPost]
        [Route("exchangeConfirm")]
        public async Task<IActionResult> ExchangeConfirm([FromBody]ExchangeRequestModel request)
        {
            var currentUserBlockChangeRequest = new BlockChangeRequest();
            currentUserBlockChangeRequest.BlockFrom = BlockForExchangeModel.ConvertToBlock(request.BlockFrom);
            currentUserBlockChangeRequest.BlockTo = BlockForExchangeModel.ConvertToBlock(request.BlockTo);
            currentUserBlockChangeRequest.Status = ExchangeStatus.WaitingForExchange;
            currentUserBlockChangeRequest.DateOfCreation = DateTime.Now;
            currentUserBlockChangeRequest.StudentId = Guid.Parse(request.StudentId);

            var (exchangeWasMade, otherUserBlockChangeRequest) = await _blockChangesService.AddAndFindMatch(currentUserBlockChangeRequest);

            await _notificationService.AddNotification(
                SuccessfulExchangeNotification.Create(currentUserBlockChangeRequest, otherUserBlockChangeRequest));

            await _notificationService.AddNotification(
                SuccessfulExchangeNotification.Create(otherUserBlockChangeRequest, currentUserBlockChangeRequest));

            return Ok(exchangeWasMade);
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
