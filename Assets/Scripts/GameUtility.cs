
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine.UI;

namespace Yun
{
    public static class GameUtility
    {

        // 리스트를 랜덤하게 섞는 함수
        //Fisher-Yates Shuffle
        public static void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int randomIndex = UnityEngine.Random.Range(0, i + 1);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }
        // enum MyEnum { First = 0, Second = 1, Third = 2 } key = Second일경우 1반환 
        public static int StringToEnumInt(string key,Type enumType)
        {

            var _enumValue = Enum.Parse(enumType, key);

            return Convert.ToInt32(_enumValue);
        }

     
     

    }

}


