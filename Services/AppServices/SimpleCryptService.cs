using Core.AppService;

namespace Services.AppServices
{
    public class SimpleCryptService:SimpleCryptServiceBase
    {
        protected override string EncryptionKey => "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static SimpleCryptService Factory()
        {
            return new SimpleCryptService();
        }
    } 
}
