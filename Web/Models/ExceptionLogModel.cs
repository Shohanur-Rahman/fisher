using System;
using System.Web;

namespace Web.Models
{
    public class ExceptionLogModel : ExceptionViewModel
    {
        private readonly Exception _exception;
        readonly HttpContext _currentHttpContext = null;
        public ExceptionLogModel( Exception ex)
        {
            _exception = ex;
            LogTimeUtc = DateTime.UtcNow;
        }

        public static ExceptionLogModel Factory( Exception ex)
        {
            return new ExceptionLogModel(ex);
        }

        public string ExceptionMessage => _exception.Message;

        public string StackTrace => _exception.StackTrace;


        public DateTime LogTimeUtc { get; private set; }

        public string TroubleshootHint { get; set; }

        public Uri WikiLink { get; set; }

        public string Cause { get; set; }

        public string Description { get; set; }

        public Uri RequestUrl
        {
            get
            {
                //if (_currentHttpContext != null)
                //    return _currentHttpContext.Request.Url;
                return null;
            }
        }
        public string FilePath
        {
            get
            {
                //if (_currentHttpContext!=null)
                //    return _currentHttpContext.Request.FilePath;
                return null;
            }
        }

        public override string ToString()
        {
            return _exception.ToString();
        }
    }
}