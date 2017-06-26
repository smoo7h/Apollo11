using Apollo11.Module.BusinessObjects;
using Apollo11.WorkflowServerService.helpers;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Apollo11.WorkflowServerService
{
    public class BitfinexWebsocketAPI 
    {
        //   public IObjectSpace Space { get; set; }
        // Exchange CurrentExchange;
        // OrderType OrderTypeSell;
        // OrderType OrderTypeBuy;
        //public static BitfinexWebsocketAPI(IObjectSpace space)
        //{
        //    Space = space;

        //    CurrentExchange = space.FindObject<Exchange>(new BinaryOperator("Name", "Bitfinex"));
        //    OrderTypeSell = space.FindObject<OrderType>(new BinaryOperator("Name", "Sell"));
        //    OrderTypeBuy = space.FindObject<OrderType>(new BinaryOperator("Name", "Buy"));
        //}
        public Exchange CurrentExchange { get; set; }
        public IObjectSpace Space { get; set; }
        public BitfinexWebsocketAPI(IObjectSpace space)
        {
            Count = 0;
            Space = space;
            CurrentExchange = Space.FindObject<Exchange>(new BinaryOperator("Name", "Bitfinex"));
            Socket = new WebSocketSharp.WebSocket("wss://api.bitfinex.com/ws/2");
           // Authenticate();
        }

        public WebSocketSharp.WebSocket Socket { get; set; }


        public void Authenticate()
        {

      
            string apiKey = CurrentExchange.APIKey;
            string apiSecret = CurrentExchange.APISecret;
            string authNonce = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            string eventId = "auth";
            string authPayload = "AUTH" + authNonce;
            string authSig = GetHexHashSignature(authPayload,apiSecret).ToString();


            string payload = "{\"apiKey\":\""+apiKey+"\",\"event\":\"auth\",\"authPayload\":\""+ authPayload + "\",\"authNonce\":"+authNonce+",\"authSig\":\""+authSig+"\"}";

            Socket.OnMessage += Socket_OnMessage;
            Socket.OnError += Socket_OnError;
            Socket.OnOpen += Socket_OnOpen;

            Socket.Connect();

            Socket.Send(payload);

     
            //string configpayload = "{   \"event\": \"conf\",   \"flags\": \"40 (32 XOR 8 = 40)\" }";

            //string subtrades = "{event: 'subscribe',channel: 'trades', symbol: BTCUSD}";

            //Socket.Send(subtrades);

            //Socket.Send(configpayload);

         //   Console.WriteLine("selling");
           // string payload2 = "[0, 'on', null,{ gid: 1,cid: " + authNonce + ",type: \"EXCHANGE MARKET\",symbol: \"tBTCUSD\",amount: -0.01,price: 899,hidden: 0}]";

       //    Socket.Send(payload2);
       //     Console.WriteLine("Sold");
            //  Console.Read();
            //  doneEvent.WaitOne();



        }

        private void Socket_OnOpen(object sender, EventArgs e)
        {
              string configpayload = "{   \"event\": \"conf\",   \"flags\": 40 (32 XOR 8 = 40) }";
            //   Socket.Send(configpayload);


       

            string subtrades = "{event: 'subscribe',channel: 'trades', symbol: BTCUSD}";

            Socket.Send(subtrades);

            Socket.Send(configpayload);




            Console.WriteLine("selling");
                Sell((decimal)-0.01, Convert.ToDecimal(1023.7));
                Console.WriteLine("Sold");
        }

        private void Socket_OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.Message.ToString());
        }

        public int Count { get; set; }

        private void Socket_OnMessage(object sender, MessageEventArgs e)
        {


            Console.WriteLine(e.Data.ToString());



            Count = Count + 1;
            Console.WriteLine(Count.ToString());
            if (Count == 192)
            {
                
            }
            else if (Count == 202)
            {
                string configpayload = "{   \"event\": \"conf\",   \"flags\": 40 (32 XOR 8 = 40) }";
                Socket.Send(configpayload);

                Console.WriteLine("selling");
                Sell((decimal)0.01, 1);
                Console.WriteLine("Sold");
            }
        }

        private static string GetHexHashSignature(string payload, string secret)
        {
            HMACSHA384 hmac = new HMACSHA384(Encoding.UTF8.GetBytes(secret));
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        public void Sell(decimal amount, decimal price)
        {
            string apiKey = CurrentExchange.APIKey;
            string apiSecret = CurrentExchange.APISecret;
            string authNonce = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            string eventId = "auth";
            string authPayload = "AUTH" + authNonce;
            string authSig = GetHexHashSignature(authPayload, apiSecret).ToString();

          //  DateTime.Now.ToUnixTimeSeconds().ToString();
//

            //    string payload = "[0, 'on', null,  {gid: 1,    cid: 12345,    type: \"LIMIT\",    symbol: \"tBTCUSD\",    amount: 0.0,    price: 500,    hidden: 0  }]";
            string payload = "[0,'on',null,{cid:" + authNonce + ",type:\"LIMIT\",symbol:\"tBTCUSD\",amount:" + amount.ToString() + ",price: 1023.50,hidden:0}]";

            string payload2 = "[0, 'on', null,{ gid: 1,cid: 13347,type: \"LIMIT\",symbol: \"tBTCUSD\",amount: -0.05,price: 1023.50,hidden: 0}]";

            Socket.Send(payload);


        }


        public static void GetData( IObjectSpace Space)
        {
            Exchange CurrentExchange = Space.FindObject<Exchange>(new BinaryOperator("Name", "Bitfinex"));
            OrderType OrderTypeSell = Space.FindObject<OrderType>(new BinaryOperator("Name", "Sell"));
            OrderType  OrderTypeBuy = Space.FindObject<OrderType>(new BinaryOperator("Name", "Buy"));
            float currentAsk = 0;
            float currentBid = 0;

            var doneEvent = new AutoResetEvent(false);


            using (var ws = new WebSocketSharp.WebSocket("wss://api.bitfinex.com/ws/"))
            {
                string message = "{\"event\":\"ping\"}";

                string getUSDOrderBookRAW = "{\"event\":\"subscribe\",\"channel\":\"book\",\"pair\":\"BTCUSD\",\"prec\":\"R0\"}";

                string getusdorderbook = "{\"event\":\"subscribe\",\"channel\":\"book\",\"pair\":\"BTCUSD\",\"prec\":\"P0\",\"freq\":\"F0\",\"count\":1}";




                ws.OnMessage += (sender, e) =>
                {
                    
                    try
                    {


                        //   Console.WriteLine("drag0n says: " + e.Data);
                        dynamic queryData = JsonConvert.DeserializeObject(e.Data);
                        float price = queryData[1];
                        float amount = queryData[3];


                        if (amount > 0)
                        {
                            //new bid order added

                           // currentBid = price;
                   //         Console.WriteLine("Bid " + price + "  " + amount);

                            Order newSell = Space.CreateObject<Order>();
                            newSell.OrderType = OrderTypeBuy;
                            newSell.Amount = Convert.ToDecimal(amount);
                            newSell.Price = Convert.ToDecimal(price);

                            CurrentExchange.CurrentOrder.Add(newSell);
                            Space.CommitChanges();
                            //   CreateArbTrade((decimal)0.5, "BitStamp", "quadrigacx.com", Space);
                            ArbitrageTradeHelper.CreateArbTrade((decimal)0.5, "quadrigacx.com", "Bitfinex", Space);

                        }
                        else if (amount < 0)
                        {
                            //new ask order added

                        //    Console.WriteLine("Ask " + price + "  " + amount);
                            Order newSell = Space.CreateObject<Order>();
                            newSell.OrderType = OrderTypeSell;
                            newSell.Amount = Convert.ToDecimal(amount * -1);
                            newSell.Price = Convert.ToDecimal(price);

                            CurrentExchange.CurrentOrder.Add(newSell);
                            Space.CommitChanges();
                            ArbitrageTradeHelper.CreateArbTrade((decimal)0.5, "Bitfinex", "quadrigacx.com", Space);

                            // CreateArbTrade((decimal)0.5, "BitStamp", "quadrigacx.com", Space);
                            //   Console.WriteLine("Ask " + price + "  " + amount);
                        }

                    }
                    catch (Exception ex )
                    {

                      //  Console.WriteLine(ex.Message);
                    }

                };

                ws.Connect();

                ws.Send(getusdorderbook);
              //  Console.Read();
                doneEvent.WaitOne();

            }
        }
    }
}
