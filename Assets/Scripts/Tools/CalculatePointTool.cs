using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class CalculatePointTool
{
    static int finalPoint;

    static bool isPair;
    static int pairPoint;
   
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
        CheckPair(c);


        if (isPair)
            finalPoint = pairPoint;
        

        
    }

    static void CheckPair(CardAsset[] c)
    {
        int temporaryATK = c[0].ATK;
        int count = 0;
        for (int x = 0; x < c.Length; x++)
        {
            if (c[x].ATK == temporaryATK)
            {
                count++;
            }
            if (count == c.Length)
            {
                isPair = true;
                switch(c.Length)
                {
                    case 1:
                        pairPoint = temporaryATK;
                        break;
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
                return;
            }
        }
    }
}
