using System;
using System.Collections.Generic;
using System.Linq;

namespace Market.InfrastructureLayer.Models
{
    public enum State { Basket, Submitted, Sent, Delivered }

    public class Order
    {
        public Order()
        {
            Items = new List<OrderItem>();
        }

        public long? Id { get; set; }
        public State State { get; set; }
        public User User { get; set; }
        public List<OrderItem> Items { get; }
        public decimal TotalPrice { get { return Items.Sum(item => item.TotalPrice); } }
        public int TotalCount { get { return Items.Sum(item => item.Count); } }
        public bool ItemsAvailable { get { return Items.Count > 0 && Items.All(item => item.Available); } }
        public DateTime? SubmitDate { get; set; }
        public string SubmitDatePersian { get; set; }
        public DateTime? SentDate { get; set; }
        public string SentDatePersian { get; set; }
        public DateTime? DeliverDate { get; set; }
        public string DeliverDatePersian { get; set; }
    }
}