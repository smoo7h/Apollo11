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
    public class BitFinexLiveScraper
    {
        public IObjectSpace Space { get; set; }

          private OrderType OrderTypeSell;
        private OrderType OrderTypeBuy;

        public string TopBuyValue { get; set; }
        public string TopBuyAmount { get; set; }
        public string TopBuyPrice { get; set; }

        public string TopSellValue { get; set; }
        public string TopSellAmount { get; set; }
        public string TopSellPrice { get; set; }

        public bool testtrade = true;

        public Quadrigacx QuaAPI { get; set; }

        public BitfinexRestAPI BitAPi { get; set; }

        Exchange CurrentExchange;
        public BitFinexLiveScraper(IObjectSpace space)
        {
            Space = space;
            TopBuyValue = "";
            TopBuyAmount = "";
            TopBuyPrice = "";
            CurrentExchange = space.FindObject<Exchange>(new BinaryOperator("Name", "Bitfinex"));
            OrderTypeSell = space.FindObject<OrderType>(new BinaryOperator("Name", "Sell"));
            OrderTypeBuy = space.FindObject<OrderType>(new BinaryOperator("Name", "Buy"));

        }
        public void Dispose()
        {
            driver.Dispose();
        }

        public ChromeDriver driver;

        public void GetData()
        {
            // Initialize the Chrome Driver
            driver = new ChromeDriver();

            // Go to the home page
            driver.Navigate().GoToUrl("https://www.bitfinex.com");
            //find login button
            var loginButton = driver.FindElementById("login-button");

            loginButton.Click();
            System.Threading.Thread.Sleep(1000);
            //find username and pw 
            //userPasswordField
            //login
            var userNameField =  driver.FindElementById("login");
            var userPasswordField = driver.FindElementById("auth-password");
            //send login values
            userNameField.SendKeys("");
            userPasswordField.SendKeys("");

            //click button
            var loginbtn = driver.FindElementByName("action");
            loginbtn.Click();
            System.Threading.Thread.Sleep(1000);
         //   driver.Navigate().GoToUrl("https://www.bitfinex.com/trading");

            // btn btn-green
            //sleep for a second
       //     System.Threading.Thread.Sleep(30000);

            //userPasswordField

            while (true)
            {
                //try
               // {

               
                System.Threading.Thread.Sleep(70);

                //get the tables off the page
                var table = driver.FindElementsByClassName("compact");

                    if (table != null && table.Count == 11)
                    {
                        //buy orders
                        //only get the top buy order
                        var allRows2 = table[8].FindElement(By.TagName("tbody"));
                        var allRows = allRows2.FindElements(By.TagName("tr"));
                        var cell = allRows[0].FindElements(By.XPath("./*"));

                        //store value on first run
                        if (TopBuyValue == "")
                        {
                            string temp = cell[0].Text.Replace("\r\n", "|");
                            TopBuyPrice = temp.Split('|')[1];
                            TopBuyAmount = temp.Split('|')[3];
                            TopBuyValue = (Convert.ToDecimal(TopBuyPrice) * Convert.ToDecimal(TopBuyAmount)).ToString();

                            Order newBid = Space.CreateObject<Order>();

                            newBid.Amount = Convert.ToDecimal(TopBuyAmount);
                            newBid.Price = Convert.ToDecimal(TopBuyPrice);
                            newBid.OrderType = OrderTypeBuy;
                            CurrentExchange.CurrentOrder.Add(newBid);
                            Space.CommitChanges();
                            //Console.WriteLine("bitfinex buy " + price + " " + amount + " " + value);
                         //   ArbitrageTradeHelper.CreateArbTrade((decimal)0.5, "quadrigacx.com", "Bitfinex", Space);


                        }
                        string temp2 = cell[0].Text.Replace("\r\n", "|");
                        //nully pulled values
                        string price = temp2.Split('|')[1];
                        string amount = temp2.Split('|')[3];
                        string value = (Convert.ToDecimal(price) * Convert.ToDecimal(amount)).ToString();

                        if (TopBuyPrice != price || TopBuyAmount != amount || TopBuyValue != value && amount != "0" && value != "0" && amount != "" && value != "")
                        {
                            TopBuyPrice = price;
                            TopBuyAmount = amount;
                            TopBuyValue = value;

                            //create object
                        
                            Order newBid = Space.CreateObject<Order>();

                            newBid.Amount = Convert.ToDecimal(TopBuyAmount);
                            newBid.Price = Convert.ToDecimal(TopBuyPrice);
                            newBid.OrderType = OrderTypeBuy;
                            CurrentExchange.CurrentOrder.Add(newBid);
                            Space.CommitChanges();
                            Console.WriteLine("bitfinex buy " + price + " " + amount + " " + value);
                            ArbitrageTradeHelper.CreateArbTrade((decimal)0.5, "quadrigacx.com", "Bitfinex", Space);
                        }

                        //Sell orders
                        //only get the top sell order

                        var allSellRows2 = table[9].FindElement(By.TagName("tbody"));
                        var allSellRows = allSellRows2.FindElements(By.TagName("tr"));
                        var sellCell = allSellRows[0].FindElements(By.XPath("./*"));

                        //store value on first run
                        if (TopSellValue == "")
                        {

                            string temp3 = sellCell[0].Text.Replace("\r\n", "|");
                            TopSellPrice = temp3.Split('|')[1];
                            TopSellAmount = temp3.Split('|')[2];
                            TopSellValue = (Convert.ToDecimal(TopBuyPrice) * Convert.ToDecimal(TopBuyAmount)).ToString();

                            Order newSell = Space.CreateObject<Order>();
                            newSell.OrderType = OrderTypeSell;
                            newSell.Amount = Convert.ToDecimal(TopSellAmount);
                            newSell.Price = Convert.ToDecimal(TopSellPrice);

                            CurrentExchange.CurrentOrder.Add(newSell);

                            Space.CommitChanges();
                            //create object
                        //    Console.WriteLine("bitfinex sell " + sellPrice + " " + sellAmount + " " + sellValue);


                        }

                        //nully pulled values
                        string temp4 = sellCell[0].Text.Replace("\r\n", "|");
                        string sellPrice = temp4.Split('|')[2];
                        string sellAmount = temp4.Split('|')[1];
                        string sellValue = (Convert.ToDecimal(sellPrice) * Convert.ToDecimal(sellAmount)).ToString();

                        if (TopSellPrice != sellPrice || TopSellAmount != sellAmount || TopSellValue != sellValue && sellAmount != "0" && sellValue != "0" && sellAmount != "" && sellValue != "")
                        {
                            TopSellPrice = sellPrice;
                            TopSellAmount = sellAmount;
                            TopSellValue = sellValue;

                        
                            Order newSell = Space.CreateObject<Order>();
                            newSell.OrderType = OrderTypeSell;
                            newSell.Amount = Convert.ToDecimal(TopSellAmount);
                            newSell.Price = Convert.ToDecimal(TopSellPrice);

                            CurrentExchange.CurrentOrder.Add(newSell);

                            Space.CommitChanges();
                            //create object
                            Console.WriteLine("bitfinex sell " + sellPrice + " " + sellAmount + " " + sellValue);

                            ArbitrageTradeHelper.CreateArbTrade((decimal)0.5, "Bitfinex", "quadrigacx.com", Space);

                            if (testtrade)
                            {
                              
                            }
                        }

                    }
                    }
                //}
                //catch (Exception ex )
                //{
                //    Console.WriteLine("bitfinxe error");
                //    Console.WriteLine(ex.Message);
                //}
            }
                
            
        }

    }

