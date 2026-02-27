using System.Text;
using FourthWall.NumberPattern.Controllers;
using UnityEngine;
using User.Commons;
using User.Models;

namespace FourthWall.NumberPattern.Models
{
    public class NumberControllerModel
    {
        /// <see cref="NumberPatternController.CreateRandomNumberPattern"/>
        public string CreateRandomNumberPattern()
        {
            var pattern = "0";
            
            for(var i = 0; i < 4; i++)
            {
                pattern += Random.Range(1, 10).ToString();
            }

            Debug.Log(pattern);
            return pattern;
        }

        /// <see cref="NumberPatternController.CreateRandomNumberNoise"/>
        public string CreateRandomNumberNoise(int len, bool includeZeros)
        {
            StringBuilder noise = new();

            //We want the noise to be the same for the entire playthrough, so we use a seeded random based on the user's number pattern code.
            string randomSeed = UserMvc.Instance.UserController.ProceduralData(UserDataType.NumberPatternCode);
            System.Random random = new(randomSeed.GetHashCode());

            for (var i = 0; i < len; i++)
            {
                noise.Append(includeZeros ? random.Next(0, 10) : random.Next(1, 10));
            }

            return noise.ToString();
        }
    }
}