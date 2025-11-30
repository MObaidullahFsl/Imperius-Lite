using System.Collections.Generic;
using UnityEngine;
using Imperius.Logic;
using System;

namespace Imperius.Logic
{


    public class Deck
    {


        public string Name;

        public int CardsNumber;
        public List<Card> DeckCards = new();
        public Dictionary<Element, int> ElementWiseCardsNo = new();


        public Deck(string name)
        {
            Name = name;

            CardsNumber = 20;

            foreach (Element e in Enum.GetValues(typeof(Element)))
            {
                ElementWiseCardsNo.Add(e, 0);
            }

        }

        public Deck()
        {
            Name = "Default_Deck";

            CardsNumber = 20;

            foreach (Element e in Enum.GetValues(typeof(Element)))
            {
                ElementWiseCardsNo.Add(e, 0);
            }
        }
        public Deck(Deck d) : this(d.Name)
        {
            this.CardsNumber = d.CardsNumber;

            foreach (var c in d.DeckCards)
            {
                this.AddCard(c);
            }
        }
        public void AddCard(Card c)
        {
            DeckCards.Add(c);

            ElementWiseCardsNo[c.element]++;

        }
        public void RemoveCard(Card c)
        {
            DeckCards.Remove(c);

            ElementWiseCardsNo[c.element]--;

        }


    }
}