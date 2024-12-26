using AlmazAucceon_3_.Model2;
using System.Configuration;
using System.Data;
using System.Windows;

namespace AlmazAucceon_3_
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static AuctionContext context { get; } = new AuctionContext();
    }

}
