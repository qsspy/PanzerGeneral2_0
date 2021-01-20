using System;
using System.Windows;
using System.Windows.Controls;

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

            public Builder SetOnPositiveClickButtonListener(string text, Action<object,EventArgs> listener)
            {
                positiveButton = new Button();
                positiveButton.Content = text;
                positiveButton.Padding = new Thickness(10, 0, 10, 0);
                positiveButton.Margin = new Thickness(20, 0, 20, 0);
                positiveButton.Click += new RoutedEventHandler(listener);
                return this;
            }

            public Builder SetOnNegativeClickButtonListener(string text, Action<object, EventArgs> listener)
            {
                negativeButton = new Button();
                negativeButton.Content = text;
                negativeButton.Padding = new Thickness(10, 0, 10, 0);
                negativeButton.Margin = new Thickness(20, 0, 20, 0);
                negativeButton.Click += new RoutedEventHandler(listener);
                return this;
            }

            public PanzerAlertDialog Create()
            {
                return new PanzerAlertDialog(this);
            }

            public interface IOnPositiveButtonClick {

                void Execute(object sender, EventArgs e);
            }

            public interface IOnNegativeButtonClick
            {
                void Execute(object sender, EventArgs e);
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

        public void AttachToPanel(Panel panel)
        {
            panel.Children.Add(this);
        }
    }
}
