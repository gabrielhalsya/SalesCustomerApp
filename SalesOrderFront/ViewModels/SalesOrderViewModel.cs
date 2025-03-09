using SalesOrderFront.Interfaces;
using SalesOrderFront.Models;
using SalesOrderFront.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SalesOrderFront.ViewModels
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

        private SalesOrderFilter CurrentFilter { get; set; } = new();

        public async Task LoadSalesOrders(SalesOrderFilter filter)
        {
            CurrentFilter = filter; // ✅ Store current filter for later use
            SalesOrders = await _service.GetSalesOrdersAsync(filter);
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

            // ✅ Reload sales orders using existing search filters
            await LoadSalesOrders(CurrentFilter ?? new SalesOrderFilter());

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
