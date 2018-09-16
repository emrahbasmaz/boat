using System;

namespace Boat.Business.Common
{
    public sealed class MaskCard
    {
        private string MaskDigits(string input)
        {
            //take first 6 characters
            string firstPart = input.Substring(0, 6);

            //take last 4 characters
            int len = input.Length;
            string lastPart = input.Substring(len - 4, 4);

            //take the middle part (****)
            int middlePartLenght = len - (firstPart.Length + lastPart.Length);
            string middlePart = new String('*', middlePartLenght);

            return firstPart + middlePart + lastPart;
        }

    }
}
