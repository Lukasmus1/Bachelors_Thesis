using Apps.VigenereCipher.Controllers;

namespace Apps.VigenereCipher.Commons
{
    public class VigenereMvc
    {
        private static VigenereMvc _instance;
        public static VigenereMvc Instance
        {
            get
            {
                _instance ??= new VigenereMvc();
                return _instance;
            }
        }

        public VigenereController VigenereController { get; set; }
        
        private VigenereMvc()
        {
            VigenereController = new VigenereController();
        }
    }
}