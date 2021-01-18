namespace Web.Models
{
    public class ExceptionViewModel
    {
        private string _viewMessage;

        public string ViewMessage
        {
            get
            {
                if (string.IsNullOrEmpty(_viewMessage)) _viewMessage = "There is problem processing your request.";
                return _viewMessage;
            }
            set {
                _viewMessage = value;
            }

        }
    }
}