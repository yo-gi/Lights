public interface Rechargeable {
    int MaxCharges { get; }
    int Charges { get; }
    bool Charging { get; }
    float ChargePercentage { get; }
}
