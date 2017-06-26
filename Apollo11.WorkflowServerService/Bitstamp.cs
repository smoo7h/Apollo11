using DevExpress.ExpressApp;
using Jojatekok.BitstampAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollo11.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using Jojatekok.BitstampAPI.MarketTools;
using System.Threading;


namespace Apollo11.WorkflowServerService
{
    public class Bitstamp
    {

        public Bitstamp(IObjectSpace space, string apiKey)
        {
            this.Space = space;
            this.APIKey = apiKey;
            CurrentExchange = space.FindObject<Exchange>(new BinaryOperator("Name", "BitStamp"));
            bitstampClient = new BitstampClient();

        }
        public Exchange CurrentExchange { get; set; }
        public IObjectSpace Space { get; set; }
        private BitstampClient bitstampClient { get; set; }

        public string APIKey { get; set; }

        public void GetData()
        {
            LoadMarketSummaryAsync(Space);

           // return Space;

        }

        private async void LoadMarketSummaryAsync(IObjectSpace space)
        {
            try
            {

           
           
                var market = await bitstampClient.Market.GetSummaryAsync();

                //  CurrentExchange
                CurrentExchange.Last = Convert.ToDecimal(market.PriceLast);
                CurrentExchange.Bid = Convert.ToDecimal(market.OrderTopBuy);
                CurrentExchange.Ask = Convert.ToDecimal(market.OrderTopSell);
                CurrentExchange.Low = Convert.ToDecimal(market.Price24HourLow);
                CurrentExchange.High = Convert.ToDecimal(market.Price24HourHigh);

                CurrentExchange.OrderSpreadPercentage = Convert.ToDecimal(market.OrderSpreadPercentage);
                CurrentExchange.OrderSpreadValue = Convert.ToDecimal(market.OrderSpreadValue);
                CurrentExchange.ServerResponseTimestamp = market.ServerResponseTimestamp;
                CurrentExchange.Volume = Convert.ToDecimal(market.Volume24HourBase);
                space.CommitChanges();

                Console.WriteLine(String.Format( "{0} Current Ask: {1} Current Bid: {2}",market.ServerResponseTimestamp.ToLongTimeString(), market.OrderTopBuy.ToString(), market.OrderTopSell.ToString()));
                
                //LoadMarketSummaryAsync(space);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
