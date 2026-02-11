using Apps.VigenereCipher.Controllers;

namespace Apps.VigenereCipher.Commons
{
    public class CipherMvc
    {
        private static CipherMvc _instance;
        public static CipherMvc Instance
        {
            get
            {
                _instance ??= new CipherMvc();
                return _instance;
            }
        }

        public CipherController CipherController { get; set; }
        
        private CipherMvc()
        {
            CipherController = new CipherController();
        }
    }
}