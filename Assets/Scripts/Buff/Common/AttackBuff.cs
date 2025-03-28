
using System.Diagnostics;
using UnityEditor;
using UnityEngine.InputSystem.Interactions;

public class AttackBuff : IBuff
{
    
    PlayerStats playerStats;

    public AttackBuff(PlayerStats playerStats)
    {
        this.playerStats = playerStats;
    }

   
    public float Amount { get;private set;} 
    public float AmountMult { get; private set; } = 1f;

    public float Duration{get;private set;}
    public float DurationMult { get; private set; } = 0.5f;
    


    

 

    readonly string buffName = "CommonAtkUp";
    
    
    public void Apply()
    {
    
        Duration = DurationMult * playerStats.GetSkillLevel(buffName);
        Amount = AmountMult * playerStats.GetSkillLevel(buffName);
        playerStats.AtkBuff += (int)Amount;

        
    }   

    public void Remove()
    {
        playerStats.AtkBuff -= (int)Amount;
    }

   
}
