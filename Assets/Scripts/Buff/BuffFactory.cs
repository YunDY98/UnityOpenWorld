
using System.Collections.Generic;


public class BuffFactory 
{
    public Dictionary<string,IBuff> buffEffectDic = new();

    public BuffFactory()
    {
        buffEffectDic.Add("CommonSpdUp",new SpeedBuff());
        buffEffectDic.Add("CommonAtkUp",new AttackBuff());

    }
   
   
}
