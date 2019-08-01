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

            public double GoogleBalance { get; set;}
            public double PsnBalance { get; set; }
            public double OtherBalance { get; set; }

            //public DateTime DateStart { get; set; }
            //public DateTime DateEnd { get; set; }

            async public Task GetDetails()
            {
                await User.GetSessioncounter();
                if (User.Transactions.Count == 0)
                    await User.GetTransactions(DateTime.Now);

                //DateEnd = DateTime.Parse(User.Transactions[0].date);
                //DateStart = DateTime.Parse(User.Transactions.Last().date);
            }

        }

        BalanceDetail detail1;
        BalanceDetail detail2;

        //CardDetail cardDetailFull;
        //DateTime extractDate;

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

            DateTime startDate = new DateTime(2019, 3, 1);

            FromDatePicker.DisplayDateStart = startDate;
            ToDatePicker.DisplayDateStart = startDate;
            FromDatePicker.DisplayDateEnd = ToDatePicker.DisplayDateEnd = DateTime.Today;

            //TBdateRange1.Text = string.Format("({0:dd-MM} to {1:dd-MM})", detail1.DateStart, detail1.DateEnd);
            //TBdateRange2.Text = string.Format("({0:dd-MM} to {1:dd-MM})", detail2.DateStart, detail2.DateEnd);

            //ToDatePicker.DisplayDate = DateTime.Now;
            FromDatePicker.SelectedDateChanged += SelectedDateChanged;
            ToDatePicker.SelectedDateChanged += SelectedDateChanged;

            await prepareDetails(detail1);
            await prepareDetails(detail2);

            updateTextBox();

            GridMAin.IsEnabled = true;
            GridMAin.Opacity = 1;
            TBwait.Visibility = Visibility.Collapsed;
            //BTNgetBalance.IsEnabled = true;
        }

        async private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == FromDatePicker)
            {
                if (FromDatePicker.SelectedDate.Value > ToDatePicker.SelectedDate.Value)
                {
                    ToDatePicker.SelectedDateChanged -= SelectedDateChanged;
                    ToDatePicker.SelectedDate = FromDatePicker.SelectedDate;
                    ToDatePicker.SelectedDateChanged += SelectedDateChanged;

                    if (ToDatePicker.SelectedDate.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
                    {
                        BTNupdateTodayTransactions.IsEnabled = true;
                    }
                    else
                    {
                        BTNupdateTodayTransactions.IsEnabled = false;

                    }
                }
                //string[] lines = new string[2] { MainWindow.ItelVer, FromDatePicker.SelectedDate.Value.Ticks.ToString() };
                //File.WriteAllLines(MainWindow.verPath, lines);
            }
            else
            {
                if (ToDatePicker.SelectedDate.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
                {
                    BTNupdateTodayTransactions.IsEnabled = true;
                }
                else
                {
                    BTNupdateTodayTransactions.IsEnabled = false;

                }
                if (ToDatePicker.SelectedDate.Value < FromDatePicker.SelectedDate.Value)
                {
                    FromDatePicker.SelectedDateChanged -= SelectedDateChanged;
                    FromDatePicker.SelectedDate = ToDatePicker.SelectedDate;
                    FromDatePicker.SelectedDateChanged += SelectedDateChanged;
                }
            }

            await prepareDetails(detail1);
            await prepareDetails(detail2);

            updateTextBox();
        }



        async Task prepareDetails(BalanceDetail detail, bool updateToday = false)
        {
            detail.TouchBalance = 0;
            detail.AlfaBalance = 0;
            detail.GoogleBalance = 0;
            detail.PsnBalance = 0;
            detail.OtherBalance = 0;

            //int i = 0;
            //bool begin = false;

            DateTime toDate = ToDatePicker.SelectedDate.Value;
            do
            {
                await detail.User.GetTransactions(toDate, updateToday);
                toDate = toDate.AddDays(-1);
            } while (toDate >= FromDatePicker.SelectedDate.Value);
            detail.User.SaveTransactions();

            foreach (Transaction trnx in detail.User.Transactions)
            {
                DateTime date = DateTime.Parse(trnx.date.Substring(0, 10));
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
                            else if (log.service == "GOOGLE PLAY")
                                detail.GoogleBalance += log.amount;
                            else if (log.service == "PLAY STATION NETWORK")
                                detail.PsnBalance += log.amount;
                            //else if (log.service == "TOUCH VALIDITY TRANSFER")
                            //    detail.OtherBalance += log.amount;
                            else
                            {
                                detail.OtherBalance += log.amount;
                                //if (cardDetailFull == null)
                                //{
                                //    cardDetailFull = CardDetail.Desirialize(File.ReadAllText("details.txt"));
                                //}
                                //foreach (Category category in cardDetailFull.data.categories)
                                //{
                                //    foreach (Service ser in category.services)
                                //    {
                                //        foreach (Denomination1 d1 in ser.denominations)
                                //        {
                                //            if (log.denomination == d1.name)
                                //            {
                                //                if (d1.price.StartsWith("USD"))
                                //                    detail.OtherBalance += double.Parse(d1.price.Substring(4));
                                //                else
                                //                    detail.OtherBalance += double.Parse(d1.price, System.Globalization.NumberStyles.AllowCurrencySymbol | System.Globalization.NumberStyles.AllowDecimalPoint);
                                //                break;
                                //            }
                                //        }
                                //    }
                            }
                        }
                    }
                }
                //}
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

        async private void BTNgetBalance_Click(object sender, RoutedEventArgs e)
        {
            BTNupdateTodayTransactions.IsEnabled = false;
            await prepareDetails(detail1, true);
            await prepareDetails(detail2, true);

            MainWindow win = this.Tag as MainWindow;
            await detail1.User.UpdateSessionCounter();
            await win.GetBalance();

            updateTextBox();
            BTNupdateTodayTransactions.IsEnabled = true;
        }

        private void updateTextBox()
        {
            LBLtodayDate.Content = string.Format("Today Last Updated\r\n{0}", detail1.User.Transactions[0].date);

            TBtouch1.Text = detail1.TouchBalance.ToString("0.00");
            TBalfa1.Text = detail1.AlfaBalance.ToString("0.00");
            TBGoogle1.Text = detail1.GoogleBalance.ToString("0.00");
            TBPsn1.Text = detail1.PsnBalance.ToString("0.00");
            TBother1.Text = detail1.OtherBalance.ToString("0.000");

            TBtouch2.Text = detail2.TouchBalance.ToString("0.00");
            TBalfa2.Text = detail2.AlfaBalance.ToString("0.00");
            TBgoogle2.Text = detail2.GoogleBalance.ToString("0.00");
            TBpsn2.Text = detail2.PsnBalance.ToString("0.00");
            TBother2.Text = detail2.OtherBalance.ToString("0.000");

            double total = detail1.TouchBalance + detail1.AlfaBalance + detail1.OtherBalance + detail1.GoogleBalance + detail1.PsnBalance
                + detail2.TouchBalance + detail2.AlfaBalance + detail2.GoogleBalance + detail2.PsnBalance + detail2.OtherBalance;
            TBtotlal.Text = total.ToString("0.000");
        }
    }
}
