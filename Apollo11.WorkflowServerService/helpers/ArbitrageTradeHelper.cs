using Apollo11.Module.BusinessObjects;
using Apollo11.Module.BusinessObjects.Trade;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollo11.WorkflowServerService.helpers
{
    public static class ArbitrageTradeHelper
    {

        public static void CreateArbTrade(decimal amount, string buyFrom, string sellTo, IObjectSpace space, Quadrigacx qApi, BitfinexRestAPI bAPI, bool test)
        {

            ArbitrageTrade ab = space.CreateObject<ArbitrageTrade>();
            ab.BuyFrom = space.FindObject<Exchange>(new BinaryOperator("Name", buyFrom));
            ab.SellTo = space.FindObject<Exchange>(new BinaryOperator("Name", sellTo));
       //     ArbitrageTrade trade = ab.CreateTrade((decimal)0.1);

         


            space.CommitChanges();

        }

        public static void CreateArbTrade(decimal amount, string buyFrom, string sellTo, IObjectSpace space)
        {

            ArbitrageTrade ab = space.CreateObject<ArbitrageTrade>();
            ab.BuyFrom = space.FindObject<Exchange>(new BinaryOperator("Name", buyFrom));
            ab.SellTo = space.FindObject<Exchange>(new BinaryOperator("Name", sellTo));
            ab.CreateTrade((decimal)0.1);

   //    al(1186.1), Convert.ToDecimal(0.02), TradeType.Buy);


            space.CommitChanges();

        }


    }
}
