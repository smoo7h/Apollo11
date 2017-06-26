using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using DevExpress.ExpressApp;
using Apollo11.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using Apollo11.WorkflowServerService.helpers;

namespace Apollo11.WorkflowServerService
{
    public class QuadrigacxLiveScraper : IDisposable
    {
        public IObjectSpace Space { get; set; }

        Exchange CurrentExchange;
        public QuadrigacxLiveScraper(IObjectSpace space)
        {
            Space = space;
            TopBuyValue = "";
            TopBuyAmount = "";
            TopBuyPrice = "";
            CurrentExchange = space.FindObject<Exchange>(new BinaryOperator("Name", "quadrigacx.com"));
            OrderTypeSell = space.FindObject<OrderType>(new BinaryOperator("Name", "Sell"));
            OrderTypeBuy = space.FindObject<OrderType>(new BinaryOperator("Name", "Buy"));

        }
        public void Dispose()
        {
            driver.Dispose();
        }

        private OrderType OrderTypeSell;
        private OrderType OrderTypeBuy;

        public string TopBuyValue { get; set; }
        public string TopBuyAmount { get; set; }
        public string TopBuyPrice { get; set; }

        public string TopSellValue { get; set; }
        public string TopSellAmount { get; set; }
        public string TopSellPrice { get; set; }


        public ChromeDriver driver;
        public void GetData()
        {
            // Initialize the Chrome Driver
             driver = new ChromeDriver();
           

            
            // Go to the home page
            driver.Navigate().GoToUrl("https://www.quadrigacx.com/trade");
            while (true)
            {
                try
                {

               
               
                 //sleep for a second
                    System.Threading.Thread.Sleep(50);

                    //get the tables off the page
                    var table = driver.FindElementsByClassName("table");

                    //buy orders
                    //only get the top buy order
                    var allRows2 = table[0].FindElement(By.TagName("tbody"));
                    var allRows = allRows2.FindElements(By.TagName("tr"));
                    var cell = allRows[0].FindElements(By.XPath("./*"));

                    //store value on first run
                    if (TopBuyValue == "")
                    {
                         TopBuyPrice = cell[0].Text.Replace("CAD", "").Replace(",", "").Replace("$", "");
                        TopBuyAmount = cell[1].Text.Substring(0, cell[1].Text.Length - 3);
                        TopBuyValue = cell[2].Text;
                        //create object
                        Console.WriteLine("quad buy " + cell[0].Text + " " + cell[1].Text + " " + cell[2].Text);
                        Order newBid = Space.CreateObject<Order>();

                        newBid.Amount = Convert.ToDecimal(TopBuyAmount);
                        newBid.Price = Convert.ToDecimal(TopBuyPrice);
                        newBid.OrderType = OrderTypeBuy;
                        CurrentExchange.CurrentOrder.Add(newBid);
                        Space.CommitChanges();

                    }

                    //nully pulled values
                    string price = cell[0].Text.Replace("CAD", "").Replace(",", "").Replace("$", "");
                    string amount = cell[1].Text.Substring(0, cell[1].Text.Length - 3);
                    string value = cell[2].Text;

                    if (TopBuyPrice != price || TopBuyAmount != amount || TopBuyValue != value)
                    {
                         TopBuyPrice = cell[0].Text.Replace("CAD", "").Replace(",", "").Replace("$", "");
                         TopBuyAmount = cell[1].Text.Substring(0, cell[1].Text.Length - 3);
                         TopBuyValue = cell[2].Text;

                        //create object
                        Console.WriteLine("quad buy " + cell[0].Text + " " + cell[1].Text + " " + cell[2].Text);
                        Order newBid = Space.CreateObject<Order>();
                      
                        newBid.Amount = Convert.ToDecimal(TopBuyAmount);
                        newBid.Price = Convert.ToDecimal(TopBuyPrice);
                        newBid.OrderType = OrderTypeBuy;
                        CurrentExchange.CurrentOrder.Add(newBid);
                        Space.CommitChanges();
                        ArbitrageTradeHelper.CreateArbTrade((decimal)0.5, "Bitfinex", "quadrigacx.com", Space);
                    }

                    //Sell orders
                    //only get the top sell order
                   
                    var allSellRows2 = table[1].FindElement(By.TagName("tbody"));
                    var allSellRows = allSellRows2.FindElements(By.TagName("tr"));
                    var sellCell = allSellRows[0].FindElements(By.XPath("./*"));

                    //store value on first run
                    if (TopSellValue == "")
                    {
                        TopSellPrice = sellCell[0].Text.Replace("CAD", "").Replace(",", "").Replace("$", "");
                        TopSellAmount = sellCell[1].Text.Substring(0, sellCell[1].Text.Length - 3);
                        TopSellValue = sellCell[2].Text;

                        Console.WriteLine("quad sell " + sellCell[0].Text + " " + sellCell[1].Text + " " + sellCell[2].Text);

                        Order newSell = Space.CreateObject<Order>();
                        newSell.OrderType = OrderTypeSell;
                        newSell.Amount = Convert.ToDecimal(TopSellAmount);
                        newSell.Price = Convert.ToDecimal(TopSellPrice);

                        CurrentExchange.CurrentOrder.Add(newSell);

                        Space.CommitChanges();
                    }

                    //nully pulled values
                    string sellPrice = sellCell[0].Text.Replace("CAD", "").Replace(",", "").Replace("$", "");
                    string sellAmount = sellCell[1].Text.Substring(0, sellCell[1].Text.Length - 3);
                    string sellValue = sellCell[2].Text;

                    if (TopSellPrice != sellPrice || TopSellAmount != sellAmount || TopSellValue != sellValue)
                    {
                        TopSellPrice = sellCell[0].Text.Replace("CAD", "").Replace(",", "").Replace("$", "");
                        TopSellAmount = sellCell[1].Text.Substring(0, sellCell[1].Text.Length - 3);
                        TopSellValue = sellCell[2].Text;

                        //create object
                        Console.WriteLine("quad sell " + sellCell[0].Text + " " + sellCell[1].Text + " " + sellCell[2].Text);

                        Order newSell = Space.CreateObject<Order>();
                        newSell.OrderType = OrderTypeSell;
                        newSell.Amount = Convert.ToDecimal(TopSellAmount);
                        newSell.Price = Convert.ToDecimal(TopSellPrice);

                        CurrentExchange.CurrentOrder.Add(newSell);

                        Space.CommitChanges();
                        ArbitrageTradeHelper.CreateArbTrade((decimal)0.5, "quadrigacx.com", "Bitfinex", Space);


                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("quad error");
                    Console.WriteLine(ex.Message);
                }

            }
        }

     


    }

    

    public class NewOrder
    {
        public string Price { get; set; }
        public string Value { get; set; }
        public OrderType OrderType { get; set; }

    }
}
