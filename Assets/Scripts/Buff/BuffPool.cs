
using System.Collections.Generic;


public class BuffPool 
{
    public Dictionary<string,IBuff> buffEffectDic = new();

    public BuffPool()
    {
        buffEffectDic.Add("CommonSpdUp",new SpeedBuff());
        buffEffectDic.Add("CommonAtkUp",new AttackBuff());

    }

    public Dictionary<string,IBuff> GetBuffEffDic()
    {
        return buffEffectDic;
    }
   
   
}
