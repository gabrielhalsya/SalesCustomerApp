using System.Data;
using System.Reflection;
using System.Resources;
using Microsoft.Data.SqlClient;
using SalesCustomerAPI.Models;
using SalesCustomerAPI.Repositories;


namespace SalesCustomerAPI.Class
{
    public class SalesOrderCls : ISalesOrderRepo
    {
        private readonly string _connectionString;
        private readonly ILogger<SalesOrderCls> _logger;

        public SalesOrderCls(IConfiguration configuration, ILogger<SalesOrderCls> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        private List<string> GetErrorMessages(List<string> errorCodes)
        {
            var errorMessages = new List<string>();
            try
            {
                Assembly localisationAssembly = Assembly.Load("SalesCustomerResources");
                ResourceManager resourceManager = new("SalesCustomerResources.SalesOrderResources", localisationAssembly);

                foreach (var code in errorCodes)
                {
                    var message = resourceManager.GetString(code);
                    errorMessages.Add(message);
                }
            }
            catch (MissingManifestResourceException ex)
            {
                _logger.LogError(ex, "Resource file not found or missing");
                errorMessages.Add("Error loading resource file.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching error messages");
                errorMessages.Add("Unexpected error occurred while retrieving error messages.");
            }
            return errorMessages;
        }

        public async Task<(IEnumerable<SalesOrderMaintain> Orders, List<string> ErrorCodes)> GetListSalesOrder(SalesOrderBase request)
        {
            var orders = new List<SalesOrderMaintain>();
            var errorCodes = new List<string>();

            try
            {
                await using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();
                await using var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_SO_ORDER_CRUD";
                cmd.Parameters.AddWithValue("@P_ACTION", "VIEW");
                cmd.Parameters.AddWithValue("@P_ORDER_DATE", request.OrderDate ?? (object)DBNull.Value);

                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("Err_Code")))
                        errorCodes.Add(reader["Err_Code"].ToString());
                    else
                    {
                        orders.Add(new SalesOrderMaintain
                        {
                            RecId = reader.GetInt32(reader.GetOrdinal("SO_ORDER_ID")),
                            OrderNo = reader.GetString(reader.GetOrdinal("ORDER_NO")),
                            OrderDate = reader.GetString(reader.GetOrdinal("ORDER_DATE")),
                            CustomerName = reader.IsDBNull(reader.GetOrdinal("CUSTOMER_NAME"))
                                ? ""
                                : reader.GetString(reader.GetOrdinal("CUSTOMER_NAME"))
                        });
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error in GetListSalesOrder");
                errorCodes.Add("3001");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetListSalesOrder");
                errorCodes.Add("5000");
            }

            return (orders, errorCodes);
        }


        public async Task<SalesOrderBase> GetRecordSalesOrder(int recId)
        {
            var errorCodes = new List<string>();
            try
            {
                await using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();
                await using var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_SO_ORDER_CRUD";
                cmd.Parameters.AddWithValue("@P_ACTION", "VIEW");
                cmd.Parameters.AddWithValue("@P_SO_ORDER_ID", recId);

                await using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new SalesOrderBase
                    {
                        Search = reader["ORDER_NO"].ToString(),
                        OrderDate = reader["ORDER_DATE"].ToString()
                    };
                }
                errorCodes.Add("2003");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving sales order");
                errorCodes.Add("5000");
            }
            throw new Exception(string.Join(", ", GetErrorMessages(errorCodes)));
        }

        public async Task<(int? RecId, List<string> ErrorMessages)> MaintainSalesOrder(SalesOrderMaintain request)
        {
            var errorCodes = new List<string>();
            int? recIdResult = null;

            try
            {
                await using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();
                await using var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_SO_ORDER_CRUD";
                cmd.Parameters.AddWithValue("@P_ACTION", request.StringAction.ToUpper());
                cmd.Parameters.AddWithValue("@P_SO_ORDER_ID", request.RecId ?? null);
                cmd.Parameters.AddWithValue("@P_ORDER_DATE", request.OrderDate ?? "");
                cmd.Parameters.AddWithValue("@P_COM_CUSTOMER_ID", request.CustomerId);

                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("Err_Code")))
                        errorCodes.Add(reader["Err_Code"].ToString());
                    if (!reader.IsDBNull(reader.GetOrdinal("SO_ORDER_ID")))
                        recIdResult = reader.GetInt32(reader.GetOrdinal("SO_ORDER_ID"));
                }
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error in MaintainSalesOrder");
                errorCodes.Add("3001");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in MaintainSalesOrder");
                errorCodes.Add("5000");
            }

            return (recIdResult, GetErrorMessages(errorCodes));
        }
    }

}
