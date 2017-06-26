using Cob.TradingApps.BtcData;
using Cob.TradingApps.BtcData.Exchanges;
using log4net.Config;
using RestSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradingApi.Bitfinex;
using TradingApi.ModelObjects;
using TradingApi.ModelObjects.Bitfinex.Json;
using TradingCoins.Controls;
using BitfinexApi;

namespace Apollo11.WorkflowServerService
{
    public class BitfinexRestAPI
    {

     
        public BitfinexApiV1 api { get; set; }
        public BalancesResponse bal { get; set; }



        public BitfinexRestAPI()
        {

            var secret = ConfigurationManager.AppSettings["ApiSecret"];
            var key = ConfigurationManager.AppSettings["ApiKey"];


             api = new BitfinexApiV1(key, secret);

             bal = api.GetBalances();
        }



        private static string GetHexHashSignature(string payload, string secret)
        {
            HMACSHA384 hmac = new HMACSHA384(Encoding.UTF8.GetBytes(secret));
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public void GetOrders()
        {


            ActiveOrdersResponse orders = api.GetActiveOrders();


        }

        public void GetPositions()
        {

            ActivePositionsResponse positions = api.GetActivePositions();

        }

        public  void CancelTrade(int order_id)
        {
            CancelOrderResponse cancel = api.CancelOrder(order_id);
        }

        public void CreateTrade(decimal price, decimal Amount, TradeType type)
        {

            if (type == TradeType.Buy)
            {
                NewOrderResponse sell = api.ExecuteBuyOrderBTC(Convert.ToDecimal(Amount), Convert.ToDecimal(price), OrderExchange.Bitfinex, OrderType.MarginLimit);
            }
            else
            {
                NewOrderResponse sell = api.ExecuteSellOrderBTC(Convert.ToDecimal(Amount), Convert.ToDecimal(price), OrderExchange.Bitfinex, OrderType.MarginLimit);

            }

       
        }

        public void CancelAllTrades()
        {
              api.CancelAllOrders();

        }

        public void CreateTradeOld()
        {
            var client = new RestClient("https://api.bitfinex.com/");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("account_infos", Method.POST);
            string authNonce = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
       
            //
           string payload = "{ 'request':'/v1/account_infos','nonce':"+ authNonce + "}";
            string payload64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload)); 



            string sig = GetHexHashSignature(payload64, "GksOmzQZR8Ykt0S1Wi601W3zqaK0IXugDyo8kfitnOP");
            // request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method

       //     BitfinexApi api = new BitfinexApi("GksOmzQZR8Ykt0S1Wi601W3zqaK0IXugDyo8kfitnOP", "dkFj2FG4kMRU46ic979bGll2kP2W8UbCPaDMWKAx6nj");
            
      //      BitfinexOrderBookGet boook =  api.GetOrderBook(BtcInfo.PairTypeEnum.btcusd);
            //   BitfinexOrderBookGe GetOrderBook = api.GetActiveOffers() ;
            //  IList<BitfinexOfferStatusResponse>  orders 


         //   Console.WriteLine(api.GetAccountInformation().ToString());

            // easily add HTTP Headers

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("X-BFX-APIKEY", "dkFj2FG4kMRU46ic979bGll2kP2W8UbCPaDMWKAx6nj");
            request.AddHeader("X-BFX-PAYLOAD", payload64);
            request.AddHeader("X-BFX-SIGNATURE", sig);
          //  request.a

          //  request.AddBody(payload64);
            // execute the request
            IRestResponse response = client.Execute(request);

            var content = response.Content; // raw content as string

            // or automatically deserialize result
            // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            //  RestResponse<Person> response2 = client.Execute<Person>(request);
            // var name = response2.Data.Name;

            // easy async support


            // async with deserialization


            // abort the request on demand
            //asyncHandle.Abort();

            Console.WriteLine(content.ToString());
        }


    }





    public enum TradeType
    {
        Buy,
        Sell

    }
}
