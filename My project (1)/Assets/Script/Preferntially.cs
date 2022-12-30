using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preferntially : MonoBehaviour
{
    public class OrderTiket
    {
        private int _id;
        private OrderTiket _nextPlayer;

        public int ID { get { return _id; } set { _id = value; } }
        public OrderTiket NextPlayer { get { return _nextPlayer; } set { _nextPlayer = value; } }

        public OrderTiket(int id)
        {
            _id = id;
            _nextPlayer = null;
        }
    }

    OrderTiket root;
/*
    public void Add(OrderTiket player)
    {
        if(root != null)
        {
            root = player;
        }

        OrderTiket orderTiket = root;

        while (orderTiket.NextPlayer != null)
        {
            if(GameManager.Instance.allInformations.)
            {

            }
        }

        orderTiket.NextPlayer = player;
    }*/
}
