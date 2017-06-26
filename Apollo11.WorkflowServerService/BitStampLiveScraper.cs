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
using Apollo11.Module.BusinessObjects.Trade;
using Apollo11.WorkflowServerService.helpers;

namespace Apollo11.WorkflowServerService
{
    public class BitStampLiveScraper
    {
        public IObjectSpace Space { get; set; }
        Exchange CurrentExchange;
        OrderType OrderTypeSell;
        OrderType OrderTypeBuy;
        public BitStampLiveScraper(IObjectSpace space)
        {
            Space = space;
            TopBuyValue = "";
            TopBuyAmount = "";
            TopBuyPrice = "";
            TopSellValue = "";
            TopSellAmount = "";
            TopSellPrice = "";
            CurrentExchange = space.FindObject<Exchange>(new BinaryOperator("Name", "BitStamp"));
            OrderTypeSell = space.FindObject<OrderType>(new BinaryOperator("Name", "Sell"));
            OrderTypeBuy = space.FindObject<OrderType>(new BinaryOperator("Name", "Buy"));
        }
        public void Dispose()
        {
            driver.Dispose();
        }



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
            driver.Navigate().GoToUrl("https://www.bitstamp.net/market/tradeview/");

            while (true)
            {

                try
                {


                    System.Threading.Thread.Sleep(50);

                    var table = driver.FindElementsByClassName("order-book");

                    //buy orders
                    //only get the top buy order

                    var allRows2 = table[1].FindElement(By.TagName("tbody"));

                    var allRows = allRows2.FindElements(By.TagName("tr"));

                    var cell = allRows[0].FindElements(By.XPath("./*"));

                    //store value on first run
                    if (TopBuyValue == "")
                    {
                        TopBuyPrice = cell[4].Text;
                        TopBuyAmount = cell[3].Text;
                        TopBuyValue = cell[2].Text;
                    }

                    //nully pulled values
                    string price = cell[4].Text;
                    string amount = cell[3].Text;
                    string value = cell[2].Text;

                    if (TopBuyPrice != price || TopBuyAmount != amount || TopBuyValue != value)
                    {
                        TopBuyPrice = cell[4].Text;
                        TopBuyAmount = cell[3].Text;
                        TopBuyValue = cell[2].Text;
                        Console.WriteLine("bitstamp sell " + TopBuyPrice + " " + TopBuyAmount + " " + TopBuyValue);

                        Order newBuy = Space.CreateObject<Order>();
                        newBuy.OrderType = OrderTypeBuy;
                        newBuy.Amount = Convert.ToDecimal(TopBuyAmount);
                        newBuy.Price = Convert.ToDecimal(TopBuyPrice);

                        CurrentExchange.CurrentOrder.Add(newBuy);
                        ArbitrageTradeHelper.CreateArbTrade((decimal)0.5, "quadrigacx.com", "BitStamp", Space);
                        Space.CommitChanges();


                        

                    }

                    //Sell order

                    var sellallRows2 = table[2].FindElement(By.TagName("tbody"));

                    var sellallRows = sellallRows2.FindElements(By.TagName("tr"));

                    var sellcell = sellallRows[0].FindElements(By.XPath("./*"));

                    //store value on first run
                    if (TopSellValue == "")
                    {
                       

                        TopSellPrice = sellcell[0].Text;
                        TopSellAmount = sellcell[1].Text;
                        TopSellValue = sellcell[2].Text;
                    }

                    //nully pulled values
                    string sellprice = sellcell[0].Text;
                    string sellamount = sellcell[1].Text;
                    string sellvalue = sellcell[2].Text;

                    if (TopSellPrice != sellprice || TopSellAmount != sellamount || TopSellValue != sellvalue)
                    {
                        TopSellPrice = sellprice;
                        TopSellAmount = sellamount;
                        TopSellValue = sellvalue;
                        Console.WriteLine("bitstamp sell " + TopSellPrice + " " + TopSellAmount + " " + TopSellValue);

                        Order newSell = Space.CreateObject<Order>();
                        newSell.OrderType = OrderTypeSell;
                        newSell.Amount = Convert.ToDecimal(TopSellAmount);
                        newSell.Price = Convert.ToDecimal(TopSellPrice);

                        CurrentExchange.CurrentOrder.Add(newSell);
                        Space.CommitChanges();
                        ArbitrageTradeHelper.CreateArbTrade((decimal)0.5, "BitStamp", "quadrigacx.com", Space);
                      //  BitStamp quadrigacx.com
                    }






                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
        }

    }
}
