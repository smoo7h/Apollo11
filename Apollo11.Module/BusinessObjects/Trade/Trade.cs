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

namespace Apollo11.Module.BusinessObjects.Trade
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Trade : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Trade(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }




        private TradeType _TradeType;
        public TradeType TradeType
        {
            get { return _TradeType; }
            set { SetPropertyValue<TradeType>("TradeType", ref _TradeType, value); }
        }




        private decimal _Amount;
        public decimal Amount
        {
            get { return _Amount; }
            set { SetPropertyValue<decimal>("Amount", ref _Amount, value); }
        }



        private decimal _BitcoinPrice;
        public decimal BitcoinPrice
        {
            get { return _BitcoinPrice; }
            set { SetPropertyValue<decimal>("BitcoinPrice", ref _BitcoinPrice, value); }
        }



        public decimal BitcoinPriceCad
        {
            get {
                if (this.Exchange.ExchangeType.Name == "CAD")
                {
                    //canadian
                    return BitcoinPrice;
                }
                else
                {
                    //american
                    return (decimal)this.Evaluate("BitcoinPrice * Exchange.ExchangeType.ExchangeRate.Rate");
                }

               

            }
           
        }

        public decimal FeeCad
        {
            get
            {

                if (this.Exchange.ExchangeType.Name == "CAD")
                {
                    //canadian
                    return Fee;
                }
                else
                {
                    //american
                    return Convert.ToDecimal(this.Evaluate("Fee * Exchange.ExchangeType.ExchangeRate.Rate"));
                }

            }

        }



        public decimal Fee
        {
            get {

                
                return (decimal)this.Evaluate("Exchange.TradingFee * Total");
            }
            
        }


        private decimal _Total;
        public decimal Total
        {
            get { return _Total; }
            set { SetPropertyValue<decimal>("Total", ref _Total, value); }
        }

        public decimal TotalCad
        {
            get
            {
                if (this.Exchange.ExchangeType.Name == "CAD")
                {
                    //canadian
                    return Total;
                }
                else
                {
   
                    //american
                    return (decimal)this.Evaluate("Total * Exchange.ExchangeType.ExchangeRate.Rate");
                }



            }

        }

        public decimal TotalWithFeeCad
        {
            get
            {
                if (this.Exchange.ExchangeType.Name == "CAD")
                {
                    //canadian
                    return Total + FeeCad;
                }
                else
                {

                    //american
                    return (decimal)this.Evaluate("TotalCad + FeeCad");
                }



            }

        }


        private Order _BuyOrder;
        public Order BuyOrder
        {
            get { return _BuyOrder; }
            set { SetPropertyValue<Order>("BuyOrder", ref _BuyOrder, value); }
        }



        private Order _SellOrder;
        public Order SellOrder
        {
            get { return _SellOrder; }
            set { SetPropertyValue<Order>("SellOrder", ref _SellOrder, value); }
        }



        [Association("Exchange-Trade", typeof(Exchange))]
        public Exchange Exchange
        {
            get { return this.GetPropertyValue<Exchange>("Exchange"); }
            set { this.SetPropertyValue("Exchange", value); }
        }


        private bool _LiveTrade;
        public bool LiveTrade
        {
            get { return _LiveTrade; }
            set { SetPropertyValue<bool>("LiveTrade", ref _LiveTrade, value); }
        }



    }
}