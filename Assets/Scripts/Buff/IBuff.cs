
using Unity.VisualScripting;

public interface IBuff
{   

    void Apply(); // 버프 적용
    void Remove(); // 버프 제거 

   

    float Amount { get;}  // 버프 효과 크기
    float AmountMult{get;}

    float Duration { get;}  // 지속 시간
    float DurationMult{get;}
    
}





