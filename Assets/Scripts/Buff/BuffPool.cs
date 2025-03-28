
using System.Collections.Generic;


public class BuffPool 
{
    public Dictionary<string,IBuff> buffEffectDic = new();

    public BuffPool(PlayerStats playerStats)
    {
        buffEffectDic.Add("CommonSpdUp",new SpeedBuff(playerStats.playerMove));
        buffEffectDic.Add("CommonAtkUp",new AttackBuff(playerStats));

    }

    public Dictionary<string,IBuff> GetBuffEffDic()
    {
        return buffEffectDic;
    }
   
   
}
