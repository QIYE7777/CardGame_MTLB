using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public static class CalculatePointTool
{
    static int finalPoint;

    static bool isPair;
    static int pairPoint;

    static bool isStraight;
    static int straightPoint;

    public static int FinalPoint(CardAsset[] c)
    {
        if (c.Length <= 0) return 0;
        else
        {
            Calculate(c);
            return finalPoint;
        } 
    }
    static void Calculate(CardAsset[] c)
    {
        finalPoint = 0;
        CheckPair(c);
        CheckStraight(c);

        if (isPair)
        {
            Debug.Log("isPair");
            finalPoint = pairPoint;
            return;
        }
        if (isStraight)
        {
            Debug.Log("isStraight");
            finalPoint = straightPoint;
            return;
        }
        Debug.Log("isHighCard");
        foreach(CardAsset x in c)
        {
            finalPoint += x.ATK;
        }
        Debug.Log(finalPoint);
    }

    static void CheckPair(CardAsset[] c)
    {
        isPair = false;
        int temporaryATK = c[0].ATK;
        int count = 0;
        for (int x = 0; x < c.Length; x++)
        {
            if (c[x].ATK == temporaryATK)
            {
                count++;
            }
            if (count == c.Length && c.Length>1)
            {
                isPair = true;
                switch(c.Length)
                {
                    case 2:
                        pairPoint = 3*temporaryATK;
                        break;
                    case 3:
                        pairPoint = 5*temporaryATK;
                        break;
                    case 4:
                        pairPoint = 7*temporaryATK;
                        break;
                }
            }
        }
    }
    static void CheckStraight(CardAsset[] c)
    {
        isStraight = false;

        int count = 0;

        // 按 ATK 属性进行升序排序
        Array.Sort(c, (x, y) => x.ATK.CompareTo(y.ATK));

        for (int x = 0; x < c.Length-1; x++)
        {
            if(c[x].ATK + 1 == c[x + 1].ATK )
            {
                count++;
            }
            if (count == c.Length-1 && c.Length >= 3)
            {
                isStraight = true;

                int buffPerCard = 3;
                int totalPoint = 0;
                for (int i = 0; i < c.Length; i++)
                {
                    totalPoint += c[i].ATK;
                }
                Debug.Log(totalPoint);
                switch (c.Length)
                {
                    case 3:
                        straightPoint = totalPoint +3*buffPerCard;
                        break;
                    case 4:
                        straightPoint = totalPoint + 4 * buffPerCard;
                        break;
                    case 5:
                        straightPoint = totalPoint + 5 * buffPerCard;
                        break;
                    case 6:
                        straightPoint = totalPoint + 6 * buffPerCard;
                        break;
                    case 7:
                        straightPoint = totalPoint + 7 * buffPerCard;
                        break;
                    case 8:
                        straightPoint = totalPoint + 8 * buffPerCard;
                        break;
                    case 9:
                        straightPoint = totalPoint + 9 * buffPerCard;
                        break;
                    case 10:
                        straightPoint = totalPoint + 10 * buffPerCard;
                        break;
                }
            }

        }

    }
}
