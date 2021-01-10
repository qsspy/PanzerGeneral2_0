using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PanzerGeneral2_0.Controls.Other
{
    /// <summary>
    /// Interaction logic for PanzerAlertDialog.xaml
    /// </summary>
    public partial class PanzerAlertDialog : UserControl
    {

        public class Builder
        {
            public string Message { get; set; }

            public Button positiveButton;
            public Button negativeButton;

            public Builder SetMessage(string message)
            {
                Message = message;
                return this;
            }

            public Builder setOnPositiveClickButtonListener(string text, Action<object,EventArgs> listener)
            {
                positiveButton = new Button();
                positiveButton.Content = text;
                positiveButton.Padding = new Thickness(10, 0, 10, 0);
                positiveButton.Margin = new Thickness(20, 0, 20, 0);
                positiveButton.Click += new RoutedEventHandler(listener);
                return this;
            }

            public Builder setOnNegativeClickButtonListener(string text, Action<object, EventArgs> listener)
            {
                negativeButton = new Button();
                negativeButton.Content = text;
                negativeButton.Padding = new Thickness(10, 0, 10, 0);
                negativeButton.Margin = new Thickness(20, 0, 20, 0);
                negativeButton.Click += new RoutedEventHandler(listener);
                return this;
            }

            public PanzerAlertDialog create()
            {
                return new PanzerAlertDialog(this);
            }

            public interface OnPositiveButtonClick {

                void execute(object sender, EventArgs e);
            }

            public interface OnNegativeButtonClick
            {
                void execute(object sender, EventArgs e);
            }
        }

        private PanzerAlertDialog(Builder builder)
        {
            DataContext = builder;
            InitializeComponent();

            if(builder.positiveButton != null)
            {
                ButtonPanel.Children.Add(builder.positiveButton);
            }
            if (builder.negativeButton != null)
            {
               ButtonPanel.Children.Add(builder.negativeButton);
            }
        }

        public void attachToPanel(Panel panel)
        {
            panel.Children.Add(this);
        }
    }
}
