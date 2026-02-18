namespace Domain.Entities;

using Enums;

public record CardDetails(string CardNumber, CardType CardType, CardStatus CardStatus, bool IsPinSet);