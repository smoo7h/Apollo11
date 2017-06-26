using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace Apollo11.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class Order : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Order(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            DateAdded = DateTime.Now;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        private DateTime _DateAdded;
        public DateTime DateAdded
        {
            get { return _DateAdded; }
            set { SetPropertyValue<DateTime>("DateAdded", ref _DateAdded, value); }
        }



        private OrderType _OrderType;
        public OrderType OrderType
        {
            get { return _OrderType; }
            set { SetPropertyValue<OrderType>("OrderType", ref _OrderType, value); }
        }


        private decimal _Amount;
        public decimal Amount
        {
            get { return _Amount; }
            set { SetPropertyValue<decimal>("Amount", ref _Amount, value); }
        }


        private decimal _Price;
        public decimal Price
        {
            get { return _Price; }
            set { SetPropertyValue<decimal>("Price", ref _Price, value); }
        }

        public decimal Value
        {
            get
            {
               
                    return (decimal)this.Evaluate("Price * Amount");
                
            }
        }


        [Association("Exchange-CurrentOrder", typeof(Exchange))]
        public Exchange Exchange
        {
            get { return this.GetPropertyValue<Exchange>("Exchange"); }
            set { this.SetPropertyValue("Exchange", value); }
        }



        [Association("OrderBook-Order", typeof(OrderBook))]
        public OrderBook OrderBook
        {
            get { return this.GetPropertyValue<OrderBook>("OrderBook"); }
            set { this.SetPropertyValue("OrderBook", value); }
        }





    }
}