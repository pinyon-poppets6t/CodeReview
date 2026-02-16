namespace Infrastructure.Repositories;

using Domain.Entities;

public interface ICardRepository
{
    public Task<CardDetails> GetCardAsync(string userId, string cardNumber);
}