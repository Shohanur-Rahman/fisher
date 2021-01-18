using System;
using Core.Exceptions;
using Web.Models;

namespace Web.AppCode
{
    public static class ExceptionMapper
    {
        public static ExceptionLogModel MapException(Exception ex)
        {
            if (ex is CoreException coreException) return LogModel(coreException);

            return ExceptionLogModel.Factory(ex);
        }

        private static ExceptionLogModel LogModel(CoreException coreException)
        {
            return new ExceptionLogModel(coreException)
            {
                ViewMessage =
                    string.Format("{0}",
                        coreException.Message),
                TroubleshootHint =
                    "Business logic error occured. No troubleshoot might be required."
            };

        }

    }

}