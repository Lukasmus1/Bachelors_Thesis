using FourthWall.Commons;
using FourthWall.ExternalWeb.Models;

namespace FourthWall.ExternalWeb.Controllers
{
    public class ExternalWebController
    {
        private readonly ExternalWebModel externalWebModel = new();
        
        /// <summary>
        /// Creates the URL for a post ending website based on the user's real name. The format of the URL is "https://s2vybmvsx1bhbmlj.netlify.app/?m=*text*", where the *text* is as follows:
        /// First 4 characters are the length of the user's real name, padded with zeros if necessary (e.g., "0005" for a 5-letter name).
        /// The next characters are the user's real name.
        /// The last 10 characters is the first 10 characters of SHA256 hash of the user's real name.
        /// The entire text is encrypted using a Vigenère cipher with the key "plushie".
        /// </summary>
        /// <returns>URL for the PlayerHelpsAI escaping ending</returns>
        public string CreateEndingUrl()
        {
            string realName = FourthWallMvc.Instance.UserInformationController.GetUserRealName();
            
            return externalWebModel.CreateEndingUrl(realName);
        }
    }
}