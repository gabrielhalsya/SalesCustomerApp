using Microsoft.AspNetCore.Mvc;
using SalesCustomerAPI.Models;
using SalesCustomerAPI.Repositories;

namespace SalesCustomerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesOrderController : ControllerBase
    {
        private readonly ISalesOrderRepo _salesOrderRepository;
        private readonly ILogger<SalesOrderController> _logger;

        public SalesOrderController(ISalesOrderRepo salesOrderRepository, ILogger<SalesOrderController> logger)
        {
            _salesOrderRepository = salesOrderRepository;
            _logger = logger;
        }

        [HttpPost("GetRecord")]
        public async Task<IActionResult> GetRecordSalesOrder([FromBody] int recId)
        {
            try
            {
                var result = await _salesOrderRepository.GetRecordSalesOrder(recId);
                return Ok(new { Data = result, Errors = new List<string>() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving sales order");
                return BadRequest(new { Data = (object)null, Errors = new List<string> { ex.Message } });
            }
        }

        [HttpPost("Maintain")]
        public async Task<IActionResult> MaintainSalesOrder([FromBody] SalesOrderMaintain request)
        {
            try
            {
                var (recId, errorMessages) = await _salesOrderRepository.MaintainSalesOrder(request);
                return Ok(new { Data = new { RecId = recId }, Errors = errorMessages ?? new List<string>() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error maintaining sales order");
                return BadRequest(new { Data = (object)null, Errors = new List<string> { ex.Message } });
            }
        }

        [HttpPost("GetList")]
        public async Task<IActionResult> GetListSalesOrder([FromBody] SalesOrderBase request) // ✅ Use API Model
        {
            try
            {
                var result = await _salesOrderRepository.GetListSalesOrder(request); // ✅ Response is `SalesOrderMaintain`

                return Ok(new { Data = result, Errors = new List<string>() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving sales order list");
                return BadRequest(new { Data = (object)null, Errors = new List<string> { ex.Message } });
            }
        }

    }
}
