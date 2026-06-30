using System;
using System.Collections;
using CodeBase.Entities;
using CodeBase.Entities.Player;
using PrimeTween;
using TMPro;
using UnityEngine;

namespace CodeBase.Cards
{
    public class CardManager : MonoBehaviour
    {
        [SerializeField] private PlayerEntity player;
        [SerializeField] private CardAimController cardAimController;
    
        [SerializeField] private Transform handParent;
        [SerializeField] private GameObject cardPrefab;

        [SerializeField] private TextMeshProUGUI deckCount;
        [SerializeField] private TextMeshProUGUI discardPileCount;
        public CardCollection Deck { get; private set; }
        public CardCollection Hand { get; private set; }
        public CardCollection DiscardPile { get; private set; }

        // Events for UI updates
        public event Action<CardInstance> OnCardPlayed;
        public event Action<CardInstance> OnCardDrawn;

        private CardInstance pendingCard;
        
        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Deck = new CardCollection();
            Hand = new CardCollection();
            DiscardPile = new CardCollection();

            Deck.OnCardAdded += UpdateCardCounts;
            Deck.OnCardRemoved += UpdateCardCounts;
            DiscardPile.OnCardAdded += UpdateCardCounts;
            DiscardPile.OnCardRemoved += UpdateCardCounts;
        
        
            foreach (var data in player.StartingDeck)
                Deck.Add(data.CreateInstance());

            Deck.Shuffle();
        }

        private void UpdateCardCounts(CardInstance _)
        {
            deckCount.text = Deck.Count.ToString();
            discardPileCount.text = DiscardPile.Count.ToString();
        }

        public IEnumerator DrawHandCoroutine()
        {
            const int drawCount = 3;
        
            for (var i = 0; i < drawCount; i++)
            {
                DrawCard();
                yield return new WaitForSeconds(0.15f);
            }
        }

        public void StartAimingForCard(CardInstance card)
        {
            pendingCard = card;

            cardAimController.StartAiming(
                (target) => ExecuteCard(pendingCard, target),
                () => { pendingCard = null; }
            );
        }

        public bool CanAfford(CardInstance card)
        {
            return player.Mana >= card.CurrentCost;
        }

        private void ExecuteCard(CardInstance card, ITargetable target)
        {
            if(!IsValidTarget(card, target))
                return;
            
            SpendMana(card.CurrentCost);
            
            foreach (var effect in card.Effects)
            {
                effect.Execute(target);
            }

            DiscardCard(card);

            pendingCard = null;

            OnCardPlayed?.Invoke(card);
        }

        private bool IsValidTarget(CardInstance card, ITargetable target)
        {
            if(!target.IsAlive)
                return false;
            
            var isPlayer = target is PlayerEntity;
            if(card.Data.target == Target.self)
                return isPlayer;
            return !isPlayer;
        }

        private void DiscardCard(CardInstance card)
        {
            Hand.Remove(card);
            DiscardPile.Add(card);
            DestroyCardViews(card);
        }

        private void DestroyCardViews(CardInstance card)
        {
            foreach (var cardView in card.Views)
            {
                Tween.Scale(cardView.transform, Vector3.zero, 0.15f, Ease.InExpo).
                    OnComplete(() => { Destroy(cardView.gameObject); });
            }
            card.Views.Clear();
        }

        private void SpendMana(int cardCurrentCost)
        {
            player.SpendMana(cardCurrentCost);
        }

        private void DrawCard()
        {
            if (Deck.Count == 0)
            {
                RechargeDeck();
            } 
            var card = Deck.Draw();
            Hand.Add(card);
            CreateCardView(card);
            OnCardDrawn?.Invoke(card);
        }

        private void RechargeDeck()
        {
            while (DiscardPile.Count > 0)
            {
                var card = DiscardPile.Draw();
                Deck.Add(card);
            }
        }
        
        private void CreateCardView(CardInstance card)
        {
            var go = Instantiate(cardPrefab, handParent);
            var view = go.GetComponent<CardView>();
            view.transform.localScale = Vector3.zero;
            Tween.Scale(view.transform, Vector3.one, 0.15f, ease: Ease.OutExpo);
            view.Bind(card, this);
        }

        public IEnumerator DiscardHandCoroutine()
        {
            var handCards = Hand.Cards;
            for (var i = handCards.Count - 1; i >=0 ; i--)
            {
                DiscardCard(handCards[i]);
                yield return new WaitForSeconds(0.15f);
            }
        }
    }
}