using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using Jojatekok.BitstampAPI;
using Apollo11.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using Jojatekok.BitstampAPI.MarketTools;
using QuadrigaCX;

namespace Apollo11.WorkflowServerService
{
    public class Quadrigacx
    {

        public Quadrigacx(IObjectSpace space)
        {
            this.Space = space;
   
            CurrentExchange = space.FindObject<Exchange>(new BinaryOperator("Name", "quadrigacx.com"));
            QuadrigaAPIClient = new QuadrigaAPI(Convert.ToInt32(CurrentExchange.APIClientID), CurrentExchange.APIKey, CurrentExchange.APISecret, "");
            this.APIKey = CurrentExchange.APIKey;
            this.APISecret = CurrentExchange.APISecret;
            this.APIClientID = CurrentExchange.APIClientID;
        }
        public Exchange CurrentExchange { get; set; }
        public IObjectSpace Space { get; set; }
        public QuadrigaAPI QuadrigaAPIClient { get; set; }

        public string APIKey { get; set; }
        public string APISecret { get; set; }
        public string APIClientID { get; set; }

        public void GetData()
        {
            LoadMarketSummaryAsync(Space);

        }

        public void CreateSellTrade(decimal amount, decimal price)
        {

            QuadrigaAPI api = QuadrigaAPIClient;
            try
            {
                //    var tradinginfo = api.GetCurrentTradingInformation("btc_cad",true);

                Console.WriteLine("selling");
                var order = api.SellOrderLimit(amount, price, "btc_cad");
                //frmObjectVisualizer frm = new frmObjectVisualizer(order);
                //  frm.Show();
                Console.WriteLine("sold");

            }
            catch (QuadrigaResultError ex)
            {
                Console.WriteLine(String.Format("Code: {0}, Message: {1}", ex.QuadrigaErrorCode, ex.Message));
            }





        }

        public void CreateBuyTrade(decimal amount, decimal price)
        {

            QuadrigaAPI api = QuadrigaAPIClient;
            try
            {
                //    var tradinginfo = api.GetCurrentTradingInformation("btc_cad",true);

                Console.WriteLine("Buying");
                var order = api.BuyOrderLimit(amount, price, "btc_cad");
                //frmObjectVisualizer frm = new frmObjectVisualizer(order);
                //  frm.Show();
                Console.WriteLine("bought");

            }
            catch (QuadrigaResultError ex)
            {
                Console.WriteLine(String.Format("Code: {0}, Message: {1}", ex.QuadrigaErrorCode, ex.Message));
            }





        }

        private void LoadMarketSummaryAsync(IObjectSpace space)
        {
            QuadrigaAPI api = new QuadrigaAPI();
            CurrentTradingInformation market;

          
                 market = api.GetCurrentTradingInformation("btc_cad", true);
       


            //      var orderbook = api.GetOrderBook("btc_cad", true, true);
            //      textBox1.Text = orderbook.asks[0].Price.ToString();
            //    textBox2.Text = orderbook.bids[0].Price.ToString();


            //     var market = await bitstampClient.Market.GetSummaryAsync();

            //  CurrentExchange
            if (market != null)
            {


                CurrentExchange.Last = Convert.ToDecimal(market.last);
                CurrentExchange.Bid = Convert.ToDecimal(market.bid);
                CurrentExchange.Ask = Convert.ToDecimal(market.ask);
                CurrentExchange.Low = Convert.ToDecimal(market.low);
                CurrentExchange.High = Convert.ToDecimal(market.high);


                CurrentExchange.ServerResponseTimestamp = market.timestamp;
                CurrentExchange.Volume = Convert.ToDecimal(market.volume);
                space.CommitChanges();
               // Console.WriteLine(String.Format("{0}Quadrigacx Current Ask: {1} Current Bid: {2}", market.timestamp.ToLongTimeString(), market.bid.ToString(), market.ask.ToString()));
            }
            else
            {
                Console.WriteLine("didnt get it");
            }

            LoadMarketSummaryAsync(space);

           
           

            //   TextBlockPriceLast.Text = "$" + market.PriceLast.ToStringNormalized();
        }
    }
}
