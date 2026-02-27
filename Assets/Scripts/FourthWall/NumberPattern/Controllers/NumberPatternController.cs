using FourthWall.NumberPattern.Models;

namespace FourthWall.NumberPattern.Controllers
{
    public class NumberPatternController
    {
        private readonly NumberControllerModel _numberControllerModel = new();
        
        /// <summary>
        /// Creates a 5 digit random number pattern, with a leading 0 and no zeros in the rest of the pattern.
        /// </summary>
        /// <returns>Random 5 digit number starting with a 0 and no other 0s</returns>
        public string CreateRandomNumberPattern()
        {
            return _numberControllerModel.CreateRandomNumberPattern();
        }
        
        /// <summary>
        /// Creates a random number noise string of the specified length.
        /// The noise is generated using a seeded random based on the user's number pattern code, ensuring that the same noise is generated for the entire playthrough.
        /// </summary>
        /// <param name="len">Length of the desired noise</param>
        /// <param name="includeZeros">Should the noise include 0s?</param>
        /// <returns>Random number noise string</returns>
        public string CreateRandomNumberNoise(int len, bool includeZeros)
        {
            return _numberControllerModel.CreateRandomNumberNoise(len, includeZeros);
        }
    }
}