using SalesCustomerFront.Interfaces;
using SalesCustomerFront.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SalesCustomerFront.ViewModels
{
    public class SalesOrderViewModel : ISalesOrderViewModel, INotifyPropertyChanged
    {
        private readonly ISalesOrderService _service;
        public event PropertyChangedEventHandler PropertyChanged;

        public List<SalesOrderModel> SalesOrders { get; private set; } = new();
        public string ErrorMessage { get; private set; }

        public SalesOrderViewModel(ISalesOrderService service)
        {
            _service = service;
        }

        public async Task LoadSalesOrders()
        {
            var result = await _service.GetSalesOrdersAsync();
            SalesOrders = result;
            OnPropertyChanged(nameof(SalesOrders));
        }

        public async Task<bool> MaintainOrder(SalesOrderModel order, string action)
        {
            var response = await _service.MaintainSalesOrder(order, action);
            if (!response.IsSuccess)
            {
                ErrorMessage = string.Join(", ", response.Errors);
                OnPropertyChanged(nameof(ErrorMessage));
                return false;
            }

            if (action == "INSERT" && response.Data.HasValue)
            {
                order.RecId = response.Data.Value;
            }

            await LoadSalesOrders();
            return true;
        }

        public async Task<SalesOrderModel> GetRecordById(int recId)
        {
            return await _service.GetRecordSalesOrder(recId);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
