using Core.Exceptions;

namespace Services.Domain.Models.User.Exceptions
{
    public class InvalidUserSignupException: CoreException
        
    {
        public InvalidUserSignupException(string userName) : base(string.Format("Incorrect log in details provided, please check email and password."), "APP-LOGIN-ERROR-001")
        {
            
        }
       
    }
    public class UserAlreadyLoggedInException : CoreException

    {
        public UserAlreadyLoggedInException(string userName) : base(string.Format("Currently {0}.", userName +" is logged In"), "APP-LOGIN-ERROR-001")
        {

        }
    }

    public class AnotherUserLoggedInException : CoreException

    {
        public AnotherUserLoggedInException(string userName) : base(string.Format("Previous user logged In.Please SignOut Previous User"), "APP-LOGIN-ERROR-001")
        {

        }
    }


}