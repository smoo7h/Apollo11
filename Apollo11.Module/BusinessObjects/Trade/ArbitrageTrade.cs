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
using DevExpress.Xpo.DB;

namespace Apollo11.Module.BusinessObjects.Trade
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ArbitrageTrade : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ArbitrageTrade(Session session)
            : base(session)
        {

        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Created = DateTime.Now;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        private DateTime _Created;
        public DateTime Created
        {
            get { return _Created; }
            set { SetPropertyValue<DateTime>("Created", ref _Created, value); }
        }


        private Exchange _BuyFrom;
        public Exchange BuyFrom
        {
            get { return _BuyFrom; }
            set { SetPropertyValue<Exchange>("BuyFrom", ref _BuyFrom, value); }
        }


        private Exchange _SellTo;
        public Exchange SellTo
        {
            get { return _SellTo; }
            set { SetPropertyValue<Exchange>("SellTo", ref _SellTo, value); }
        }



        private decimal _Amount;
        public decimal Amount
        {
            get { return _Amount; }
            set { SetPropertyValue<decimal>("Amount", ref _Amount, value); }
        }


        private Trade _BuyTrade;
        public Trade BuyTrade
        {
            get { return _BuyTrade; }
            set { SetPropertyValue<Trade>("BuyTrade", ref _BuyTrade, value); }
        }


        private Trade _SellTrade;
        public Trade SellTrade
        {
            get { return _SellTrade; }
            set { SetPropertyValue<Trade>("SellTrade", ref _SellTrade, value); }
        }


        private decimal _Profit;
        public decimal Profit
        {
            get { return _Profit; }
            set { SetPropertyValue<decimal>("Profit", ref _Profit, value); }
        }


        private decimal _ProfitPercent;
        public decimal ProfitPercent
        {
            get { return _ProfitPercent; }
            set { SetPropertyValue<decimal>("ProfitPercent", ref _ProfitPercent, value); }
        }



        private decimal _MarketAmount;
        public decimal MarketAmount
        {
            get { return _MarketAmount; }
            set { SetPropertyValue<decimal>("MarketAmount", ref _MarketAmount, value); }
        }



        private decimal _MaxAmount;
        public decimal MaxAmount
        {
            get { return _MaxAmount; }
            set { SetPropertyValue<decimal>("MaxAmount", ref _MaxAmount, value); }
        }









        public void CreateTrade(decimal maxAmount)
        {

            MaxAmount = maxAmount;
            //  Order buyOrder = (BuyFrom.CurrentOrder as IList<Order>).Where(p => p.OrderType.Name == "Sell").OrderByDescending(o => o.DateAdded).ToList()[0];
            // Order sellOrder = (SellTo.CurrentOrder as IList<Order>).Where(q => q.OrderType.Name == "Buy").OrderByDescending(r => r.DateAdded).ToList()[0];

            //      CriteriaOperator criteria =  CriteriaOperator.And(new BinaryOperator("Name",BuyFrom.Name), new Bin)
            //     new OperandProperty("City"), new OperandValue("Chicago"),
            //   BinaryOperatorType.NotEqual);


            //   Order buyOrder = (Session as IObjectSpace).FindObject<Order>(new BinaryOperator());
            //            Order sellOrder = (Session as IObjectSpace).FindObject<Order>(CriteriaOperator.And(new BinaryOperator("Exchange.Oid",BuyFrom.Oid)),

            Order buyOrder = null;
         //   BinaryOperator b1 = new BinaryOperator("Exchange.Oid", BuyFrom.Oid);
            BinaryOperator b2 = new BinaryOperator("OrderType.Name", "Sell");
            SortProperty s1 = new SortProperty("DateAdded", DevExpress.Xpo.DB.SortingDirection.Descending);


            XPCollection<Order> orders = BuyFrom.CurrentOrder;
            orders.Criteria = b2; // CriteriaOperator.And(b1, b2);
            orders.Sorting.Add(new SortingCollection(s1));

            buyOrder = orders[0];

            Order sellOrder = null;
            //BinaryOperator c1 = new BinaryOperator("Exchange.Oid", SellTo.Oid);
            BinaryOperator c2 = new BinaryOperator("OrderType.Name", "Buy");



            XPCollection<Order> sellorders = SellTo.CurrentOrder;
            orders.Criteria = c2;
            orders.Sorting.Add(new SortingCollection(s1));

            sellOrder = sellorders[0];




            if (buyOrder != null && sellOrder != null)
            {


                //get amount based on orderbook
                if (buyOrder.Amount > sellOrder.Amount)
                {
                    Amount = sellOrder.Amount;
                }
                else if (buyOrder.Amount < sellOrder.Amount)
                {
                    Amount = buyOrder.Amount;
                }
                else if ((buyOrder.Amount == sellOrder.Amount))
                {
                    Amount = sellOrder.Amount;
                }
                MarketAmount = Amount;
                if (Amount > MaxAmount)
                {
                    Amount = MaxAmount;
                }

                if (Amount != 0)
                {
                    //create a new buy trade
                    BuyTrade = new Trade(Session);
                    BuyTrade.Amount = Amount;
                    BuyTrade.Exchange = BuyFrom;
                    BuyTrade.TradeType = Session.FindObject<TradeType>(new BinaryOperator("Name", "Sell"));
                    BuyTrade.BuyOrder = buyOrder;
                    //get the top order book price
                    BuyTrade.BitcoinPrice = buyOrder.Price;
                    BuyTrade.Total = BuyTrade.BitcoinPrice * BuyTrade.Amount;
                    //create new sell trade
                    SellTrade = new Trade(Session);
                    SellTrade.Amount = Amount;
                    SellTrade.Exchange = SellTo;
                    SellTrade.TradeType = Session.FindObject<TradeType>(new BinaryOperator("Name", "Buy"));
                    //get the top order book price
                    SellTrade.SellOrder = sellOrder;
                    SellTrade.BitcoinPrice = sellOrder.Price;
                    SellTrade.Total = SellTrade.BitcoinPrice * SellTrade.Amount;


                    this.Profit = SellTrade.TotalWithFeeCad - BuyTrade.TotalWithFeeCad; //in canadain
                    this.ProfitPercent = (Profit / BuyTrade.TotalWithFeeCad ) * 100;
                    Session.CommitTransaction();
                }
            }

        }






    }
}