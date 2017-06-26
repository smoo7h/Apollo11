using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using Apollo11.Module.BusinessObjects.Trade;
using Apollo11.Module.BusinessObjects;

namespace Apollo11.Module.Win.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ArbitrageVC : ViewController
    {
        public ArbitrageVC()
        {
            InitializeComponent();
        //    this.TargetViewType = ViewType.ListView;
            this.TargetObjectType = typeof(ArbitrageTrade);
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void Trade_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ArbitrageTrade ab = this.View.ObjectSpace.CreateObject<ArbitrageTrade>();
          //  ab.Amount = 1;
            ab.MaxAmount = 1;
            ab.BuyFrom = this.View.ObjectSpace.FindObject<Exchange>(new BinaryOperator("Name", "BitFinex"));
            ab.SellTo = this.View.ObjectSpace.FindObject<Exchange>(new BinaryOperator("Name", "quadrigacx.com"));
            ab.CreateTrade(1);
            this.View.ObjectSpace.CommitChanges();

            this.View.RefreshDataSource();
        }
    }
}
