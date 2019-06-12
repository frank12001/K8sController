using K8sController.Controllers;

namespace K8sController
{
    public class ErrorMsg
    {
        public static ErrorMsg Instance => _instance ??= new ErrorMsg();
        private static ErrorMsg _instance;

        public ScaleController.Msg Scale { get; }

        private ErrorMsg()
        {
            Scale = new ScaleController.Msg();
        }
    }
}