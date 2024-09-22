using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/admin/transaction")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransaction _Transaction;
        public TransactionController(ITransaction Transaction)
        {
            _Transaction = Transaction;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTransaction()
        {
            var data = await _Transaction.GetAllTransaction();
            return Ok(new ApiResponse() { Status = data.Any(), Message = data.Any() ? "TransactionList Generated Sucessfully" : "Not Generated Try Again !", Data = data });
        }

        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetTransactionById(int transactionId)
        {
            var data = await _Transaction.GetTransactionById(transactionId);
            return Ok(new ApiResponse() { Status = data != null, Message = data != null ? "Transaction fetched by Id Sucessfully" : "Not Fetched by Id Try Again !", Data = data });
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody]TransactionModel model)
        {
            var data = await _Transaction.InsertUpdateTransaction(model);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Created Transaction" : "Not Created Try Again", Data = data });
        }

        [HttpPut("{transactionId}")]
        public async Task<IActionResult> EditTransaction(int transactionId,[FromBody] TransactionModel model)
        {
            var data = await _Transaction.InsertUpdateTransaction(model);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Updated Transaction" : "Not Updated Try Again", Data = data });
        }

        [HttpDelete("{transactionId}")]
        public async Task<IActionResult> DeleteTransaction(int transactionId)
        {
            var data = await _Transaction.DeleteTransaction(transactionId);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Deleted Transaction" : "Not Deleted Try Again", Data = data });
        }
    }
}
