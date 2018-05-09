using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for WindowProfit.xaml
    /// </summary>
    public partial class WindowProfit : Window
    {
        int cardTouchProfit = 8900;
        int cardAlfaProfit = 6900;

        double profit = 0.0;

        MatchCollection matchesDollars, matchesTotal, matchesDate;
        int count, index = 0;

        private void TBdate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (index == 0) return;
            index--;
            getProfit();
        }

        private void TBdate_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((index + 1) == count) return;
            index++;
            getProfit();
        }

        public WindowProfit()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string path = @"D:\Dropbox\Grandstream new\Progs\JaroorDolarat.txt";
            string regDollars = @"dollars	\: (\d{2,4}(?:\.\d{1,2})?)\(MTC\) \+ (\d{2,4}(?:\.\d{1,2})?)\(Alfa\)";
            string regTotal = @"total   \= (\d{2,5}(?:\.\d{1,2})?)\$(?: \+ \((\d{2,4})\$)?";
            string regDate = @"\d{2}\-\d{2}-\d{4}";
            string data = File.ReadAllText(path);
            matchesDollars = Regex.Matches(data, regDollars);
            matchesTotal = Regex.Matches(data, regTotal);
            matchesDate = Regex.Matches(data, regDate);

            count = matchesDollars.Count - 1;
            getProfit();
        }

        private void getProfit()
        {
            double touchDollar1, touchDollar2, alfaDollar1, alfaDollar2, total1, total2;
            string date;
            int cash = 0;

            touchDollar1 = double.Parse(matchesDollars[count - index - 1].Groups[1].Value);
            alfaDollar1 = double.Parse(matchesDollars[count - index - 1].Groups[2].Value);
            touchDollar2 = double.Parse(matchesDollars[count - index].Groups[1].Value);
            alfaDollar2 = double.Parse(matchesDollars[count - index].Groups[2].Value);

            total1 = double.Parse(matchesTotal[count - index - 1].Groups[1].Value);
            total2 = double.Parse(matchesTotal[count - index].Groups[1].Value);

            int.TryParse(matchesTotal[count - index].Groups[2].Value, out cash);

            date = matchesDate[count - index].Value;
            DateTime date1, date2;
            date2 = DateTime.Parse(date);
            date1 = DateTime.Parse(matchesDate[count - index - 1].Value);
            var dayCount = date2 - date1;
            int days = dayCount.Days;

            profit = 0;
            profit -= (touchDollar2 - touchDollar1) / 20 * cardTouchProfit;
            profit -= (alfaDollar2 - alfaDollar1) / 20 * cardAlfaProfit;
            profit += (total2 - total1) * 1500;
            profit += cash * 1500;

            TBdaysCount.Text = "(" + days.ToString() + " day" + (days > 1 ? "s)" : ")");
            TBdate.Text = date;
            TBprofit.Text = profit.ToString("0,0.00");
        }
    }
}
