using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

            public Builder SetWaitingMessage(string message)
            {
                _panzerLoadingDialog.WaitingMessage = message;
                return this;
            }

            public Builder SetFinishLoadingMessage(string message)
            {
                _panzerLoadingDialog.FinishLoadingMessage = message;
                return this;
            }

            public Builder SetSimulatedWaitTime(int timeMilis)
            {
                _panzerLoadingDialog.SimulatedWaitTime = timeMilis;
                return this;
            }

            public Builder SetConfirmButtonText(string text)
            {
                _panzerLoadingDialog.ConfirmButtonText = text;
                return this;
            }

            public Builder SetOnLoadingListener(Action listener)
            {
                _panzerLoadingDialog.OnLoadingListener = listener;
                return this;
            }

            public Builder SetOnFinishButtonClickListener(Action<object,EventArgs> listener)
            {
                _panzerLoadingDialog.OnFinishButtonClickListener = listener;
                return this;
            }

            public PanzerLoadingDialogControl Create()
            {
                _panzerLoadingDialog.Initailize();
                return _panzerLoadingDialog;
            }
        }

        private int _dotCount = 1;
        private int _dotCountEditInterval = 200;

        public int SimulatedWaitTime { get; set; } = 0;
        public string WaitingMessage { get; set; } = "Loading";
        public string FinishLoadingMessage { get; set; } = "Loading Finished!";
        public string ConfirmButtonText { get; set; } = "OK";
        public Action OnLoadingListener { get; set; } = null;
        private bool _onLoadingTaskCompleted = false;
        public Action<object, EventArgs> OnFinishButtonClickListener { get; set; } = null;

        private PanzerLoadingDialogControl() {}

        private void Initailize()
        {
            InitializeComponent();
            Message.Text = WaitingMessage;
            FinishButton.Content = ConfirmButtonText;
            if(OnFinishButtonClickListener != null)
            {
                FinishButton.Click += new RoutedEventHandler(OnFinishButtonClickListener);
            }
        }

        public void StartLoading()
        {
            Task.Run(RunWaitingMessageAnimation);
            if(OnLoadingListener != null)
            {
                Task.Run(ExecuteLoadingTask);
            }
        }

        private async void RunWaitingMessageAnimation()
        {
            long timeElapsed = 0;

            //simulated wait

            while (timeElapsed < SimulatedWaitTime || !_onLoadingTaskCompleted)
            {
                string newWaitingMessage = WaitingMessage;
                var i = 0;
                while(i < _dotCount)
                {
                    newWaitingMessage += ".";
                    i++;
                }
                _dotCount = _dotCount % 3 + 1;
                Application.Current.Dispatcher.Invoke(new Action(() => { Message.Text = newWaitingMessage; }));
                timeElapsed += _dotCountEditInterval;
                await Task.Delay(_dotCountEditInterval);
            }

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Message.Text = FinishLoadingMessage;
                FinishButton.Visibility = Visibility.Visible;
            }));
        }

        private void ExecuteLoadingTask()
        {
            OnLoadingListener?.Invoke();
            _onLoadingTaskCompleted = true;
        }

        public PanzerLoadingDialogControl AttachToPanel(Panel panel)
        {
            panel.Children.Add(this);
            return this;
        }
    }
}
