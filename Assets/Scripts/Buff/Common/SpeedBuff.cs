

using UnityEditor.ShortcutManagement;

public class SpeedBuff : IBuff 
{
    
    PlayerStats playerStats = PlayerStats.Instance;
     public float Amount { get;private set;} 
    public float AmountMult { get; private set; } = 0.1f;

    public float Duration{get;private set;}
    public float DurationMult { get; private set; } = 1f;
    



    PlayerMove playerMove;

    public SpeedBuff()
    {
        playerMove = playerStats.playerMove;
        
    }

    

    readonly string buffName = "CommonSpdUp";
    
    
    public void Apply()
    {
        Duration = DurationMult * playerStats.GetSkillLevel(buffName);
        Amount = AmountMult * playerStats.GetSkillLevel(buffName);
        playerMove.moveSpeed += Amount;
        
        
    }   

    public void Remove()
    {
        playerMove.moveSpeed -= Amount;
    }

    
}
