using System.Collections.Generic;

namespace QuanLyNhaHang.ViewModels
{
    public class ThongKeViewModel
    {
        public ThongKeTongHop TongHop { get; set; }
        public List<ChartDataPoint> Bieudo { get; set; }
        public OrderListResult DanhSachDonHang { get; set; }

        public ThongKeViewModel()
        {
            TongHop = new ThongKeTongHop();
            Bieudo = new List<ChartDataPoint>();
            DanhSachDonHang = new OrderListResult();
        }
    }

    public class ThongKeTongHop
    {
        public int TongSoTiec { get; set; }
        public int SoTiecDaHuy { get; set; }
        public string DoanhThuDuKien { get; set; }
        public string ThucThu { get; set; }
        public string ConLai { get; set; }
        public int SoDonCoc { get; set; }
        public int SoDonHoanTat { get; set; }
    }

    public class ChartDataPoint
    {
        public string Label { get; set; }
        public int TotalCount { get; set; }
        public int CancelCount { get; set; }
        public double Percentage { get; set; }
    }

    public class OrderListResult
    {
        public List<OrderItem> Items { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public OrderListResult() { Items = new List<OrderItem>(); }
    }

    public class OrderItem
    {
        public string MaDatTiec { get; set; }
        public string NgayToChuc { get; set; }
        public string KhachHang { get; set; }
        public string TongTien { get; set; }
        public string TrangThai { get; set; }
        public string TinhTrangThanhToan { get; set; }
        public bool IsCancelled { get; set; }
    }
}