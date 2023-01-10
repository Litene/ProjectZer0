using UnityEngine;

public interface IRandomEvent {
    public enum EventThreat {
        Low,
        Medium,
        High
    }
    
    [Tooltip("The relative chance of this specific event to fire, compared to other event-weights in the pool of possible ones.")]
    public float Weight {
        get;
        set;
    }
    
    public void DoEvent();
}
