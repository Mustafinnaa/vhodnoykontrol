using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace vhodnoykontrol
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<CommissionResult> Results { get; set; } = new ObservableCollection<CommissionResult>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeComboBox();
            ResultsListView.ItemsSource = Results;
        }

        private void InitializeComboBox()
        {
            FIOComboBox.ItemsSource = Data.Info.Select(i => i.FIO).ToList();
        }

        private void FIOComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFIO = FIOComboBox.SelectedItem as string;
            if (selectedFIO != null)
            {
                var info = Data.Info.FirstOrDefault(i => i.FIO == selectedFIO);
                if (info != null)
                {
                    DateTextBox.Text = info.Date;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string selectedFIO = FIOComboBox.SelectedItem as string;
            if (selectedFIO != null)
            {
                var info = Data.Info.FirstOrDefault(i => i.FIO == selectedFIO);
                if (info != null)
                {
                    if (string.IsNullOrWhiteSpace(Vyruchka.Text) || string.IsNullOrWhiteSpace(DateTextBox.Text))
                    {
                        MessageBox.Show("заполните все поля!");
                        return;
                    }

                    decimal vyruchka;
                    try
                    {
                        vyruchka = Convert.ToDecimal(Vyruchka.Text);
                    }
                    catch
                    {
                        MessageBox.Show("введите корректную сумму");
                        return;
                    }

                    DateTime Date;
                    try
                    {
                        Date = Convert.ToDateTime(DateTextBox.Text);
                    }
                    catch
                    {
                        MessageBox.Show("введите корректную дату (дд.мм.гггг.)");
                        return;
                    }

                    if (info.Date != DateTextBox.Text)
                    {
                        MessageBox.Show("дата приема на работу не может быть изменена");
                        return;
                    }

                    int yearsOfService = (DateTime.Now - Date).Days / 365;
                    decimal commission = CalculateCommission(vyruchka, yearsOfService);

                    var result = Results.FirstOrDefault(r => r.FIO == info.FIO);
                    if (result != null)
                    {
                        result.Commission = commission;
                        result.Vyruchka = vyruchka;
                        result.Years = yearsOfService;
                    }
                    else
                    {
                        Results.Add(new CommissionResult
                        {
                            FIO = info.FIO,
                            Commission = commission,
                            Vyruchka = vyruchka,
                            Years = yearsOfService
                        });
                    }
                    FIOComboBox.SelectedIndex = -1;
                    DateTextBox.Text = string.Empty;
                    Vyruchka.Text = string.Empty;
                }
            }
        }

        private decimal CalculateCommission(decimal vyruchka, int yearsOfService)
        {
            decimal commissionRate = vyruchka < 50000 ? 0.03m : 0.06m;
            if (yearsOfService > 3)
            {
                commissionRate += 0.05m;
            }
            return vyruchka * commissionRate;
        }
    }

    public class CommissionResult
    {
        public string FIO { get; set; }
        public decimal Commission { get; set; }
        public decimal Vyruchka { get; set; }
        public int Years { get; set; }
    }
}