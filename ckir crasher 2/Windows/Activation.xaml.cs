using ckir_crasher_2.Classes;
using System.Windows;
using System.Windows.Input;

namespace ckir_crasher_2.Windows
{
    /// <summary>
    /// Interaction logic for Activation.xaml
    /// </summary>
    public partial class Activation : Window
    {
        private string username = null;
        private string password = null;

        public Activation()
        {
            InitializeComponent();
        }

        public Activation(string u_id, string u_pw)
        {
            InitializeComponent();
            username = u_id;
            password = u_pw;
        }


        //controls
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void closeapp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //activation
        private void button_Activate_Click(object sender, RoutedEventArgs e)
        {
            if (HandyControl.Controls.MessageBox.Show("다음 계정에 해당 키를 귀속합니다.\nID : " + username + "\nPW : " + password, null, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                if (SignWithPHP.check_isKeyAvailable(textbox_serial.Text))
                {
                    SignWithPHP.add_key_used(textbox_serial.Text);
                    SignWithPHP.add_approve(username + password);
                    HandyControl.Controls.MessageBox.Show("성공적으로 등록했습니다.");
                    this.Close();
                }
                else
                {
                    if (SignWithPHP.error_type_key == 1)
                    {
                        HandyControl.Controls.MessageBox.Show("올바르지 않은 키 입니다.", null, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (SignWithPHP.error_type_key == 2)
                    {
                        HandyControl.Controls.MessageBox.Show("이미 등록된 키 입니다.", null, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {

                    }
                }
            }
        }

    }
}
