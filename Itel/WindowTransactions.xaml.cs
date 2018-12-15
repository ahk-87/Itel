using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Itel
{
    /// <summary>
    /// Interaction logic for WindowTransactions.xaml
    /// </summary>
    public partial class WindowTransactions : Window
    {

        class BalanceDetail
        {
            public BalanceDetail(ItelUser u)
            {
                User = u;
            }
            public ItelUser User { get; set; }
            public double TouchBalance { get; set; }
            public double AlfaBalance { get; set; }
            public double OtherBalance { get; set; }

            public DateTime DateStart { get; set; }
            public DateTime DateEnd { get; set; }

            async public Task GetDetails()
            {
                await User.GetSessioncounter();
                if (User.Transactions.Count == 0)
                    await User.GetTransactions();

                DateEnd = DateTime.Parse(User.Transactions[0].date);
                DateStart = DateTime.Parse(User.Transactions.Last().date);
            }

        }

        BalanceDetail detail1;
        BalanceDetail detail2;

        CardDetail cardDetailFull;
        DateTime extractDate;

        public WindowTransactions(ItelUser u1, ItelUser u2)
        {
            InitializeComponent();
            detail1 = new BalanceDetail(u1);
            detail2 = new BalanceDetail(u2);
        }

        async private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await detail1.GetDetails();
            await detail2.GetDetails();

            ToDatePicker.SelectedDate = DateTime.Today;
            FromDatePicker.SelectedDate = DateTime.Today;
            //if (File.Exists(MainWindow.verPath))
            //{
            //    extractDate = DateTime.FromBinary(long.Parse(File.ReadAllLines(MainWindow.verPath)[1]));
            //    FromDatePicker.SelectedDate = extractDate;
            //}
            //else

            FromDatePicker.DisplayDateStart = detail2.DateStart;
            ToDatePicker.DisplayDateStart = FromDatePicker.SelectedDate;
            FromDatePicker.DisplayDateEnd = ToDatePicker.DisplayDateEnd = DateTime.Now;

            TBdateRange1.Text = string.Format("({0:dd-MM} to {1:dd-MM})", detail1.DateStart, detail1.DateEnd);
            TBdateRange2.Text = string.Format("({0:dd-MM} to {1:dd-MM})", detail2.DateStart, detail2.DateEnd);

            //ToDatePicker.DisplayDate = DateTime.Now;
            FromDatePicker.SelectedDateChanged += SelectedDateChanged;
            ToDatePicker.SelectedDateChanged += SelectedDateChanged;

            SelectedDateChanged(null, null);

            GridMAin.IsEnabled = true;
            GridMAin.Opacity = 1;
            TBwait.Visibility = Visibility.Collapsed;
            //BTNgetBalance.IsEnabled = true;
        }

        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == FromDatePicker)
            {
                ToDatePicker.DisplayDateStart = FromDatePicker.SelectedDate;
                //string[] lines = new string[2] { MainWindow.ItelVer, FromDatePicker.SelectedDate.Value.Ticks.ToString() };
                //File.WriteAllLines(MainWindow.verPath, lines);
            }
            else
                FromDatePicker.DisplayDateEnd = ToDatePicker.SelectedDate;

            prepareDetails(detail1);
            prepareDetails(detail2);

            TBtouch1.Text = detail1.TouchBalance.ToString("0.00");
            TBalfa1.Text = detail1.AlfaBalance.ToString("0.00");
            TBother1.Text = detail1.OtherBalance.ToString("0.000");

            TBtouch2.Text = detail2.TouchBalance.ToString("0.00");
            TBalfa2.Text = detail2.AlfaBalance.ToString("0.00");
            TBother2.Text = detail2.OtherBalance.ToString("0.000");

            double total = detail1.TouchBalance + detail1.AlfaBalance + detail1.OtherBalance
               + detail2.TouchBalance + detail2.AlfaBalance + detail2.OtherBalance;
            TBtotlal.Text = total.ToString("0.000");
        }



        void prepareDetails(BalanceDetail detail)
        {
            detail.TouchBalance = 0;
            detail.AlfaBalance = 0;
            detail.OtherBalance = 0;

            //int i = 0;
            //bool begin = false;


            foreach (Transaction trnx in detail.User.Transactions)
            {
                DateTime date = DateTime.Parse(trnx.date);
                if (date <= ToDatePicker.SelectedDate && date >= FromDatePicker.SelectedDate)
                {
                    foreach (MyLogInvoiceResponse invoice in trnx.myLogInvoiceResponses)
                    {
                        foreach (LogDetail log in invoice.logDetails)
                        {
                            if (log.service == "TOUCH")
                                detail.TouchBalance += log.amount;
                            else if (log.service == "ALFA")
                                detail.AlfaBalance += log.amount;
                            else
                            {
                                if (cardDetailFull == null)
                                {
                                    cardDetailFull = CardDetail.Desirialize(File.ReadAllText("details.txt"));
                                }
                                foreach (Category category in cardDetailFull.data.categories)
                                {
                                    foreach (Service ser in category.services)
                                    {
                                        foreach (Denomination1 d1 in ser.denominations)
                                        {
                                            if (log.description == d1.name)
                                            {
                                                if (d1.price.StartsWith("USD"))
                                                    detail.OtherBalance += double.Parse(d1.price.Substring(4));
                                                else
                                                    detail.OtherBalance += double.Parse(d1.price, System.Globalization.NumberStyles.AllowCurrencySymbol | System.Globalization.NumberStyles.AllowDecimalPoint);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //do
            //{
            //    if (begin || transactions[i].date != toDateString)
            //    {
            //        begin = true;
            //        foreach (MyLogInvoiceResponse invoice in transactions[i].myLogInvoiceResponses)
            //        {
            //            foreach (LogDetail log in invoice.logDetails)
            //            {
            //                if (log.service == "TOUCH")
            //                    detail.TouchBalance += log.amount;
            //                else if (log.service == "ALFA")
            //                    detail.AlfaBalance += log.amount;
            //                else
            //                    detail.OtherBalance += log.amount;
            //            }
            //        }
            //    }

            //    i++;
            //} while (transactions[i].date != fromDateString);

        }

        private void BTNgetBalance_Click(object sender, RoutedEventArgs e)
        {
            prepareDetails(detail1);
            prepareDetails(detail2);

            TBtouch1.Text = detail1.TouchBalance.ToString("0.00");
            TBalfa1.Text = detail1.AlfaBalance.ToString("0.00");
            TBother1.Text = detail1.OtherBalance.ToString("0.000");

            TBtouch2.Text = detail2.TouchBalance.ToString("0.00");
            TBalfa2.Text = detail2.AlfaBalance.ToString("0.00");
            TBother2.Text = detail2.OtherBalance.ToString("0.000");

        }
    }
}
