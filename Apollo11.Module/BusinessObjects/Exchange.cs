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
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Exchange : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Exchange(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.OrderBook = new OrderBook(Session);
            LiveData = false;
        }


        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue<string>("Name", ref _Name, value); }
        }


        private string _Website;
        public string Website
        {
            get { return _Website; }
            set { SetPropertyValue<string>("Website", ref _Website, value); }
        }




        private string _APIKey;
        public string APIKey
        {
            get { return _APIKey; }
            set { SetPropertyValue<string>("APIKey", ref _APIKey, value); }
        }


        private string _APISecret;
        public string APISecret
        {
            get { return _APISecret; }
            set { SetPropertyValue<string>("APISecret", ref _APISecret, value); }
        }


        private string _APIClientID;
        public string APIClientID
        {
            get { return _APIClientID; }
            set { SetPropertyValue<string>("APIClientID", ref _APIClientID, value); }
        }



        private decimal _BTCHolding;
        public decimal BTCHolding
        {
            get { return _BTCHolding; }
            set { SetPropertyValue<decimal>("BTCHolding", ref _BTCHolding, value); }
        }


        private decimal _DollarHolding;
        public decimal DollarHolding
        {
            get { return _DollarHolding; }
            set { SetPropertyValue<decimal>("DollarHolding", ref _DollarHolding, value); }
        }


        private ExchangeType _ExchangeType;
        public ExchangeType ExchangeType
        {
            get { return _ExchangeType; }
            set { SetPropertyValue<ExchangeType>("ExchangeType", ref _ExchangeType, value); }
        }


        private OrderBook _OrderBook;
        public OrderBook OrderBook
        {
            get { return _OrderBook; }
            set { SetPropertyValue<OrderBook>("OrderBook", ref _OrderBook, value); }
        }




        private decimal _Volume;
        public decimal Volume
        {
            get { return _Volume; }
            set { SetPropertyValue<decimal>("Volume", ref _Volume, value); }
        }


        private decimal _Low;
        public decimal Low
        {
            get { return _Low; }
            set { SetPropertyValue<decimal>("Low", ref _Low, value); }
        }


        private decimal _High;
        public decimal High
        {
            get { return _High; }
            set { SetPropertyValue<decimal>("High", ref _High, value); }
        }


        private decimal _Last;
        public decimal Last
        {
            get { return _Last; }
            set { SetPropertyValue<decimal>("Last", ref _Last, value); }
        }


        private decimal _Ask;
        public decimal Ask
        {
            get { return _Ask; }
            set { SetPropertyValue<decimal>("Ask", ref _Ask, value); }
        }


        private decimal _Bid;
        public decimal Bid
        {
            get { return _Bid; }
            set { SetPropertyValue<decimal>("Bid", ref _Bid, value); }
        }


        private decimal _OrderSpreadPercentage;
        public decimal OrderSpreadPercentage
        {
            get { return _OrderSpreadPercentage; }
            set { SetPropertyValue<decimal>("OrderSpreadPercentage", ref _OrderSpreadPercentage, value); }
        }




        private decimal _OrderSpreadValue;
        public decimal OrderSpreadValue
        {
            get { return _OrderSpreadValue; }
            set { SetPropertyValue<decimal>("OrderSpreadValue", ref _OrderSpreadValue, value); }
        }


        private decimal _TradingFee;
        public decimal TradingFee
        {
            get { return _TradingFee; }
            set { SetPropertyValue<decimal>("TradingFee", ref _TradingFee, value); }
        }






        private decimal _PriceAverage;
        public decimal PriceAverage
        {
            get { return _PriceAverage; }
            set { SetPropertyValue<decimal>("PriceAverage", ref _PriceAverage, value); }
        }



        private DateTime _ServerResponseTimestamp;
        public DateTime ServerResponseTimestamp
        {
            get { return _ServerResponseTimestamp; }
            set { SetPropertyValue<DateTime>("ServerResponseTimestamp", ref _ServerResponseTimestamp, value); }
        }




        [Association("Exchange-CurrentOrder", typeof(Order))]
        public XPCollection<Order> CurrentOrder
        {
            get { return GetCollection<Order>("CurrentOrder"); }
        }




        private decimal _Open;
        public decimal Open
        {
            get { return _Open; }
            set { SetPropertyValue<decimal>("Open", ref _Open, value); }
        }


        [Association("Exchange-Trade", typeof(Trade.Trade))]
        public XPCollection<Trade.Trade> Trade
        {
            get { return GetCollection<Trade.Trade>("Trade"); }
        }


        private bool _LiveData;
        public bool LiveData
        {
            get { return _LiveData; }
            set { SetPropertyValue<bool>("LiveData", ref _LiveData, value); }
        }




    }
}