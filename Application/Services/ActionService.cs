using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services
{
    public class ActionService : IActionService
    {
        public IEnumerable<string> GetAllowedActions(CardDetails card)
        {
            return (card.CardType, card.CardStatus, card.IsPinSet) switch
            {
                (CardType.Prepaid or CardType.Debit, CardStatus.Ordered, true) => ["ACTION3", "ACTION4", "ACTION6", "ACTION8", "ACTION9", "ACTION10", "ACTION12", "ACTION13"],
                (CardType.Prepaid or CardType.Debit, CardStatus.Ordered, false) => ["ACTION3", "ACTION4", "ACTION7", "ACTION8", "ACTION9", "ACTION10", "ACTION12", "ACTION13"],
                (CardType.Prepaid or CardType.Debit, CardStatus.Inactive, true) => ["ACTION2", "ACTION3", "ACTION4", "ACTION6", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13"],
                (CardType.Prepaid or CardType.Debit, CardStatus.Inactive, false) => ["ACTION2", "ACTION3", "ACTION4", "ACTION7", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13"],
                (CardType.Prepaid or CardType.Debit, CardStatus.Active, true) => ["ACTION1", "ACTION3", "ACTION4", "ACTION6", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13"],
                (CardType.Prepaid or CardType.Debit, CardStatus.Active, false) => ["ACTION1", "ACTION3", "ACTION4", "ACTION7", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13"],
                (CardType.Prepaid or CardType.Debit, CardStatus.Restricted, _) => ["ACTION3", "ACTION4", "ACTION9"],
                (CardType.Prepaid or CardType.Debit, CardStatus.Blocked, true) => ["ACTION3", "ACTION4", "ACTION6", "ACTION7", "ACTION8", "ACTION9"],
                (CardType.Prepaid or CardType.Debit, CardStatus.Blocked, false) => ["ACTION3", "ACTION4", "ACTION8", "ACTION9"],
                (CardType.Prepaid or CardType.Debit, CardStatus.Expired, _) => ["ACTION3", "ACTION4", "ACTION9"],
                (CardType.Prepaid or CardType.Debit, CardStatus.Closed, _) => ["ACTION3", "ACTION4", "ACTION9"],
                (CardType.Credit, CardStatus.Ordered, true) => ["ACTION3", "ACTION4", "ACTION5", "ACTION6", "ACTION8", "ACTION9", "ACTION10", "ACTION12", "ACTION13"],
                (CardType.Credit, CardStatus.Ordered, false) => ["ACTION3", "ACTION4", "ACTION5", "ACTION7", "ACTION8", "ACTION9", "ACTION10", "ACTION12", "ACTION13"],
                (CardType.Credit, CardStatus.Inactive, true) => ["ACTION2", "ACTION3", "ACTION4", "ACTION5", "ACTION6", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13"],
                (CardType.Credit, CardStatus.Inactive, false) => ["ACTION2", "ACTION3", "ACTION4", "ACTION5", "ACTION7", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13"],
                (CardType.Credit, CardStatus.Active, true) => ["ACTION1", "ACTION3", "ACTION4", "ACTION5", "ACTION6", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13"],
                (CardType.Credit, CardStatus.Active, false) => ["ACTION1", "ACTION3", "ACTION4", "ACTION5", "ACTION7", "ACTION8", "ACTION9", "ACTION10", "ACTION11", "ACTION12", "ACTION13"],
                (CardType.Credit, CardStatus.Restricted, _) => ["ACTION3", "ACTION4", "ACTION5", "ACTION9"],
                (CardType.Credit, CardStatus.Blocked, true) => ["ACTION3", "ACTION4", "ACTION5", "ACTION6", "ACTION7", "ACTION8", "ACTION9"],
                (CardType.Credit, CardStatus.Blocked, false) => ["ACTION3", "ACTION4", "ACTION5", "ACTION8", "ACTION9"],
                (CardType.Credit, CardStatus.Expired, _) => ["ACTION3", "ACTION4", "ACTION5", "ACTION9"],
                (CardType.Credit, CardStatus.Closed, _) => ["ACTION3", "ACTION4", "ACTION5", "ACTION9"],
                _ => new List<string>()
            };
        }
    }
}
