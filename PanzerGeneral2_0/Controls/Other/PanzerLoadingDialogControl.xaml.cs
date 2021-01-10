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
    /// Interaction logic for PanzerLoadingDialogControl.xaml
    /// </summary>
    public partial class PanzerLoadingDialogControl : UserControl
    {

        public class Builder
        {
            private PanzerLoadingDialogControl _panzerLoadingDialog;

            public Builder()
            {
                _panzerLoadingDialog = new PanzerLoadingDialogControl();
            }

            public Builder setWaitingMessage(string message)
            {
                _panzerLoadingDialog.WaitingMessage = message;
                return this;
            }

            public Builder setFinishLoadingMessage(string message)
            {
                _panzerLoadingDialog.FinishLoadingMessage = message;
                return this;
            }

            public Builder setSimulatedWaitTime(int timeMilis)
            {
                _panzerLoadingDialog.SimulatedWaitTime = timeMilis;
                return this;
            }

            public Builder setConfirmButtonText(string text)
            {
                _panzerLoadingDialog.ConfirmButtonText = text;
                return this;
            }

            public Builder setOnLoadingListener(Action listener)
            {
                _panzerLoadingDialog.OnLoadingListener = listener;
                return this;
            }

            public Builder setOnFinishButtonClickListener(Action<object,EventArgs> listener)
            {
                _panzerLoadingDialog.OnFinishButtonClickListener = listener;
                return this;
            }

            public PanzerLoadingDialogControl create()
            {
                _panzerLoadingDialog.initailize();
                return _panzerLoadingDialog;
            }
        }


        private int _dotCount = 1;
        private int _dotCountEditInterval = 500;
        public int SimulatedWaitTime { get; set; } = 0;
        public string WaitingMessage { get; set; } = "Loading";
        public string FinishLoadingMessage { get; set; } = "Loading Finished!";
        public string ConfirmButtonText { get; set; } = "OK";
        public Action OnLoadingListener { get; set; } = null;
        public Action<object, EventArgs> OnFinishButtonClickListener { get; set; } = null;


        private PanzerLoadingDialogControl() {}

        private void initailize()
        {
            InitializeComponent();
            Message.Text = WaitingMessage;
            FinishButton.Content = ConfirmButtonText;
            if(OnFinishButtonClickListener != null)
            {
                FinishButton.Click += new RoutedEventHandler(OnFinishButtonClickListener);
            }
        }



        public void startLoading()
        {
            Task.Run(executeTask);
        }

        private async void executeTask()
        {
            long timeElapsed = 0;

            //simulated wait

            while(timeElapsed < SimulatedWaitTime)
            {
                string newWaitingMessage = WaitingMessage;
                foreach(var i in Enumerable.Range(0, _dotCount))
                {
                    newWaitingMessage += ".";
                }
                _dotCount = _dotCount % 3 + 1;
                Application.Current.Dispatcher.Invoke(new Action(() => { Message.Text = newWaitingMessage; }));
                timeElapsed += _dotCountEditInterval;
                await Task.Delay(_dotCountEditInterval);
            }

            //target async operation

            OnLoadingListener?.Invoke();

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Message.Text = FinishLoadingMessage;
                FinishButton.Visibility = Visibility.Visible;
            }));

        }

        public PanzerLoadingDialogControl attachToPanel(Panel panel)
        {
            panel.Children.Add(this);
            return this;
        }
    }
}
