using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Repositories;

public class CardRepository : ICardRepository
{
    private readonly Dictionary<string, Dictionary<string, CardDetails>> _userCards = CreateSampleUserCards();

    public async Task<CardDetails> GetCardAsync(string userId, string cardNumber)
    {
        if (!_userCards.TryGetValue(userId, out var cards) || !cards.TryGetValue(cardNumber, out var cardDetails))
        {
            throw new Exception("Unable to find the requested card");
        }

        return await Task.FromResult(cardDetails);
    }

    private static Dictionary<string, Dictionary<string, CardDetails>> CreateSampleUserCards()
    {
        var userCards = new Dictionary<string, Dictionary<string, CardDetails>>();
        for (var i = 1; i <= 3; i++)
        {
            var cards = new Dictionary<string, CardDetails>();
            var cardIndex = 1;
            foreach (CardType cardType in Enum.GetValues(typeof(CardType)))
            {
                foreach (CardStatus cardStatus in Enum.GetValues(typeof(CardStatus)))
                {
                    var cardNumber = $"Card{i}{cardIndex}";
                    cards.Add(cardNumber,
                        new CardDetails(
                            CardNumber: cardNumber,
                            CardType: cardType,
                            CardStatus: cardStatus,
                            IsPinSet: cardIndex % 2 == 0));
                    cardIndex++;
                }
            }
            var userId = $"User{i}";
            userCards.Add(userId, cards);
        }
        return userCards;
    }
}