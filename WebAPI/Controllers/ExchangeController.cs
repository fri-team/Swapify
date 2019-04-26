using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models.Exchanges;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ExchangeController : BaseController
    {        
        private readonly IBlockChangesService _blockChangesService;
        public ExchangeController(IBlockChangesService blockChangeService) : base()
        {
            _blockChangesService = blockChangeService;           
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
